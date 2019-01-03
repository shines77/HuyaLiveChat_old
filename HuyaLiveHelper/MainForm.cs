using System;
using System.Diagnostics;
using System.Windows.Forms;

using HuyaLive;

namespace HuyaLiveHelper
{
    public partial class MainForm : Form, ClientListener
    {
        private HuyaLiveClient client = null;
        private Logger logger = null;

        public MainForm()
        {
            InitializeComponent();
            CheckForIllegalCrossThreadCalls = false;

            logger = new Logger(new HuyaLive.Debugger());
        }

        public void FlushLog()
        {
            Debug.Flush();
        }

        public void CloseLog()
        {
            Debug.Close();
        }

        public void Print(string message)
        {
            Debug.Print(message);
        }

        public void Print(string format, params object[] args)
        {
            Debug.Print(format, args);
        }

        public void Write(string message)
        {
            Debug.Write(message);
        }

        public void Write(string format, params object[] args)
        {
            Debug.Print(format, args);
        }

        public void WriteLine(string message)
        {
            Debug.WriteLine(message);
        }

        public void WriteLine(string format, params object[] args)
        {
            Debug.WriteLine(format, args);
        }

        public void FuncEnter(string message)
        {
            Debug.WriteLine("-------------------------------------------");
            Debug.WriteLine(message + " enter.");
        }

        public void FuncLeave(string message)
        {
            Debug.WriteLine(message + " leave.");
            Debug.WriteLine("-------------------------------------------");
        }

        public void OnClientStart(object sender)
        {
            Logger log = ((HuyaLiveClient)sender)?.GetLogger();
            log?.WriteLine("  MainForm::OnClientStart()");
        }

        public void OnClientClose(object sender)
        {
            Logger log = ((HuyaLiveClient)sender)?.GetLogger();
            log?.WriteLine("  MainForm::OnClientClose()");
        }

        public void OnClientError(object sender, Exception exception, string message)
        {
            Logger log = ((HuyaLiveClient)sender)?.GetLogger();
            log?.WriteLine("--------------------------------------------------------");
            log?.WriteLine("  MainForm::OnClientError()");
            log?.WriteLine("  Eexception: " + exception.ToString());
            log?.WriteLine("  Message: " + message);
            log?.WriteLine("--------------------------------------------------------");
        }

        public void OnClientEnter(object sender, EnterMessage message)
        {
            Logger log = ((HuyaLiveClient)sender)?.GetLogger();
            log?.WriteLine("  MainForm::OnClientEnter()");
        }

        public void OnClientChat(object sender, ChatMessage message)
        {
            Logger log = ((HuyaLiveClient)sender)?.GetLogger();
            log?.WriteLine("  MainForm::OnClientChat()");
            log?.WriteLine("  uid = {0}, nickname = {1}, timestamp = {2}, content = {3}, length = {4}.",
                message.uid, message.nickname, message.timestamp, message.content, message.length);
        }

        public void OnClientGift(object sender, GiftMessage message)
        {
            Logger log = ((HuyaLiveClient)sender)?.GetLogger();
            log?.WriteLine("  MainForm::OnClientGift()");
        }

        public void OnClientGiftList(object sender, GiftListMessage message)
        {
            Logger log = ((HuyaLiveClient)sender)?.GetLogger();
            log?.WriteLine("  MainForm::OnClientGiftList()");
        }

        public void OnClientOnline(object sender, OnlineMessage message)
        {
            Logger log = ((HuyaLiveClient)sender)?.GetLogger();
            log?.WriteLine("  MainForm::OnClientOnline()");
        }

        private void MainForm_Load(object sender, EventArgs e)
        {
            try
            {
                client = new HuyaLiveClient(this);
                // Shen tu
                client.Start("666007");
                // Yang qi huang
                //client.Start("18001");
            }
            catch (Exception ex)
            {
                Debug.WriteLine("Exception: " + ex.ToString());
                Debug.WriteLine("");
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
                client.SetListener(null);
                client = null;
            }
        }
    }
}
