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
        static public int None = 0;
        static public int RegisterRequest = 1;
        static public int RegisterResponse = 2;
        static public int WupRequest = 3;
        static public int WupResponse = 4;
        static public int HeartBeat = 5;
        static public int HeartBeatAck = 6;
        static public int MsgPushRequest = 7;
        static public int UnregisterRequest = 8;
        static public int UnregisterResponse = 9;
        static public int VerifyCookieRequest = 10;
        static public int VerifyCookieResponse = 11;
        static public int VerifyTokenRequest = 12;
        static public int VerifyTokenResponse = 13;
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
}
