using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace HuyaLive
{
    public interface LoggerListener
    {
        void Print(string message);
        void Print(string format, params object[] args);
        void Write(string message);
        void Write(string format, params object[] args);
        void WriteLine(string message);
        void WriteLine(string format, params object[] args);
        void FlushLog();
        void CloseLog();

        void FuncEnter(string message);
        void FuncLeave(string message);
    }

    public interface ClientListener
    {
        void OnClientStart(object sender);
        void OnClientStop(object sender);
        void OnClientError(object sender, Exception exception, string message);

        void OnUserChat(object sender, UserChatMessage message);
        void OnUserGift(object sender, UserGiftMessage message);
        void OnFreshGiftList(object sender, GiftListMessage message);
        void OnRoomOnlineUser(object sender, OnlineUserMessage message);
        void OnVipEnter(object sender, VipEnterMessage message);
        void OnNobleOnline(object sender, NobleOnlineMessage message);
    }
}
