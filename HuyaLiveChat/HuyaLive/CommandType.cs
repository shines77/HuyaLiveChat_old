using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Tup;
using Tup.Tars;

namespace HuyaLive
{
    public enum CommandType
    {
        None                 = 0,
        RegisterRequest      = 1,
        RegisterResponse     = 2,
        WupRequest           = 3,
        WupResponse          = 4,
        HeartBeat            = 5,
        HeartBeatAck         = 6,
        MsgPushRequest       = 7,
        UnregisterRequest    = 8,
        UnregisterResponse   = 9,
        VerifyCookieRequest  = 10,
        VerifyCookieResponse = 11,
        VerifyTokenRequest   = 12,
        VerifyTokenResponse  = 13,
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
            tUserId = (UserId)_is.Read(tUserId, 1, true);
            sMd5 = (string)_is.Read(sMd5, 2, true);
            iTemplateType = (int)_is.Read(iTemplateType, 3, true);
            sVersion = (string)_is.Read(sVersion, 4, true);
            iAppId = (int)_is.Read(iAppId, 5, true);
            lPresenterUid = (long)_is.Read(lPresenterUid, 6, true);
            lSid = (long)_is.Read(lSid, 7, true);
            lSubSid = (long)_is.Read(lSubSid, 8, true);
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
}
