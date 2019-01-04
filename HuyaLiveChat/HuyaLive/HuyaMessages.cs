using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HuyaLive
{
    public class EnterMessage
    {
        public string uid;
        public string nickname;
    }

    public class ChatMessage
    {
        public string uid;
        public string nickname;
        public string content;
        public int length;
        public long timestamp;
    }

    public class GiftMessage
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
}
