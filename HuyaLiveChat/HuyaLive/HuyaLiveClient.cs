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

using Tup;
using Tup.Tars;

namespace HuyaLive
{
    public enum ClientState
    {
        Connecting = 0,
        Connected = 1,
        Running = 2,
        Closing = 3,
        Closed = 4
    }

    class HuyaChatInfo
    {
        public long subsid = 0;
        public long topsid = 0;
        public long yyuid = 0;

        public HuyaChatInfo()
        {
            Reset();
        }

        public void SetInfo(long subsid, long topsid, long yyuid)
        {
            this.subsid = subsid;
            this.topsid = topsid;
            this.yyuid = yyuid;
        }

        public void Reset()
        {
            subsid = 0;
            topsid = 0;
            yyuid = 0;
        }
    }

    public class HuyaLiveClient
    {
        private ClientListener listener = null;
        private string roomId = "";
        private ClientState state = ClientState.Closed;

        private HttpClient httpClient = null;
        private WebSocketSharp.WebSocket websocket = null;        

        private System.Threading.Timer heartbeatTimer = null;
        private System.Threading.Timer freshGiftListTimer = null;

        private const int timeout_ms = 30000;
        // The heartbeat interval: 60 seconds.
        private const int heartbeat_ms = 60000;
        // The fresh gift list interval: 1 hours = 3600 seconds
        private const int freshGiftList_ms = 3600 * 1000;

        HuyaChatInfo chatInfo = null;

        UserId mainUserId = null;

        private object locker = new object();

        public HuyaLiveClient(ClientListener listener = null)
        {
            SetListener(listener);
        }

        public ClientListener GetListener()
        {
            return listener;
        }

        public void SetListener(ClientListener listener)
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

        private bool SendWUP(string action, string callback, TarsStruct request)
        {
            bool result = false;

            if (listener != null)
            {
                string text = string.Format("HuyaChatClient::SendWUP(), action = {0}, callback = {1}.",
                                            action, callback);
                Logger.WriteLine(listener, text);
            }

            try
            {
                TarsUniPacket wup = new TarsUniPacket();
                //wup.SetVersion(Const.PACKET_TYPE_TUP3);
                wup.ServantName = action;
                wup.FuncName = callback;
                wup.Put("tReq", request);

                WebSocketCommand command = new WebSocketCommand();
                command.iCmdType = CommandType.WupRequest;
                command.vData = wup.Encode();

                TarsOutputStream stream = new TarsOutputStream();
                command.WriteTo(stream);

                if (WsIsAlive())
                {
                    websocket.Send(stream.ToByteArray());
                    result = true;
                }
            }
            catch (Exception ex)
            {
                if (listener != null)
                {
                    listener.OnClientError(this, ex, "HuyaChatClient::SendWUP() error.");
                }
            }

            return result;
        }

        private bool ReadGiftList()
        {
            bool result = false;
            Logger.Enter(listener, "HuyaChatClient::ReadGiftList()");

            try
            {
                GetPropsListRequest propRequest = new GetPropsListRequest();
                propRequest.tUserId = mainUserId;
                propRequest.iTemplateType = (int)ClientTemplateMask.Mirror;

                result = SendWUP("PropsUIServer", "getPropsList", propRequest);
            }
            catch (Exception ex)
            {
                if (listener != null)
                {
                    listener.OnClientError(this, ex, "HuyaChatClient::ReadGiftList() error.");
                }
            }

            Logger.Leave(listener, "HuyaChatClient::ReadGiftList()");
            return result;
        }

        private bool BindWsInfo()
        {
            bool result = false;

            try
            {
                UserInfo wsUserInfo = new UserInfo();
                wsUserInfo.lUid = chatInfo.yyuid;
                wsUserInfo.bAonymous = (chatInfo.yyuid == 0);
                wsUserInfo.sGuid = mainUserId.sGuid;
                wsUserInfo.sToken = "";
                wsUserInfo.lTid = chatInfo.topsid;
                wsUserInfo.lSid = chatInfo.subsid;
                wsUserInfo.lGroupId = chatInfo.yyuid;
                wsUserInfo.lGroupType = 3;

                TarsOutputStream wsStream = new TarsOutputStream();
                wsUserInfo.WriteTo(wsStream);

                TarsOutputStream stream = new TarsOutputStream();
                WebSocketCommand wsCommand = new WebSocketCommand();
                wsCommand.iCmdType = CommandType.RegisterRequest;
                wsCommand.vData = wsStream.ToByteArray();
                wsCommand.WriteTo(stream);

                if (WsIsAlive())
                {
                    websocket.Send(stream.ToByteArray());
                    result = true;
                }
            }
            catch (Exception ex)
            {
                if (listener != null)
                {
                    listener.OnClientError(this, ex, "HuyaChatClient::BindWsInfo() error.");
                }
            }

            return result;
        }

        private bool Heartbeat()
        {
            Logger.Enter(listener, "HuyaChatClient::Heartbeat()");

            UserId userId = new UserId();
            userId.sHuyaUA = "webh5&1.0.0&websocket";

            UserHeartBeatRequest heartbeatRequest = new UserHeartBeatRequest();
            heartbeatRequest.tId = userId;
            heartbeatRequest.lTid = chatInfo.topsid;
            heartbeatRequest.lSid = chatInfo.subsid;
            heartbeatRequest.lPid = chatInfo.yyuid;
            heartbeatRequest.iLineType = (int)StreamLineType.WebSocket;

            bool result = SendWUP("onlineui", "OnUserHeartBeat", heartbeatRequest);

            Logger.Leave(listener, "HuyaChatClient::Heartbeat()");
            return result;
        }

        private void OnHeartbeat(object state)
        {
            bool success = Heartbeat();
        }

        private void OnFreshGiftList(object state)
        {
            bool success = ReadGiftList();
        }

        private void OnOpen(object sender, EventArgs eventArgs)
        {
            Logger.Enter(listener, "HuyaChatClient::OnOpen()");

            if (WsIsAlive())
            {
                // WebSocket is connected.
                state = ClientState.Connected;

                bool success;
                success  = ReadGiftList();
                success &= BindWsInfo();
                success &= Heartbeat();

                //
                // See: https://www.cnblogs.com/arxive/p/7015853.html
                //
                heartbeatTimer = new System.Threading.Timer(new TimerCallback(OnHeartbeat), null, heartbeat_ms, heartbeat_ms);
                freshGiftListTimer = new System.Threading.Timer(new TimerCallback(OnFreshGiftList), null, freshGiftList_ms, freshGiftList_ms);

                if (listener != null)
                {
                    listener.OnClientStart(this);
                }
            }

            Logger.Leave(listener, "HuyaChatClient::OnOpen()");
        }

        private void OnMessage(object sender, WebSocketSharp.MessageEventArgs eventArgs)
        {
            Logger.Enter(listener, "HuyaChatClient::OnMessage()");

            if (WsIsAlive())
            {
                string jsonStr, dataType;
                int dataLen = 0;
                if (eventArgs.IsText)
                {
                    jsonStr = eventArgs.Data;
                    dataType = "IsText";
                    dataLen = eventArgs.Data.Length;
                }
                else if (eventArgs.IsBinary)
                {
                    jsonStr = Encoding.UTF8.GetString(eventArgs.RawData);
                    dataType = "IsBinary";
                    dataLen = eventArgs.RawData.Length;
                }
                else if (eventArgs.IsPing)
                {
                    dataType = "IsPing";
                    dataLen = eventArgs.RawData.Length;
                    //return;
                }
                else
                {
                    jsonStr = "ping";
                    dataType = "Other";
                    dataLen = eventArgs.Data.Length;
                }

                try
                {
                    if (listener != null)
                    {
                        ChatMessage message = new ChatMessage();
                        message.uid = "0";
                        message.nickname = "shines77";
                        message.content = dataType;
                        message.length = dataLen;
                        message.timestamp = TimeStamp.now_ms();
                        lock (locker)
                        {
                            listener.OnClientChat(this, message);
                        }
                    }
                }
                catch (Exception ex)
                {
                    Logger.WriteLine(listener, ex);
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

            DestoryTimer();

            if (listener != null)
            {
                listener.OnClientClose(this);
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

        private HuyaChatInfo ReadChatInfo(string roomId)
        {
            HuyaChatInfo result = null;
            Logger.Enter(listener, "HuyaChatClient::ReadChatInfo()");           

            if (httpClient != null)
            {
                httpClient.Dispose();
            }

            if (httpClient == null)
            {
                string roomUrl = "https://m.huya.com/" + roomId;

                //
                // See: https://www.jianshu.com/p/f8616ef87df6
                //
                httpClient = new HttpClient();
                //
                // See: https://stackoverflow.com/questions/10547895/how-can-i-tell-when-httpclient-has-timed-out
                //
                httpClient.Timeout = TimeSpan.FromMilliseconds(timeout_ms);
                httpClient.DefaultRequestHeaders.Add("Accept", "application/json;odata=verbose");
                httpClient.DefaultRequestHeaders.Add("Accept-Encoding", "gzip,deflate");
                httpClient.DefaultRequestHeaders.Add("Connection", "keep-alive");
                httpClient.DefaultRequestHeaders.Add("Cache-Control", "max-age=0");
                httpClient.DefaultRequestHeaders.Add("User-Agent",
                    "Mozilla/5.0 (Linux; Android 5.1.1; Nexus 6 Build/LYZ28E) " +
                    "AppleWebKit/537.36 (KHTML, like Gecko) " +
                    "Chrome/63.0.3239.84 Mobile Safari/537.36");

                EnumerateHttpHeaders(httpClient.DefaultRequestHeaders);

                result = new HuyaChatInfo();
                result.Reset();

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

                    result.SetInfo(topsid, subsid, yyuid);

                    EnumerateHttpHeaders(response.Headers);
                }
            }

            Logger.Leave(listener, "HuyaChatClient::ReadChatInfo()");
            return result;
        }

        public bool StartWebSocket(string roomId)
        {
            bool result = false;
            Logger.Enter(listener, "HuyaChatClient::StartWebSocket()");

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

                    result = true;
                }
                catch (Exception ex)
                {
                    string what = ex.ToString();
                    Logger.WriteLine(listener, "Exception: " + what);
                }
            }

            Logger.Leave(listener, "HuyaChatClient::StartWebSocket()");
            return result;
        }

        public void Start(string roomId)
        {
            Logger.Enter(listener, "HuyaChatClient::Start()");

            if (IsRunning())
            {
                Stop();
            }

            chatInfo = ReadChatInfo(roomId);
            if (chatInfo != null && chatInfo.yyuid != 0)
            {
                mainUserId = new UserId();
                mainUserId.lUid = chatInfo.yyuid;
                mainUserId.sHuyaUA = "webh5&1.0.0&websocket";

                bool success = StartWebSocket(roomId);
            }

            this.roomId = roomId;

            Logger.Leave(listener, "HuyaChatClient::Start()");
        }

        public void Stop()
        {
            Logger.Enter(listener, "HuyaChatClient::Stop()");

            DestoryTimer();

            if (httpClient != null)
            {
                httpClient.Dispose();
                httpClient = null;
            }

            if (websocket != null)
            {
                websocket.Close();
                websocket = null;
            }

            state = ClientState.Closed;

            Logger.Leave(listener, "HuyaChatClient::Stop()");
        }

        private void DestoryTimer()
        {
            if (heartbeatTimer != null)
            {
                heartbeatTimer.Dispose();
                heartbeatTimer = null;
            }

            if (freshGiftListTimer != null)
            {
                freshGiftListTimer.Dispose();
                freshGiftListTimer = null;
            }
        }

        public void Dispose()
        {
            this.Stop();
        }
    }
}
