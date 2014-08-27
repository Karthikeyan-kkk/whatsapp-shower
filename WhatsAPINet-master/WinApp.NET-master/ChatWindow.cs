using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using WhatsAppApi.Helper;
using System.Threading;
using WinAppNET.AppCode;
using System.IO;
using System.Runtime.InteropServices;
using WinAppNET.Controls;
using System.Media;

namespace WinAppNET
{
    public partial class ChatWindow : Form
    {
        const int MESSAGE_LIMIT = 100;

        protected List<WappMessage> messages = new List<WappMessage>();
        public string target;
        ClientState state = ClientState.ONLINE;
        public bool IsGroup = false;
        public bool stealFocus = false;
        private SoundPlayer sp;
        public Boolean IsLoaded = false;
        public bool onTop;

        enum ClientState
        {
            ONLINE,
            TYPING
        }

        void ProcessChat()
        {
            this.Text = ContactStore.GetContactByJid(this.target).FullName;
            WappSocket.Instance.SendQueryLastOnline(this.target);
            WappSocket.Instance.SendPresenceSubscriptionRequest(this.target);

            //load image
            string filepath = getCacheImagePath(this.target);
            if (File.Exists(filepath))
            {
                try
                {
                    Image img = Image.FromFile(filepath);
                    this.pictureBox1.Image = img;
                }
                catch (Exception)
                {
                    GetImageAsync(this.target, false);
                }

            }
            else
            {
                GetImageAsync(this.target, false);
            }
        }

        public static string getCacheImagePath(string target)
        {
            return Directory.GetCurrentDirectory() + "\\data\\profilecache\\" + target + ".jpg";
        }

        public static string GetImageAsync(string jid, bool large)
        {
            return WappSocket.Instance.SendGetPhoto(jid, string.Empty, large);
        }

        void ProcessGroupChat()
        {
            this.Text = "Group chat " + ContactStore.GetContactByJid(this.target).ToString();
            WappSocket.Instance.SendGetGroupInfo(this.target);
        }

        public ChatWindow(string target, bool stealFocus, bool onTop)
        {
            this.stealFocus = stealFocus;
            this.onTop = onTop;
            this.TopMost = onTop;
            if (stealFocus)
            {
                this.WindowState = FormWindowState.Normal;
            }
            else
            {
                this.WindowState = FormWindowState.Minimized;
            }
            if(target.Contains("-"))
                this.IsGroup = true;
            this.target = target;
            InitializeComponent();

            try
            {
                string path = Directory.GetCurrentDirectory() + "\\notify.wav";
                path = new System.IO.FileInfo(path).FullName;
                this.sp = new SoundPlayer(path);
                this.sp.Load();
            }
            catch (Exception)
            {
                this.sp = null;
            }

            Contact con = ContactStore.GetContactByJid(target);

            if (con == null)
            {
                con = new Contact(-1, target, "", "", "", "");
                ContactStore.AddContact(con);
            }

            this.lblNick.Text = con.nickname;
            this.lblUserStatus.Text = con.status;
            this.SetUnavailable();

            if (this.IsGroup)
                this.ProcessGroupChat();
            else
                this.ProcessChat();

            WappMessage[] oldmessages = MessageStore.GetAllMessagesForContact(target);
            this.messages.AddRange(oldmessages);

            this.limitMessages();
            //this.redraw();
        }

        private void redraw()
        {
            this.flowLayoutPanel1.Controls.Clear();
            foreach (WappMessage msg in this.messages)
            {
                ListChat chat = new ListChat(msg);
                this.flowLayoutPanel1.Controls.Add(chat);
            }
        }

        delegate void ParentDelegate();

        public void DoActivate()
        {
            if (this.InvokeRequired)
            {
                ParentDelegate f = new ParentDelegate(DoActivate);
                this.Invoke(f);
            }
            else
            {
                //this.Show();
                bool focused = (this == Form.ActiveForm);

                if (this.sp != null && this.sp.IsLoadCompleted && !focused)
                { 
                    this.sp.Play(); 
                }
                FlashWindow(this.Handle, true);
            }
        }

        protected void ScrollToBottom()
        {
            if (this.InvokeRequired)
            {
                ParentDelegate p = new ParentDelegate(ScrollToBottom);
                this.Invoke(p);
            }
            else
            {
                this.flowLayoutPanel1.VerticalScroll.Value = this.flowLayoutPanel1.VerticalScroll.Maximum;
            }
            //int visibleItems = this.listBox1.ClientSize.Height / this.listBox1.ItemHeight;
            //this.listBox1.TopIndex = Math.Max(this.listBox1.Items.Count - visibleItems + 1, 0);
        }

        [DllImport("user32.dll")]
        static extern bool FlashWindow(IntPtr hwnd, bool bInvert);

        public void DoDispose()
        {
            if (this.InvokeRequired)
            {
                ParentDelegate f = new ParentDelegate(DoDispose);
                this.Invoke(f);
            }
            else
            {
                this.Dispose();
            }

        }

        delegate void AddMessageCallback(string data);
        delegate void AddMessageCustomCallback(string from, string type, bool fromMe);
        delegate void AddMessageCallbackNode(ProtocolTreeNode node);
        delegate void SetOnlineCallback();
        delegate void SetLastSeenCallback(DateTime time);
        delegate void SetPictureCallback(Image img);

        public void SetPicture(Image img)
        {
            if (this.label1.InvokeRequired)
            {
                SetPictureCallback t = new SetPictureCallback(SetPicture);
                this.Invoke(t, img);
            }
            else
            {
                this.pictureBox1.Image = img;
            }
        }

        public void SetLastSeen(DateTime time)
        {
            if (this.label1.InvokeRequired)
            {
                SetLastSeenCallback t = new SetLastSeenCallback(SetLastSeen);
                this.Invoke(t, time);
            }
            else
            {
                this.label1.Text = "Last seen on " + time.ToString();
            }
        }

        public void SetOnline()
        {
            if (this.label1.InvokeRequired)
            {
                SetOnlineCallback t = new SetOnlineCallback(SetOnline);
                this.Invoke(t, null);
            }
            else
            {
                this.label1.Text = "Online";
            }
        }

        public void SetUnavailable()
        {
            if (this.label1.InvokeRequired)
            {
                SetOnlineCallback t = new SetOnlineCallback(SetUnavailable);
                this.Invoke(t, null);
            }
            else
            {
                this.label1.Text = "Unavailable";
            }
        }

        public void SetTyping()
        {
            if (this.label1.InvokeRequired)
            {
                SetOnlineCallback t = new SetOnlineCallback(SetTyping);
                this.Invoke(t, null);
            }
            else
            {
                this.label1.Text = "Typing...";
            }
        }

        public void AddMessage(string message)
        {
            if (this.flowLayoutPanel1.InvokeRequired)
            {
                AddMessageCallback r = new AddMessageCallback(AddMessage);
                this.Invoke(r, new object[] {message});
            }
            else
            {
                WappMessage msg = new WappMessage(message, this.target);
                this.messages.Add(msg);
                this.limitMessages();
                MessageStore.AddMessage(msg);
                this.addChatMessage(msg);
                this.ScrollToBottom();
            }
        }

        public void AddMessage(string from, string data, bool fromMe)
        {
            if (this.flowLayoutPanel1.InvokeRequired)
            {
                AddMessageCustomCallback call = new AddMessageCustomCallback(AddMessage);
                this.Invoke(call, new object[] { from, data, fromMe } );
            }
            else
            {
                WappMessage msg = new WappMessage(from, data, fromMe);
                this.messages.Add(msg);
                this.limitMessages();
                MessageStore.AddMessage(msg);
                this.addChatMessage(msg);
                this.ScrollToBottom();
            }
        }

        private void addChatMessage(WappMessage message)
        {
            ListChat lc = new ListChat(message);
            this.flowLayoutPanel1.Controls.Add(lc);
            while (this.flowLayoutPanel1.Controls.Count > MESSAGE_LIMIT)
            {
                this.flowLayoutPanel1.Controls.Remove(this.flowLayoutPanel1.Controls[0]);
            }
        }

        //public void AddMessage(ProtocolTreeNode node)
        //{
        //    if (this.flowLayoutPanel1.InvokeRequired)
        //    {
        //        AddMessageCallbackNode r = new AddMessageCallbackNode(AddMessage);
        //        this.Invoke(r, new object[] { node });
        //    }
        //    else
        //    {
        //        string author = String.Empty;
        //        WappMessage msg = new WappMessage(node, this.target);
        //        if (this.IsGroup)
        //        {
        //            //extract author
        //            msg.author = node.GetAttribute("author");
        //        }
        //        this.messages.Add(msg);
        //        this.limitMessages();
        //        MessageStore.AddMessage(msg);
        //        this.addChatMessage(msg);
        //        this.ScrollToBottom();
        //    }
        //}

        private void limitMessages()
        {
            while (this.messages.Count > MESSAGE_LIMIT)
            {
                this.messages.Remove(this.messages.First());
            }
        }

        private void textBox1_TextChanged(object sender, EventArgs e)
        {
            if (!String.IsNullOrEmpty(this.textBox1.Text))
            {
                if (this.state == ClientState.ONLINE)
                {
                    this.state = ClientState.TYPING;
                    this.button1.Enabled = true;
                    WappSocket.Instance.SendComposing(this.target);
                }
            }
            else
            {
                if (state == ClientState.TYPING)
                {
                    this.state = ClientState.ONLINE;
                    this.button1.Enabled = false;
                    WappSocket.Instance.SendPaused(this.target);
                }
            }
        }

        private void button1_Click(object sender, EventArgs e)
        {
            if (!String.IsNullOrEmpty(this.textBox1.Text))
            {
                WappSocket.Instance.SendMessage(this.target, textBox1.Text);
                this.AddMessage(this.textBox1.Text);
                this.textBox1.Clear();
            }
        }

        private void ChatWindow_Load(object sender, EventArgs e)
        {
            this.textBox1.Focus();
            if (!this.stealFocus || !this.onTop)
            {
                this.DoActivate();
                //this.WindowState = FormWindowState.Minimized;
            }
            if (this.stealFocus)
            {
                this.Activate();
            }
            this.stealFocus = false;//do not steal focus on incoming messages
            this.flowLayoutPanel1.HorizontalScroll.Visible = false;
            //Thread fill = new Thread(new ParameterizedThreadStart(redraw));
            //fill.IsBackground = true;
            //fill.Start();
            this.redraw();
            this.ScrollToBottom();
            this.IsLoaded = true;
        }

        private void pictureBox1_Click(object sender, EventArgs e)
        {
            //redownload image
            this.pictureBox1.Image.Dispose();
            GetImageAsync(this.target, false);
        }

        protected override bool ShowWithoutActivation
        {
            get
            {
                return false;
            }
        }

        private void MediaPick()
        {
            OpenFileDialog dial = new OpenFileDialog();
            if (dial.ShowDialog() == System.Windows.Forms.DialogResult.OK)
            {
                string filename = dial.FileName;
                if (File.Exists(filename))
                {
                    filename = this.copyFileLocal(filename);

                    WappSocket.Instance.SendMessageImage(this.target, File.ReadAllBytes(filename), WhatsAppApi.ApiBase.ImageType.JPEG);
                    //this.AddMessage(filename, "image");
                    this.ClearTextbox();
                }
            }
        }

        delegate void ClearTextboxDelegate();
        private void ClearTextbox()
        {
            if (this.textBox1.InvokeRequired)
            {
                ClearTextboxDelegate d = new ClearTextboxDelegate(ClearTextbox);
                this.Invoke(d);
            }
            else
            {
                this.textBox1.Clear();
            }
        }

        private void Button1_Click(object sender, EventArgs e)
        {
            Thread t = new Thread(new ThreadStart(MediaPick));
            t.SetApartmentState(ApartmentState.STA);
            t.IsBackground = true;
            t.Start();
        }

        private string copyFileLocal(string filename)
        {
            string dst = Directory.GetCurrentDirectory() + "\\data\\media\\" + filename.Split('\\').Last();
            if (File.Exists(dst))
            {
                File.Delete(dst);
            }
            File.Copy(filename, dst);
            return dst;
        }
    }
}
