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
        public long uid = 0;
        public bool anonymous = true;
        public string GUID = "";
        public string token = "";
        public string HuyaUA = "";
        public string cookie = "";

        public override void ReadFrom(TarsInputStream _is)
        {
            uid = (long)_is.Read(uid, 0, true);
            anonymous = (bool)_is.Read(anonymous, 1, true);
            GUID = (string)_is.Read(GUID, 2, true);
            token = (string)_is.Read(token, 3, true);
            HuyaUA = (string)_is.Read(HuyaUA, 4, true);
            cookie = (string)_is.Read(cookie, 5, true);
        }

        public override void WriteTo(TarsOutputStream _os)
        {
            _os.Write(uid, 0);
            _os.Write(anonymous, 1);
            _os.Write(GUID, 2);
            _os.Write(token, 3);
            _os.Write(HuyaUA, 4);
            _os.Write(cookie, 5);
        }

        public override void Display(StringBuilder sb, int level)
        {
            TarsDisplayer _ds = new TarsDisplayer(sb, level);
            _ds.Display(uid, "uid");
            _ds.Display(anonymous, "anonymous");
            _ds.Display(GUID, "GUID");
            _ds.Display(token, "token");
            _ds.Display(HuyaUA, "HuyaUA");
            _ds.Display(cookie, "cookie");
        }
    }

    public class UserInfo : TarsStruct
    {
        public long uid = 0;
        public bool anonymous = true;
        public string GUID = "";
        public string token = "";
        public long tid = 0;
        public long sid = 0;
        public long groupId = 0;
        public long groupType = 0;

        public override void ReadFrom(TarsInputStream _is)
        {
            uid = (long)_is.Read(uid, 0, true);
            anonymous = (bool)_is.Read(anonymous, 1, true);
            GUID = (string)_is.Read(GUID, 2, true);
            token = (string)_is.Read(token, 3, true);
            tid = (long)_is.Read(tid, 4, true);
            sid = (long)_is.Read(sid, 5, true);
            groupId = (long)_is.Read(groupId, 6, true);
            groupType = (long)_is.Read(groupType, 7, true);
        }

        public override void WriteTo(TarsOutputStream _os)
        {
            _os.Write(uid, 0);
            _os.Write(anonymous, 1);
            _os.Write(GUID, 2);
            _os.Write(token, 3);
            _os.Write(tid, 4);
            _os.Write(sid, 5);
            _os.Write(groupId, 6);
            _os.Write(groupType, 7);
        }

        public override void Display(StringBuilder sb, int level)
        {
            TarsDisplayer _ds = new TarsDisplayer(sb, level);
            _ds.Display(uid, "uid");
            _ds.Display(anonymous, "anonymous");
            _ds.Display(GUID, "GUID");
            _ds.Display(token, "token");
            _ds.Display(tid, "tid");
            _ds.Display(sid, "sid");
            _ds.Display(groupId, "groupId");
            _ds.Display(groupType, "groupType");
        }
    }

    public class WebSocketCommand : TarsStruct
    {
        public int cmdType = 0;
        public byte[] data = null;

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
            cmdType = (int)_is.Read(cmdType, 0, true);
            data = (byte[])_is.Read(data, 1, true);
        }

        public override void WriteTo(TarsOutputStream _os)
        {
            _os.Write(cmdType, 0);
            _os.Write(data, 1);
        }

        public override void Display(StringBuilder sb, int level)
        {
            TarsDisplayer _ds = new TarsDisplayer(sb, level);
            _ds.Display(cmdType, "cmdType");
            _ds.Display(data, "data");
        }
    }
}
