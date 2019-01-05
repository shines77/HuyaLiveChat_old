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

    public class HuyaLiveClient
    {
        public delegate void OnWupRspEventHandler(TarsUniPacket wup);

        private Logger logger = null;
        private ClientListener listener = null;
        private string roomId = "";
        private ClientState state = ClientState.Closed;

        private bool isMobile = true;

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

        private Dictionary<int, GiftInfo> giftInfoList = new Dictionary<int, GiftInfo>();

        private object locker = new object();

        public HuyaLiveClient(ClientListener listener = null)
        {
            SetListener(listener);
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

        public bool IsMobile()
        {
            return isMobile;
        }

        public void SetMobileMode(bool isMobile)
        {
            this.isMobile = isMobile;
        }

        public ClientListener GetListener()
        {
            return listener;
        }

        public void SetListener(ClientListener listener)
        {
            this.listener = listener;
        }

        public Logger GetLogger()
        {
            return logger;
        }

        public void SetLogger(Logger logger)
        {
            this.logger = logger;
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

        public void Start(string roomId)
        {
            logger?.Enter("HuyaLiveClient::Start()");

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

            logger?.Leave("HuyaLiveClient::Start()");
        }

        public void Stop()
        {
            logger?.Enter("HuyaLiveClient::Stop()");

            DestoryTimer();

            if (websocket != null)
            {
                websocket.Close();
                websocket = null;
            }

            if (httpClient != null)
            {
                httpClient.Dispose();
                httpClient = null;
            }

            state = ClientState.Closed;

            logger?.Leave("HuyaLiveClient::Stop()");
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

        static private string ParseMatchString(Match match)
        {
            if (match.Groups.Count >= 2)
            {
                return match.Groups[1].Value;
            }
            else
            {
                return "";
            }
        }

        private void EnumerateHttpHeaders(HttpHeaders headers)
        {
            logger?.WriteLine("");

            foreach (var header in headers)
            {
                var value = "";
                foreach (var val in header.Value)
                {
                    value += val + " ";
                }
                logger?.WriteLine(header.Key + ": " + value);
            }

            logger?.WriteLine("");
        }

        private HuyaChatInfo ReadChatInfo(string roomId)
        {
            HuyaChatInfo result = null;
            logger?.Enter("HuyaLiveClient::ReadChatInfo()");

            if (httpClient != null)
            {
                httpClient.Dispose();
            }

            if (httpClient == null)
            {
                string roomUrl;
                if (isMobile)
                    roomUrl = "https://m.huya.com/" + roomId;
                else
                    roomUrl = "https://www.huya.com/" + roomId;

                //
                // See: https://www.jianshu.com/p/f8616ef87df6
                //
                httpClient = new HttpClient();
                //
                // See: https://stackoverflow.com/questions/10547895/how-can-i-tell-when-httpclient-has-timed-out
                //
                httpClient.Timeout = TimeSpan.FromMilliseconds(timeout_ms);

                if (isMobile)
                {
                    httpClient.DefaultRequestHeaders.Add("Accept", "text/html,application/xhtml+xml,application/xml;q=0.9,image/webp,image/apng,*/*;q=0.8");
                    httpClient.DefaultRequestHeaders.Add("Accept-Encoding", "gzip,deflate,br");
                    httpClient.DefaultRequestHeaders.Add("Accept-Language", "zh-CN,zh;q=0.9,en;q=0.8");
                    httpClient.DefaultRequestHeaders.Add("Connection", "keep-alive");
                    httpClient.DefaultRequestHeaders.Add("Pragma", "no-cache");
                    httpClient.DefaultRequestHeaders.Add("Cache-Control", "no-cache");
                    httpClient.DefaultRequestHeaders.Add("User-Agent",
                        "Mozilla/5.0 (Linux; Android 6.0; Nexus 7 Build/MRA58N) " +
                        "AppleWebKit/537.36 (KHTML, like Gecko) " +
                        "Chrome/63.0.3239.132 Mobile Safari/537.36");
                    //httpClient.DefaultRequestHeaders.Add("User-Agent",
                    //    "Mozilla/5.0 (Linux; Android 5.1.1; Nexus 6 Build/LYZ28E) " +
                    //    "AppleWebKit/537.36 (KHTML, like Gecko) " +
                    //    "Chrome/63.0.3239.84 Mobile Safari/537.36");
                }
                else
                {
                    httpClient.DefaultRequestHeaders.Add("Accept", "text/html,application/xhtml+xml,application/xml;q=0.9,image/webp,image/apng,*/*;q=0.8");
                    httpClient.DefaultRequestHeaders.Add("Accept-Encoding", "deflate,br");
                    httpClient.DefaultRequestHeaders.Add("Accept-Language", "zh-CN,zh;q=0.9,en;q=0.8");
                    httpClient.DefaultRequestHeaders.Add("Connection", "keep-alive");
                    httpClient.DefaultRequestHeaders.Add("Pragma", "no-cache");
                    httpClient.DefaultRequestHeaders.Add("Cache-Control", "no-cache");
                    httpClient.DefaultRequestHeaders.Add("User-Agent",
                        "Mozilla/5.0 (Windows NT 6.1; WOW64) AppleWebKit/537.36 (KHTML, like Gecko) " +
                        "Ubuntu Chromium/70.0.3538.77 Chrome/70.0.3538.77 Safari/537.36");
                    //httpClient.DefaultRequestHeaders.Add("User-Agent",
                    //    "Mozilla/5.0 (X11; Linux x86_64) AppleWebKit/537.36 (KHTML, like Gecko) " +
                    //    "Ubuntu Chromium/70.0.3538.77 Chrome/70.0.3538.77 Safari/537.36");
                }

                EnumerateHttpHeaders(httpClient.DefaultRequestHeaders);

                result = new HuyaChatInfo();
                result.Reset();

                HttpResponseMessage response = httpClient.GetAsync(roomUrl).Result;
                if (response.IsSuccessStatusCode)
                {
                    logger?.WriteLine("Response Status Code and Reason Phrase: " +
                                      response.StatusCode + " " + response.ReasonPhrase);

                    string html = response.Content.ReadAsStringAsync().Result;

                    logger?.WriteLine("Received payload of " + html.Length + " characters.");
                    logger?.WriteLine("Html contont:\n\n{0}", html);

                    if (isMobile)
                    {
                        //
                        // See: https://www.cnblogs.com/caokai520/p/4511848.html
                        //
                        Match topsid_set = Regex.Match(html, @"var TOPSID = '(.*)';");
                        Match subsid_set = Regex.Match(html, @"var SUBSID = '(.*)';");
                        Match yyuid_set = Regex.Match(html, @"ayyuid: '(.*)',");

                        long topsid = ParseMatchLong(topsid_set);
                        long subsid = ParseMatchLong(subsid_set);
                        long yyuid = ParseMatchLong(yyuid_set);

                        logger?.WriteLine("");
                        logger?.WriteLine("topsid = \"{0}\"", topsid);
                        logger?.WriteLine("subsid = \"{0}\"", subsid);
                        logger?.WriteLine("yyuid  = \"{0}\"", yyuid);
                        logger?.WriteLine("");

                        result.SetInfo(topsid, subsid, yyuid, roomId);
                    }
                    else
                    {
                        //
                        // See: https://www.cnblogs.com/caokai520/p/4511848.html
                        //

                        // var TT_ROOM_DATA = { "channel": "122222", "liveChannel": "2312323" };
                        Match room_data_set = Regex.Match(html, @"var[\s]*TT_ROOM_DATA[\s]*=[\s]*([\s\S]+?);");
                        string room_data = ParseMatchString(room_data_set);

                        Match channel_set = Regex.Match(room_data, @"""channel"":""([0-9]*)"",");
                        Match liveChannel_set = Regex.Match(room_data, @"""liveChannel"":""([0-9]*)"",");

                        long topsid = ParseMatchLong(channel_set);
                        long subsid = ParseMatchLong(liveChannel_set);

                        // var TT_PROFILE_INFO = { "lp": "13213123", "profileRoom": "666007" };
                        Match profile_info_set = Regex.Match(html, @"var[\s]*TT_PROFILE_INFO[\s]*=[\s]*([\s\S]+?);");
                        string profile_info = ParseMatchString(profile_info_set);

                        Match lp_set = Regex.Match(profile_info, @"""lp"":""([0-9]*)"",");
                        Match profileRoom_set = Regex.Match(profile_info, "\"profileRoom\":\"([^ \f\n\r\t\v\"]*)\"");

                        long yyuid = ParseMatchLong(lp_set);
                        string room_name = ParseMatchString(profileRoom_set);

                        logger?.WriteLine("");
                        logger?.WriteLine("topsid    = \"{0}\"", topsid);
                        logger?.WriteLine("subsid    = \"{0}\"", subsid);
                        logger?.WriteLine("yyuid     = \"{0}\"", yyuid);
                        logger?.WriteLine("room_name = \"{0}\"", room_name);
                        logger?.WriteLine("");

                        result.SetInfo(topsid, subsid, yyuid, room_name);
                    }

                    EnumerateHttpHeaders(response.Headers);
                }
            }

            logger?.Leave("HuyaLiveClient::ReadChatInfo()");
            return result;
        }

        public bool StartWebSocket(string roomId)
        {
            bool result = false;
            logger?.Enter("HuyaLiveClient::StartWebSocket()");

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
                string apiUrl;
                string originalUrl;

                if (isMobile)
                {
                    apiUrl = "wss://cdnws.api.huya.com";
                    originalUrl = "https://m.huya.com";
                }
                else
                {
                    apiUrl = "wss://cdnws.api.huya.com";
                    originalUrl = "https://www.huya.com";
                }

                this.roomId = roomId;

                try
                {
                    websocket = new WebSocketSharp.WebSocket(apiUrl);
                    websocket.Origin = originalUrl;
                    websocket.Compression = CompressionMethod.Deflate;

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
                    logger?.WriteLine("Exception: " + what);
                    logger?.WriteLine("");
                }
            }

            logger?.Leave("HuyaLiveClient::StartWebSocket()");
            return result;
        }

        private bool SendWup(string action, string callback, TarsStruct request)
        {
            bool result = false;

            if (logger != null)
            {
                string text = string.Format("HuyaLiveClient::SendWup(), action = {0}, callback = {1}.",
                                            action, callback);
                logger?.WriteLine(text);
            }

            try
            {
                TarsUniPacket wup = new TarsUniPacket();
                wup.SetVersion(Const.TUP_VERSION_3);
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
                    listener?.OnClientError(this, ex, "HuyaLiveClient::SendWup() error.");
                }
            }

            return result;
        }

        private bool ReadGiftList()
        {
            bool result = false;
            logger?.Enter("HuyaLiveClient::ReadGiftList()");

            try
            {
                GetPropsListRequest propRequest = new GetPropsListRequest();
                propRequest.tUserId = mainUserId;
                propRequest.iTemplateType = (int)ClientTemplateMask.Mirror;

                result = SendWup("PropsUIServer", "getPropsList", propRequest);
            }
            catch (Exception ex)
            {
                if (listener != null)
                {
                    listener?.OnClientError(this, ex, "HuyaLiveClient::ReadGiftList() error.");
                }
            }

            logger?.Leave("HuyaLiveClient::ReadGiftList()");
            return result;
        }

        private bool BindWebSocketInfo()
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

                TarsOutputStream stream = new TarsOutputStream();
                wsUserInfo.WriteTo(stream);

                WebSocketCommand command = new WebSocketCommand();
                command.iCmdType = CommandType.RegisterRequest;
                command.vData = stream.ToByteArray();

                TarsOutputStream cmdStream = new TarsOutputStream();
                command.WriteTo(cmdStream);

                if (WsIsAlive())
                {
                    websocket.Send(cmdStream.ToByteArray());
                    result = true;
                }
            }
            catch (Exception ex)
            {
                if (listener != null)
                {
                    listener?.OnClientError(this, ex, "HuyaLiveClient::BindWsInfo() error.");
                }
            }

            return result;
        }

        private bool SendRegisterGroup(long presenterUid)
        {
            bool result = false;
            string chatId = string.Format("chat:{0}", presenterUid);

            logger?.WriteLine("HuyaLiveClient::SendRegisterGroup(), chatId = \"" + chatId + "\"");

            try
            {
                RegisterGroupRequest request = new RegisterGroupRequest();
                request.vGroupId.Add(chatId);

                TarsOutputStream stream = new TarsOutputStream();
                request.WriteTo(stream);

                WebSocketCommand command = new WebSocketCommand();
                command.iCmdType = CommandType.RegisterGroupRequest;
                command.vData = stream.ToByteArray();

                TarsOutputStream cmdStream = new TarsOutputStream();
                command.WriteTo(cmdStream);

                if (WsIsAlive())
                {
                    websocket.Send(cmdStream.ToByteArray());
                    result = true;
                }
            }
            catch (Exception ex)
            {
                if (listener != null)
                {
                    listener?.OnClientError(this, ex, "HuyaLiveClient::SendRegisterGroup() error.");
                }
            }

            return result;
        }

        private bool Heartbeat()
        {
            logger?.Enter("HuyaLiveClient::Heartbeat()");

            UserId userId = new UserId();
            userId.sHuyaUA = "webh5&1.0.0&websocket";

            UserHeartBeatRequest heartbeatRequest = new UserHeartBeatRequest();
            heartbeatRequest.tId = userId;
            heartbeatRequest.lTid = chatInfo.topsid;
            heartbeatRequest.lSid = chatInfo.subsid;
            heartbeatRequest.lPid = chatInfo.yyuid;
            heartbeatRequest.iLineType = (int)StreamLineType.WebSocket;

            bool result = SendWup("onlineui", "OnUserHeartBeat", heartbeatRequest);

            logger?.Leave("HuyaLiveClient::Heartbeat()");
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
            logger?.Enter("HuyaLiveClient::OnOpen()");

            if (WsIsAlive())
            {
                // WebSocket is connected.
                state = ClientState.Connected;

                // Sec-WebSocket-Extensions: "permessage-deflate; client_max_window_bits"
                logger?.WriteLine("websocket.Extensions = \"" + websocket.Extensions + "\"");

                bool success;
                success  = ReadGiftList();
                success &= BindWebSocketInfo();
                success &= Heartbeat();

                success &= SendRegisterGroup(chatInfo.yyuid);

                //
                // See: https://www.cnblogs.com/arxive/p/7015853.html
                //
                heartbeatTimer = new System.Threading.Timer(new TimerCallback(OnHeartbeat), null, heartbeat_ms, heartbeat_ms);
                freshGiftListTimer = new System.Threading.Timer(new TimerCallback(OnFreshGiftList), null, freshGiftList_ms, freshGiftList_ms);

                if (listener != null)
                {
                    listener?.OnClientStart(this);
                }
            }

            logger?.Leave("HuyaLiveClient::OnOpen()");
        }

        private void OnMessage(object sender, WebSocketSharp.MessageEventArgs eventArgs)
        {
            logger?.Enter("HuyaLiveClient::OnMessage()");

            try
            {
                if (WsIsAlive())
                {
                    string jsonStr, dataType;
                    int dataLen = 0;
                    if (eventArgs.IsBinary)
                    {
                        jsonStr = Encoding.UTF8.GetString(eventArgs.RawData);
                        dataType = "IsBinary";
                        dataLen = eventArgs.RawData.Length;
                        TarsInputStream stream = new TarsInputStream(eventArgs.RawData);
                        WebSocketCommand command = new WebSocketCommand();
                        command.ReadFrom(stream);

                        // Handle websocket commands.
                        OnWebSocketCommand(command);
                    }
                    else if (eventArgs.IsText)
                    {
                        jsonStr = eventArgs.Data;
                        dataType = "IsText";
                        dataLen = eventArgs.Data.Length;
                    }
                    else if (eventArgs.IsPing)
                    {
                        jsonStr = "ping";
                        dataType = "IsPing";
                        dataLen = eventArgs.RawData.Length;
                    }
                    else
                    {
                        jsonStr = "other";
                        dataType = "Other";
                        dataLen = eventArgs.Data.Length;
                    }

                    if (logger != null)
                    {
                        logger.WriteLine(">>>  dataType = " + dataType + ", dataLen = " + dataLen);
                    }
                }
            }
            catch (Exception ex)
            {
                string what = ex.ToString();
                logger?.WriteLine("Exception: " + what);
                logger?.WriteLine("");
            }

            logger?.Leave("HuyaLiveClient::OnMessage()");
        }

        private void OnError(object sender, WebSocketSharp.ErrorEventArgs eventArgs)
        {
            logger?.Enter("HuyaLiveClient::OnError()");
            if (listener != null)
            {
                listener?.OnClientError(this, eventArgs.Exception, eventArgs.Message);
            }
            logger?.Leave("HuyaLiveClient::OnError()");
        }

        private void OnClose(object sender, WebSocketSharp.CloseEventArgs eventArgs)
        {
            logger?.Enter("HuyaLiveClient::OnClose()");

            // Is closing.
            state = ClientState.Closing;

            DestoryTimer();

            if (listener != null)
            {
                listener?.OnClientStop(this);
            }

            logger?.Leave("HuyaLiveClient::OnClose()");
        }

        private void OnWebSocketCommand(WebSocketCommand command)
        {
            switch (command.iCmdType)
            {
                case CommandType.RegisterResponse:
                    {
                        TarsInputStream stream = new TarsInputStream(command.vData);
                        RegisterResponse response = new RegisterResponse();
                        response.ReadFrom(stream);

                        OnRegisterResponse(response);
                    }
                    break;

                case CommandType.WupResponse:
                    {
                        TarsUniPacket wup = new TarsUniPacket();
                        wup.SetVersion(Const.TUP_VERSION_3);
                        wup.Decode(command.vData);

                        OnWupResponse(wup);
                    }
                    break;

                case CommandType.HeartBeat:
                    {
                        TarsInputStream stream = new TarsInputStream(command.vData);
                        HeartBeatResponse response = new HeartBeatResponse();
                        response.ReadFrom(stream);

                        logger?.WriteLine("CommandType = HeartBeat (5), res.iState = " + response.iState);
                    }
                    break;

                case CommandType.HeartBeatAck:
                    {
                        logger?.WriteLine("CommandType = HeartBeatAck (6)");
                    }
                    break;

                case CommandType.MsgPushRequest:
                    {
                        TarsInputStream stream = new TarsInputStream(command.vData);
                        PushMessageResponse msg = new PushMessageResponse();
                        msg.ReadFrom(stream);

                        OnMsgPushRequest(msg);
                    }
                    break;

                case CommandType.VerifyCookieResponse:
                    {
                        TarsInputStream stream = new TarsInputStream(command.vData);
                        VerifyCookieResponse response = new VerifyCookieResponse();
                        response.ReadFrom(stream);

                        OnVerifyCookieResponse(response);
                    }
                    break;

                case CommandType.RegisterGroupResponse:
                    {
                        TarsInputStream stream = new TarsInputStream(command.vData);
                        RegisterGroupResponse response = new RegisterGroupResponse();
                        response.ReadFrom(stream);

                        OnRegisterGroupResponse(response);
                    }
                    break;

                default:
                    {
                        logger?.WriteLine("CommandType = ** " + command.iCmdType);
                    }
                    break;
            }
        }

        private void OnRegisterResponse(RegisterResponse response)
        {
            logger?.WriteLine("CommandType = RegisterResponse (2), " +
                              "res.iResCode = " + response.iResCode + ", " +
                              "res.lRequestId: " + response.lRequestId + ", " +
                              "res.sMessage: \"" + response.sMessage + "\".");
        }

        private void OnRegisterGroupResponse(RegisterGroupResponse response)
        {
            logger?.WriteLine("CommandType = RegisterGroupResponse (17), res.iResCode = " + response.iResCode);
        }

        private void OnWupResponse(TarsUniPacket wup)
        {
            bool hasTraced = true;
            string funcName = wup.FuncName;
            switch (funcName)
            {
                case "doLaunch":
                    {
                        logger?.WriteLine("CommandType = WupResponse (4), funcName: " + funcName);

                        LiveLaunchResponse response = wup.Get2<LiveLaunchResponse>("tRsp", "tResp", new LiveLaunchResponse());
                        if (response != null)
                        {
                            logger?.WriteLine("res.sGuid = " + response.sGuid + ", res.sGuid = ", response.sGuid);
                        }
                    }
                    break;

                case "speak":
                    {
                        logger?.WriteLine("CommandType = WupResponse (4), funcName: " + funcName);

                        NobleSpeakResponse response = wup.Get2<NobleSpeakResponse>("tRsp", "tResp", new NobleSpeakResponse());
                        if (response != null)
                        {
                            //
                        }
                    }
                    break;

                case "OnUserEvent":
                    {
                        logger?.WriteLine("CommandType = WupResponse (4), funcName: " + funcName);

                        UserEventResponse response = wup.Get2<UserEventResponse>("tRsp", "tResp", new UserEventResponse());
                        if (response != null)
                        {
                            logger?.WriteLine("res.lTid =  " + response.lTid);
                            logger?.WriteLine("res.lSid =  " + response.lSid);
                            logger?.WriteLine("res.iUserHeartBeatInterval =  " + response.iUserHeartBeatInterval);
                            logger?.WriteLine("res.iPresentHeartBeatInterval =  " + response.iPresentHeartBeatInterval);
                        }
                    }
                    break;

                case "getPropsList":
                    {
                        logger?.WriteLine("CommandType = WupResponse (4), funcName: " + funcName);

                        GetPropsListResponse response = wup.Get2<GetPropsListResponse>("tRsp", "tResp", new GetPropsListResponse());
                        if (response != null)
                        {
                            logger?.WriteLine("res.vPropsItemList.Count() =  " + response.vPropsItemList.Count());

                            giftInfoList.Clear();
                            foreach (var propsItem in response.vPropsItemList)
                            {
                                GiftInfo giftInfo = new GiftInfo(propsItem.sPropsName, propsItem.iPropsYb / 100);
                                giftInfoList.Add(propsItem.iPropsId, giftInfo);
                            }
                        }
                    }
                    break;

                case "OnUserHeartBeat":
                    {
                        logger?.WriteLine("CommandType = WupResponse (4), funcName: " + funcName);

                        UserHeartBeatResponse response = wup.Get2<UserHeartBeatResponse>("tRsp", "tResp", new UserHeartBeatResponse());
                        if (response != null)
                        {
                            logger?.WriteLine("response.iRet = " + response.iRet);
                        }
                    }
                    break;

                case "getLivingInfo":
                    {
                        logger?.WriteLine("CommandType = WupResponse (4), funcName: " + funcName);

                        GetLivingInfoResponse response = wup.Get2<GetLivingInfoResponse>("tRsp", "tResp", new GetLivingInfoResponse());
                        if (response != null)
                        {
                            logger?.WriteLine("response.iId = " + response.iId);
                        }
                    }
                    break;

                default:
                    {
                        logger?.WriteLine("CommandType = WupResponse (4), funcName: " + funcName + " (**)");
                    }
                    break;
            }

            if (!hasTraced)
            {
                logger?.WriteLine("CommandType = WupResponse (4), funcName: " + funcName);
            }
        }

        private void OnMsgPushRequest(PushMessageResponse msg)
        {
            bool hasTraced = true;
            TarsInputStream stream = new TarsInputStream(msg.sMsg);
            int iUri = msg.iUri;
            switch (iUri)
            {
                case UriType.NobleEnterNotice:
                    {
                        logger?.WriteLine("CommandType = MsgPushRequest (7), msg.iUri: {0} - NobleEnterNotice", iUri);
                    }
                    break;

                case UriType.MessageNotice:
                    {
                        logger?.WriteLine("CommandType = MsgPushRequest (7), msg.iUri: {0} - MessageNotice", iUri);

                        MessageNotice noticeMsg = new MessageNotice();
                        noticeMsg.ReadFrom(stream);

                        UserChatMessage chatMsg = new UserChatMessage();
                        chatMsg.uid = noticeMsg.tUserInfo.lUid;
                        chatMsg.imid = noticeMsg.tUserInfo.lImid;
                        chatMsg.nickname = noticeMsg.tUserInfo.sNickName;
                        chatMsg.content = noticeMsg.sContent;
                        chatMsg.length = noticeMsg.sContent.Length;
                        chatMsg.timestamp = TimeStamp.now_ms();

                        lock (locker)
                        {
                            listener?.OnUserChat(this, chatMsg);
                        }
                    }
                    break;

                case UriType.SendItemSubBroadcastPacket:
                    {
                        logger?.WriteLine("CommandType = MsgPushRequest (7), msg.iUri: {0} - SendItemSubBroadcastPacket", iUri);

                        SendItemSubBroadcastPacket packet = new SendItemSubBroadcastPacket();
                        packet.ReadFrom(stream);

                        if (packet.lPresenterUid == chatInfo.yyuid)
                        {
                            GiftInfo giftInfo = giftInfoList[packet.iItemType];

                            UserGiftMessage giftMsg = new UserGiftMessage();
                            giftMsg.presenterUid = packet.lPresenterUid;
                            giftMsg.uid = packet.lSenderUid;
                            giftMsg.imid = packet.lSenderUid; ;
                            giftMsg.nickname = packet.sSenderNick;
                            giftMsg.itemName = giftInfo.name;
                            giftMsg.itemCount = packet.iItemCount;
                            giftMsg.itemPrice = packet.iItemCount * giftInfo.price;
                            giftMsg.timestamp = TimeStamp.now_ms();

                            lock (locker)
                            {
                                listener?.OnUserGift(this, giftMsg);
                            }
                        }
                    }
                    break;

                case UriType.AttendeeCountNotice:
                    {
                        logger?.WriteLine("CommandType = MsgPushRequest (7), msg.iUri: {0} - AttendeeCountNotice", iUri);

                        AttendeeCountNotice packet = new AttendeeCountNotice();
                        packet.ReadFrom(stream);

                        OnlineUserMessage onlineMsg = new OnlineUserMessage();
                        onlineMsg.roomId = roomId;
                        onlineMsg.roomName = "";
                        onlineMsg.onlineUsers = packet.iAttendeeCount;
                        onlineMsg.timestamp = TimeStamp.now_ms();

                        lock (locker)
                        {
                            listener?.OnRoomOnlineUser(this, onlineMsg);
                        }
                    }
                    break;

                default:
                    {
                        logger?.WriteLine("CommandType = MsgPushRequest (7), msg.iUri: " + iUri + " (**)");
                    }
                    break;
            }

            if (!hasTraced)
            {
                logger?.WriteLine("CommandType = MsgPushRequest (7), msg.iUri: " + iUri);
            }
        }

        private void OnVerifyCookieResponse(VerifyCookieResponse response)
        {
            logger?.WriteLine("CommandType = VerifyCookieResponse (11), res.iValidate: " + response.iValidate);
        }
    }
}
