using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace HuyaWebChat.HuyaLive
{
    public interface ClientLinstener
    {
        void OnClientStart(object sender);
        void OnClientClose(object sender);
        void OnClientError(object sender, Exception exception, string message);

        void OnClientEnter(object sender, EnterMessage message);
        void OnClientChat(object sender, ChatMessage message);
        void OnClientGift(object sender, GiftMessage message);
        void OnClientGiftList(object sender, GiftListMessage message);
        void OnClientOnline(object sender, OnlineMessage message);

        void Print(string message);
        void Print(string format, params object[] args);
        void Write(string message);
        void Write(string format, params object[] args);
        void WriteLine(string message);
        void WriteLine(string format, params object[] args);
        void FlushLogger();
        void CloseLogger();
    }
}
