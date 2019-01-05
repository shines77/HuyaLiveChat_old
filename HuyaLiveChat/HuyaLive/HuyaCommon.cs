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
            lUid = (long)_is.Read(lUid, 0, true);
            sGuid = (string)_is.Read(sGuid, 1, true);
            sToken = (string)_is.Read(sToken, 2, true);
            sHuyaUA = (string)_is.Read(sHuyaUA, 3, true);
            sCookie = (string)_is.Read(sCookie, 4, true);
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
            lUid = (long)_is.Read(lUid, 0, true);
            bAonymous = (bool)_is.Read(bAonymous, 1, true);
            sGuid = (string)_is.Read(sGuid, 2, true);
            sToken = (string)_is.Read(sToken, 3, true);
            lTid = (long)_is.Read(lTid, 4, true);
            lSid = (long)_is.Read(lSid, 5, true);
            lGroupId = (long)_is.Read(lGroupId, 6, true);
            lGroupType = (long)_is.Read(lGroupType, 7, true);
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
}
