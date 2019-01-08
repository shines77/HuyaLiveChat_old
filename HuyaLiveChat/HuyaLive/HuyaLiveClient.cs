using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;

using System.Net.Http;
using System.Net.Http.Headers;
using System.Text.RegularExpressions;
using System.Net;
using System.Runtime.InteropServices;

using WebSocketSharp;

using Tup;
using Tup.Tars;

namespace HuyaLive
{
    public enum ClientState
    {
        Connecting = 0,
        Connected = 1,
        Openning = 2,
        Running = 3,
        Closing = 4,
        Closed = 5
    }

    public class HuyaLiveClient
    {
        [DllImport("wininet.dll", CharSet = CharSet.Auto, SetLastError = true)]
        public static extern bool InternetSetCookie(string lpszUrlName, string lbszCookieName, string lpszCookieData);

        [DllImport("wininet.dll", CharSet = CharSet.Auto, SetLastError = true)]
        public static extern bool InternetGetCookie(string lpszUrlName, string lbszCookieName, StringBuilder lpszCookieData, ref int lpdwSize);

        [DllImport("kernel32.dll")]
        public static extern Int32 GetLastError();

        private const int ERROR_INSUFFICIENT_BUFFER = 122;
        private const int ERROR_NO_MORE_ITEMS = 259;

        public delegate void OnWupRspEventHandler(TarsUniPacket wup);

        private Logger logger = null;
        private ClientListener listener = null;
        private string roomId = "";
        private ClientState state = ClientState.Closed;

        private bool isMobile = true;
        private bool isHttps = true;

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
        private Dictionary<int, string> petInfoList = new Dictionary<int, string>();

        private object locker = new object();

        HttpClientHandler httpClientHandler = null;
        CookieContainer cookieContainer = null;

        Dictionary<string, string> cookiesMap = new Dictionary<string, string>();
        Dictionary<string, string> ieCookiesMap = new Dictionary<string, string>();

        private readonly string[] ieCookieNames = {
            "guid",            
            "yyuid",
            "username",
            "nickname",
            "avatar",
            "account_token",
            "stamp",

            "udb_passport",
            "udb_uid",
            "udb_version",
            "udb_guiddata",
            "udb_origin",
            "udb_other",
            "udb_status",
            "udb_accdata",
            "udb_passdata",
            "udb_biztoken",
            "udb_l",
            "udb_n",
            "udb_oar",

            "h_unt",

            "Hm_lvt_51700b6c722f5bb4cf39906a596ea41f",
            "Hm_lpvt_51700b6c722f5bb4cf39906a596ea41f",

            "__yamid_tt1",
            "__yamid_new",
            "__yasmid",
            "__yaoldyyuid",
            "_yasids",

            "alphaValue",
            "SoundValue",
            "isInLiveRoom",
        };

        //private string fullCookie = "__yamid_tt1=0.17434570529241689; __yamid_new=C7DB527294E00001A3D987F0179DCD10; alphaValue=0.80; udb_guiddata=8b19eb642ae34aeaa70d104b22211133; guid=7160c3aa7cf8155ca4416a0b6b0be86b; SoundValue=0.20; udb_biztoken=AQAQMxPaf4DfKWOiGqBPs2cuRLPXs4gS3P9zmKTxBTYkQzslnfvKQu8MWQJ4zH2RWlS4NO-eq-c6DfEIqz3K68dri0CfRUimnoG8r_gydkSYnLi-St8S5IB49ylLkCbmdH0qPnYgaMSKxQTRP3nDHSf0H09_n4FYQIpUiKraeerR_T3rBXA8CzELoFkN1wYjAbCPBvzcfT311z5vfZQWni9EXORskL6DNS0S_jltZCAw-_SqZkCFWXngzkBhwltl9_Iic_9u197KwT13_yeX8Bwe4ZTeAo-Vk9ZI5OzoqfbwrwwaaSzRcM4HjJqDe2NnfVRbOCOMWarsWW7RmXgsTsJt; udb_origin=3; udb_other=%7B%22lt%22%3A%221546648515426%22%2C%22isRem%22%3A%221%22%7D; udb_passport=wokss66; udb_status=1; udb_uid=497167975; udb_version=1.0; username=wokss66; yyuid=497167975; udb_accdata=wokss66; __yasmid=0.17434570529241689; __yaoldyyuid=497167975; _yasids=__rootsid%3DC848659AE780000172C0C62018B0B900; h_unt=1546726490; isInLiveRoom=true; Hm_lvt_51700b6c722f5bb4cf39906a596ea41f=1544943741,1544966304,1546648466,1546726490; udb_passdata=3; Hm_lpvt_51700b6c722f5bb4cf39906a596ea41f=1546727912";
        //private string fullCookie = "__yamid_tt1=0.17434570529241689; __yamid_new=C7DB527294E00001A3D987F0179DCD10; alphaValue=0.80; udb_guiddata=8b19eb642ae34aeaa70d104b22211133; guid=7160c3aa7cf8155ca4416a0b6b0be86b; SoundValue=0.20; udb_biztoken=AQAQMxPaf4DfKWOiGqBPs2cuRLPXs4gS3P9zmKTxBTYkQzslnfvKQu8MWQJ4zH2RWlS4NO-eq-c6DfEIqz3K68dri0CfRUimnoG8r_gydkSYnLi-St8S5IB49ylLkCbmdH0qPnYgaMSKxQTRP3nDHSf0H09_n4FYQIpUiKraeerR_T3rBXA8CzELoFkN1wYjAbCPBvzcfT311z5vfZQWni9EXORskL6DNS0S_jltZCAw-_SqZkCFWXngzkBhwltl9_Iic_9u197KwT13_yeX8Bwe4ZTeAo-Vk9ZI5OzoqfbwrwwaaSzRcM4HjJqDe2NnfVRbOCOMWarsWW7RmXgsTsJt; udb_origin=3; udb_other=%7B%22lt%22%3A%221546648515426%22%2C%22isRem%22%3A%221%22%7D; udb_passport=wokss66; udb_status=1; udb_uid=497167975; udb_version=1.0; username=wokss66; yyuid=497167975; udb_accdata=wokss66; __yasmid=0.17434570529241689; __yaoldyyuid=497167975; _yasids=__rootsid%3DC848659AE780000172C0C62018B0B900; isInLiveRoom=true; Hm_lvt_51700b6c722f5bb4cf39906a596ea41f=1544943741,1544966304,1546648466,1546726490; udb_passdata=3; h_unt=1546728584; Hm_lpvt_51700b6c722f5bb4cf39906a596ea41f=1546728584";
        //private string fullCookie = "udb_biztoken=AQCgmAQvxRpZ297JOCK-B-tV1_YzefI6aqLoPDCz_WI8ryAi6eM7cHVjJ80_Jo-Ui9lzj1pDO0V7_fReLvvTeO4fiut-NfkLRF8dlmUY9IRZfllTOACXa_8oUvUO_tc9jw3R2uSddRqWBSBdOSbnpBr9cNqguEeKJ953UaZufhIWUKy6v08y0dmHMRvPXHIW9qEcY91HRQhvoStbhxWsHTbCyBVgZuPxN9qxeAKXtE6rmLLrwH7FzCVOhDTda0eFgvW-n8M-9k1J7_0qV9qlMvzrRML2yU9IR2B4MVGl_U-tHCEQtB5lY_3okIUBFEQth8K50T-ahd_lKMsZy682Zdm3; Hm_lvt_51700b6c722f5bb4cf39906a596ea41f=1546794211,1546804156,1546955687,1546971884; stamp=768153742; udb_status=1; isInLiveRoom=true; username=wokss66; udb_other=%7B%22lt%22%3A%221546955702657%22%2C%22isRem%22%3A%221%22%7D; yyuid=497167975; __yamid_tt1=0.8704622099901131; guid=b7247c9d8c6f285ce1ae565857584a4e; udb_passport=wokss66; udb_version=1.0; udb_guiddata=9d98835a54cf4de793cef1d832b958c4; udb_origin=3; h_unt=1546971882; __yamid_new=C7D9621BECF000012E53951B12013D10; udb_uid=497167975; udb_accdata=wokss66; SoundValue=0; udb_passdata=3; __yasmid=0.8704622099901131; __yaoldyyuid=497167975; _yasids=__rootsid%3DC8497634C70000017B15829A6820C2F0; Hm_lpvt_51700b6c722f5bb4cf39906a596ea41f=1546971884";
        private readonly string fullCookie = "nickname=Cpp\u4e36\u90ed\u5b50; udb_biztoken=AQCgmAQvxRpZ297JOCK-B-tV1_YzefI6aqLoPDCz_WI8ryAi6eM7cHVjJ80_Jo-Ui9lzj1pDO0V7_fReLvvTeO4fiut-NfkLRF8dlmUY9IRZfllTOACXa_8oUvUO_tc9jw3R2uSddRqWBSBdOSbnpBr9cNqguEeKJ953UaZufhIWUKy6v08y0dmHMRvPXHIW9qEcY91HRQhvoStbhxWsHTbCyBVgZuPxN9qxeAKXtE6rmLLrwH7FzCVOhDTda0eFgvW-n8M-9k1J7_0qV9qlMvzrRML2yU9IR2B4MVGl_U-tHCEQtB5lY_3okIUBFEQth8K50T-ahd_lKMsZy682Zdm3; Hm_lvt_51700b6c722f5bb4cf39906a596ea41f=1546794211,1546804156,1546955687,1546971884; stamp=784056872; udb_status=1; isInLiveRoom=true; username=wokss66; udb_other=%7B%22lt%22%3A%221546955702657%22%2C%22isRem%22%3A%221%22%7D; yyuid=497167975; __yamid_tt1=0.8704622099901131; guid=b7247c9d8c6f285ce1ae565857584a4e; udb_passport=wokss66; udb_version=1.0; udb_guiddata=9d98835a54cf4de793cef1d832b958c4; udb_origin=3; h_unt=1546972358; __yamid_new=C7D9621BECF000012E53951B12013D10; udb_uid=497167975; udb_accdata=wokss66; SoundValue=0; udb_passdata=3; __yasmid=0.8704622099901131; __yaoldyyuid=497167975; _yasids=__rootsid%3DC8497634C70000017B15829A6820C2F0; Hm_lpvt_51700b6c722f5bb4cf39906a596ea41f=1546972359; PHPSESSID=m76hh1alh2cfp9bcv8n0jq8ck0";
        private readonly string user_agent = "Mozilla/5.0 (Windows NT 10.0; WOW64; Trident/7.0; rv:11.0) like Gecko";

        public HuyaLiveClient(ClientListener listener = null)
        {
            SetListener(listener);

            petInfoList.Add(0, "剑士");
            petInfoList.Add(1, "骑士");
            petInfoList.Add(2, "领主");
            petInfoList.Add(3, "公爵");
            petInfoList.Add(4, "君王");
            petInfoList.Add(5, "帝王");
            petInfoList.Add(6, "未知");

            fullCookie = fullCookie.Replace(",", "%2C");
            GetCookiesMap();
        }

        public bool IsMobile()
        {
            return isMobile;
        }

        public void SetMobileMode(bool isMobile)
        {
            this.isMobile = isMobile;
        }

        public bool IsHttps()
        {
            return isHttps;
        }

        public void SetHttps(bool isHttps)
        {
            this.isHttps = isHttps;
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

            Stop();

            state = ClientState.Openning;

            if (cookieContainer == null)
            {
                cookieContainer = new CookieContainer();
            }
            if (httpClientHandler == null)
            {
                httpClientHandler = new HttpClientHandler();
            }

            chatInfo = ReadChatInfo(roomId);
            if (chatInfo != null && chatInfo.yyuid != 0)
            {
                mainUserId = new UserId();
                mainUserId.lUid = chatInfo.yyuid;
                mainUserId.sHuyaUA = "webh5&1.0.0&websocket";

                CheckLogon();
                CheckUserNick();

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

            if (httpClientHandler != null)
            {
                httpClientHandler.Dispose();
                httpClientHandler = null; ;
            }

            if (cookieContainer != null)
            {
                cookieContainer = null;
            }

            state = ClientState.Closed;

            logger?.Leave("HuyaLiveClient::Stop()");
        }

        public void Dispose()
        {
            this.Stop();
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

        private void GetCookiesMap()
        {
            cookiesMap.Clear();

            string[] cookie_array = fullCookie.Split(';');
            foreach (var cookie_str in cookie_array)
            {
                string cookie_str2 = cookie_str.Trim();
                string[] cookie_pair = cookie_str2.Split('=');
                if (cookie_pair.Length == 2)
                {
                    cookie_pair[0] = cookie_pair[0].Trim();
                    cookie_pair[1] = WebUtility.UrlEncode(cookie_pair[1].Trim());
                    cookiesMap.Add(cookie_pair[0], cookie_pair[1]);
                }
            }
        }

        private bool GetIECookie(string url, string name, ref string value)
        {
            int bufSize = 1024;
            StringBuilder cookie_data = new StringBuilder(bufSize);
            if (InternetGetCookie(url, name, cookie_data, ref bufSize))
            {
                value = cookie_data.ToString();
                return true;
            }
            else
            {
                int errorCode = GetLastError();
                if (errorCode != ERROR_NO_MORE_ITEMS && errorCode != ERROR_INSUFFICIENT_BUFFER)
                {
                    logger?.WriteLine("InternetGetCookie(), errorCode = {0}", errorCode);
                }
                value = "";
                return false;
            }
        }

        private void GetInternetExplorerCookies(string domain)
        {
            string value = "";
            try
            {
                ieCookiesMap.Clear();
                foreach (var name in ieCookieNames)
                {
                    if (GetIECookie(domain, name, ref value))
                    {
                        if (!ieCookiesMap.ContainsKey(name))
                        {
                            ieCookiesMap.Add(name, value);
                        }
                        logger?.WriteLine("IE get cookie: domain = {0}, name = {1}, value = {2}", domain, name, value);
                    }
                    else
                    {
                        logger?.WriteLine("IE get cookie: domain = {0}, name = {1}, is no exists.", domain, name);
                    }
                }
                logger?.WriteLine("");
            }
            catch (Exception ex)
            {
                logger?.WriteLine(ex);
            }
        }

        private bool SetIECookie(string url, string name, string value)
        {
            if (InternetSetCookie(url, name, value))
            {
                return true;
            }
            else
            {
                int errorCode = GetLastError();
                logger?.WriteLine("InternetSetCookie(), errorCode = {0}", errorCode);
                return true;
            }
        }

        private void SetInternetExplorerCookies(string domain)
        {
            try
            {
                foreach (var cookie in cookiesMap)
                {
                    if (SetIECookie(domain, cookie.Key, cookie.Value + "; expires=Sun, 22-Feb-2099 00:00:00 GMT; path=/; domain=.huya.com"))
                    {
                        logger?.WriteLine("IE set cookie: domain = {0}, name = {1}, value = {2}", domain, cookie.Key, cookie.Value);
                    }
                    else
                    {
                        logger?.WriteLine("IE set cookie: domain = {0}, name = {1}, value = {2}, set failed.", domain, cookie.Key);
                    }
                }
                logger?.WriteLine("");
            }
            catch (Exception ex)
            {
                logger?.WriteLine(ex);
            }
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

        private int SetHttpCookies(Uri domainUri, CookieContainer _cookieContainer)
        {
            if (_cookieContainer != null)
            {
                foreach (var cookie in cookiesMap)
                {
                    _cookieContainer.Add(domainUri, new Cookie(cookie.Key, cookie.Value, "/", ".huya.com"));
                }
            }

            return cookiesMap.Count;
        }

        private HuyaChatInfo ReadChatInfo(string roomId)
        {
            HuyaChatInfo result = null;
            logger?.Enter("HuyaLiveClient::ReadChatInfo()");

            if (httpClient == null)
            {
                string roomUrl;
                if (isMobile)
                    roomUrl = "https://m.huya.com/" + roomId;
                else
                    roomUrl = "https://www.huya.com/" + roomId;

                GetInternetExplorerCookies("https://huya.com/");
                GetInternetExplorerCookies(roomUrl);

                SetInternetExplorerCookies("https://www.huya.com/");
                GetInternetExplorerCookies("https://www.huya.com/");

                Uri domainUri;
                if (isMobile)
                    domainUri = new Uri("https://m.huya.com");
                else
                    domainUri = new Uri("https://www.huya.com");

                httpClientHandler.CookieContainer = cookieContainer;
                httpClientHandler.UseCookies = true;

                SetHttpCookies(domainUri, cookieContainer);

                //
                // See: https://www.jianshu.com/p/f8616ef87df6
                //
                httpClient = new HttpClient(httpClientHandler);
                //
                // See: https://stackoverflow.com/questions/10547895/how-can-i-tell-when-httpclient-has-timed-out
                //
                httpClient.Timeout = TimeSpan.FromMilliseconds(timeout_ms);

                if (isMobile)
                {
                    httpClient.DefaultRequestHeaders.Clear();
                    httpClient.DefaultRequestHeaders.Add("Accept", "text/html,application/xhtml+xml,application/xml;q=0.9,image/webp,image/apng,*/*;q=0.8");
                    httpClient.DefaultRequestHeaders.Add("Accept-Encoding", "gzip, deflate, br");
                    httpClient.DefaultRequestHeaders.Add("Accept-Language", "zh-CN,zh;q=0.9,en;q=0.8");
                    httpClient.DefaultRequestHeaders.Add("Connection", "keep-alive");
                    httpClient.DefaultRequestHeaders.Add("Pragma", "no-cache");
                    httpClient.DefaultRequestHeaders.Add("Cache-Control", "no-cache");
                    //httpClient.DefaultRequestHeaders.Add("Cookie", fullCookie);
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
                    httpClient.DefaultRequestHeaders.Clear();
                    httpClient.DefaultRequestHeaders.Add("Accept", "text/html,application/xhtml+xml,application/xml;q=0.9,image/webp,image/apng,*/*;q=0.8");
                    //httpClient.DefaultRequestHeaders.Add("Accept-Encoding", "gzip, deflate, br");
                    httpClient.DefaultRequestHeaders.Add("Accept-Encoding", "deflate, br");
                    httpClient.DefaultRequestHeaders.Add("Accept-Language", "zh-CN,zh;q=0.9,en;q=0.8");
                    httpClient.DefaultRequestHeaders.Add("Connection", "keep-alive");
                    httpClient.DefaultRequestHeaders.Add("Pragma", "no-cache");
                    httpClient.DefaultRequestHeaders.Add("Cache-Control", "no-cache");
                    //httpClient.DefaultRequestHeaders.Add("Cookie", fullCookie);
                    httpClient.DefaultRequestHeaders.Add("User-Agent", user_agent);
                    /*
                    httpClient.DefaultRequestHeaders.Add("User-Agent",
                        "Mozilla/5.0 (Windows NT 10.0; Win64; x64) AppleWebKit/537.36 (KHTML, like Gecko) " +
                        "Chrome/63.0.3239.132 Safari/537.36");
                    *///
                    //httpClient.DefaultRequestHeaders.Add("User-Agent",
                    //    "Mozilla/5.0 (Windows NT 6.1; WOW64) AppleWebKit/537.36 (KHTML, like Gecko) " +
                    //    "Ubuntu Chromium/70.0.3538.77 Chrome/70.0.3538.77 Safari/537.36");
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

                    IEnumerable<Cookie> responseCookies = cookieContainer.GetCookies(domainUri).Cast<Cookie>();
                    foreach (Cookie cookie in responseCookies)
                    {
                        logger?.WriteLine("domain = {0}, name = {1}, value = {2}",
                                          cookie.Domain, cookie.Name, cookie.Value);
                    }

                    EnumerateHttpHeaders(response.Headers);
                }
            }

            logger?.Leave("HuyaLiveClient::ReadChatInfo()");
            return result;
        }

        /*
         * Struggling trying to get cookie out of response with HttpClient in .net 4.5
         *
         * See: https://stackoverflow.com/questions/13318102/struggling-trying-to-get-cookie-out-of-response-with-httpclient-in-net-4-5
         * See: https://codeday.me/bug/20170731/51674.html
         *
         * InternetSetCookie(url, cookie.Name, cookie.Value + "; expires=\"Sun, 22-Feb-2099 00:00:00 GMT\"");
         */
        public void CheckLogon()
        {
            const string checkLogonURL = "https://www.huya.com/udb_web/checkLogin.php";

            logger?.Enter("HuyaLiveClient::CheckLogon()");
#if true
            Uri domainUri;
            if (isMobile)
                domainUri = new Uri("https://m.huya.com");
            else
                domainUri = new Uri("https://www.huya.com");
            SetHttpCookies(domainUri, cookieContainer);

            httpClient.DefaultRequestHeaders.Clear();
            httpClient.DefaultRequestHeaders.Add("Accept", "text/html,application/xhtml+xml,application/xml;q=0.9,image/webp,image/apng,*/*;q=0.8");
            httpClient.DefaultRequestHeaders.Add("Accept-Encoding", "gzip, deflate, br");
            //httpClient.DefaultRequestHeaders.Add("Accept-Encoding", "deflate, br");
            httpClient.DefaultRequestHeaders.Add("Accept-Language", "zh-CN,zh;q=0.9,en;q=0.8");
            httpClient.DefaultRequestHeaders.Add("Connection", "keep-alive");
            httpClient.DefaultRequestHeaders.Add("Pragma", "no-cache");
            httpClient.DefaultRequestHeaders.Add("Cache-Control", "no-cache");
            //httpClient.DefaultRequestHeaders.Add("Cookie", fullCookie);
            httpClient.DefaultRequestHeaders.Add("User-Agent", user_agent);
            /*
            httpClient.DefaultRequestHeaders.Add("User-Agent",
                "Mozilla/5.0 (Windows NT 10.0; Win64; x64) AppleWebKit/537.36 (KHTML, like Gecko) " +
                "Chrome/63.0.3239.132 Safari/537.36");
            //*/
#endif
            EnumerateHttpHeaders(httpClient.DefaultRequestHeaders);

            HttpResponseMessage response = httpClient.GetAsync(checkLogonURL).Result;
            if (response.IsSuccessStatusCode)
            {
                string html = response.Content.ReadAsStringAsync().Result;

                logger?.WriteLine("Html contont:\n\n{0}", html);

                EnumerateHttpHeaders(response.Headers);

                IEnumerable<Cookie> responseCookies = cookieContainer.GetCookies(domainUri).Cast<Cookie>();
                foreach (Cookie cookie in responseCookies)
                {
                    logger?.WriteLine("domain = {0}, name = {1}, value = {2}",
                                      cookie.Domain, cookie.Name, cookie.Value);
                }
            }

            logger?.Leave("HuyaLiveClient::CheckLogon()");
        }

        public void CheckUserNick()
        {
            const string checkLogonURL = "https://www.huya.com/udb_web/udbport2.php?m=HuyaHome&do=checkUserNick";

            logger?.Enter("HuyaLiveClient::CheckUserNick()");
#if true
            Uri domainUri;
            if (isMobile)
                domainUri = new Uri("https://m.huya.com");
            else
                domainUri = new Uri("https://www.huya.com");
            SetHttpCookies(domainUri, cookieContainer);

            httpClient.DefaultRequestHeaders.Clear();
            httpClient.DefaultRequestHeaders.Add("Accept", "text/html,application/xhtml+xml,application/xml;q=0.9,image/webp,image/apng,*/*;q=0.8");
            httpClient.DefaultRequestHeaders.Add("Accept-Encoding", "gzip, deflate, br");
            //httpClient.DefaultRequestHeaders.Add("Accept-Encoding", "deflate, br");
            httpClient.DefaultRequestHeaders.Add("Accept-Language", "zh-CN,zh;q=0.9,en;q=0.8");
            httpClient.DefaultRequestHeaders.Add("Connection", "keep-alive");
            httpClient.DefaultRequestHeaders.Add("Pragma", "no-cache");
            httpClient.DefaultRequestHeaders.Add("Cache-Control", "no-cache");
            //httpClient.DefaultRequestHeaders.Add("Cookie", fullCookie);
            httpClient.DefaultRequestHeaders.Add("User-Agent", user_agent);
            /*
            httpClient.DefaultRequestHeaders.Add("User-Agent",
                "Mozilla/5.0 (Windows NT 10.0; Win64; x64) AppleWebKit/537.36 (KHTML, like Gecko) " +
                "Chrome/63.0.3239.132 Safari/537.36");
            //*/
#endif
            EnumerateHttpHeaders(httpClient.DefaultRequestHeaders);

            HttpResponseMessage response = httpClient.GetAsync(checkLogonURL).Result;
            if (response.IsSuccessStatusCode)
            {
                string html = response.Content.ReadAsStringAsync().Result;

                logger?.WriteLine("Html contont:\n\n{0}", html);

                EnumerateHttpHeaders(response.Headers);

                IEnumerable<Cookie> responseCookies = cookieContainer.GetCookies(domainUri).Cast<Cookie>();
                foreach (Cookie cookie in responseCookies)
                {
                    logger?.WriteLine("domain = {0}, name = {1}, value = {2}",
                                      cookie.Domain, cookie.Name, cookie.Value);
                }
            }

            logger?.Leave("HuyaLiveClient::CheckUserNick()");
        }

        public int SetWebSocketCookies(WebSocket _websocket)
        {
            if (_websocket != null)
            {
#if false
                foreach (var cookie in cookiesMap)
                {
                    _websocket.SetCookie(new WebSocketSharp.Net.Cookie(cookie.Key, cookie.Value, "/", "www.huya.com"));
                }
#else
                Uri domainUri;
                if (isMobile)
                    domainUri = new Uri("https://m.huya.com");
                else
                    domainUri = new Uri("https://www.huya.com");
                IEnumerable<Cookie> responseCookies = cookieContainer.GetCookies(domainUri).Cast<Cookie>();
                foreach (var cookie in responseCookies)
                {
                    _websocket.SetCookie(new WebSocketSharp.Net.Cookie(cookie.Name, cookie.Value,
                                                                       cookie.Path, cookie.Domain));
                }
#endif
            }
            
            return cookiesMap.Count;
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
                    //
                    // websocket-sharp manual
                    //
                    // See: https://www.jianshu.com/p/c48ce95b58d2
                    // See: https://blog.csdn.net/lxrj2008/article/details/83025938
                    //
                    websocket = new WebSocketSharp.WebSocket(apiUrl);
                    websocket.Origin = originalUrl;
                    websocket.Compression = CompressionMethod.Deflate;

                    ///*
                    int nCookieCount = SetWebSocketCookies(websocket);
                    foreach (var cookie in websocket.Cookies)
                    {
                        logger?.WriteLine("domain = {0}, name = {1}, value = {2}",
                                          cookie.Domain, cookie.Name, cookie.Value);
                    }
                    //*/

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

            logger?.WriteLine("HuyaLiveClient::SendWup(), action = {0}, callback = {1}.",
                              action, callback);

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

        private bool SendRegister()
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
                    listener?.OnClientError(this, ex, "HuyaLiveClient::SendRegister() error.");
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
                success &= SendRegister();
                success &= Heartbeat();

                //success &= SendRegisterGroup(chatInfo.yyuid);

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
                                GiftInfo giftInfo = new GiftInfo(propsItem.sPropsName, propsItem.iPropsYb);
                                giftInfoList.Add(propsItem.iPropsId, giftInfo);
                            }

                            listener?.OnFreshGiftList(this, null);
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
                case UriType.NobleNotice:
                    {
                        logger?.WriteLine("CommandType = MsgPushRequest (7), msg.iUri: {0} - NobleNotice", iUri);

                        NobleEnterNotice noticeMsg = new NobleEnterNotice();
                        noticeMsg.ReadFrom(stream);

                        NobleOnlineMessage enterMsg = new NobleOnlineMessage();
                        enterMsg.uid = noticeMsg.tNobleInfo.lUid;
                        enterMsg.imid = noticeMsg.tNobleInfo.lPid;
                        enterMsg.nickname = noticeMsg.tNobleInfo.sNickName;
                        enterMsg.noblename = noticeMsg.tNobleInfo.sName;
                        enterMsg.level = noticeMsg.tNobleInfo.iLevel;
                        enterMsg.timestamp = TimeStamp.now_ms();

                        lock (locker)
                        {
                            listener?.OnNobleOnline(this, enterMsg);
                        }
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

                case UriType.VipEnterBanner:
                    {
                        logger?.WriteLine("CommandType = MsgPushRequest (7), msg.iUri: {0} - VipEnterBanner", iUri);

                        VipEnterBanner vipMsg = new VipEnterBanner();
                        vipMsg.ReadFrom(stream);

                        VipEnterMessage enterMsg = new VipEnterMessage();
                        enterMsg.uid = vipMsg.tNobelInfo.lUid;
                        enterMsg.imid = vipMsg.tNobelInfo.lPid;
                        enterMsg.nickname = vipMsg.sNickName;
                        enterMsg.noblename = vipMsg.tNobelInfo.sNobleName;
                        enterMsg.level = vipMsg.tNobelInfo.iNobleLevel;
                        enterMsg.timestamp = TimeStamp.now_ms();

                        lock (locker)
                        {
                            listener?.OnVipEnter(this, enterMsg);
                        }
                    }
                    break;

                case UriType.VipBarListResponse:
                    {
                        logger?.WriteLine("CommandType = MsgPushRequest (7), msg.iUri: {0} - VipBarListResponse", iUri);
                    }
                    break;

                case UriType.SendItemSubBroadcastPacket:
                    {
                        logger?.WriteLine("CommandType = MsgPushRequest (7), msg.iUri: {0} - SendItemSubBroadcastPacket", iUri);

                        SendItemSubBroadcastPacket packet = new SendItemSubBroadcastPacket();
                        packet.ReadFrom(stream);

                        if (packet.lPresenterUid == chatInfo.yyuid)
                        {
                            if (giftInfoList.ContainsKey(packet.iItemType))
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
                    }
                    break;

                case UriType.SendItemNoticeWordBroadcastPacket:
                    {
                        logger?.WriteLine("CommandType = MsgPushRequest (7), msg.iUri: {0} - SendItemNoticeWordBroadcastPacket", iUri);

                        SendItemNoticeWordBroadcastPacket packet = new SendItemNoticeWordBroadcastPacket();
                        packet.ReadFrom(stream);

                        if (packet.lPresenterUid == chatInfo.yyuid)
                        {
                            if (giftInfoList.ContainsKey(packet.iItemType))
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
                    }
                    break;

                case UriType.SendItemNoticeGameBroadcastPacket:
                    {
                        logger?.WriteLine("CommandType = MsgPushRequest (7), msg.iUri: {0} - SendItemNoticeGameBroadcastPacket", iUri);

                        SendItemNoticeGameBroadcastPacket packet = new SendItemNoticeGameBroadcastPacket();
                        packet.ReadFrom(stream);

                        if (giftInfoList.ContainsKey(packet.iItemType))
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

                case UriType.TreasureResultBroadcastPacket:
                    {
                        logger?.WriteLine("CommandType = MsgPushRequest (7), msg.iUri: {0} - TreasureResultBroadcastPacket", iUri);

                        TreasureResultBroadcastPacket packet = new TreasureResultBroadcastPacket();
                        packet.ReadFrom(stream);

                        {
                            logger?.WriteLine("packet.lStarterUid = {0}, packet.sStarterNick = {1}, packet.sTreasureName = {2}",
                                              packet.lStarterUid, packet.sStarterNick, packet.sTreasureName);
                        }
                    }
                    break;

                case UriType.BeginLiveNotice:
                    {
                        logger?.WriteLine("CommandType = MsgPushRequest (7), msg.iUri: {0} - BeginLiveNotice", iUri);
                    }
                    break;

                case UriType.EndLiveNotice:
                    {
                        logger?.WriteLine("CommandType = MsgPushRequest (7), msg.iUri: {0} - EndLiveNotice", iUri);
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
