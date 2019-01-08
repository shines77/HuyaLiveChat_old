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
        // Base commands
        public const int None = 0;
        public const int RegisterRequest = 1;
        public const int RegisterResponse = 2;
        public const int WupRequest = 3;
        public const int WupResponse = 4;
        public const int HeartBeat = 5;
        public const int HeartBeatAck = 6;
        public const int MsgPushRequest = 7;
        public const int UnRegisterRequest = 8;
        public const int UnRegisterResponse = 9;
        public const int VerifyCookieRequest = 10;
        public const int VerifyCookieResponse = 11;
        public const int VerifyTokenRequest = 12;
        public const int VerifyTokenResponse = 13;
        // Extended commands
        public const int UNVerifyRequest = 14;
        public const int UNVerifyResponse = 15;
        public const int RegisterGroupRequest = 16;
        public const int RegisterGroupResponse = 17;
        public const int UnRegisterGroupRequest = 18;
        public const int UnRegisterGroupResponse = 19;
        public const int HeartBeatRequest = 20;
        public const int HeartBeatResponse = 21;
        public const int MsgPushRequest_V2 = 22;
    }

    public struct UriType
    {
        public const int NobleNotice = 1001;
        public const int NobleSpeakBrust = 1002;
        public const int NobleEnterNotice = 1005;
        public const int MessageNotice = 1400;

        public const int VipEnterBanner = 6110;

        public const int EnterPushInfo = 6200;
        /*
            var i = new HUYA.GetGameAdReq;
            i.tUserId = e.userId,
            i.lTid = u.id,
            i.lSid = u.sid,
            i.lPid = o.lp,
            t.sendWup2("liveui", "getCurrentGameAd", i);
        */
        public const int GameAdNotice = 6201;
        public const int AdvanceUserEnterNotice = 6202;
        public const int ViewerListResponse = 6203;

        public const int VipBarListResponse = 6210;
        public const int WeekRankListResponse = 6220;
        public const int WeekRankEnterBanner = 6221;
        public const int FansSupportListResponse = 6223;
        public const int FansRankListResponse = 6230;

        public const int BadgeInfo = 6231;

        // FansBadgeUpNotice
        public const int BadgeScoreChanged = 6232;

        /*
            var t = new HUYA.BadgeInfoListReq;
            t.tUserId = e.userId,
            i.sendWup2("liveui", "queryBadgeInfoList", t, a);
        */

        /*
            var n = new HUYA.UseBadgeReq;
            n.tUserId = d.userId,
            n.lBadgeId = a,
            n.iBadgeType = e,
            i.sendWup2("liveui", "useBadgeV2", n, t);
        */

        /*
            var d = new HUYA.BadgeItemReq;
            d.tUserId = t.userId,
            d.lPid = a,
            t.sendWup2("liveui", "getBadgeItem", d, e);
        */

        public const int FansInfoNotice = 6233;
        public const int UserGiftNotice = 6234;
        public const int WeekStarPropsIds = 6235;

        public const int GiftBarResponse = 6250;

        public const int SendItemSubBroadcastPacket = 6501;
        public const int SendItemNoticeWordBroadcastPacket = 6502;
        public const int SendItemNoticeGameBroadcastPacket = 6507;

        public const int TreasureResultBroadcastPacket = 6602;

        public const int ShowRaffleWinnerNotice = 7054;
        public const int MatchRaffleResultNotice = 7055;

        public const int BeginLiveNotice = 8000;
        // LiveStatusChange
        public const int EndLiveNotice = 8001;
        public const int StreamSettingNotice = 8002;
        public const int LiveInfoChangedNotice = 8004;
        // WebTotalCount
        public const int AttendeeCountNotice = 8006;
        public const int AttendeeCountNoticeResponse = 8007;

        public const int ReplayPresenterInLiveNotify = 9010;

        // i.sendWup2("liveui", "getAuditorRole", n, r);
        // OnAuditorEnter
        public const int AuditorEnterLiveNotice = 10040;
        // OnAuditorRoleChange
        public const int AuditorRoleChangeNotice = 10041;
        public const int GetRoomAuditConfResponse = 10042;

        public const int LinkMicStatusChangeNotice = 42008;
        public const int InterveneCountRsp = 44000;

        // getUserLevelUpgrade
        public const int UserLevelUpgradeNotice = 1000106;
        // userLevelTaskComplete
        public const int UserNovieTaskCompleteNotice = 1000107;
        public const int GuardianPresenterInfoNotice = 1020001;

        // SafetyVerification
        public const int UDB_UnionAuthPushMsg = 10220054;

        /*
         *   var n = new HUYA.GetRctTimedMessageReq;
         *   n.tUserId = i.userId,
         *   n.lPid = s.lp,
         *   n.lTid = r.id,
         *   n.lSid = r.sid,
         *   i.sendWup2("mobileui", "getRctTimedMessage", n, r);
         */

        /*
         *   var t = new HUYA.SendMessageReq;
         *   t.tUserId = n.userId,
         *   t.lTid = h,
         *   t.lSid = v,
         *   t.lPid = o.lp,
         *   t.sContent = e,
         *   t.tBulletFormat = $.extend(t.tBulletFormat || {}, y),
         *   n.sendWup("liveui", "sendMessage", t);
         */
    }

    public class WebSocketCommand : TarsStruct
    {
        public int iCmdType = 0;
        public byte[] vData = new byte[0];
        public long lRequestId = 0;
        public string sTraceId = "";

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
            iCmdType = (int)_is.Read(iCmdType, 0, false);
            vData = (byte[])_is.Read(vData, 1, false);
            lRequestId = (long)_is.Read(lRequestId, 2, false);
            sTraceId = (string)_is.Read(sTraceId, 3, false);
        }

        public override void WriteTo(TarsOutputStream _os)
        {
            _os.Write(iCmdType, 0);
            _os.Write(vData, 1);
            _os.Write(lRequestId, 2);
            _os.Write(sTraceId, 3);
        }

        public override void Display(StringBuilder sb, int level)
        {
            TarsDisplayer _ds = new TarsDisplayer(sb, level);
            _ds.Display(iCmdType, "iCmdType");
            _ds.Display(vData, "vData");
            _ds.Display(lRequestId, "lRequestId");
            _ds.Display(sTraceId, "sTraceId");
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
        }

        public override void ReadFrom(TarsInputStream _is)
        {
            iPropsIdType = (int)_is.Read(iPropsIdType, 1, false);
            sPropsPic18 = (string)_is.Read(sPropsPic18, 2, false);
            sPropsPic24 = (string)_is.Read(sPropsPic24, 3, false);
            sPropsPicGif = (string)_is.Read(sPropsPicGif, 4, false);
            sPropsBannerResource = (string)_is.Read(sPropsBannerResource, 5, false);
            sPropsBannerSize = (string)_is.Read(sPropsBannerSize, 6, false);
            sPropsBannerMaxTime = (string)_is.Read(sPropsBannerMaxTime, 7, false);
            sPropsChatBannerResource = (string)_is.Read(sPropsChatBannerResource, 8, false);
            sPropsChatBannerSize = (string)_is.Read(sPropsChatBannerSize, 9, false);
            sPropsChatBannerMaxTime = (string)_is.Read(sPropsChatBannerMaxTime, 10, false);
            iPropsChatBannerPos = (int)_is.Read(iPropsChatBannerPos, 11, false);
            iPropsChatBannerIsCombo = (int)_is.Read(iPropsChatBannerIsCombo, 12, false);
            sPropsRollContent = (string)_is.Read(sPropsRollContent, 13, false);
            iPropsBannerAnimationstyle = (int)_is.Read(iPropsBannerAnimationstyle, 14, false);
            sPropFaceu = (string)_is.Read(sPropFaceu, 15, false);
            sPropH5Resource = (string)_is.Read(sPropH5Resource, 16, false);
            sPropsWeb = (string)_is.Read(sPropsWeb, 17, false);
            iWitch = (int)_is.Read(iWitch, 18, false);
            sCornerMark = (string)_is.Read(sCornerMark, 19, false);
            iPropViewId = (int)_is.Read(iPropViewId, 20, false);
            sPropStreamerResource = (string)_is.Read(sPropStreamerResource, 21, false);
            iStreamerFrameRate = (short)_is.Read(iStreamerFrameRate, 22, false);
            sPropsPic108 = (string)_is.Read(sPropsPic108, 23, false);
            sPcBannerResource = (string)_is.Read(sPcBannerResource, 24, false);
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

    public class DisplayInfo : TarsStruct
    {
        public int iMarqueeScopeMin = 0;
        public int iMarqueeScopeMax = 0;
        public int iCurrentVideoNum = 0;
        public int iCurrentVideoMin = 0;
        public int iCurrentVideoMax = 0;
        public int iAllVideoNum = 0;
        public int iAllVideoMin = 0;
        public int iAllVideoMax = 0;
        public int iCurrentScreenNum = 0;
        public int iCurrentScreenMin = 0;
        public int iCurrentScreenMax = 0;

        public DisplayInfo()
        {
        }

        public override void ReadFrom(TarsInputStream _is)
        {
            iMarqueeScopeMin = (int)_is.Read(iMarqueeScopeMin, 1, false);
            iMarqueeScopeMax = (int)_is.Read(iMarqueeScopeMax, 2, false);
            iCurrentVideoNum = (int)_is.Read(iCurrentVideoNum, 3, false);
            iCurrentVideoMin = (int)_is.Read(iCurrentVideoMin, 4, false);
            iCurrentVideoMax = (int)_is.Read(iCurrentVideoMax, 5, false);
            iAllVideoNum = (int)_is.Read(iAllVideoNum, 6, false);
            iAllVideoMin = (int)_is.Read(iAllVideoMin, 7, false);
            iAllVideoMax = (int)_is.Read(iAllVideoMax, 8, false);
            iCurrentScreenNum = (int)_is.Read(iCurrentScreenNum, 9, false);
            iCurrentScreenMin = (int)_is.Read(iCurrentScreenMin, 10, false);
            iCurrentScreenMax = (int)_is.Read(iCurrentScreenMax, 11, false);
        }

        public override void WriteTo(TarsOutputStream _os)
        {
            _os.Write(iMarqueeScopeMin, 1);
            _os.Write(iMarqueeScopeMax, 2);
            _os.Write(iCurrentVideoNum, 3);
            _os.Write(iCurrentVideoMin, 4);
            _os.Write(iCurrentVideoMax, 5);
            _os.Write(iAllVideoNum, 6);
            _os.Write(iAllVideoMin, 7);
            _os.Write(iAllVideoMax, 8);
            _os.Write(iCurrentScreenNum, 9);
            _os.Write(iCurrentScreenMin, 10);
            _os.Write(iCurrentScreenMax, 11);
        }

        public override void Display(StringBuilder sb, int level)
        {
            TarsDisplayer _ds = new TarsDisplayer(sb, level);
            _ds.Display(iMarqueeScopeMin, "iMarqueeScopeMin");
            _ds.Display(iMarqueeScopeMax, "iMarqueeScopeMax");
            _ds.Display(iCurrentVideoNum, "iCurrentVideoNum");
            _ds.Display(iCurrentVideoMin, "iCurrentVideoMin");
            _ds.Display(iCurrentVideoMax, "iCurrentVideoMax");
            _ds.Display(iAllVideoNum, "iAllVideoNum");
            _ds.Display(iAllVideoMin, "iAllVideoMin");
            _ds.Display(iAllVideoMax, "iAllVideoMax");
            _ds.Display(iCurrentScreenNum, "iCurrentScreenNum");
            _ds.Display(iCurrentScreenMin, "iCurrentScreenMin");
            _ds.Display(iCurrentScreenMax, "iCurrentScreenMax");
        }
    }

    public class SpecialInfo : TarsStruct
    {
        public int iFirstSingle = 0;
        public int iFirstGroup = 0;
        public string sFirstTips = "";
        public int iSecondSingle = 0;
        public int iSecondGroup = 0;
        public string sSecondTips = "";
        public int iThirdSingle = 0;
        public int iThirdGroup = 0;
        public string sThirdTips = "";
        public int iWorldSingle = 0;
        public int iWorldGroup = 0;

        public SpecialInfo()
        {
        }

        public override void ReadFrom(TarsInputStream _is)
        {
            iFirstSingle = (int)_is.Read(iFirstSingle, 1, false);
            iFirstGroup = (int)_is.Read(iFirstGroup, 2, false);
            sFirstTips = (string)_is.Read(sFirstTips, 3, false);
            iSecondSingle = (int)_is.Read(iSecondSingle, 4, false);
            iSecondGroup = (int)_is.Read(iSecondGroup, 5, false);
            sSecondTips = (string)_is.Read(sSecondTips, 6, false);
            iThirdSingle = (int)_is.Read(iThirdSingle, 7, false);
            iThirdGroup = (int)_is.Read(iThirdGroup, 8, false);
            sThirdTips = (string)_is.Read(sThirdTips, 9, false);
            iWorldSingle = (int)_is.Read(iWorldSingle, 10, false);
            iWorldGroup = (int)_is.Read(iWorldGroup, 11, false);
        }

        public override void WriteTo(TarsOutputStream _os)
        {
            _os.Write(iFirstSingle, 1);
            _os.Write(iFirstGroup, 2);
            _os.Write(sFirstTips, 3);
            _os.Write(iSecondSingle, 4);
            _os.Write(iSecondGroup, 5);
            _os.Write(sSecondTips, 6);
            _os.Write(iThirdSingle, 7);
            _os.Write(iThirdGroup, 8);
            _os.Write(sThirdTips, 9);
            _os.Write(iWorldSingle, 10);
            _os.Write(iWorldGroup, 11);
        }

        public override void Display(StringBuilder sb, int level)
        {
            TarsDisplayer _ds = new TarsDisplayer(sb, level);
            _ds.Display(iFirstSingle, "iFirstSingle");
            _ds.Display(iFirstGroup, "iFirstGroup");
            _ds.Display(sFirstTips, "sFirstTips");
            _ds.Display(iSecondSingle, "iSecondSingle");
            _ds.Display(iSecondGroup, "iSecondGroup");
            _ds.Display(sSecondTips, "sSecondTips");
            _ds.Display(iThirdSingle, "iThirdSingle");
            _ds.Display(iThirdGroup, "iThirdGroup");
            _ds.Display(sThirdTips, "sThirdTips");
            _ds.Display(iWorldSingle, "iWorldSingle");
            _ds.Display(iWorldGroup, "iWorldGroup");
        }
    }

    public class PropView : TarsStruct
    {
        public int id = 0;
        public string name = "";
        public Dictionary<long, short> uids = new Dictionary<long, short>();
        public string tips = "";

        public PropView()
        {
        }

        public override void ReadFrom(TarsInputStream _is)
        {
            id = (int)_is.Read(id, 0, false);
            name = (string)_is.Read(name, 1, false);
            uids = (Dictionary<long, short>)_is.Read(uids, 2, false);
            tips = (string)_is.Read(tips, 3, false);
        }

        public override void WriteTo(TarsOutputStream _os)
        {
            _os.Write(id, 0);
            _os.Write(name, 1);
            _os.Write(uids, 2);
            _os.Write(tips, 3);
        }

        public override void Display(StringBuilder sb, int level)
        {
            TarsDisplayer _ds = new TarsDisplayer(sb, level);
            _ds.Display(id, "id");
            _ds.Display(name, "name");
            _ds.Display(uids, "uids");
            _ds.Display(tips, "tips");
        }
    }

    public class PropsItem : TarsStruct
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
        }

        public override void ReadFrom(TarsInputStream _is)
        {
            iPropsId = (int)_is.Read(iPropsId, 1, false);
            sPropsName = (string)_is.Read(sPropsName, 2, false);
            iPropsYb = (int)_is.Read(iPropsYb, 3, false);
            iPropsGreenBean = (int)_is.Read(iPropsGreenBean, 4, false);
            iPropsWhiteBean = (int)_is.Read(iPropsWhiteBean, 5, false);
            iPropsGoldenBean = (int)_is.Read(iPropsGoldenBean, 6, false);
            iPropsRed = (int)_is.Read(iPropsRed, 7, false);
            iPropsPopular = (int)_is.Read(iPropsPopular, 8, false);
            iPropsExpendNum = (int)_is.Read(iPropsExpendNum, 9, false);
            iPropsFansValue = (int)_is.Read(iPropsFansValue, 10, false);

            vPropsNum = (List<int>)_is.Read(vPropsNum, 11, false);
            iPropsMaxNum = (int)_is.Read(iPropsMaxNum, 12, false);
            iPropsBatterFlag = (int)_is.Read(iPropsBatterFlag, 13, false);
            vPropsChannel = (List<int>)_is.Read(vPropsChannel, 14, false);
            sPropsToolTip = (string)_is.Read(sPropsToolTip, 15, false);
            vPropsIdentity = (List<PropsIdentity>)_is.Read(vPropsIdentity, 16, false);
            iPropsWeights = (int)_is.Read(iPropsWeights, 17, false);
            iPropsLevel = (int)_is.Read(iPropsLevel, 18, false);
            tDisplayInfo = (DisplayInfo)_is.Read(tDisplayInfo, 19, false);
            tSpecialInfo = (SpecialInfo)_is.Read(tSpecialInfo, 20, false);

            iPropsGrade = (int)_is.Read(iPropsGrade, 21, false);
            iPropsGroupNum = (int)_is.Read(iPropsGroupNum, 22, false);
            sPropsCommBannerResource = (string)_is.Read(sPropsCommBannerResource, 23, false);
            sPropsOwnBannerResource = (string)_is.Read(sPropsOwnBannerResource, 24, false);
            iPropsShowFlag = (int)_is.Read(iPropsShowFlag, 25, false);
            iTemplateType = (int)_is.Read(iTemplateType, 26, false);
            iShelfStatus = (int)_is.Read(iShelfStatus, 27, false);
            sAndroidLogo = (string)_is.Read(sAndroidLogo, 28, false);
            sIpadLogo = (string)_is.Read(sIpadLogo, 29, false);
            sIphoneLogo = (string)_is.Read(sIphoneLogo, 30, false);

            sPropsCommBannerResourceEx = (string)_is.Read(sPropsCommBannerResourceEx, 31, false);
            sPropsOwnBannerResourceEx = (string)_is.Read(sPropsOwnBannerResourceEx, 32, false);
            vPresenterUid = (List<long>)_is.Read(vPresenterUid, 33, false);
            vPropView = (List<PropView>)_is.Read(vPropView, 34, false);
            iFaceUSwitch = (int)_is.Read(iFaceUSwitch, 35, false);
        }

        public override void WriteTo(TarsOutputStream _os)
        {
            _os.Write(iPropsId, 1);
            _os.Write(sPropsName, 2);
            _os.Write(iPropsYb, 3);
            _os.Write(iPropsGreenBean, 4);
            _os.Write(iPropsWhiteBean, 5);
            _os.Write(iPropsGoldenBean, 6);
            _os.Write(iPropsRed, 7);
            _os.Write(iPropsPopular, 8);
            _os.Write(iPropsExpendNum, 9);
            _os.Write(iPropsFansValue, 10);

            _os.Write(vPropsNum, 11);
            _os.Write(iPropsMaxNum, 12);
            _os.Write(iPropsBatterFlag, 13);
            _os.Write(vPropsChannel, 14);
            _os.Write(sPropsToolTip, 15);
            _os.Write(vPropsIdentity, 16);
            _os.Write(iPropsWeights, 17);
            _os.Write(iPropsLevel, 18);
            _os.Write(tDisplayInfo, 19);
            _os.Write(tSpecialInfo, 20);

            _os.Write(iPropsGrade, 21);
            _os.Write(iPropsGroupNum, 22);
            _os.Write(sPropsCommBannerResource, 23);
            _os.Write(sPropsOwnBannerResource, 24);
            _os.Write(iPropsShowFlag, 25);
            _os.Write(iTemplateType, 26);
            _os.Write(iShelfStatus, 27);
            _os.Write(sAndroidLogo, 28);
            _os.Write(sIpadLogo, 29);
            _os.Write(sIphoneLogo, 30);

            _os.Write(sPropsCommBannerResourceEx, 31);
            _os.Write(sPropsOwnBannerResourceEx, 32);
            _os.Write(vPresenterUid, 33);
            _os.Write(vPropView, 34);
            _os.Write(iFaceUSwitch, 35);
        }

        public override void Display(StringBuilder sb, int level)
        {
            TarsDisplayer _ds = new TarsDisplayer(sb, level);
            _ds.Display(iPropsId, "iPropsId");
            _ds.Display(sPropsName, "sPropsName");
            _ds.Display(iPropsYb, "iPropsYb");
            _ds.Display(iPropsGreenBean, "iPropsGreenBean");
            _ds.Display(iPropsWhiteBean, "iPropsWhiteBean");
            _ds.Display(iPropsGoldenBean, "iPropsGoldenBean");
            _ds.Display(iPropsRed, "iPropsRed");
            _ds.Display(iPropsPopular, "iPropsPopular");
            _ds.Display(iPropsExpendNum, "iPropsExpendNum");
            _ds.Display(iPropsFansValue, "iPropsFansValue");

            _ds.Display(vPropsNum, "vPropsNum");
            _ds.Display(iPropsMaxNum, "iPropsMaxNum");
            _ds.Display(iPropsBatterFlag, "iPropsBatterFlag");
            _ds.Display(vPropsChannel, "vPropsChannel");
            _ds.Display(sPropsToolTip, "sPropsToolTip");
            _ds.Display(vPropsIdentity, "vPropsIdentity");
            _ds.Display(iPropsWeights, "iPropsWeights");
            _ds.Display(iPropsLevel, "iPropsLevel");
            _ds.Display(tDisplayInfo, "tDisplayInfo");
            _ds.Display(tSpecialInfo, "tSpecialInfo");

            _ds.Display(iPropsGrade, "iPropsGrade");
            _ds.Display(iPropsGroupNum, "iPropsGroupNum");
            _ds.Display(sPropsCommBannerResource, "sPropsCommBannerResource");
            _ds.Display(sPropsOwnBannerResource, "sPropsOwnBannerResource");
            _ds.Display(iPropsShowFlag, "iPropsShowFlag");
            _ds.Display(iTemplateType, "iTemplateType");
            _ds.Display(iShelfStatus, "iShelfStatus");
            _ds.Display(sAndroidLogo, "sAndroidLogo");
            _ds.Display(sIpadLogo, "sIpadLogo");
            _ds.Display(sIphoneLogo, "sIphoneLogo");

            _ds.Display(sPropsCommBannerResourceEx, "sPropsCommBannerResourceEx");
            _ds.Display(sPropsOwnBannerResourceEx, "sPropsOwnBannerResourceEx");
            _ds.Display(vPresenterUid, "vPresenterUid");
            _ds.Display(vPropView, "vPropView");
            _ds.Display(iFaceUSwitch, "iFaceUSwitch");
        }
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
            tUserId = (UserId)_is.Read(tUserId, 1, false);
            sMd5 = (string)_is.Read(sMd5, 2, false);
            iTemplateType = (int)_is.Read(iTemplateType, 3, false);
            sVersion = (string)_is.Read(sVersion, 4, false);
            iAppId = (int)_is.Read(iAppId, 5, false);
            lPresenterUid = (long)_is.Read(lPresenterUid, 6, false);
            lSid = (long)_is.Read(lSid, 7, false);
            lSubSid = (long)_is.Read(lSubSid, 8, false);
        }

        public override void WriteTo(TarsOutputStream _os)
        {
            _os.Write(tUserId, 1);
            _os.Write(sMd5, 2);
            _os.Write(iTemplateType, 3);
            _os.Write(sVersion, 4);
            _os.Write(iAppId, 5);
            _os.Write(lPresenterUid, 6);
            _os.Write(lSid, 7);
            _os.Write(lSubSid, 8);
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
            vPropsItemList = (List<PropsItem>)_is.Read(vPropsItemList, 1, false);
            sMd5 = (string)_is.Read(sMd5, 2, false);
            iNewEffectSwitch = (short)_is.Read(iNewEffectSwitch, 3, false);
            iMirrorRoomShowNum = (short)_is.Read(iMirrorRoomShowNum, 4, false);
            iGameRoomShowNum = (short)_is.Read(iGameRoomShowNum, 5, false);
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

    public class HeartBeatResponse : TarsStruct
    {
        public int iState = 0;

        public HeartBeatResponse()
        {
        }

        public override void ReadFrom(TarsInputStream _is)
        {
            iState = (int)_is.Read(iState, 0, false);
        }

        public override void WriteTo(TarsOutputStream _os)
        {
            _os.Write(iState, 0);
        }

        public override void Display(StringBuilder sb, int level)
        {
            TarsDisplayer _ds = new TarsDisplayer(sb, level);
            _ds.Display(iState, "iState");
        }
    }

    public class UserEventRequest : TarsStruct
    {
        public UserId tId = new UserId();
        public long lTid = 0;
        public long lSid = 0;
        public long lShortTid = 0;
        public int eOp = 0;
        public string sChan = "";
        public int eSource = 0;
        public long lPid = 0;
        public bool bWatchVideo = false;
        public bool bAnonymous = false;
        public int eTemplateType = (int)TemplateType.Primary;

        public UserEventRequest()
        {
        }

        public override void ReadFrom(TarsInputStream _is)
        {
            tId = (UserId)_is.Read(tId, 0, false);
            lTid = (long)_is.Read(lTid, 1, false);
            lSid = (long)_is.Read(lSid, 2, false);
            lShortTid = (long)_is.Read(lShortTid, 3, false);
            eOp = (int)_is.Read(eOp, 4, false);
            sChan = (string)_is.Read(sChan, 5, false);
            eSource = (int)_is.Read(eSource, 6, false);
            lPid = (long)_is.Read(lPid, 7, false);
            bWatchVideo = (bool)_is.Read(bWatchVideo, 8, false);
            bAnonymous = (bool)_is.Read(bAnonymous, 9, false);
            eTemplateType = (int)_is.Read(eTemplateType, 10, false);
        }

        public override void WriteTo(TarsOutputStream _os)
        {
            _os.Write(tId, 0);
            _os.Write(lTid, 1);
            _os.Write(lSid, 2);
            _os.Write(lShortTid, 3);
            _os.Write(eOp, 4);
            _os.Write(sChan, 5);
            _os.Write(eSource, 6);
            _os.Write(lPid, 7);
            _os.Write(bWatchVideo, 8);
            _os.Write(bAnonymous, 9);
            _os.Write(eTemplateType, 10);
        }

        public override void Display(StringBuilder sb, int level)
        {
            TarsDisplayer _ds = new TarsDisplayer(sb, level);
            _ds.Display(tId, "tId");
            _ds.Display(lTid, "lTid");
            _ds.Display(lSid, "lSid");
            _ds.Display(lShortTid, "lShortTid");
            _ds.Display(eOp, "eOp");
            _ds.Display(sChan, "sChan");
            _ds.Display(eSource, "eSource");
            _ds.Display(lPid, "lPid");
            _ds.Display(bWatchVideo, "bWatchVideo");
            _ds.Display(bAnonymous, "bAnonymous");
            _ds.Display(eTemplateType, "eTemplateType");
        }
    }

    public class UserEventResponse: TarsStruct
    {
        public long lTid = 0;
        public long lSid = 0;
        public int iUserHeartBeatInterval = 60;
        public int iPresentHeartBeatInterval = 60;

        public UserEventResponse()
        {
        }

        public override void ReadFrom(TarsInputStream _is)
        {
            lTid = (long)_is.Read(lTid, 0, false);
            lSid = (long)_is.Read(lSid, 1, false);
            iUserHeartBeatInterval = (int)_is.Read(iUserHeartBeatInterval, 2, false);
            iPresentHeartBeatInterval = (int)_is.Read(iPresentHeartBeatInterval, 3, false);
        }

        public override void WriteTo(TarsOutputStream _os)
        {
            _os.Write(lTid, 0);
            _os.Write(lSid, 1);
            _os.Write(iUserHeartBeatInterval, 2);
            _os.Write(iPresentHeartBeatInterval, 3);
        }

        public override void Display(StringBuilder sb, int level)
        {
            TarsDisplayer _ds = new TarsDisplayer(sb, level);
            _ds.Display(lTid, "lTid");
            _ds.Display(lSid, "lSid");
            _ds.Display(iUserHeartBeatInterval, "iUserHeartBeatInterval");
            _ds.Display(iPresentHeartBeatInterval, "iPresentHeartBeatInterval");
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
            tId = (UserId)_is.Read(tId, 0, false);
            lTid = (long)_is.Read(lTid, 1, false);
            lSid = (long)_is.Read(lSid, 2, false);
            lShortTid = (long)_is.Read(lShortTid, 3, false);
            lPid = (long)_is.Read(lPid, 4, false);
            bWatchVideo = (bool)_is.Read(bWatchVideo, 5, false);
            iLineType = (int)_is.Read(iLineType, 6, false);
            iFps = (int)_is.Read(iFps, 7, false);
            iAttendee = (int)_is.Read(iAttendee, 8, false);
            iBandWidth = (int)_is.Read(iBandWidth, 9, false);
            iLastHeartElapseTime = (int)_is.Read(iLastHeartElapseTime, 10, false);
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

    public class UserHeartBeatResponse : TarsStruct
    {
        public int iRet = 0;

        public UserHeartBeatResponse()
        {
        }

        public override void ReadFrom(TarsInputStream _is)
        {
            iRet = (int)_is.Read(iRet, 0, false);
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

    public class LiveProxyValue : TarsStruct
    {
        public int iProxyType = 0;
        public List<string> sProxy = new List<string>();

        public LiveProxyValue()
        {
        }

        public override void ReadFrom(TarsInputStream _is)
        {
            iProxyType = (int)_is.Read(iProxyType, 0, false);
            sProxy = (List<string>)_is.Read(sProxy, 1, false);
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
            sGuid = (string)_is.Read(sGuid, 0, false);
            iTime = (int)_is.Read(iTime, 1, false);
            vProxyList = (List<LiveProxyValue>)_is.Read(vProxyList, 2, false);
            iAccess = (int)_is.Read(iAccess, 3, false);
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
            iId = (int)_is.Read(iId, 0, false);
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
            iId = (int)_is.Read(iId, 0, false);
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

    public class RegisterResponse : TarsStruct
    {
        public int iResCode = 0;
        public long lRequestId = 0;
        public string sMessage = "";

        public RegisterResponse()
        {
        }

        public override void ReadFrom(TarsInputStream _is)
        {
            iResCode = (int)_is.Read(iResCode, 0, false);
            lRequestId = (long)_is.Read(lRequestId, 1, false);
            sMessage = (string)_is.Read(sMessage, 2, false);
        }

        public override void WriteTo(TarsOutputStream _os)
        {
            _os.Write(iResCode, 0);
            _os.Write(lRequestId, 1);
            _os.Write(sMessage, 2);
        }

        public override void Display(StringBuilder sb, int level)
        {
            TarsDisplayer _ds = new TarsDisplayer(sb, level);
            _ds.Display(iResCode, "iResCode");
            _ds.Display(lRequestId, "lRequestId");
            _ds.Display(sMessage, "sMessage");
        }
    }

    public class PushMessageResponse : TarsStruct
    {
        public int iPushType = 0;
        public int iUri = 0;
        public byte[] sMsg = new byte[0];
        public int iProtocolType = 0;

        public PushMessageResponse()
        {
        }

        public override void ReadFrom(TarsInputStream _is)
        {
            iPushType = (int)_is.Read(iPushType, 0, false);
            iUri = (int)_is.Read(iUri, 1, false);
            sMsg = (byte[])_is.Read(sMsg, 2, false);
            iProtocolType = (int)_is.Read(iProtocolType, 3, false);
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

    public class VerifyCookieRequest : TarsStruct
    {
        public long lUid = 0;
        public string sUA = "";
        public string sCookie = "";

        public VerifyCookieRequest()
        {
        }

        public override void ReadFrom(TarsInputStream _is)
        {
            lUid = (long)_is.Read(lUid, 0, false);
            sUA = (string)_is.Read(sUA, 1, false);
            sCookie = (string)_is.Read(sCookie, 2, false);
        }

        public override void WriteTo(TarsOutputStream _os)
        {
            _os.Write(lUid, 0);
            _os.Write(sUA, 1);
            _os.Write(sCookie, 2);
        }

        public override void Display(StringBuilder sb, int level)
        {
            TarsDisplayer _ds = new TarsDisplayer(sb, level);
            _ds.Display(lUid, "lUid");
            _ds.Display(sUA, "sUA");
            _ds.Display(sCookie, "sCookie");
        }
    }

    public class VerifyCookieResponse : TarsStruct
    {
        public int iValidate = 0;

        public VerifyCookieResponse()
        {
        }

        public override void ReadFrom(TarsInputStream _is)
        {
            iValidate = (int)_is.Read(iValidate, 0, false);
        }

        public override void WriteTo(TarsOutputStream _os)
        {
            _os.Write(iValidate, 0);
        }

        public override void Display(StringBuilder sb, int level)
        {
            TarsDisplayer _ds = new TarsDisplayer(sb, level);
            _ds.Display(iValidate, "iValidate");
        }
    }

    public class RegisterGroupRequest : TarsStruct
    {
        public List<string> vGroupId = new List<string>();
        public string sToken = "";

        public RegisterGroupRequest()
        {
        }

        public override void ReadFrom(TarsInputStream _is)
        {
            vGroupId = (List<string>)_is.Read(vGroupId, 0, false);
            sToken = (string)_is.Read(sToken, 1, false);
        }

        public override void WriteTo(TarsOutputStream _os)
        {
            _os.Write(vGroupId, 0);
            _os.Write(sToken, 1);
        }

        public override void Display(StringBuilder sb, int level)
        {
            TarsDisplayer _ds = new TarsDisplayer(sb, level);
            _ds.Display(vGroupId, "vGroupId");
            _ds.Display(sToken, "sToken");
        }
    }

    public class RegisterGroupResponse : TarsStruct
    {
        public int iResCode = 0;

        public RegisterGroupResponse()
        {
        }

        public override void ReadFrom(TarsInputStream _is)
        {
            iResCode = (int)_is.Read(iResCode, 0, false);
        }

        public override void WriteTo(TarsOutputStream _os)
        {
            _os.Write(iResCode, 0);
        }

        public override void Display(StringBuilder sb, int level)
        {
            TarsDisplayer _ds = new TarsDisplayer(sb, level);
            _ds.Display(iResCode, "iResCode");
        }
    }
}
