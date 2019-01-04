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
        public string nickname;
        public string content;
        public int length;
        public long timestamp;
    }

    public class UserGiftMessage
    {
        public string uid;
        public string nickname;
        public string itemName;
        public uint itemCount;
        public uint timestamp;
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
        public string roomid;
        public string name;
        public uint onlineUsers;
    }

    public class SenderInfo : TarsStruct
    {
        public long lUid = 0;
        public long lImid = 0;
        public string sNickName = "";
        public int iGender = 0;

        public SenderInfo()
        {
            //
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
            //
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
            //
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
            //
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
            //
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
            //
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
}
