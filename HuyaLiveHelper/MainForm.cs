
using System;
using System.Diagnostics;
using System.Windows.Forms;
using System.Drawing;

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
    public struct RoomIdInfo
    {
        public string roomId;
        public string roomIntro;
    }

    public partial class MainForm : Form, ClientListener
    {
        private HuyaLiveClient client = null;
#if DEBUG
        private Logger logger = new Logger(new HuyaLive.Debugger());
#else
        private Logger logger = new Logger(new HuyaLive.DummyLogger());
#endif

        private AtomicInt msg_cnt = new AtomicInt();
        private AtomicInt closing = new AtomicInt();

        private bool isActived = false;
        private string curRoomId = "";

        private static RoomIdInfo[] roomIdInfos = {
            new RoomIdInfo { roomId = "0",        roomIntro = "请选择一个主播" },
            new RoomIdInfo { roomId = "18001",    roomIntro = "18001 (洋气黄)" },
            new RoomIdInfo { roomId = "399910",   roomIntro = "399910 (神秘狗)" },
            new RoomIdInfo { roomId = "458911",   roomIntro = "458911 (向阳哥哥)" },
            new RoomIdInfo { roomId = "520880",   roomIntro = "520880 (拉风龙)" },
            new RoomIdInfo { roomId = "521000",   roomIntro = "521000 (卡尔)" },
            new RoomIdInfo { roomId = "626813",   roomIntro = "626813 (女王盐)" },
            new RoomIdInfo { roomId = "666007",   roomIntro = "666007 (大申屠)" },
            new RoomIdInfo { roomId = "908400",   roomIntro = "908400 (董小飒)" },
            new RoomIdInfo { roomId = "931827",   roomIntro = "931827 (大圣归来)" },
            new RoomIdInfo { roomId = "15382773", roomIntro = "15382773 (郭子)" },
        };

        private delegate void delegateChatMsg(string nickname, string content);
        private delegate void delegateGiftMsg(string nickname, string itemName, int itemCount);

        public MainForm()
        {
            InitializeComponent();
            CheckForIllegalCrossThreadCalls = false;
            closing.Set(0);
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

        public void OnUserChat(object sender, UserChatMessage message)
        {
            logger?.WriteLine(">>>  MainForm::OnUserChat()");
            logger?.WriteLine(">>>  timestamp = {0}", message.timestamp);
            logger?.WriteLine(">>>  uid = {0}, imid = {1}, nickname = \"{2}\"",
                              message.uid, message.imid, message.nickname);
            logger?.WriteLine(">>>  content = \"{0}\", length = {1}",
                              message.content, message.length);

            AppendChatMsg(message.nickname, message.content);
        }

        public void OnUserGift(object sender, UserGiftMessage message)
        {
            logger?.WriteLine(">>>  MainForm::OnUserGift()");
            logger?.WriteLine(">>>  timestamp = {0}", message.timestamp);
            logger?.WriteLine(">>>  uid = {0}, imid = {1}, nickname = \"{2}\"",
                              message.uid, message.imid, message.nickname);
            logger?.WriteLine(">>>  itemName = \"{0}\", itemCount = {1}, itemPrice = {2}, ",
                              message.itemName, message.itemCount, message.itemPrice);

            AppendGiftMsg(message.nickname, message.itemName, message.itemCount);
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

            AppendChatMsg("系统消息", "当前在线人数: " + message.onlineUsers.ToString());
        }

        public void OnVipEnter(object sender, VipEnterMessage message)
        {
            logger?.WriteLine(">>>  MainForm::OnVipEnter()");
            logger?.WriteLine(">>>  timestamp = {0}", message.timestamp);
            logger?.WriteLine(">>>  uid = {0}, imid = {1}, nickname = \"{2}\"",
                              message.uid, message.imid, message.nickname);
            logger?.WriteLine(">>>  name = \"{0}\", level = {1}",
                              message.noblename, message.level);

            string welcome = string.Format("欢迎 [{0}] [{1}] 进入直播间。",
                                           message.noblename, message.nickname);
            AppendChatMsg("系统消息", welcome);
        }

        public void OnNobleOnline(object sender, NobleOnlineMessage message)
        {
            logger?.WriteLine(">>>  MainForm::OnNobleOnline()");
            logger?.WriteLine(">>>  timestamp = {0}", message.timestamp);
            logger?.WriteLine(">>>  uid = {0}, imid = {1}, nickname = \"{2}\"",
                              message.uid, message.imid, message.nickname);
            logger?.WriteLine(">>>  name = \"{0}\", level = {1}",
                              message.noblename, message.level);
        }

        public void ClearChatContent()
        {
            chatContent.Focus();
            chatContent.Clear();
            chatContent.ClearUndo();
        }

        public void AppendChatMsg(string nickname, string content)
        {
            if (this.Handle == null || closing.GetAndAdd(0) == 1)
            {
                return;
            }

            if (chatContent.InvokeRequired)
            {
                delegateChatMsg delegates = new delegateChatMsg(AppendChatMsg);
                chatContent.Invoke(delegates, nickname, content);
            }
            else
            {
                int cnt = msg_cnt.Increment();
                if (cnt > 500)
                {
                    chatContent.Clear();
                    chatContent.ClearUndo();
                    msg_cnt.Set(0);
                    GC.Collect();
                }

                if (!isActived && (cnt % 10) == 9)
                {
                    chatContent.Focus();
                    chatContent.Select(chatContent.TextLength, 0);
                    chatContent.ScrollToCaret();
                }

                string text;
                //text = string.Format("[{0}]: {1}\n", nickname, content);
                //text = cnt.ToString() + "\n";
                //chatContent.AppendText(text);

                chatContent.SelectionStart = chatContent.TextLength;
                chatContent.SelectionLength = 0;
                chatContent.SelectionColor = Color.Black;
                chatContent.AppendText("[");

                chatContent.SelectionStart = chatContent.TextLength;
                chatContent.SelectionLength = 0;
                chatContent.SelectionColor = Color.Blue;
                chatContent.AppendText(nickname);

                chatContent.SelectionStart = chatContent.TextLength;
                chatContent.SelectionLength = 0;
                chatContent.SelectionColor = Color.Black;
                text = string.Format("]: {0}\n", content);
                chatContent.AppendText(text);

                //chatContent.Focus();
            }
        }

        public void AppendGiftMsg(string nickname, string itemName, int itemCount)
        {
            if (this.Handle == null || closing.GetAndAdd(0) == 1)
            {
                return;
            }

            if (chatContent.InvokeRequired)
            {
                delegateGiftMsg delegates = new delegateGiftMsg(AppendGiftMsg);
                chatContent.Invoke(delegates, nickname, itemName, itemCount);
            }
            else
            {
                int cnt = msg_cnt.Increment();
                if (cnt > 500)
                {
                    chatContent.Clear();
                    chatContent.ClearUndo();
                    msg_cnt.Set(0);
                    GC.Collect();
                }

                //chatContent.Focus();

                if (!isActived && (cnt % 10) == 9)
                {
                    chatContent.Focus();
                    chatContent.Select(chatContent.TextLength, 0);
                    chatContent.ScrollToCaret();
                }

                string text;
                //text = string.Format("[{0}]: {1} x {2}\n",
                //                     nickname, itemName, itemCount);
                //text = cnt.ToString() + "\n";
                //chatContent.AppendText(text);

                chatContent.SelectionStart = chatContent.TextLength;
                chatContent.SelectionLength = 0;
                chatContent.SelectionColor = Color.Black;
                chatContent.AppendText("[");

                chatContent.SelectionStart = chatContent.TextLength;
                chatContent.SelectionLength = 0;
                chatContent.SelectionColor = Color.Blue;
                chatContent.AppendText(nickname);

                chatContent.SelectionStart = chatContent.TextLength;
                chatContent.SelectionLength = 0;
                chatContent.SelectionColor = Color.Black;
                chatContent.AppendText("]: ");

                chatContent.SelectionStart = chatContent.TextLength;
                chatContent.SelectionLength = 0;
                chatContent.SelectionColor = Color.Red;
                chatContent.AppendText(itemName);

                chatContent.SelectionStart = chatContent.TextLength;
                chatContent.SelectionLength = 0;
                chatContent.SelectionColor = Color.Black;
                text = string.Format(" x {0}\n", itemCount);
                chatContent.AppendText(text);

                //chatContent.Focus();
            }
        }

        private void MainForm_Load(object sender, EventArgs e)
        {
            /*
            try
            {
                client = new HuyaLiveClient(this);
                //client.SetMobileMode(false);
                client.SetLogger(logger);
                // Shen tu
                client.Start("666007");
                // Yang qi huang
                //client.Start("18001");
                // Uzi
                //client.Start("666888");
                // Da sheng gui lai
                //client.Start("931827");
                // Shen mi gou
                //client.Start("399910");
                // Qiqi
                //client.Start("11807215");
            }
            catch (Exception ex)
            {
                logger?.WriteLine("Exception: " + ex.ToString());
                logger?.WriteLine("");
            }
            //*/

            try
            {
                client = new HuyaLiveClient(this);
                client.SetMobileMode(false);
                client.SetLogger(logger);

                isActived = true;

                cbBoxRoomId.Items.Clear();

                int infoLength = roomIdInfos.Length;
                ListItem[] listItems = new ListItem[infoLength];
                for (int i = 0; i < infoLength; i++)
                {
                    RoomIdInfo info = roomIdInfos[i];
                    listItems[i] = new ListItem(info.roomId, info.roomIntro);
                    cbBoxRoomId.Items.Add(listItems[i]);
                }

                cbBoxRoomId.SelectedIndex = 0;
                cbBoxRoomId.SelectedItem = listItems[0];
            }
            catch (Exception ex)
            {
                logger?.WriteLine("Exception: " + ex.ToString());
                logger?.WriteLine("");
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

        private void btnLogin_Click(object sender, EventArgs e)
        {
            try
            {
                if (client == null)
                {
                    client = new HuyaLiveClient(this);
                    //client.SetMobileMode(false);
                    client.SetLogger(logger);
                }

                if (client != null)
                {
                    string roomId = txtBoxRoomId.Text;
                    roomId.Trim();

                    if (roomId == "")
                    {
                        MessageBox.Show("房间号不能为空!", "HuyaHelper");
                        return;
                    }

                    if (roomId == curRoomId && roomId != "")
                    {
                        // Current roomId is not change, directly return.
                        return;
                    }

                    if (client.IsRunning())
                    {
                        client.Stop();
                    }

                    ClearChatContent();

                    client.Start(roomId);

                    // Record connected roomId.
                    curRoomId = roomId;
                }
            }
            catch (Exception ex)
            {
                logger?.WriteLine("Exception: " + ex.ToString());
                logger?.WriteLine("");
            }
        }

        private void cbBoxRoomId_SelectedIndexChanged(object sender, EventArgs e)
        {
            ListItem listItem = cbBoxRoomId.SelectedItem as ListItem;
            if (listItem != null)
            {
                string roomId = listItem.Id;
                roomId.Trim();
                if (roomId != "" && roomId != "0")
                {
                    txtBoxRoomId.Text = roomId;
                }
            }
        }
    }
}
