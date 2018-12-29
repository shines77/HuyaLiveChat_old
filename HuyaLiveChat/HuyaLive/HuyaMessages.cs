using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HuyaLive
{
    public class EnterMessage
    {
        public string rid;
        public string nickname;
    }

    public class ChatMessage
    {
        public string rid;
        public string nickname;
        public string content;
        public uint timestamp;
    }

    public class GiftMessage
    {
        public string rid;
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

    public class OnlineMessage
    {
        public string roomid;
        public string name;
        public uint online;
    }
}
