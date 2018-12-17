using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Tup;
using Tup.Tars;

namespace HuyaWebChat.HuyaLive
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

    public class UserId : TarsStruct
    {
        public long lUid = 0;
        public bool bAnonymous = true;
        public string sGuid = "";
        public string sToken = "";
        public string sHuyaUA = "";
        public string sCookie = "";

        public override void ReadFrom(TarsInputStream _is)
        {
            lUid = (long)_is.Read(lUid, 1, true);
            bAnonymous = (bool)_is.Read(bAnonymous, 2, true);
            sGuid = (string)_is.Read(sGuid, 3, true);
            sToken = (string)_is.Read(sToken, 4, true);
            sHuyaUA = (string)_is.Read(sHuyaUA, 5, true);
            sCookie = (string)_is.Read(sCookie, 6, true);
        }

        public override void WriteTo(TarsOutputStream _os)
        {
            _os.Write(lUid, 1);
            _os.Write(bAnonymous, 2);
            _os.Write(sGuid, 3);
            _os.Write(sToken, 4);
            _os.Write(sHuyaUA, 5);
            _os.Write(sCookie, 6);
        }

        public override void Display(StringBuilder sb, int level)
        {
            TarsDisplayer _ds = new TarsDisplayer(sb, level);
            _ds.Display(lUid, "uid");
            _ds.Display(bAnonymous, "anonymous");
            _ds.Display(sGuid, "GUID");
            _ds.Display(sToken, "token");
            _ds.Display(sHuyaUA, "HuyaUA");
            _ds.Display(sCookie, "cookie");
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

        public override void ReadFrom(TarsInputStream _is)
        {
            lUid = (long)_is.Read(lUid, 1, true);
            bAonymous = (bool)_is.Read(bAonymous, 2, true);
            sGuid = (string)_is.Read(sGuid, 3, true);
            sToken = (string)_is.Read(sToken, 4, true);
            lTid = (long)_is.Read(lTid, 5, true);
            lSid = (long)_is.Read(lSid, 6, true);
            lGroupId = (long)_is.Read(lGroupId, 7, true);
            lGroupType = (long)_is.Read(lGroupType, 8, true);
        }

        public override void WriteTo(TarsOutputStream _os)
        {
            _os.Write(lUid, 1);
            _os.Write(bAonymous, 2);
            _os.Write(sGuid, 3);
            _os.Write(sToken, 4);
            _os.Write(lTid, 5);
            _os.Write(lSid, 6);
            _os.Write(lGroupId, 7);
            _os.Write(lGroupType, 8);
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

    public class WebSocketCommand : TarsStruct
    {
        public int iCmdType = 0;
        public byte[] vData = null;

        static public void Read(TarsInputStream _is, ref WebSocketCommand command,
                                int tag, bool require = true)
        {
            command = (WebSocketCommand)_is.Read(command, tag, require);
        }

        static public void Write(TarsOutputStream _os, WebSocketCommand command, int tag)
        {
            _os.Write(command, tag);
        }

        public override void ReadFrom(TarsInputStream _is)
        {
            iCmdType = (int)_is.Read(iCmdType, 1, true);
            vData = (byte[])_is.Read(vData, 2, true);
        }

        public override void WriteTo(TarsOutputStream _os)
        {
            _os.Write(iCmdType, 1);
            _os.Write(vData, 2);
        }

        public override void Display(StringBuilder sb, int level)
        {
            TarsDisplayer _ds = new TarsDisplayer(sb, level);
            _ds.Display(iCmdType, "iCmdType");
            _ds.Display(vData, "vData");
        }
    }
}
