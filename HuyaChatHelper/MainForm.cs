using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Diagnostics;
using System.Windows.Forms;

using HuyaWebChat.HuyaLive;

namespace HuyaChatHelper
{
    public partial class MainForm : Form, ClientListener
    {
        private HuyaChatClient client = null;
        private Logger logger = null;

        public MainForm()
        {
            InitializeComponent();
            CheckForIllegalCrossThreadCalls = false;

            logger = new Logger(new HuyaWebChat.HuyaLive.Debugger());
        }

        public void FlushLogger()
        {
            Debug.Flush();
        }

        public void CloseLogger()
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

        public void OnClientStart(object sender)
        {
            Debug.WriteLine("  MainForm::OnClientStart()");
        }

        public void OnClientClose(object sender)
        {
            Debug.WriteLine("  MainForm::OnClientClose()");
        }

        public void OnClientError(object sender, Exception exception, string message)
        {
            Debug.WriteLine("--------------------------------------------------------");
            Debug.WriteLine("  MainForm::OnClientError()");
            Debug.WriteLine("  Eexception: " + exception.ToString());
            Debug.WriteLine("  Message: " + message);
            Debug.WriteLine("--------------------------------------------------------");
        }

        public void OnClientEnter(object sender, EnterMessage message)
        {
            Debug.WriteLine("  MainForm::OnClientEnter()");
        }

        public void OnClientChat(object sender, ChatMessage message)
        {
            Debug.WriteLine("  MainForm::OnClientChat()");
            Debug.WriteLine("  rid = {0}, nickname = {0}, timestamp = {0}, content = {0}.",
                        message.rid, message.nickname, message.timestamp, message.content);
        }

        public void OnClientGift(object sender, GiftMessage message)
        {
            Debug.WriteLine("  MainForm::OnClientGift()");
        }

        public void OnClientGiftList(object sender, GiftListMessage message)
        {
            Debug.WriteLine("  MainForm::OnClientGiftList()");
        }

        public void OnClientOnline(object sender, OnlineMessage message)
        {
            Debug.WriteLine("  MainForm::OnClientOnline()");
        }

        private void MainForm_Load(object sender, EventArgs e)
        {
            try
            {
                client = new HuyaChatClient(this);
                client.Start("666007");
            }
            catch (Exception ex)
            {
                Debug.WriteLine("Exception: " + ex.ToString());
                Console.WriteLine();
            }
        }

        private void MainForm_FormClosing(object sender, FormClosingEventArgs e)
        {
            if (client != null)
            {
                if (client.IsRunning())
                {
                    client.Dispose();
                    client = null;
                }
            }
        }
    }
}
