using System;
using System.Diagnostics;
using System.Windows.Forms;

using HuyaLive;

namespace HuyaLiveHelper
{
    public partial class MainForm : Form, ClientListener
    {
        private HuyaLiveClient client = null;
        private Logger logger = new Logger(new HuyaLive.Consoler());

        public MainForm()
        {
            InitializeComponent();
            CheckForIllegalCrossThreadCalls = false;
        }

        public void OnClientStart(object sender)
        {
            logger?.WriteLine("  MainForm::OnClientStart()");
        }

        public void OnClientStop(object sender)
        {
            logger?.WriteLine("  MainForm::OnClientStop()");
        }

        public void OnClientError(object sender, Exception exception, string message)
        {
            logger?.WriteLine("--------------------------------------------------------");
            logger?.WriteLine("  MainForm::OnClientError()");
            logger?.WriteLine("  Eexception: " + exception.ToString());
            logger?.WriteLine("  Message: " + message);
            logger?.WriteLine("--------------------------------------------------------");
        }

        public void OnClientEnter(object sender, EnterMessage message)
        {
            logger?.WriteLine("  MainForm::OnClientEnter()");
        }

        public void OnClientChat(object sender, ChatMessage message)
        {
            logger?.WriteLine("  MainForm::OnClientChat()");
            logger?.WriteLine("  uid = {0}, nickname = {1}, timestamp = {2}, content = {3}, length = {4}.",
                message.uid, message.nickname, message.timestamp, message.content, message.length);
        }

        public void OnClientGift(object sender, GiftMessage message)
        {
            logger?.WriteLine("  MainForm::OnClientGift()");
        }

        public void OnClientGiftList(object sender, GiftListMessage message)
        {
            logger?.WriteLine("  MainForm::OnClientGiftList()");
        }

        public void OnClientOnline(object sender, OnlineMessage message)
        {
            logger?.WriteLine("  MainForm::OnClientOnline()");
        }

        private void MainForm_Load(object sender, EventArgs e)
        {
            try
            {
                client = new HuyaLiveClient(this);
                client.SetLogger(logger);
                // Shen tu
                client.Start("666007");
                // Yang qi huang
                //client.Start("18001");
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
