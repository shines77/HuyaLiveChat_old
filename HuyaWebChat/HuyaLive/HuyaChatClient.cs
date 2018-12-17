using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Diagnostics;

using System.Net.Http;
using WebSocketSharp;
using System.Net.Http.Headers;
using System.Text.RegularExpressions;

namespace HuyaWebChat.HuyaLive
{
    public enum ClientState : ushort
    {
        Connecting = 0,
        Connected = 1,
        Running = 2,
        Closing = 3,
        Closed = 4
    }

    public class HuyaChatClient
    {
        private ClientListener listener = null;
        private string roomId = "";
        private ClientState state = ClientState.Closed;

        private HttpClient httpClient = null;
        private WebSocketSharp.WebSocket websocket = null;        
        private System.Threading.Timer heartbeatTimer = null;

        private object locker = new object();

        public HuyaChatClient(ClientListener listener = null)
        {
            Setlistener(listener);
        }

        public ClientListener Getlistener()
        {
            return listener;
        }

        public void Setlistener(ClientListener listener)
        {
            this.listener = listener;
        }

        public ClientState GetState()
        {
            return state;
        }

        public void SetState(ClientState state)
        {
            this.state = state;
        }

        public bool IsRunning()
        {
            return (state == ClientState.Running);
        }

        public bool WsIsAlive()
        {
            return ((websocket != null) ?
                    (websocket.ReadyState == WebSocketState.Open) : false);
        }

        public bool WsIsOpen()
        {
            return (websocket.ReadyState == WebSocketState.Open);
        }

        public bool WsIsClosed()
        {
            return (websocket.ReadyState == WebSocketState.Closed);
        }

        private void OnHeartbeat(object state)
        {
            Logger.WriteLine(listener, "HuyaChatClient::OnHeartbeat()");

            if (WsIsAlive())
            {
                //websocket.Send("ping");
            }
        }

        private void OnOpen(object sender, EventArgs eventArgs)
        {
            Logger.Enter(listener, "HuyaChatClient::OnOpen()");

            if (WsIsAlive())
            {
                // WebSocket is connected.
                state = ClientState.Connected;

                if (listener != null)
                {
                    listener.OnClientStart(this);
                }
                    
                //
                // See: https://www.cnblogs.com/arxive/p/7015853.html
                //
                heartbeatTimer = new System.Threading.Timer(new TimerCallback(OnHeartbeat), null, 0, 15000);
            }

            Logger.Leave(listener, "HuyaChatClient::OnOpen()");
        }

        private void OnMessage(object sender, WebSocketSharp.MessageEventArgs eventArgs)
        {
            Logger.Enter(listener, "HuyaChatClient::OnMessage()");

            if (WsIsAlive())
            {
                string jsonStr;
                if (eventArgs.IsText)
                {
                    jsonStr = eventArgs.Data;
                }
                else if (eventArgs.IsBinary)
                {
                    jsonStr = Encoding.UTF8.GetString(eventArgs.RawData);
                }
                else if (eventArgs.IsPing)
                {
                    return;
                }
                else
                {
                    jsonStr = "ping";
                }

                try
                {
                    if (listener != null)
                    {
                        ChatMessage message = new ChatMessage();
                        message.rid = "0";
                        message.nickname = "shines77";
                        message.timestamp = TimeStamp.now();
                        message.content = "test";
                        lock (locker)
                        {
                            listener.OnClientChat(this, message);
                        }
                    }
                }
                catch (Exception ex)
                {
                    Debug.WriteLine(ex);
                }
            }

            Logger.Leave(listener, "HuyaChatClient::OnMessage()");
        }

        private void OnError(object sender, WebSocketSharp.ErrorEventArgs eventArgs)
        {
            Logger.Enter(listener, "HuyaChatClient::OnError()");
            if (listener != null)
            {
                listener.OnClientError(this, eventArgs.Exception, eventArgs.Message);
            }
            Logger.Leave(listener, "HuyaChatClient::OnError()");
        }

        private void OnClose(object sender, WebSocketSharp.CloseEventArgs eventArgs)
        {
            Logger.Enter(listener, "HuyaChatClient::OnClose()");

            // Is closing.
            state = ClientState.Closing;

            if (listener != null)
            {
                listener.OnClientClose(this);
            }

            if (heartbeatTimer != null)
            {
                heartbeatTimer.Dispose();
                heartbeatTimer = null;
            }

            Logger.Leave(listener, "HuyaChatClient::OnClose()");
        }

        private void EnumerateHttpHeaders(HttpHeaders headers)
        {
            Logger.WriteLine(listener, "");

            foreach (var header in headers)
            {
                var value = "";
                foreach (var val in header.Value)
                {
                    value += val + " ";
                }
                Logger.WriteLine(listener, header.Key + ": " + value);
            }

            Logger.WriteLine(listener, "");
        }

        static private long ParseMatchLong(Match match)
        {
            if (match.Groups.Count >= 2)
            {
                return ((match.Groups[1].Value.Trim() == "") ? 0 : long.Parse(match.Groups[1].Value));
            }
            else
            {
                return 0;
            }
        }

        public void Start(string roomId)
        {
            Logger.Enter(listener, "HuyaChatClient::Start()");

            if (httpClient != null)
            {
                httpClient.Dispose();
            }

            if (httpClient == null)
            {
                string roomUrl = "https://m.huya.com/" + roomId;

                httpClient = new HttpClient();
                //httpClient.Timeout = new TimeSpan(30000);
                httpClient.DefaultRequestHeaders.Add("Accept", "application/json;odata=verbose");
                httpClient.DefaultRequestHeaders.Add("Accept-Encoding", "gzip,deflate");
                httpClient.DefaultRequestHeaders.Add("Connection", "keep-alive");
                httpClient.DefaultRequestHeaders.Add("Cache-Control", "max-age=0");
                httpClient.DefaultRequestHeaders.Add("User-Agent",
                    "Mozilla/5.0 (Linux; Android 5.1.1; Nexus 6 Build/LYZ28E) " +
                    "AppleWebKit/537.36 (KHTML, like Gecko) " +
                    "Chrome/63.0.3239.84 Mobile Safari/537.36");

                EnumerateHttpHeaders(httpClient.DefaultRequestHeaders);

                HttpResponseMessage response = httpClient.GetAsync(roomUrl).Result;
                if (response.IsSuccessStatusCode)
                {
                    Logger.WriteLine(listener, "Response Status Code and Reason Phrase: " +
                                     response.StatusCode + " " + response.ReasonPhrase);

                    string html = response.Content.ReadAsStringAsync().Result;

                    Logger.WriteLine(listener, "Received payload of " + html.Length + " characters.");

                    //
                    // See: https://www.cnblogs.com/caokai520/p/4511848.html
                    //
                    Match topsid_set = Regex.Match(html, @"var TOPSID = '(.*)';");
                    Match subsid_set = Regex.Match(html, @"var SUBSID = '(.*)';");
                    Match yyuid_set = Regex.Match(html, @"ayyuid: '(.*)',");

                    long topsid = ParseMatchLong(topsid_set);
                    long subsid = ParseMatchLong(subsid_set);
                    long yyuid = ParseMatchLong(yyuid_set);

                    Logger.WriteLine(listener, "Html contont:\n\n{0}", html);

                    Logger.WriteLine(listener, "");
                    Logger.WriteLine(listener, "topsid = \"{0}\"", topsid);
                    Logger.WriteLine(listener, "subsid = \"{0}\"", subsid);
                    Logger.WriteLine(listener, "yyuid  = \"{0}\"", yyuid);
                    Logger.WriteLine(listener, "");

                    EnumerateHttpHeaders(response.Headers);
                }
            }

            if (websocket != null)
            {
                if (!WsIsClosed())
                {
                    websocket.Close();
                    websocket = null;
                }
            }

            if (websocket == null)
            {
                this.roomId = roomId;

                string apiUrl = "ws://ws.api.huya.com";
                try
                {
                    websocket = new WebSocketSharp.WebSocket(apiUrl);

                    websocket.OnOpen += OnOpen;
                    websocket.OnMessage += OnMessage;
                    websocket.OnError += OnError;
                    websocket.OnClose += OnClose;

                    state = ClientState.Connecting;
                    websocket.Connect();
                    state = ClientState.Running;
                }
                catch (Exception ex)
                {
                    string what = ex.ToString();
                    Debug.WriteLine("Exception: " + what);
                }
            }

            Logger.Leave(listener, "HuyaChatClient::Start()");
        }

        public void Stop()
        {
            Logger.Enter(listener, "HuyaChatClient::Stop()");

            if (websocket != null)
            {
                websocket.Close();
                websocket = null;

                state = ClientState.Closed;
            }

            Logger.Leave(listener, "HuyaChatClient::Stop()");
        }

        public void Dispose()
        {
            this.Stop();

            listener = null;
        }
    }
}
