using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using WinAppNET.AppCode;
using System.Data.Common;
using System.Data.SQLite;
using System.Threading;
using System.IO;
using WhatsAppApi.Helper;
using WinAppNET.Controls;
using WinAppNET.Dialogs;
using System.Configuration;

namespace WinAppNET
{
    public partial class ContactsList : Form
    {
        public BindingList<Contact> contacts = new BindingList<Contact>();
        public Dictionary<string, ChatWindow> ChatWindows = new Dictionary<string, ChatWindow>();
        protected string username;
        protected string password;
        private static List<string> picturesToSync = new List<string>();
        private static Thread WAlistener;

        private string GetPassword()
        {
            string pass = System.Configuration.ConfigurationManager.AppSettings.Get("Password");
            try
            {
                byte[] foo = Convert.FromBase64String(pass);
                return pass;
            }
            catch (Exception)
            {
                return string.Empty;
            }
        }

        private string getUsername()
        {
            return System.Configuration.ConfigurationManager.AppSettings.Get("Username");
        }

        public ContactsList()
        {
            InitializeComponent();
            this.FormClosing += this.ContactsList_OnClosing;
        }

        private void ContactsList_OnClosing(object sender, System.ComponentModel.CancelEventArgs e)
        {
            foreach (ChatWindow w in this.ChatWindows.Values)
            {
                //close chat windows
                w.DoDispose();
            }
        }

        delegate void remoteDelegate();

        protected void _loadConversations()
        {
            
            if (this.InvokeRequired)
            {
                remoteDelegate r = new remoteDelegate(_loadConversations);
                this.Invoke(r);
            }
            else
            {
                this.flowLayoutPanel1.Controls.Clear();
                DbProviderFactory fact = DbProviderFactories.GetFactory("System.Data.SQLite");
                using (DbConnection cnn = fact.CreateConnection())
                {
                    cnn.ConnectionString = MessageStore.ConnectionString;
                    cnn.Open();
                    DbCommand cmd = cnn.CreateCommand();
                    cmd = cnn.CreateCommand();
                    cmd.CommandText = "SELECT jid FROM Messages GROUP BY jid";
                    DbDataReader reader = cmd.ExecuteReader();
                    while (reader.Read())
                    {
                        string jid = reader["jid"].ToString();
                        Contact contact = ContactStore.GetContactByJid(jid);
                        if (contact != null)
                        {
                            this.contacts.Add(contact);

                            //add to richtextbox
                            ListContact c = new ListContact(contact.jid);
                            c.DoubleClick += this.contact_dblClick;
                            this.flowLayoutPanel1.Controls.Add(c);
                        }
                    }
                }

                //done
                this.label1.Hide();
            }
        }

        protected void SyncWaContactsAsync()
        {
            ContactStore.SyncWaContacts();
            //resync pictures
            picturesToSync = this.refreshContactPictures();
            this.requestProfilePicture();

            this._loadConversations();
        }

        private void contact_dblClick(object sender, EventArgs e)
        {
            ListContact c = sender as ListContact;
            if (c != null)
            {
                this.OpenConversationThread(c.contact.jid, true, true);
            }
        }

        public void OpenConversation(object jid)
        {
            Helper.ChatWindowParameters cwp = jid as Helper.ChatWindowParameters;
            this._openConversation(cwp.jid, cwp.stealFocus, cwp.onTop);
        }

        protected void _openConversation(object jid, bool stealFocus, bool onTop)
        {
            if (!this.ChatWindows.ContainsKey(jid.ToString()))
            {
                ChatWindow c = new ChatWindow(jid.ToString(), stealFocus, onTop);
                c.TopMost = false;
                this.ChatWindows.Add(jid.ToString(), c);//create
            }
            else if (this.ChatWindows[jid.ToString()].IsDisposed)
            {
                ChatWindow c = new ChatWindow(jid.ToString(), stealFocus, onTop);
                c.TopMost = false;
                this.ChatWindows[jid.ToString()] = c;//renew
            }
            else
            {
                this.ChatWindows[jid.ToString()].DoActivate();
                return;
            }
            try
            {
                Application.Run(this.ChatWindows[jid.ToString()]);
            }
            catch (Exception e)
            {
                throw e;
            }
        }

        public void OpenConversationThread(string jid, bool stealFocus, bool onTop)
        {
            Helper.ChatWindowParameters cwp = new Helper.ChatWindowParameters(jid, stealFocus, onTop);
            try
            {
                Thread t = new Thread(new ParameterizedThreadStart(OpenConversation));
                t.IsBackground = true;
                t.Start(cwp);
            }
            catch (Exception e)
            {
                throw e;
            }
        }

        protected ChatWindow getChat(string jid, bool forceOpen, bool onTop)
        {
            if (forceOpen)
            {
                this.OpenConversationThread(jid, forceOpen, onTop);
            }

            if (this.ChatWindows.ContainsKey(jid) && !this.ChatWindows[jid].IsDisposed)
            {
                return this.ChatWindows[jid];
            }

            while (forceOpen)
            {
                Thread.Sleep(100);
                if (this.ChatWindows.ContainsKey(jid) && this.ChatWindows[jid].IsLoaded)
                {
                    return this.ChatWindows[jid];
                }
            }
            return null;
        }

        //protected void ProcessMessages(ProtocolTreeNode[] nodes)
        //{
        //    foreach (ProtocolTreeNode node in nodes)
        //    {
        //        if (node.tag.Equals("message"))
        //        {
        //            ProtocolTreeNode body = node.GetChild("body");
        //            ProtocolTreeNode media = node.GetChild("media");
        //            ProtocolTreeNode paused = node.GetChild("paused");
        //            ProtocolTreeNode composing = node.GetChild("composing");
        //            ProtocolTreeNode notification = node.GetChild("notification");
        //            string jid = node.GetAttribute("from");

        //            if (body != null || media != null)
        //            {
        //                //extract and save nickname
        //                if (node.GetChild("notify") != null && node.GetChild("notify").GetAttribute("name") != null)
        //                {
        //                    string nick = node.GetChild("notify").GetAttribute("name");
        //                    Contact c = ContactStore.GetContactByJid(jid);
        //                    if (c != null)
        //                    {
        //                        c.nickname = nick;
        //                        ContactStore.UpdateNickname(c);
        //                    }
        //                }

        //                try
        //                {
        //                    this.getChat(jid, true, false).AddMessage(node);
        //                }
        //                catch (Exception)
        //                { }

        //                //refresh list
        //                this._loadConversations();
        //            }
        //            if (paused != null)
        //            {
        //                try
        //                {
        //                    if (this.getChat(jid, false, false) != null)
        //                    {
        //                        this.getChat(jid, false, false).SetOnline();
        //                    }
        //                }
        //                catch (Exception) 
        //                {
        //                    //throw e;
        //                }
        //            }
        //            if (composing != null)
        //            {
        //                try
        //                {
        //                    if (this.getChat(jid, false, false) != null)
        //                    {
        //                        this.getChat(jid, false, false).SetTyping();
        //                    }
        //                }
        //                catch (Exception) 
        //                {
        //                    //throw e;
        //                }
        //            }
        //            if (notification != null)
        //            {
        //                if (notification.GetAttribute("type") == "picture" && notification.GetChild("set") != null)
        //                {
        //                    ChatWindow.GetImageAsync(notification.GetChild("set").GetAttribute("jid"), false);
        //                }
        //            }
        //        }
        //        else if (node.tag.Equals("presence"))
        //        {
        //            string jid = node.GetAttribute("from");
        //            if (node.GetAttribute("type") != null && node.GetAttribute("type").Equals("available"))
        //            {
        //                try
        //                {
        //                    if (this.getChat(jid, false, false) != null)
        //                    {
        //                        this.getChat(jid, false, false).SetOnline();
        //                    }
        //                }
        //                catch (Exception) 
        //                {
        //                    //throw e;
        //                }
        //            }
        //            if (node.GetAttribute("type") != null && node.GetAttribute("type").Equals("unavailable"))
        //            {
        //                try
        //                {
        //                    if (this.getChat(jid, false, false) != null)
        //                    {
        //                        this.getChat(jid, false, false).SetUnavailable();
        //                    }
        //                }
        //                catch (Exception) 
        //                { }
        //            }
        //        }
        //        else if (node.tag.Equals("iq"))
        //        {
        //            string jid = node.GetAttribute("from");

        //            if (node.children.First().tag.Equals("query"))
        //            {
        //                DateTime lastseen = DateTime.Now;
        //                int seconds = Int32.Parse(node.GetChild("query").GetAttribute("seconds"));
        //                lastseen = lastseen.Subtract(new TimeSpan(0, 0, seconds));
        //                try
        //                {
        //                    if (this.getChat(jid, false, false) != null)
        //                    {
        //                        getChat(jid, false, false).SetLastSeen(lastseen);
        //                    }
        //                }
        //                catch (Exception)
        //                { }
        //            }
        //            else if (node.children.First().tag.Equals("group"))
        //            {
        //                string subject = node.children.First().GetAttribute("subject");
        //                Contact cont = ContactStore.GetContactByJid(jid);
        //                if (cont != null)
        //                {
        //                    cont.nickname = subject;
        //                    ContactStore.UpdateNickname(cont);
        //                }
        //                else
        //                {

        //                }
        //            }
        //            else if (node.children.First().tag.Equals("picture"))
        //            {
        //                string pjid = node.GetAttribute("from");
        //                string id = node.GetAttribute("id");
        //                byte[] rawpicture = node.GetChild("picture").GetData();
        //                Contact c = ContactStore.GetContactByJid(pjid);
        //                if (c != null)
        //                {
        //                    Image img = null;
        //                    using (var ms = new MemoryStream(rawpicture))
        //                    {
        //                        try
        //                        {
        //                            img = Image.FromStream(ms);
        //                            string targetdir = Directory.GetCurrentDirectory() + "\\data\\profilecache";
        //                            if (!Directory.Exists(targetdir))
        //                            {
        //                                Directory.CreateDirectory(targetdir);
        //                            }
        //                            img.Save(targetdir + "\\" + c.jid + ".jpg");
        //                            try
        //                            {
        //                                if (this.getChat(pjid, false, false) != null)
        //                                {
        //                                    this.getChat(pjid, false, false).SetPicture(img);
        //                                }
        //                            }
        //                            catch (Exception e)
        //                            {
        //                                throw e;
        //                            }
        //                        }
        //                        catch (Exception e)
        //                        {
        //                            throw e;
        //                        }
        //                    }
        //                }
        //                if(picturesToSync.Remove(pjid))
        //                    this.requestProfilePicture();
                        
        //            }
        //            else if (node.children.First().tag.Equals("error") && node.children.First().GetAttribute("code") == "404")
        //            {
        //                string pjid = node.GetAttribute("from");
        //                picturesToSync.Remove(pjid);
        //                this.requestProfilePicture();
        //            }
        //        }
        //    }
        //}

        private void KeepAlive()
        {
            while (true)
            {
                Thread.Sleep(150000);
                WappSocket.Instance.SendActive();
            }
        }

        protected void Listen()
        {
            while (true)
            {
                try
                {
                    WappSocket.Instance.PollMessages();
                }
                catch (Exception)
                {
                    //reset
                    WappSocket.Instance.Disconnect();
                    WappSocket.Instance.Connect();
                    return;
                }
                Thread.Sleep(500);
            }
        }

        private void doRefreshContactPictures()
        {
            picturesToSync = this.refreshContactPictures();
            this.requestProfilePicture();
        }

        private List<string> refreshContactPictures()
        {
            Contact[] contacts = ContactStore.GetAllContacts();
            List<string> toSync = new List<string>();
            foreach (Contact c in contacts)
            {
                string path = ChatWindow.getCacheImagePath(c.jid);
                if(!File.Exists(path))
                {
                    toSync.Add(c.jid);
                }
            }
            return toSync;
        }

        private bool checkCredentials()
        {
            if (!(WappSocket.Instance != null && WappSocket.Instance.ConnectionStatus == WhatsAppApi.WhatsApp.CONNECTION_STATUS.LOGGEDIN))
            {
                this.username = this.getUsername();
                this.password = this.GetPassword();
                if (!string.IsNullOrEmpty(this.username) && !string.IsNullOrEmpty(this.password))
                {
                    WappSocket.Create(username, password, "WinApp.NET", false);
                    WappSocket.Instance.Connect();
                    WappSocket.Instance.Login();
                    if (WappSocket.Instance.ConnectionStatus == WhatsAppApi.WhatsApp.CONNECTION_STATUS.LOGGEDIN)
                    {
                        return true;
                    }
                    else
                    {
                        WappSocket.Instance.Disconnect();
                    }
                }
                return false;
            }
            else
            {
                return true;
            }
        }

        private void ContactsList_Load(object sender, EventArgs e)
        {
            string nickname = "WhatsAPINet";
            this.username = this.getUsername();
            this.password = this.GetPassword();

            ContactStore.CheckTable();
            MessageStore.CheckTable();

            WappSocket.Create(this.username, this.password, nickname, true);
            
            this.bindAll();

            Thread t = new Thread(new ThreadStart(WappSocket.Instance.Connect));
            t.IsBackground = true;
            t.Start();

            int vertScrollWidth = SystemInformation.VerticalScrollBarWidth;
            this.flowLayoutPanel1.Padding = new Padding(0, 0, vertScrollWidth, 0);

            this._loadConversations();
        }

        private void bindAll()
        {
            //bindings
            WappSocket.Instance.OnConnectFailed += Instance_OnConnectFailed;
            WappSocket.Instance.OnConnectSuccess += Instance_OnConnectSuccess;
            WappSocket.Instance.OnDisconnect += Instance_OnDisconnect;
            WappSocket.Instance.OnError += Instance_OnError;
            WappSocket.Instance.OnGetGroupParticipants += Instance_OnGetGroupParticipants;
            WappSocket.Instance.OnGetGroups += Instance_OnGetGroups;
            WappSocket.Instance.OnGetLastSeen += Instance_OnGetLastSeen;
            WappSocket.Instance.OnGetMessage += Instance_OnGetMessage;
            WappSocket.Instance.OnGetContactName += Instance_OnGetContactName;
            WappSocket.Instance.OnGetMessageAudio += Instance_OnGetMessageAudio;
            WappSocket.Instance.OnGetMessageImage += Instance_OnGetMessageImage;
            WappSocket.Instance.OnGetMessageLocation += Instance_OnGetMessageLocation;
            WappSocket.Instance.OnGetMessageReceivedClient += Instance_OnGetMessageReceivedClient;
            WappSocket.Instance.OnGetMessageReceivedServer += Instance_OnGetMessageReceivedServer;
            WappSocket.Instance.OnGetMessageVcard += Instance_OnGetMessageVcard;
            WappSocket.Instance.OnGetMessageVideo += Instance_OnGetMessageVideo;
            WappSocket.Instance.OnGetPaused += Instance_OnGetPaused;
            WappSocket.Instance.OnGetPhoto += Instance_OnGetPhoto;
            WappSocket.Instance.OnGetPhotoPreview += Instance_OnGetPhotoPreview;
            WappSocket.Instance.OnGetPresence += Instance_OnGetPresence;
            WappSocket.Instance.OnGetTyping += Instance_OnGetTyping;
            WappSocket.Instance.OnLoginFailed += Instance_OnLoginFailed;
            WappSocket.Instance.OnLoginSuccess += Instance_OnLoginSuccess;
            WappSocket.Instance.OnNotificationPicture += Instance_OnNotificationPicture;
            WappSocket.Instance.OnGetSyncResult += Instance_OnGetSyncResult;
        }

        void Instance_OnGetSyncResult(int index, string sid, Dictionary<string, string> existingUsers, string[] failedNumbers)
        {
            ContactStore.OnSyncResult(existingUsers, failedNumbers);
        }

        private void Instance_OnGetGroups(WhatsAppApi.Response.WaGroupInfo[] groups)
        {
            throw new NotImplementedException();
        }

        void Instance_OnGetContactName(string from, string contactName)
        {
            this.updateNick(from, contactName);
        }

        void Instance_OnNotificationPicture(string type, string jid, string id)
        {
            throw new NotImplementedException();
        }

        void Instance_OnLoginSuccess(string number, byte[] data)
        {
            this.saveConfig();

            WAlistener = new Thread(new ThreadStart(Listen));
            WAlistener.IsBackground = true;
            WAlistener.Start();

            Thread alive = new Thread(new ThreadStart(KeepAlive));
            alive.IsBackground = true;
            alive.Start();

            Thread picsync = new Thread(new ThreadStart(this.doRefreshContactPictures));
            picsync.IsBackground = true;
            picsync.Start();
        }

        void Instance_OnLoginFailed(string data)
        {
            if (data == "not-authorized")
            {
                //show reg form
                    WappCredentials creds = new WappCredentials();
                    DialogResult r = creds.ShowDialog();
                    if (r != System.Windows.Forms.DialogResult.OK)
                    {
                        //cancelled, close application
                        Application.Exit();
                        return;
                    }
                    this.username = this.getUsername();
                    this.password = this.GetPassword();
                    if (!string.IsNullOrEmpty(this.username) && !string.IsNullOrEmpty(this.password))
                    {
                        WappSocket.Create(this.username, this.password, "WinApp.NET", true);
                        this.bindAll();
                        WappSocket.Instance.Connect();
                    }
            }
            if (data == "blocked")
            {
                throw new Exception("blocked!");
            }
        }

        private void saveConfig()
        {
            Configuration config = ConfigurationManager.OpenExeConfiguration(ConfigurationUserLevel.None);
            AppSettingsSection app = config.AppSettings;
            app.Settings.Remove("Username");
            app.Settings.Add("Username", this.username);
            app.Settings.Remove("Password");
            app.Settings.Add("Password", this.password);
            config.Save(ConfigurationSaveMode.Modified);
        }

        void Instance_OnGetTyping(string from)
        {
            try
            {
                this.getChat(from, false, false).SetTyping();
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex);
            }
        }

        void Instance_OnGetPresence(string from, string type)
        {
            if (!from.StartsWith(this.username))
            {
                try
                {
                    switch (type)
                    {
                        case "available":
                            this.getChat(from, false, false).SetOnline();
                            break;
                        case "unavailable":
                            this.getChat(from, false, false).SetUnavailable();
                            break;
                        default:
                            throw new Exception(type);
                    }
                }
                catch (Exception ex)
                {
                    Console.WriteLine(ex);
                }
            }
        }

        void Instance_OnGetPhotoPreview(string from, string id, byte[] data)
        {
            Contact c = ContactStore.GetContactByJid(from);
            if (c != null)
            {
                Image img = null;
                using (var ms = new MemoryStream(data))
                {
                    try
                    {
                        img = Image.FromStream(ms);
                        string targetdir = Directory.GetCurrentDirectory() + "\\data\\profilecache";
                        if (!Directory.Exists(targetdir))
                        {
                            Directory.CreateDirectory(targetdir);
                        }
                        img.Save(targetdir + "\\" + c.jid + ".jpg");
                        try
                        {
                            if (this.getChat(from, false, false) != null)
                            {
                                this.getChat(from, false, false).SetPicture(img);
                            }
                        }
                        catch (Exception e)
                        {
                            throw e;
                        }
                    }
                    catch (Exception e)
                    {
                        throw e;
                    }
                }
            }
            if (picturesToSync.Remove(from))
            {
                this.requestProfilePicture();
            }
        }

        void Instance_OnGetPhoto(string from, string id, byte[] data)
        {
            throw new NotImplementedException();
        }

        void Instance_OnGetPaused(string from)
        {
            try
            {
                this.getChat(from, false, false).SetOnline();
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex);
            }
        }

        void Instance_OnGetMessageVideo(string from, string id, string fileName, int fileSize, string url, byte[] preview)
        {
            throw new NotImplementedException();
        }

        void Instance_OnGetMessageVcard(string from, string id, string name, byte[] data)
        {
            throw new NotImplementedException();
        }

        void Instance_OnGetMessageReceivedServer(string from, string id)
        {
            //throw new NotImplementedException();
        }

        void Instance_OnGetMessageReceivedClient(string from, string id)
        {
            //throw new NotImplementedException();
        }

        void Instance_OnGetMessageLocation(string from, string id, double lon, double lat, string url, string name, byte[] preview)
        {
            throw new NotImplementedException();
        }
        delegate void SetImgCallback(string from, string id, string fileName, int fileSize, string url, byte[] preview);
        void Instance_OnGetMessageImage(string from, string id, string fileName, int fileSize, string url, byte[] preview)
        {
            PictureBox PictureBox = new PictureBox
            {
                Name = "pictureBox1",
                Size = new Size(316, 320),
                BorderStyle = BorderStyle.FixedSingle,
                SizeMode = PictureBoxSizeMode.Zoom
            };
            PictureBox.ImageLocation = "c:\\1.jpg";
            ChatWindow cw = this.getChat(from, true, true);

            if (cw.InvokeRequired)
            {
                SetImgCallback s = new SetImgCallback(Instance_OnGetMessageImage);
                this.Invoke(s, new object[] { from, id, fileName, fileSize, url, preview });
            }
            else
            {
                cw.Controls.Add(PictureBox);
            }
           // Invoke(new Action(() => this.Controls.Add(PictureBox)));
           // Invoke(new Action(() => 
                
                
                
                
            //    ));
            //this.getChat(from, true, true).Controls.Add(PictureBox);
            
            //throw new NotImplementedException();
        }

        void Instance_OnGetMessageAudio(string from, string id, string fileName, int fileSize, string url, byte[] preview)
        {
            throw new NotImplementedException();
        }

        void Instance_OnGetMessage(ProtocolTreeNode messageNode, string from, string id, string name, string message, bool receipt_sent)
        {
            try
            {
                this.getChat(from, true, true).AddMessage(from, message, (from.StartsWith(this.username)));
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex);
            }
            this._loadConversations();
        }

        void Instance_OnGetLastSeen(string from, DateTime lastSeen)
        {
            try
            {
                this.getChat(from, false, false).SetLastSeen(lastSeen);
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex);
            }
        }

        void Instance_OnGetGroupParticipants(string gjid, string[] jids)
        {
            throw new NotImplementedException();
        }

        void Instance_OnError(string id, string from, int code, string text)
        {
            if(id.StartsWith("get_photo_"))
            {
                if (picturesToSync.Remove(from))
                {
                    this.requestProfilePicture();
                }
            }
        }

        void Instance_OnDisconnect(Exception ex)
        {
            if (ex != null)
            {
                throw new NotImplementedException();
            }
        }

        void Instance_OnConnectSuccess()
        {
            WappSocket.Instance.Login();
        }

        void Instance_OnConnectFailed(Exception ex)
        {
            throw new NotImplementedException();
        }

        private void updateNick(string jid, string nick)
        {
            Contact c = ContactStore.GetContactByJid(jid);
            if (c != null)
            {
                c.nickname = nick;
                ContactStore.UpdateNickname(c);
            }
        }

        private void requestProfilePicture()
        {
            if (picturesToSync.Count > 0)
            {
                string jid = picturesToSync.First();
                ChatWindow.GetImageAsync(jid, false);
            }
        }

        private void tileNew_Click(object sender, EventArgs e)
        {
            ContactsSelector selector = new ContactsSelector();

            DialogResult res = selector.ShowDialog();
            if (res == DialogResult.OK)
            {
                this.OpenConversationThread(selector.SelectedJID, true, true);
            }
            selector.Dispose();
        }

        private void tileGoogle_Click(object sender, EventArgs e)
        {
            //sync contacts
            Dialogs.frmGoogleSync gsync = new Dialogs.frmGoogleSync();
            gsync.ShowDialog();

            //reset
            this.contacts.Clear();
            this.label1.Text = "Updating contacts...";
            this.label1.Show();

            //picturesToSync = this.refreshContactPictures();
            //this.requestProfilePicture();

            Thread t = new Thread(new ThreadStart(SyncWaContactsAsync));
            t.IsBackground = true;
            t.Start();
        }
    }
}
