using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Diagnostics;

using WebSocketSharp;

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
        private ClientLinstener linstener = null;
        private string roomId = "";
        private ClientState state = ClientState.Closed;

        private WebSocketSharp.WebSocket websocket = null;        
        private System.Threading.Timer heartbeatTimer = null;

        private object locker = new object();

        public HuyaChatClient(ClientLinstener linstener = null)
        {
            SetLinstener(linstener);
        }

        public ClientLinstener GetLinstener()
        {
            return linstener;
        }

        public void SetLinstener(ClientLinstener linstener)
        {
            this.linstener = linstener;
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
            Logger.WriteLine(linstener, "HuyaChatClient::OnHeartbeat()");

            if (WsIsAlive())
            {
                websocket.Send("ping");
            }
        }

        private void OnOpen(object sender, EventArgs eventArgs)
        {
            Logger.Enter(linstener, "HuyaChatClient::OnOpen()");

            if (WsIsAlive())
            {
                // WebSocket is connected.
                state = ClientState.Connected;

                if (linstener != null)
                {
                    linstener.OnClientStart(this);
                }
                    
                //
                // See: https://www.cnblogs.com/arxive/p/7015853.html
                //
                heartbeatTimer = new System.Threading.Timer(new TimerCallback(OnHeartbeat), null, 0, 15000);
            }

            Logger.Leave(linstener, "HuyaChatClient::OnOpen()");
        }

        private void OnMessage(object sender, WebSocketSharp.MessageEventArgs eventArgs)
        {
            Logger.Enter(linstener, "HuyaChatClient::OnMessage()");

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
                    if (linstener != null)
                    {
                        ChatMessage message = new ChatMessage();
                        message.rid = "0";
                        message.nickname = "shines77";
                        message.timestamp = TimeStamp.now();
                        message.content = "test";
                        lock (locker)
                        {
                            linstener.OnClientChat(this, message);
                        }
                    }
                }
                catch (Exception ex)
                {
                    Debug.WriteLine(ex);
                }
            }

            Logger.Leave(linstener, "HuyaChatClient::OnMessage()");
        }

        private void OnError(object sender, WebSocketSharp.ErrorEventArgs eventArgs)
        {
            Logger.Enter(linstener, "HuyaChatClient::OnError()");
            if (linstener != null)
            {
                linstener.OnClientError(this, eventArgs.Exception, eventArgs.Message);
            }
            Logger.Leave(linstener, "HuyaChatClient::OnError()");
        }

        private void OnClose(object sender, WebSocketSharp.CloseEventArgs eventArgs)
        {
            Logger.Enter(linstener, "HuyaChatClient::OnClose()");

            // Is closing.
            state = ClientState.Closing;

            if (linstener != null)
            {
                linstener.OnClientClose(this);
            }

            if (heartbeatTimer != null)
            {
                heartbeatTimer.Dispose();
                heartbeatTimer = null;
            }

            Logger.Leave(linstener, "HuyaChatClient::OnClose()");
        }

        public void Start(string roomId)
        {
            Logger.Enter(linstener, "HuyaChatClient::Start()");

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

            Logger.Leave(linstener, "HuyaChatClient::Start()");
        }

        public void Stop()
        {
            Logger.Enter(linstener, "HuyaChatClient::Stop()");

            if (websocket != null)
            {
                websocket.Close();
                websocket = null;

                state = ClientState.Closed;
            }

            Logger.Leave(linstener, "HuyaChatClient::Stop()");
        }

        public void Dispose()
        {
            this.Stop();

            linstener = null;
        }
    }
}
