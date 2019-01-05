using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Tup;
using Tup.Tars;

namespace HuyaLive
{
    public class NobleEnterMessage
    {
        public string uid;
        public string nickname;
    }

    public class UserChatMessage
    {
        public long uid;
        public long imid;
        public string nickname;
        public string content;
        public int length;
        public long timestamp;
    }

    public class UserGiftMessage
    {
        public long presenterUid;
        public long uid;
        public long imid;
        public string nickname;
        public string itemName;
        public int itemCount;
        public int itemPrice;
        public long timestamp;
    }

    public class GiftListMessage
    {
        public string itemName;
        public uint number;
        public uint price;
        public uint earn;
    }

    public class OnlineUserMessage
    {
        public string roomId;
        public string roomName;
        public int onlineUsers;
        public long timestamp;
    }

    public class SenderInfo : TarsStruct
    {
        public long lUid = 0;
        public long lImid = 0;
        public string sNickName = "";
        public int iGender = 0;

        public SenderInfo()
        {
        }

        public override void ReadFrom(TarsInputStream _is)
        {
            lUid = (long)_is.Read(lUid, 0, true);
            lImid = (long)_is.Read(lImid, 1, true);
            sNickName = (string)_is.Read(sNickName, 2, true);
            iGender = (int)_is.Read(iGender, 3, true);
        }

        public override void WriteTo(TarsOutputStream _os)
        {
            _os.Write(lUid, 0);
            _os.Write(lImid, 1);
            _os.Write(sNickName, 2);
            _os.Write(iGender, 3);
        }

        public override void Display(StringBuilder sb, int level)
        {
            TarsDisplayer _ds = new TarsDisplayer(sb, level);
            _ds.Display(lUid, "lUid");
            _ds.Display(lImid, "lImid");
            _ds.Display(sNickName, "sNickName");
            _ds.Display(iGender, "iGender");
        }
    }

    public class ContentFormat : TarsStruct
    {
        public int iFontColor = -1;
        public int iFontSize = 4;
        public int iPopupStyle = 0;

        public ContentFormat()
        {
        }

        public override void ReadFrom(TarsInputStream _is)
        {
            iFontColor = (int)_is.Read(iFontColor, 0, true);
            iFontSize = (int)_is.Read(iFontSize, 1, true);
            iPopupStyle = (int)_is.Read(iPopupStyle, 2, true);
        }

        public override void WriteTo(TarsOutputStream _os)
        {
            _os.Write(iFontColor, 0);
            _os.Write(iFontSize, 1);
            _os.Write(iPopupStyle, 2);
        }

        public override void Display(StringBuilder sb, int level)
        {
            TarsDisplayer _ds = new TarsDisplayer(sb, level);
            _ds.Display(iFontColor, "iFontColor");
            _ds.Display(iFontSize, "iFontSize");
            _ds.Display(iPopupStyle, "iPopupStyle");
        }
    }

    public class BulletFormat : TarsStruct
    {
        public int iFontColor = -1;
        public int iFontSize = 4;
        public int iTextSpeed = 0;
        public int iTransitionType = 1;
        public int iPopupStyle = 0;

        public BulletFormat()
        {
        }

        public override void ReadFrom(TarsInputStream _is)
        {
            iFontColor = (int)_is.Read(iFontColor, 0, true);
            iFontSize = (int)_is.Read(iFontSize, 1, true);
            iTextSpeed = (int)_is.Read(iTextSpeed, 2, true);
            iTransitionType = (int)_is.Read(iTransitionType, 3, true);
            iPopupStyle = (int)_is.Read(iPopupStyle, 4, true);
        }

        public override void WriteTo(TarsOutputStream _os)
        {
            _os.Write(iFontColor, 0);
            _os.Write(iFontSize, 1);
            _os.Write(iTextSpeed, 2);
            _os.Write(iTransitionType, 3);
            _os.Write(iPopupStyle, 4);
        }

        public override void Display(StringBuilder sb, int level)
        {
            TarsDisplayer _ds = new TarsDisplayer(sb, level);
            _ds.Display(iFontColor, "iFontColor");
            _ds.Display(iFontSize, "iFontSize");
            _ds.Display(iTextSpeed, "iTextSpeed");
            _ds.Display(iTransitionType, "iTransitionType");
            _ds.Display(iPopupStyle, "iPopupStyle");
        }
    }

    public class DecorationInfo : TarsStruct
    {
        public int iAppId = 0;
        public int iViewType = 0;
        public byte[] vData = new byte[0];

        public DecorationInfo()
        {
        }

        public override void ReadFrom(TarsInputStream _is)
        {
            iAppId = (int)_is.Read(iAppId, 0, true);
            iViewType = (int)_is.Read(iViewType, 1, true);
            vData = (byte[])_is.Read(vData, 2, true);
        }

        public override void WriteTo(TarsOutputStream _os)
        {
            _os.Write(iAppId, 0);
            _os.Write(iViewType, 1);
            _os.Write(vData, 2);
        }

        public override void Display(StringBuilder sb, int level)
        {
            TarsDisplayer _ds = new TarsDisplayer(sb, level);
            _ds.Display(iAppId, "iAppId");
            _ds.Display(iViewType, "iViewType");
            _ds.Display(vData, "vData");
        }
    }

    public class UidNickName : TarsStruct
    {
        public long lUid = 0;
        public string sNickName = "";

        public UidNickName()
        {
        }

        public override void ReadFrom(TarsInputStream _is)
        {
            lUid = (long)_is.Read(lUid, 0, true);
            sNickName = (string)_is.Read(sNickName, 1, true);
        }

        public override void WriteTo(TarsOutputStream _os)
        {
            _os.Write(lUid, 0);
            _os.Write(sNickName, 1);
        }

        public override void Display(StringBuilder sb, int level)
        {
            TarsDisplayer _ds = new TarsDisplayer(sb, level);
            _ds.Display(lUid, "lUid");
            _ds.Display(sNickName, "sNickName");
        }
    }

    public class MessageNotice : TarsStruct
    {
        public SenderInfo tUserInfo = new SenderInfo();
        public long lTid = 0;
        public long lSid = 0;
        public string sContent = "";
        public int iShowMode = 0;
        public ContentFormat tFormat = new ContentFormat();
        public BulletFormat tBulletFormat = new BulletFormat();
        public int iTermType = 0;
        public List<DecorationInfo> vDecorationPrefix = new List<DecorationInfo>();
        public List<DecorationInfo> vDecorationSuffix = new List<DecorationInfo>();
        public List<UidNickName> vAtSomeone = new List<UidNickName>();
        public long lPid = 0;

        public MessageNotice()
        {
        }

        public override void ReadFrom(TarsInputStream _is)
        {
            tUserInfo = (SenderInfo)_is.Read(tUserInfo, 0, true);
            lTid = (long)_is.Read(lTid, 1, true);
            lSid = (long)_is.Read(lSid, 2, true);
            sContent = (string)_is.Read(sContent, 3, true);
            iShowMode = (int)_is.Read(iShowMode, 4, true);
            tFormat = (ContentFormat)_is.Read(tFormat, 5, true);
            tBulletFormat = (BulletFormat)_is.Read(tBulletFormat, 6, true);
            iTermType = (int)_is.Read(iTermType, 7, true);
            vDecorationPrefix = (List<DecorationInfo>)_is.Read(vDecorationPrefix, 8, true);
            vDecorationSuffix = (List<DecorationInfo>)_is.Read(vDecorationSuffix, 9, true);
            vAtSomeone = (List<UidNickName>)_is.Read(vAtSomeone, 10, true);
            lPid = (long)_is.Read(lPid, 11, true);
        }

        public override void WriteTo(TarsOutputStream _os)
        {
            _os.Write(tUserInfo, 0);
            _os.Write(lTid, 1);
            _os.Write(lSid, 2);
            _os.Write(sContent, 3);
            _os.Write(iShowMode, 4);
            _os.Write(tFormat, 5);
            _os.Write(tBulletFormat, 6);
            _os.Write(iTermType, 7);
            _os.Write(vDecorationPrefix, 8);
            _os.Write(vDecorationSuffix, 9);
            _os.Write(vAtSomeone, 10);
            _os.Write(lPid, 11);
        }

        public override void Display(StringBuilder sb, int level)
        {
            TarsDisplayer _ds = new TarsDisplayer(sb, level);
            _ds.Display(tUserInfo, "tUserInfo");
            _ds.Display(lTid, "lTid");
            _ds.Display(lSid, "lSid");
            _ds.Display(sContent, "sContent");
            _ds.Display(iShowMode, "iShowMode");
            _ds.Display(tFormat, "tFormat");
            _ds.Display(tBulletFormat, "tBulletFormat");
            _ds.Display(iTermType, "iTermType");
            _ds.Display(vDecorationPrefix, "vDecorationPrefix");
            _ds.Display(vDecorationSuffix, "vDecorationSuffix");
            _ds.Display(vAtSomeone, "vAtSomeone");
            _ds.Display(lPid, "lPid");
        }
    }

    public class SendItemSubBroadcastPacket : TarsStruct
    {
        public int iItemType = 0;
        public string strPayId = "";
        public int iItemCount = 0;
        public long lPresenterUid = 0;
        public long lSenderUid = 0;
        public string sPresenterNick = "";
        public string sSenderNick = "";
        public string sSendContent = "";
        public int iItemCountByGroup = 0;
        public int iItemGroup = 0;
        public int iSuperPupleLevel = 0;
        public int iComboScore = 0;
        public int iDisplayInfo = 0;
        public int iEffectType = 0;
        public string sSenderIcon = "";
        public string sPresenterIcon = "";
        public int iTemplateType = 0;
        public string sExpand = "";
        public bool bBusi = false;
        public int iColorEffectType = 0;

        public SendItemSubBroadcastPacket()
        {
        }

        public override void ReadFrom(TarsInputStream _is)
        {
            iItemType = (int)_is.Read(iItemType, 0, true);
            strPayId = (string)_is.Read(strPayId, 1, true);
            iItemCount = (int)_is.Read(iItemCount, 2, true);
            lPresenterUid = (long)_is.Read(lPresenterUid, 3, true);
            lSenderUid = (long)_is.Read(lSenderUid, 4, true);
            sPresenterNick = (string)_is.Read(sPresenterNick, 5, true);
            sSenderNick = (string)_is.Read(sSenderNick, 6, true);
            sSendContent = (string)_is.Read(sSendContent, 7, true);
            iItemCountByGroup = (int)_is.Read(iItemCountByGroup, 8, true);
            iItemGroup = (int)_is.Read(iItemGroup, 9, true);
            iSuperPupleLevel = (int)_is.Read(iSuperPupleLevel, 10, true);
            iComboScore = (int)_is.Read(iComboScore, 11, true);
            iDisplayInfo = (int)_is.Read(iDisplayInfo, 12, true);
            iEffectType = (int)_is.Read(iEffectType, 13, true);
            sSenderIcon = (string)_is.Read(sSenderIcon, 14, true);
            sPresenterIcon = (string)_is.Read(sPresenterIcon, 15, true);
            iTemplateType = (int)_is.Read(iTemplateType, 16, true);
            sExpand = (string)_is.Read(sExpand, 17, true);
            bBusi = (bool)_is.Read(bBusi, 18, true);
            iColorEffectType = (int)_is.Read(iColorEffectType, 19, true);
        }

        public override void WriteTo(TarsOutputStream _os)
        {
            _os.Write(iItemType, 0);
            _os.Write(strPayId, 1);
            _os.Write(iItemCount, 2);
            _os.Write(lPresenterUid, 3);
            _os.Write(lSenderUid, 4);
            _os.Write(sPresenterNick, 5);
            _os.Write(sSenderNick, 6);
            _os.Write(sSendContent, 7);
            _os.Write(iItemCountByGroup, 8);
            _os.Write(iItemGroup, 9);
            _os.Write(iSuperPupleLevel, 10);
            _os.Write(iComboScore, 11);
            _os.Write(iDisplayInfo, 12);
            _os.Write(iEffectType, 13);
            _os.Write(sSenderIcon, 14);
            _os.Write(sPresenterIcon, 15);
            _os.Write(iTemplateType, 16);
            _os.Write(sExpand, 17);
            _os.Write(bBusi, 18);
            _os.Write(iColorEffectType, 19);
        }

        public override void Display(StringBuilder sb, int level)
        {
            TarsDisplayer _ds = new TarsDisplayer(sb, level);
            _ds.Display(iItemType, "iItemType");
            _ds.Display(strPayId, "strPayId");
            _ds.Display(iItemCount, "iItemCount");
            _ds.Display(lPresenterUid, "lPresenterUid");
            _ds.Display(lSenderUid, "lSenderUid");
            _ds.Display(sPresenterNick, "sPresenterNick");
            _ds.Display(sSenderNick, "sSenderNick");
            _ds.Display(sSendContent, "sSendContent");
            _ds.Display(iItemCountByGroup, "iItemCountByGroup");
            _ds.Display(iItemGroup, "iItemGroup");
            _ds.Display(iSuperPupleLevel, "iSuperPupleLevel");
            _ds.Display(iComboScore, "iComboScore");
            _ds.Display(iDisplayInfo, "iDisplayInfo");
            _ds.Display(iEffectType, "iEffectType");
            _ds.Display(sSenderIcon, "sSenderIcon");
            _ds.Display(sPresenterIcon, "sPresenterIcon");
            _ds.Display(iTemplateType, "iTemplateType");
            _ds.Display(sExpand, "sExpand");
            _ds.Display(bBusi, "bBusi");
            _ds.Display(iColorEffectType, "iColorEffectType");
        }
    }

    public class SendItemNoticeWordBroadcastPacket : TarsStruct
    {
        public int iItemType = 0;
        public int iItemCount = 0;
        public long lSenderSid = 0;
        public long lSenderUid = 0;
        public string sSenderNick = "";
        public long lPresenterUid = 0;
        public string sPresenterNick = "";
        public long lNoticeChannelCount = 0;
        public int iItemCountByGroup = 0;
        public int iItemGroup = 0;
        public int iDisplayInfo = 0;
        public int iSuperPupleLevel = 0;
        public int iTemplateType = 0;
        public string sExpand = "";
        public bool bBusi = false;

        public SendItemNoticeWordBroadcastPacket()
        {
        }

        public override void ReadFrom(TarsInputStream _is)
        {
            iItemType = (int)_is.Read(iItemType, 0, true);
            iItemCount = (int)_is.Read(iItemCount, 1, true);
            lSenderSid = (long)_is.Read(lSenderSid, 2, true);
            lSenderUid = (long)_is.Read(lSenderUid, 3, true);
            sSenderNick = (string)_is.Read(sSenderNick, 4, true);
            lPresenterUid = (long)_is.Read(lPresenterUid, 5, true);
            sPresenterNick = (string)_is.Read(sPresenterNick, 6, true);
            lNoticeChannelCount = (long)_is.Read(lNoticeChannelCount, 7, true);
            iItemCountByGroup = (int)_is.Read(iItemCountByGroup, 8, true);
            iItemGroup = (int)_is.Read(iItemGroup, 9, true);
            iDisplayInfo = (int)_is.Read(iDisplayInfo, 10, true);
            iSuperPupleLevel = (int)_is.Read(iSuperPupleLevel, 11, true);
            iTemplateType = (int)_is.Read(iTemplateType, 12, true);
            sExpand = (string)_is.Read(sExpand, 13, true);
            bBusi = (bool)_is.Read(bBusi, 14, true);
        }

        public override void WriteTo(TarsOutputStream _os)
        {
            _os.Write(iItemType, 0);
            _os.Write(iItemCount, 1);
            _os.Write(lSenderSid, 2);
            _os.Write(lSenderUid, 3);
            _os.Write(sSenderNick, 4);
            _os.Write(lPresenterUid, 5);
            _os.Write(sPresenterNick, 6);
            _os.Write(lNoticeChannelCount, 7);
            _os.Write(iItemCountByGroup, 8);
            _os.Write(iItemGroup, 9);
            _os.Write(iDisplayInfo, 10);
            _os.Write(iSuperPupleLevel, 11);
            _os.Write(iTemplateType, 12);
            _os.Write(sExpand, 13);
            _os.Write(bBusi, 14);
        }

        public override void Display(StringBuilder sb, int level)
        {
            TarsDisplayer _ds = new TarsDisplayer(sb, level);
            _ds.Display(iItemType, "iItemType");
            _ds.Display(iItemCount, "iItemCount");
            _ds.Display(lSenderSid, "lSenderSid");
            _ds.Display(lSenderUid, "lSenderUid");
            _ds.Display(sSenderNick, "sSenderNick");
            _ds.Display(lPresenterUid, "lPresenterUid");
            _ds.Display(sPresenterNick, "sPresenterNick");
            _ds.Display(lNoticeChannelCount, "lNoticeChannelCount");
            _ds.Display(iItemCountByGroup, "iItemCountByGroup");
            _ds.Display(iItemGroup, "iItemGroup");
            _ds.Display(iDisplayInfo, "iDisplayInfo");
            _ds.Display(iSuperPupleLevel, "iSuperPupleLevel");
            _ds.Display(iTemplateType, "iTemplateType");
            _ds.Display(sExpand, "sExpand");
            _ds.Display(bBusi, "bBusi");

        }
    }

    public class AttendeeCountNotice : TarsStruct
    {
        public int iAttendeeCount = 0;

        public AttendeeCountNotice()
        {
        }

        public override void ReadFrom(TarsInputStream _is)
        {
            iAttendeeCount = (int)_is.Read(iAttendeeCount, 0, true);
        }

        public override void WriteTo(TarsOutputStream _os)
        {
            _os.Write(iAttendeeCount, 0);
        }

        public override void Display(StringBuilder sb, int level)
        {
            TarsDisplayer _ds = new TarsDisplayer(sb, level);
            _ds.Display(iAttendeeCount, "iAttendeeCount");
        }
    }

    public class UserChannelRequest : TarsStruct
    {
        public UserId tId = new UserId();
        public long lTopcid = 0;
        public long lSubcid = 0;
        public string sSendContent = "";

        public UserChannelRequest()
        {
        }

        public override void ReadFrom(TarsInputStream _is)
        {
            tId = (UserId)_is.Read(tId, 0, true);
            lTopcid = (long)_is.Read(lTopcid, 1, true);
            lSubcid = (long)_is.Read(lSubcid, 2, true);
            sSendContent = (string)_is.Read(sSendContent, 3, true);
        }

        public override void WriteTo(TarsOutputStream _os)
        {
            _os.Write(tId, 0);
            _os.Write(lTopcid, 1);
            _os.Write(lSubcid, 2);
            _os.Write(sSendContent, 3);
        }

        public override void Display(StringBuilder sb, int level)
        {
            TarsDisplayer _ds = new TarsDisplayer(sb, level);
            _ds.Display(tId, "tId");
            _ds.Display(lTopcid, "lTopcid");
            _ds.Display(lSubcid, "lSubcid");
            _ds.Display(sSendContent, "sSendContent");
        }
    }
}
