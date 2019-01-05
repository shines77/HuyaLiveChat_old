using System;
using System.Diagnostics;
using System.Windows.Forms;

using HuyaLive;

/*
 *
 * How to make VS compile time automatically reference debug | release version of DLL.
 *
 * See: https://blog.csdn.net/freeboy1015/article/details/7470801
 *
 */

namespace HuyaLiveHelper
{
    public partial class MainForm : Form, ClientListener
    {
        private HuyaLiveClient client = null;
#if DEBUG
        private Logger logger = new Logger(new HuyaLive.Debugger());
#else
        private Logger logger = new Logger(new HuyaLive.Consoler());
#endif
        public MainForm()
        {
            InitializeComponent();
            CheckForIllegalCrossThreadCalls = false;
        }

        public void OnClientStart(object sender)
        {
            logger?.WriteLine(">>>  MainForm::OnClientStart()");
        }

        public void OnClientStop(object sender)
        {
            logger?.WriteLine(">>>  MainForm::OnClientStop()");
        }

        public void OnClientError(object sender, Exception exception, string message)
        {
            logger?.WriteLine("--------------------------------------------------------");
            logger?.WriteLine(">>>  MainForm::OnClientError()");
            logger?.WriteLine("");
            logger?.WriteLine(">>>  Message: " + message);
            logger?.WriteLine("");
            logger?.WriteLine(">>>  Eexception: " + exception.ToString());
            logger?.WriteLine("");            
            logger?.WriteLine("--------------------------------------------------------");
        }

        public void OnNobleEnter(object sender, NobleEnterMessage message)
        {
            logger?.WriteLine(">>>  MainForm::OnNobleEnter()");
        }

        public void OnUserChat(object sender, UserChatMessage message)
        {
            logger?.WriteLine(">>>  MainForm::OnUserChat()");
            logger?.WriteLine(">>>  timestamp = {0}", message.timestamp);
            logger?.WriteLine(">>>  uid = {0}, imid = {1}, nickname = \"{2}\"",
                              message.uid, message.imid, message.nickname);
            logger?.WriteLine(">>>  content = \"{0}\", length = {1}",
                              message.content, message.length);
        }

        public void OnUserGift(object sender, UserGiftMessage message)
        {
            logger?.WriteLine(">>>  MainForm::OnUserGift()");
            logger?.WriteLine(">>>  timestamp = {0}", message.timestamp);
            logger?.WriteLine(">>>  uid = {0}, imid = {1}, nickname = \"{2}\"",
                              message.uid, message.imid, message.nickname);
            logger?.WriteLine(">>>  itemName = \"{0}\", itemCount = {1}, itemPrice = {2}, ",
                              message.itemName, message.itemCount, message.itemPrice);
        }

        public void OnFreshGiftList(object sender, GiftListMessage message)
        {
            logger?.WriteLine(">>>  MainForm::OnFreshGiftList()");
        }

        public void OnRoomOnlineUser(object sender, OnlineUserMessage message)
        {
            logger?.WriteLine(">>>  MainForm::OnRoomOnlineUser()");
            logger?.WriteLine(">>>  timestamp = {0}", message.timestamp);
            logger?.WriteLine(">>>  roomId = {0}, onlineUsers = {1}",
                              message.roomId, message.onlineUsers);
        }

        private void MainForm_Load(object sender, EventArgs e)
        {
            try
            {
                client = new HuyaLiveClient(this);
                //client.SetMobileMode(false);
                client.SetLogger(logger);
                // Shen tu
                //client.Start("666007");
                // Yang qi huang
                //client.Start("18001");
                // Uzi
                //client.Start("666888");
                // Da sheng gui lai
                client.Start("931827");
                // Shen mi gou
                //client.Start("399910");
            }
            catch (Exception ex)
            {
                logger.WriteLine("Exception: " + ex.ToString());
                logger.WriteLine("");
            }
        }

        private void MainForm_FormClosing(object sender, FormClosingEventArgs e)
        {
            if (client != null)
            {
                if (client.IsRunning())
                {
                    client.Stop();
                }

                client.Dispose();
                client.SetLogger(null);
                client.SetListener(null);
                client = null;
            }
        }
    }
}
