using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Tup;
using Tup.Tars;

namespace HuyaLive
{
    public struct CommandType
    {
        public const int None = 0;
        public const int RegisterRequest = 1;
        public const int RegisterResponse = 2;
        public const int WupRequest = 3;
        public const int WupResponse = 4;
        public const int HeartBeat = 5;
        public const int HeartBeatAck = 6;
        public const int MsgPushRequest = 7;
        public const int UnregisterRequest = 8;
        public const int UnregisterResponse = 9;
        public const int VerifyCookieRequest = 10;
        public const int VerifyCookieResponse = 11;
        public const int VerifyTokenRequest = 12;
        public const int VerifyTokenResponse = 13;
    }

    public class GetPropsListRequest : TarsStruct
    {
        public UserId tUserId = new UserId();
        public string sMd5 = "";
        public int iTemplateType = 64;
        public string sVersion = "";
        public int iAppId = 0;
        public long lPresenterUid = 0;
        public long lSid = 0;
        public long lSubSid = 0;

        public GetPropsListRequest()
        {
        }

        public override void ReadFrom(TarsInputStream _is)
        {
            tUserId = (UserId)_is.Read(tUserId, 0, true);
            sMd5 = (string)_is.Read(sMd5, 1, true);
            iTemplateType = (int)_is.Read(iTemplateType, 2, true);
            sVersion = (string)_is.Read(sVersion, 3, true);
            iAppId = (int)_is.Read(iAppId, 4, true);
            lPresenterUid = (long)_is.Read(lPresenterUid, 5, true);
            lSid = (long)_is.Read(lSid, 6, true);
            lSubSid = (long)_is.Read(lSubSid, 7, true);
        }

        public override void WriteTo(TarsOutputStream _os)
        {
            _os.Write(tUserId, 0);
            _os.Write(sMd5, 1);
            _os.Write(iTemplateType, 2);
            _os.Write(sVersion, 3);
            _os.Write(iAppId, 4);
            _os.Write(lPresenterUid, 5);
            _os.Write(lSid, 6);
            _os.Write(lSubSid, 7);
        }

        public override void Display(StringBuilder sb, int level)
        {
            TarsDisplayer _ds = new TarsDisplayer(sb, level);
            _ds.Display(tUserId, "tUserId");
            _ds.Display(sMd5, "sMd5");
            _ds.Display(iTemplateType, "iTemplateType");
            _ds.Display(sVersion, "sVersion");
            _ds.Display(iAppId, "iAppId");
            _ds.Display(lPresenterUid, "lPresenterUid");
            _ds.Display(lSid, "lSid");
            _ds.Display(lSubSid, "lSubSid");
        }
    }

    public class PropsIdentity : TarsStruct
    {
        public int iPropsIdType = 0;
        public string sPropsPic18 = "";
        public string sPropsPic24 = "";
        public string sPropsPicGif = "";
        public string sPropsBannerResource = "";
        public string sPropsBannerSize = "";
        public string sPropsBannerMaxTime = "";
        public string sPropsChatBannerResource = "";
        public string sPropsChatBannerSize = "";
        public string sPropsChatBannerMaxTime = "";
        public int iPropsChatBannerPos = 0;
        public int iPropsChatBannerIsCombo = 0;
        public string sPropsRollContent = "";
        public int iPropsBannerAnimationstyle = 0;
        public string sPropFaceu = "";
        public string sPropH5Resource = "";
        public string sPropsWeb = "";
        public int iWitch = 0;
        public string sCornerMark = "";
        public int iPropViewId = 0;
        public string sPropStreamerResource = "";
        public short iStreamerFrameRate = 0;
        public string sPropsPic108 = "";
        public string sPcBannerResource = "";

        public PropsIdentity()
        {
            //
        }

        public override void ReadFrom(TarsInputStream _is)
        {
            iPropsIdType = (int)_is.Read(iPropsIdType, 1, true);
            sPropsPic18 = (string)_is.Read(sPropsPic18, 2, true);
            sPropsPic24 = (string)_is.Read(sPropsPic24, 3, true);
            sPropsPicGif = (string)_is.Read(sPropsPicGif, 4, true);
            sPropsBannerResource = (string)_is.Read(sPropsBannerResource, 5, true);
            sPropsBannerSize = (string)_is.Read(sPropsBannerSize, 6, true);
            sPropsBannerMaxTime = (string)_is.Read(sPropsBannerMaxTime, 7, true);
            sPropsChatBannerResource = (string)_is.Read(sPropsChatBannerResource, 8, true);
            sPropsChatBannerSize = (string)_is.Read(sPropsChatBannerSize, 9, true);
            sPropsChatBannerMaxTime = (string)_is.Read(sPropsChatBannerMaxTime, 10, true);
            iPropsChatBannerPos = (int)_is.Read(iPropsChatBannerPos, 11, true);
            iPropsChatBannerIsCombo = (int)_is.Read(iPropsChatBannerIsCombo, 12, true);
            sPropsRollContent = (string)_is.Read(sPropsRollContent, 13, true);
            iPropsBannerAnimationstyle = (int)_is.Read(iPropsBannerAnimationstyle, 14, true);
            sPropFaceu = (string)_is.Read(sPropFaceu, 15, true);
            sPropH5Resource = (string)_is.Read(sPropH5Resource, 16, true);
            sPropsWeb = (string)_is.Read(sPropsWeb, 17, true);
            iWitch = (int)_is.Read(iWitch, 18, true);
            sCornerMark = (string)_is.Read(sCornerMark, 19, true);
            iPropViewId = (int)_is.Read(iPropViewId, 20, true);
            sPropStreamerResource = (string)_is.Read(sPropStreamerResource, 21, true);
            iStreamerFrameRate = (short)_is.Read(iStreamerFrameRate, 22, true);
            sPropsPic108 = (string)_is.Read(sPropsPic108, 23, true);
            sPcBannerResource = (string)_is.Read(sPcBannerResource, 24, true);
        }

        public override void WriteTo(TarsOutputStream _os)
        {
            _os.Write(iPropsIdType, 1);
            _os.Write(sPropsPic18, 2);
            _os.Write(sPropsPic24, 3);
            _os.Write(sPropsPicGif, 4);
            _os.Write(sPropsBannerResource, 5);
            _os.Write(sPropsBannerSize, 6);
            _os.Write(sPropsBannerMaxTime, 7);
            _os.Write(sPropsChatBannerResource, 8);
            _os.Write(sPropsChatBannerSize, 9);
            _os.Write(sPropsChatBannerMaxTime, 10);
            _os.Write(iPropsChatBannerPos, 11);
            _os.Write(iPropsChatBannerIsCombo, 12);
            _os.Write(sPropsRollContent, 13);
            _os.Write(iPropsBannerAnimationstyle, 14);
            _os.Write(sPropFaceu, 15);
            _os.Write(sPropH5Resource, 16);
            _os.Write(sPropsWeb, 17);
            _os.Write(iWitch, 18);
            _os.Write(sCornerMark, 19);
            _os.Write(iPropViewId, 20);
            _os.Write(sPropStreamerResource, 21);
            _os.Write(iStreamerFrameRate, 22);
            _os.Write(sPropsPic108, 23);
            _os.Write(sPcBannerResource, 24);
        }

        public override void Display(StringBuilder sb, int level)
        {
            TarsDisplayer _ds = new TarsDisplayer(sb, level);
            _ds.Display(iPropsIdType, "iPropsIdType");
            _ds.Display(sPropsPic18, "sPropsPic18");
            _ds.Display(sPropsPic24, "sPropsPic24");
            _ds.Display(sPropsPicGif, "sPropsPicGif");
            _ds.Display(sPropsBannerResource, "sPropsBannerResource");
            _ds.Display(sPropsBannerSize, "sPropsBannerSize");
            _ds.Display(sPropsBannerMaxTime, "sPropsBannerMaxTime");
            _ds.Display(sPropsChatBannerResource, "sPropsChatBannerResource");
            _ds.Display(sPropsChatBannerSize, "sPropsChatBannerSize");
            _ds.Display(sPropsChatBannerMaxTime, "sPropsChatBannerMaxTime");
            _ds.Display(iPropsChatBannerPos, "iPropsChatBannerPos");
            _ds.Display(iPropsChatBannerIsCombo, "iPropsChatBannerIsCombo");
            _ds.Display(sPropsRollContent, "sPropsRollContent");
            _ds.Display(iPropsBannerAnimationstyle, "iPropsBannerAnimationstyle");
            _ds.Display(sPropFaceu, "sPropFaceu");
            _ds.Display(sPropH5Resource, "sPropH5Resource");
            _ds.Display(sPropsWeb, "sPropsWeb");
            _ds.Display(iWitch, "iWitch");
            _ds.Display(sCornerMark, "sCornerMark");
            _ds.Display(iPropViewId, "iPropViewId");
            _ds.Display(sPropStreamerResource, "sPropStreamerResource");
            _ds.Display(iStreamerFrameRate, "iStreamerFrameRate");
            _ds.Display(sPropsPic108, "sPropsPic108");
            _ds.Display(sPcBannerResource, "sPcBannerResource");
        }
    }

    public class DisplayInfo
    {
        public DisplayInfo()
        {
            //
        }
    }

    public class SpecialInfo
    {
        public SpecialInfo()
        {
            //
        }
    }

    public class PropView
    {
        public PropView()
        {
            //
        }
    }

    public class PropsItem
    {
        public int iPropsId = 0;
        public string sPropsName = "";
        public int iPropsYb = 0;
        public int iPropsGreenBean = 0;
        public int iPropsWhiteBean = 0;
        public int iPropsGoldenBean = 0;
        public int iPropsRed = 0;
        public int iPropsPopular = 0;
        public int iPropsExpendNum = -1;
        public int iPropsFansValue = -1;
        public List<int> vPropsNum = new List<int>();
        public int iPropsMaxNum = 0;
        public int iPropsBatterFlag = 0;
        public List<int> vPropsChannel = new List<int>();
        public string sPropsToolTip = "";
        public List<PropsIdentity> vPropsIdentity = new List<PropsIdentity>();
        public int iPropsWeights = 0;
        public int iPropsLevel = 0;
        public DisplayInfo tDisplayInfo = new DisplayInfo();
        public SpecialInfo tSpecialInfo = new SpecialInfo();
        public int iPropsGrade = 0;
        public int iPropsGroupNum = 0;
        public string sPropsCommBannerResource = "";
        public string sPropsOwnBannerResource = "";
        public int iPropsShowFlag = 0;
        public int iTemplateType = 0;
        public int iShelfStatus = 0;
        public string sAndroidLogo = "";
        public string sIpadLogo = "";
        public string sIphoneLogo = "";
        public string sPropsCommBannerResourceEx = "";
        public string sPropsOwnBannerResourceEx = "";
        public List<long> vPresenterUid = new List<long>();
        public List<PropView> vPropView = new List<PropView>();
        public int iFaceUSwitch = 0;

        public PropsItem()
        {
            //
        }
    }

    public class GetPropsListResponse : TarsStruct
    {
        public List<PropsItem> vPropsItemList = new List<PropsItem>();
        public string sMd5 = "";
        public short iNewEffectSwitch = 0;
        public short iMirrorRoomShowNum = 0;
        public short iGameRoomShowNum = 0;

        public GetPropsListResponse()
        {
        }

        public override void ReadFrom(TarsInputStream _is)
        {
            vPropsItemList = (List<PropsItem>)_is.Read(vPropsItemList, 1, true);
            sMd5 = (string)_is.Read(sMd5, 2, true);
            iNewEffectSwitch = (short)_is.Read(iNewEffectSwitch, 3, true);
            iMirrorRoomShowNum = (short)_is.Read(iMirrorRoomShowNum, 4, true);
            iGameRoomShowNum = (short)_is.Read(iGameRoomShowNum, 5, true);
        }

        public override void WriteTo(TarsOutputStream _os)
        {
            _os.Write(vPropsItemList, 1);
            _os.Write(sMd5, 2);
            _os.Write(iNewEffectSwitch, 3);
            _os.Write(iMirrorRoomShowNum, 4);
            _os.Write(iGameRoomShowNum, 5);
        }

        public override void Display(StringBuilder sb, int level)
        {
            TarsDisplayer _ds = new TarsDisplayer(sb, level);
            _ds.Display(vPropsItemList, "vPropsItemList");
            _ds.Display(sMd5, "sMd5");
            _ds.Display(iNewEffectSwitch, "iNewEffectSwitch");
            _ds.Display(iMirrorRoomShowNum, "iMirrorRoomShowNum");
            _ds.Display(iGameRoomShowNum, "iGameRoomShowNum");
        }
    }

    public class UserHeartBeatResponse : TarsStruct
    {
        public int iRet = 0;

        public UserHeartBeatResponse()
        {
        }

        public override void ReadFrom(TarsInputStream _is)
        {
            iRet = (int)_is.Read(iRet, 0, true);
        }

        public override void WriteTo(TarsOutputStream _os)
        {
            _os.Write(iRet, 0);
        }

        public override void Display(StringBuilder sb, int level)
        {
            TarsDisplayer _ds = new TarsDisplayer(sb, level);
            _ds.Display(iRet, "iRet");
        }
    }

    public class UserHeartBeatRequest : TarsStruct
    {
        public UserId tId = new UserId();
        public long lTid = 0;
        public long lSid = 0;
        public long lShortTid = 0;
        public long lPid = 0;
        public bool bWatchVideo = false;
        public int iLineType = (int)StreamLineType.OldYY;
        public int iFps = 0;
        public int iAttendee = 0;
        public int iBandWidth = 0;
        public int iLastHeartElapseTime = 0;

        public UserHeartBeatRequest()
        {
        }

        public override void ReadFrom(TarsInputStream _is)
        {
            tId = (UserId)_is.Read(tId, 0, true);
            lTid = (long)_is.Read(lTid, 1, true);
            lSid = (long)_is.Read(lSid, 2, true);
            lShortTid = (long)_is.Read(lShortTid, 3, true);
            lPid = (long)_is.Read(lPid, 4, true);
            bWatchVideo = (bool)_is.Read(bWatchVideo, 5, true);
            iLineType = (int)_is.Read(iLineType, 6, true);
            iFps = (int)_is.Read(iFps, 7, true);
            iAttendee = (int)_is.Read(iAttendee, 8, true);
            iBandWidth = (int)_is.Read(iBandWidth, 9, true);
            iLastHeartElapseTime = (int)_is.Read(iLastHeartElapseTime, 10, true);
        }

        public override void WriteTo(TarsOutputStream _os)
        {
            _os.Write(tId, 0);
            _os.Write(lTid, 1);
            _os.Write(lSid, 2);
            _os.Write(lShortTid, 3);
            _os.Write(lPid, 4);
            _os.Write(bWatchVideo, 5);
            _os.Write(iLineType, 6);
            _os.Write(iFps, 7);
            _os.Write(iAttendee, 8);
            _os.Write(iBandWidth, 9);
            _os.Write(iLastHeartElapseTime, 10);
        }

        public override void Display(StringBuilder sb, int level)
        {
            TarsDisplayer _ds = new TarsDisplayer(sb, level);
            _ds.Display(tId, "tId");
            _ds.Display(lTid, "lTid");
            _ds.Display(lSid, "lSid");
            _ds.Display(lShortTid, "lShortTid");
            _ds.Display(lPid, "lPid");
            _ds.Display(bWatchVideo, "bWatchVideo");
            _ds.Display(iLineType, "iLineType");
            _ds.Display(iFps, "iFps");
            _ds.Display(iAttendee, "iAttendee");
            _ds.Display(iBandWidth, "iBandWidth");
            _ds.Display(iLastHeartElapseTime, "iLastHeartElapseTime");
        }
    }

    public class WebSocketCommand : TarsStruct
    {
        public int iCmdType = 0;
        public byte[] vData = null;

        public WebSocketCommand()
        {
        }

        /*
        static public void Read(TarsInputStream _is, ref WebSocketCommand command,
                                int tag, bool require = true)
        {
            command = (WebSocketCommand)_is.Read(command, tag, require);
        }

        static public void Write(TarsOutputStream _os, WebSocketCommand command, int tag)
        {
            _os.Write(command, tag);
        }
        //*/

        public override void ReadFrom(TarsInputStream _is)
        {
            iCmdType = (int)_is.Read(iCmdType, 0, true);
            vData = (byte[])_is.Read(vData, 1, true);
        }

        public override void WriteTo(TarsOutputStream _os)
        {
            _os.Write(iCmdType, 0);
            _os.Write(vData, 1);
        }

        public override void Display(StringBuilder sb, int level)
        {
            TarsDisplayer _ds = new TarsDisplayer(sb, level);
            _ds.Display(iCmdType, "iCmdType");
            _ds.Display(vData, "vData");
        }
    }

    public class LiveProxyValue : TarsStruct
    {
        public int iProxyType = 0;
        public List<string> sProxy = new List<string>();

        public LiveProxyValue()
        {
        }

        public override void ReadFrom(TarsInputStream _is)
        {
            iProxyType = (int)_is.Read(iProxyType, 0, true);
            sProxy = (List<string>)_is.Read(sProxy, 1, true);
        }

        public override void WriteTo(TarsOutputStream _os)
        {
            _os.Write(iProxyType, 0);
            _os.Write(sProxy, 1);
        }

        public override void Display(StringBuilder sb, int level)
        {
            TarsDisplayer _ds = new TarsDisplayer(sb, level);
            _ds.Display(iProxyType, "iProxyType");
            _ds.Display(sProxy, "sProxy");
        }
    }

    public class LiveLaunchResponse : TarsStruct
    {
        public string sGuid = "";
        public int iTime = 0;
        public List<LiveProxyValue> vProxyList = new List<LiveProxyValue>();
        public int iAccess = 0;

        public LiveLaunchResponse()
        {
        }

        public override void ReadFrom(TarsInputStream _is)
        {
            sGuid = (string)_is.Read(sGuid, 0, true);
            iTime = (int)_is.Read(iTime, 1, true);
            vProxyList = (List<LiveProxyValue>)_is.Read(vProxyList, 2, true);
            iAccess = (int)_is.Read(iAccess, 3, true);
        }

        public override void WriteTo(TarsOutputStream _os)
        {
            _os.Write(sGuid, 0);
            _os.Write(iTime, 1);
            _os.Write(vProxyList, 2);
            _os.Write(iAccess, 3);
        }

        public override void Display(StringBuilder sb, int level)
        {
            TarsDisplayer _ds = new TarsDisplayer(sb, level);
            _ds.Display(sGuid, "sGuid");
            _ds.Display(iTime, "iTime");
            _ds.Display(vProxyList, "vProxyList");
            _ds.Display(iAccess, "iAccess");
        }
    }

    public class NobleSpeakResponse : TarsStruct
    {
        public int iId = 0;

        public NobleSpeakResponse()
        {
        }

        public override void ReadFrom(TarsInputStream _is)
        {
            iId = (int)_is.Read(iId, 0, true);
        }

        public override void WriteTo(TarsOutputStream _os)
        {
            _os.Write(iId, 0);
        }

        public override void Display(StringBuilder sb, int level)
        {
            TarsDisplayer _ds = new TarsDisplayer(sb, level);
            _ds.Display(iId, "iId");
        }
    }

    public class GetLivingInfoResponse : TarsStruct
    {
        public int iId = 0;

        public GetLivingInfoResponse()
        {
        }

        public override void ReadFrom(TarsInputStream _is)
        {
            iId = (int)_is.Read(iId, 0, true);
        }

        public override void WriteTo(TarsOutputStream _os)
        {
            _os.Write(iId, 0);
        }

        public override void Display(StringBuilder sb, int level)
        {
            TarsDisplayer _ds = new TarsDisplayer(sb, level);
            _ds.Display(iId, "iId");
        }
    }

    public class WSPushMessage : TarsStruct
    {
        public int iPushType = 0;
        public int iUri = 0;
        public byte[] sMsg = new byte[0];
        public int iProtocolType = 0;

        public WSPushMessage()
        {
        }

        public override void ReadFrom(TarsInputStream _is)
        {
            iPushType = (int)_is.Read(iPushType, 0, true);
            iUri = (int)_is.Read(iUri, 1, true);
            sMsg = (byte[])_is.Read(sMsg, 2, true);
            iProtocolType = (int)_is.Read(iProtocolType, 3, true);
        }

        public override void WriteTo(TarsOutputStream _os)
        {
            _os.Write(iPushType, 0);
            _os.Write(iUri, 1);
            _os.Write(sMsg, 2);
            _os.Write(iProtocolType, 3);
        }

        public override void Display(StringBuilder sb, int level)
        {
            TarsDisplayer _ds = new TarsDisplayer(sb, level);
            _ds.Display(iPushType, "iPushType");
            _ds.Display(iUri, "iUri");
            _ds.Display(sMsg, "sMsg");
            _ds.Display(iProtocolType, "iProtocolType");
        }
    }   
}
