using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Tup;
using Tup.Tars;

namespace HuyaLive
{
    public enum Gender
    {
        Male = 0,
        Female = 1,
        Unknown = 2,
    }

    public enum ClientType
    {
        YY_PC = 0,
        Huya_PC = 1,
        Huya_Mobile = 2,
        Huya_Web = 3,
        YY_Mobile = 4,
        YY_Web = 5,
    }

    public enum ClientTemplateMask
    {
        Mirror = 1,
        HuyaApp = 2,
        Match = 4,
        Texas = 8,
        JieDai = 16,
        Web = 32,
        PC = 64,
    }

    public enum TemplateType
    {
        Primary = 1,
        Reception = 2,
    }

    public enum StreamLineType
    {
        OldYY = 0,
        WebSocket = 1,
        NewYY = 2,
        AL = 3,
        Huya = 4,
    }

    public enum UserAction
    {
        UserIn = 1,
        UserOut = 2,
        UserMove = 3,
    }

    public class HuyaChatInfo
    {
        public long subsid = 0;
        public long topsid = 0;
        public long yyuid = 0;
        public string room_name = "";

        public HuyaChatInfo()
        {
            Reset();
        }

        public void SetInfo(long subsid, long topsid, long yyuid, string room_name)
        {
            this.subsid = subsid;
            this.topsid = topsid;
            this.yyuid = yyuid;
            this.room_name = room_name;
        }

        public void Reset()
        {
            subsid = 0;
            topsid = 0;
            yyuid = 0;
            room_name = "";
        }
    }

    public class GiftInfo
    {
        public string name;
        public int price;

        public GiftInfo(string name, int price)
        {
            this.name = name;
            this.price = price;
        }
    }

    public class UserId : TarsStruct
    {
        public long lUid = 0;
        public string sGuid = "";
        public string sToken = "";
        public string sHuyaUA = "";
        public string sCookie = "";

        public UserId()
        {
        }

        public override void ReadFrom(TarsInputStream _is)
        {
            lUid = (long)_is.Read(lUid, 0, false);
            sGuid = (string)_is.Read(sGuid, 1, false);
            sToken = (string)_is.Read(sToken, 2, false);
            sHuyaUA = (string)_is.Read(sHuyaUA, 3, false);
            sCookie = (string)_is.Read(sCookie, 4, false);
        }

        public override void WriteTo(TarsOutputStream _os)
        {
            _os.Write(lUid, 0);
            _os.Write(sGuid, 1);
            _os.Write(sToken, 2);
            _os.Write(sHuyaUA, 3);
            _os.Write(sCookie, 4);
        }

        public override void Display(StringBuilder sb, int level)
        {
            TarsDisplayer _ds = new TarsDisplayer(sb, level);
            _ds.Display(lUid, "lUid");
            _ds.Display(sGuid, "sGuid");
            _ds.Display(sToken, "sToken");
            _ds.Display(sHuyaUA, "sHuyaUA");
            _ds.Display(sCookie, "sCookie");
        }
    }

    public class UserInfo : TarsStruct
    {
        public long lUid = 0;
        public bool bAonymous = true;
        public string sGuid = "";
        public string sToken = "";
        public long lTid = 0;
        public long lSid = 0;
        public long lGroupId = 0;
        public long lGroupType = 0;

        public UserInfo()
        {
        }

        public override void ReadFrom(TarsInputStream _is)
        {
            lUid = (long)_is.Read(lUid, 0, false);
            bAonymous = (bool)_is.Read(bAonymous, 1, false);
            sGuid = (string)_is.Read(sGuid, 2, false);
            sToken = (string)_is.Read(sToken, 3, false);
            lTid = (long)_is.Read(lTid, 4, false);
            lSid = (long)_is.Read(lSid, 5, false);
            lGroupId = (long)_is.Read(lGroupId, 6, false);
            lGroupType = (long)_is.Read(lGroupType, 7, false);
        }

        public override void WriteTo(TarsOutputStream _os)
        {
            _os.Write(lUid, 0);
            _os.Write(bAonymous, 1);
            _os.Write(sGuid, 2);
            _os.Write(sToken, 3);
            _os.Write(lTid, 4);
            _os.Write(lSid, 5);
            _os.Write(lGroupId, 6);
            _os.Write(lGroupType, 7);
        }

        public override void Display(StringBuilder sb, int level)
        {
            TarsDisplayer _ds = new TarsDisplayer(sb, level);
            _ds.Display(lUid, "lUid");
            _ds.Display(bAonymous, "bAonymous");
            _ds.Display(sGuid, "sGuid");
            _ds.Display(sToken, "sToken");
            _ds.Display(lTid, "lTid");
            _ds.Display(lSid, "lSid");
            _ds.Display(lGroupId, "lGroupId");
            _ds.Display(lGroupType, "lGroupType");
        }
    }

    public class NobleBase : TarsStruct
    {
        public long lUid = 0;
        public string sNickName = "";
        public int iLevel = 0;
        public string sName = "";
        public int iPet = 0;
        public long lPid = 0;
        public long lTid = 0;
        public long lSid = 0;
        public long lStartTime = 0;
        public long lEndTime = 0;
        public int iLeftDay = 0;
        public int iStatus = 0;
        public int iOpenFlag = 0;

        public NobleBase()
        {
        }

        public override void ReadFrom(TarsInputStream _is)
        {
            lUid = (long)_is.Read(lUid, 0, false);
            sNickName = (string)_is.Read(sNickName, 1, false);
            iLevel = (int)_is.Read(iLevel, 2, false);
            sName = (string)_is.Read(sName, 3, false);
            iPet = (int)_is.Read(iPet, 4, false);
            lPid = (long)_is.Read(lPid, 5, false);
            lTid = (long)_is.Read(lTid, 6, false);
            lSid = (long)_is.Read(lSid, 7, false);
            lStartTime = (long)_is.Read(lStartTime, 8, false);
            lEndTime = (long)_is.Read(lEndTime, 9, false);
            iLeftDay = (int)_is.Read(iLeftDay, 10, false);
            iStatus = (int)_is.Read(iStatus, 11, false);
            iOpenFlag = (int)_is.Read(iOpenFlag, 12, false);
        }

        public override void WriteTo(TarsOutputStream _os)
        {
            _os.Write(lUid, 0);
            _os.Write(sNickName, 1);
            _os.Write(iLevel, 2);
            _os.Write(sName, 3);
            _os.Write(iPet, 4);
            _os.Write(lPid, 5);
            _os.Write(lTid, 6);
            _os.Write(lSid, 7);
            _os.Write(lStartTime, 8);
            _os.Write(lEndTime, 9);
            _os.Write(iLeftDay, 10);
            _os.Write(iStatus, 11);
            _os.Write(iOpenFlag, 12);
        }

        public override void Display(StringBuilder sb, int level)
        {
            TarsDisplayer _ds = new TarsDisplayer(sb, level);
            _ds.Display(lUid, "lUid");
            _ds.Display(sNickName, "sNickName");
            _ds.Display(iLevel, "iLevel");
            _ds.Display(sName, "sName");
            _ds.Display(iPet, "iPet");
            _ds.Display(lPid, "lPid");
            _ds.Display(lTid, "lTid");
            _ds.Display(lSid, "lSid");
            _ds.Display(lStartTime, "lStartTime");
            _ds.Display(lEndTime, "lEndTime");
            _ds.Display(iLeftDay, "iLeftDay");
            _ds.Display(iStatus, "iStatus");
            _ds.Display(iOpenFlag, "iOpenFlag");
        }
    }

    public class NobleEnterNotice : TarsStruct
    {
        public NobleBase tNobleInfo = new NobleBase();

        public NobleEnterNotice()
        {
        }

        public override void ReadFrom(TarsInputStream _is)
        {
            tNobleInfo = (NobleBase)_is.Read(tNobleInfo, 0, false);
        }

        public override void WriteTo(TarsOutputStream _os)
        {
            _os.Write(tNobleInfo, 0);
        }

        public override void Display(StringBuilder sb, int level)
        {
            TarsDisplayer _ds = new TarsDisplayer(sb, level);
            _ds.Display(tNobleInfo, "tNobleInfo");
        }
    }

    public class NobleInfo : TarsStruct
    {
        public long lUid = 0;
        public long lPid = 0;
        public long lValidDate = 0;
        public string sNobleName = "";
        public int iNobleLevel = 0;
        public int iNoblePet = 0;
        public int iNobleStatus = 0;
        public int iNobleType = 0;
        public int iRemainDays = 0;

        public NobleInfo()
        {
        }

        public override void ReadFrom(TarsInputStream _is)
        {
            lUid = (long)_is.Read(lUid, 0, false);
            lPid = (long)_is.Read(lPid, 1, false);
            lValidDate = (long)_is.Read(lValidDate, 2, false);
            sNobleName = (string)_is.Read(sNobleName, 3, false);
            iNobleLevel = (int)_is.Read(iNobleLevel, 4, false);
            iNoblePet = (int)_is.Read(iNoblePet, 5, false);
            iNobleStatus = (int)_is.Read(iNobleStatus, 6, false);
            iNobleType = (int)_is.Read(iNobleType, 7, false);
            iRemainDays = (int)_is.Read(iRemainDays, 8, false);
        }

        public override void WriteTo(TarsOutputStream _os)
        {
            _os.Write(lUid, 0);
            _os.Write(lPid, 1);
            _os.Write(lValidDate, 2);
            _os.Write(sNobleName, 3);
            _os.Write(iNobleLevel, 4);
            _os.Write(iNoblePet, 5);
            _os.Write(iNobleStatus, 6);
            _os.Write(iNobleType, 7);
            _os.Write(iRemainDays, 8);
        }

        public override void Display(StringBuilder sb, int level)
        {
            TarsDisplayer _ds = new TarsDisplayer(sb, level);
            _ds.Display(lUid, "lUid");
            _ds.Display(lPid, "lPid");
            _ds.Display(lValidDate, "lValidDate");
            _ds.Display(sNobleName, "sNobleName");
            _ds.Display(iNobleLevel, "iNobleLevel");
            _ds.Display(iNoblePet, "iNoblePet");
            _ds.Display(iNobleStatus, "iNobleStatus");
            _ds.Display(iNobleType, "iNobleType");
            _ds.Display(iRemainDays, "iRemainDays");
        }
    }

    public class GuardInfo : TarsStruct
    {
        public long lUid = 0;
        public long lPid = 0;
        public int iGuardLevel = 0;
        public long lEndTime = 0;

        public GuardInfo()
        {
        }

        public override void ReadFrom(TarsInputStream _is)
        {
            lUid = (long)_is.Read(lUid, 0, false);
            lPid = (long)_is.Read(lPid, 1, false);
            iGuardLevel = (int)_is.Read(iGuardLevel, 2, false);
            lEndTime = (long)_is.Read(lEndTime, 3, false);
        }

        public override void WriteTo(TarsOutputStream _os)
        {
            _os.Write(lUid, 0);
            _os.Write(lPid, 1);
            _os.Write(iGuardLevel, 2);
            _os.Write(lEndTime, 3);
        }

        public override void Display(StringBuilder sb, int level)
        {
            TarsDisplayer _ds = new TarsDisplayer(sb, level);
            _ds.Display(lUid, "lUid");
            _ds.Display(lPid, "lPid");
            _ds.Display(iGuardLevel, "iGuardLevel");
            _ds.Display(lEndTime, "lEndTime");
        }
    }

    public class FansInfo : TarsStruct
    {
        public long lUid = 0;
        public long lBadgeId = 0;
        public int iBadgeLevel = 0;
        public int iScore = 0;

        public FansInfo()
        {
        }

        public override void ReadFrom(TarsInputStream _is)
        {
            lUid = (long)_is.Read(lUid, 0, false);
            lBadgeId = (long)_is.Read(lBadgeId, 1, false);
            iBadgeLevel = (int)_is.Read(iBadgeLevel, 2, false);
            iScore = (int)_is.Read(iScore, 3, false);
        }

        public override void WriteTo(TarsOutputStream _os)
        {
            _os.Write(lUid, 0);
            _os.Write(lBadgeId, 1);
            _os.Write(iBadgeLevel, 2);
            _os.Write(iScore, 3);
        }

        public override void Display(StringBuilder sb, int level)
        {
            TarsDisplayer _ds = new TarsDisplayer(sb, level);
            _ds.Display(lUid, "lUid");
            _ds.Display(lBadgeId, "lBadgeId");
            _ds.Display(iBadgeLevel, "iBadgeLevel");
            _ds.Display(iScore, "iScore");
        }
    }

    public class NobleInfoBase : TarsStruct
    {
        public int iLevel = 0;
        public int iPet = 0;
        public long lUid = 0;
        public string sNickName = "";
        public string sName = "";
        public long lPid = 0;
        public long lTid = 0;
        public long lSid = 0;
        public long lStartTime = 0;
        public long lEndTime = 0;
        public int iLeftDay = 0;
        public int iStatus = 0;
        public int iOpenFlag = 0;

        public NobleInfoBase()
        {
        }

        public override void ReadFrom(TarsInputStream _is)
        {
            iLevel = (int)_is.Read(iLevel, 0, false);
            iPet = (int)_is.Read(iPet, 1, false);
            lUid = (long)_is.Read(lUid, 2, false);
            sNickName = (string)_is.Read(sNickName, 3, false);
            sName = (string)_is.Read(sName, 4, false);
            lPid = (long)_is.Read(lPid, 5, false);
            lTid = (long)_is.Read(lTid, 6, false);
            lSid = (long)_is.Read(lSid, 7, false);
            lStartTime = (long)_is.Read(lStartTime, 8, false);
            lEndTime = (long)_is.Read(lEndTime, 9, false);
            iLeftDay = (int)_is.Read(iLeftDay, 10, false);
            iStatus = (int)_is.Read(iStatus, 11, false);
            iOpenFlag = (int)_is.Read(iOpenFlag, 12, false);
        }

        public override void WriteTo(TarsOutputStream _os)
        {
            _os.Write(lUid, 0);
            _os.Write(sNickName, 1);
            _os.Write(iLevel, 2);
            _os.Write(sName, 3);
            _os.Write(iPet, 4);
            _os.Write(lPid, 5);
            _os.Write(lTid, 6);
            _os.Write(lSid, 7);
            _os.Write(lStartTime, 8);
            _os.Write(lEndTime, 9);
            _os.Write(iLeftDay, 10);
            _os.Write(iStatus, 11);
            _os.Write(iOpenFlag, 12);
        }

        public override void Display(StringBuilder sb, int level)
        {
            TarsDisplayer _ds = new TarsDisplayer(sb, level);
            _ds.Display(lUid, "lUid");
            _ds.Display(sNickName, "sNickName");
            _ds.Display(iLevel, "iLevel");
            _ds.Display(sName, "sName");
            _ds.Display(iPet, "iPet");
            _ds.Display(lPid, "lPid");
            _ds.Display(lTid, "lTid");
            _ds.Display(lSid, "lSid");
            _ds.Display(lStartTime, "lStartTime");
            _ds.Display(lEndTime, "lEndTime");
            _ds.Display(iLeftDay, "iLeftDay");
            _ds.Display(iStatus, "iStatus");
            _ds.Display(iOpenFlag, "iOpenFlag");
        }
    }

    public class WeekRankInfo : TarsStruct
    {
        public long lUid = 0;
        public int iRank = 0;

        public WeekRankInfo()
        {
        }

        public override void ReadFrom(TarsInputStream _is)
        {
            lUid = (long)_is.Read(lUid, 0, false);
            iRank = (int)_is.Read(iRank, 1, false);
        }

        public override void WriteTo(TarsOutputStream _os)
        {
            _os.Write(lUid, 0);
            _os.Write(iRank, 1);
        }

        public override void Display(StringBuilder sb, int level)
        {
            TarsDisplayer _ds = new TarsDisplayer(sb, level);
            _ds.Display(lUid, "lUid");
            _ds.Display(iRank, "iRank");
        }
    }

    public class ChannelPair : TarsStruct
    {
        public long lTid = 0;
        public long lSid = 0;
        public long lPid = 0;

        public ChannelPair()
        {
        }

        public override void ReadFrom(TarsInputStream _is)
        {
            lTid = (long)_is.Read(lTid, 0, false);
            lSid = (long)_is.Read(lSid, 1, false);
            lPid = (long)_is.Read(lPid, 2, false);
        }

        public override void WriteTo(TarsOutputStream _os)
        {
            _os.Write(lTid, 0);
            _os.Write(lSid, 1);
            _os.Write(lPid, 2);
        }

        public override void Display(StringBuilder sb, int level)
        {
            TarsDisplayer _ds = new TarsDisplayer(sb, level);
            _ds.Display(lTid, "lTid");
            _ds.Display(lSid, "lSid");
            _ds.Display(lPid, "lPid");
        }
    }

    public class DecorationInfoResponse : TarsStruct
    {
        public List<DecorationInfo> vDecorationPrefix = new List<DecorationInfo>();
        public List<DecorationInfo> vDecorationSuffix = new List<DecorationInfo>();
        public ContentFormat tFormat = new ContentFormat();
        public BulletFormat tBulletFormat = new BulletFormat();
        public List<ChannelPair> vForwardChannels = new List<ChannelPair>();
        public int iModifyMask = 0;
        public List<DecorationInfo> vBulletPrefix = new List<DecorationInfo>();
        public SenderInfo tUserInfo = new SenderInfo();
        public List<DecorationInfo> vBulletSuffix = new List<DecorationInfo>();

        public DecorationInfoResponse()
        {
        }

        public override void ReadFrom(TarsInputStream _is)
        {
            vDecorationPrefix = (List<DecorationInfo>)_is.Read(vDecorationPrefix, 0, false);
            vDecorationSuffix = (List<DecorationInfo>)_is.Read(vDecorationSuffix, 1, false);
            tFormat = (ContentFormat)_is.Read(tFormat, 2, false);
            tBulletFormat = (BulletFormat)_is.Read(tBulletFormat, 3, false);
            vForwardChannels = (List<ChannelPair>)_is.Read(vForwardChannels, 4, false);
            iModifyMask = (int)_is.Read(iModifyMask, 5, false);
            vBulletPrefix = (List<DecorationInfo>)_is.Read(vBulletPrefix, 6, false);
            tUserInfo = (SenderInfo)_is.Read(tUserInfo, 7, false);
            vBulletSuffix = (List<DecorationInfo>)_is.Read(vBulletSuffix, 8, false);
        }

        public override void WriteTo(TarsOutputStream _os)
        {
            _os.Write(vDecorationPrefix, 0);
            _os.Write(vDecorationSuffix, 1);
            _os.Write(tFormat, 2);
            _os.Write(tBulletFormat, 3);
            _os.Write(vForwardChannels, 4);
            _os.Write(iModifyMask, 5);
            _os.Write(vBulletPrefix, 6);
            _os.Write(tUserInfo, 7);
            _os.Write(vBulletSuffix, 8);
        }

        public override void Display(StringBuilder sb, int level)
        {
            TarsDisplayer _ds = new TarsDisplayer(sb, level);
            _ds.Display(vDecorationPrefix, "vDecorationPrefix");
            _ds.Display(vDecorationSuffix, "vDecorationSuffix");
            _ds.Display(tFormat, "tFormat");
            _ds.Display(tBulletFormat, "tBulletFormat");
            _ds.Display(vForwardChannels, "vForwardChannels");
            _ds.Display(iModifyMask, "iModifyMask");
            _ds.Display(vBulletPrefix, "vBulletPrefix");
            _ds.Display(tUserInfo, "tUserInfo");
            _ds.Display(vBulletSuffix, "vBulletSuffix");
        }
    }

    public class VipEnterBanner : TarsStruct
    {
        public long lUid = 0;
        public string sNickName = "";
        public long lPid = 0;
        public NobleInfo tNobelInfo = new NobleInfo();
        public GuardInfo tGuardInfo = new GuardInfo();
        public WeekRankInfo tWeekRankInfo = new WeekRankInfo();
        public string sAvatarUrl = "";
        public bool bFromNearby = false;
        public string sLocation = "";
        public DecorationInfoResponse tDecorationInfo = new DecorationInfoResponse();
        public WeekRankInfo tWeekHeartStirRankInfo = new WeekRankInfo();
        public WeekRankInfo tWeekHeartBlockRankInfo = new WeekRankInfo();

        public VipEnterBanner()
        {
        }

        public override void ReadFrom(TarsInputStream _is)
        {
            lUid = (int)_is.Read(lUid, 0, false);
            sNickName = (string)_is.Read(sNickName, 1, false);
            lPid = (int)_is.Read(lPid, 2, false);
            tNobelInfo = (NobleInfo)_is.Read(tNobelInfo, 3, false);
            tGuardInfo = (GuardInfo)_is.Read(tGuardInfo, 4, false);
            tWeekRankInfo = (WeekRankInfo)_is.Read(tWeekRankInfo, 5, false);
            sAvatarUrl = (string)_is.Read(sAvatarUrl, 6, false);
            bFromNearby = (bool)_is.Read(bFromNearby, 7, false);
            sLocation = (string)_is.Read(sLocation, 8, false);
            tDecorationInfo = (DecorationInfoResponse)_is.Read(tDecorationInfo, 9, false);
            tWeekHeartStirRankInfo = (WeekRankInfo)_is.Read(tWeekHeartStirRankInfo, 10, false);
            tWeekHeartBlockRankInfo = (WeekRankInfo)_is.Read(tWeekHeartBlockRankInfo, 11, false);
        }

        public override void WriteTo(TarsOutputStream _os)
        {
            _os.Write(lUid, 0);
            _os.Write(sNickName, 1);
            _os.Write(lPid, 2);
            _os.Write(tNobelInfo, 3);
            _os.Write(tGuardInfo, 4);
            _os.Write(tWeekRankInfo, 5);
            _os.Write(sAvatarUrl, 6);
            _os.Write(bFromNearby, 7);
            _os.Write(sLocation, 8);
            _os.Write(tDecorationInfo, 9);
            _os.Write(tWeekHeartStirRankInfo, 10);
            _os.Write(tWeekHeartBlockRankInfo, 11);
        }

        public override void Display(StringBuilder sb, int level)
        {
            TarsDisplayer _ds = new TarsDisplayer(sb, level);
            _ds.Display(lUid, "lUid");
            _ds.Display(sNickName, "sNickName");
            _ds.Display(lPid, "lPid");
            _ds.Display(tNobelInfo, "tNobelInfo");
            _ds.Display(tGuardInfo, "tGuardInfo");
            _ds.Display(tWeekRankInfo, "tWeekRankInfo");
            _ds.Display(sAvatarUrl, "sAvatarUrl");
            _ds.Display(bFromNearby, "iSuperPupleLevel");
            _ds.Display(sLocation, "sLocation");
            _ds.Display(tDecorationInfo, "tDecorationInfo");
            _ds.Display(tWeekHeartStirRankInfo, "tWeekHeartStirRankInfo");
            _ds.Display(tWeekHeartBlockRankInfo, "tWeekHeartBlockRankInfo");
        }
    }
}
