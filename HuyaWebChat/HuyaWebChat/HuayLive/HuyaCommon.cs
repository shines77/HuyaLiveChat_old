using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Tup;
using Tup.Tars;

namespace HuayLive
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
        Mirror  = 1,
        HuyaApp = 2,
        Match   = 4,
        Texas   = 8,
        JieDai  = 16,
        Web     = 32,
        PC      = 64,
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

    public class WebSocketCommand : TarsStruct
    {
        public int cmdType;
        public byte[] data;

        static public void Read(TarsInputStream _is, ref WebSocketCommand command)
        {
            command = (WebSocketCommand) _is.Read(command, 0, true);
        }

        static public void Write(TarsOutputStream _os, WebSocketCommand command)
        {
            _os.Write(command, 0);
        }

        public override void ReadFrom(TarsInputStream _is)
        {
            cmdType = (int) _is.Read(cmdType, 0, true);
            data = (byte[]) _is.Read(data, 1, true);
        }

        public override void WriteTo(TarsOutputStream _os)
        {
            _os.Write(cmdType, 0);
            _os.Write(data, 1);
        }

        public override void Display(StringBuilder sb, int level)
        {
            TarsDisplayer displayer = new TarsDisplayer(sb, level);
            displayer.Display(cmdType, "cmdType");
            displayer.Display(data, "data");
        }
    }
}
