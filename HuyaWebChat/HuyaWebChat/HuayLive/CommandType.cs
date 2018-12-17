using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

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
}
