using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using System.Windows.Media.Effects;
using System.Windows.Media.Animation;
using System.IO;
using System.Net;
using System.Threading;
using WhatsAppApi;
using WhatsAppApi.Helper;
using WhatsAppApi.Account;
using System.Windows.Threading;
using log4net;

namespace whatsAppShowerWpf
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        string nickname = "test12";
        bool showExample = false;
        string sender = "972524376363"; // Mobile number with country code (but without + or 00)
        string password = "TicJAMworhafW+84w3vuA4yMS5o=";//v2 password
        string target = "972504219841";// Mobile number to send the message to
        private static readonly ILog msgsLog = log4net.LogManager.GetLogger("msgsLog");
        private static readonly ILog systemLog = log4net.LogManager.GetLogger("systemsLog");
        private static readonly ILog msgHistoryLog = log4net.LogManager.GetLogger("msgHistory");
        private DispatcherTimer sideImageTimer = new DispatcherTimer();
        Queue<string> queue = new Queue<string>();

        public string Nickname
        {
            get { return nickname; }
            set { nickname = value; }
        }
        public string Sender
        {
            get { return sender; }
            set { sender = value; }
        }
        public string Password
        {
            get { return password; }
            set { password = value; }
        }
        public string Target
        {
            get { return target; }
            set { target = value; }
        }
        public bool ShowExample
        {
            get { return showExample; }
            set { showExample = value; }
        }

        public MainWindow()
        {
           
          Login login = new Login();
          login.ShowDialog();
          InitializeComponent();
          if (login.DialogResult.HasValue && login.DialogResult.Value){
              string phoneNumber = login.txtUserName.Text;
              this.Sender = phoneNumber;
              string token = login.txtPassword.Password;
              this.Password = token;
              initWhatsAppConnect();
            }
            else
            {
                addTexts();
                //addImages();
                //addImageSide();
            }
          if (WhatsappProperties.Instance.ShowSideImages)
          {
              sideImageTimer.Tick += new EventHandler(startSideMethod); 
              sideImageTimer.Interval = TimeSpan.FromSeconds(WhatsappProperties.Instance.SideImageRunEveryInSec);
              sideImageTimer.Start();
          }
            
            this.WindowState =  WindowState.Maximized;
            startRunningText();
        }

        private void addImageSide()
        {
            queue.Enqueue(@"C:\whatsappPicTest\1.jpg");
            queue.Enqueue(@"C:\whatsappPicTest\2.jpg");
            queue.Enqueue(@"C:\whatsappPicTest\3.jpg");
            queue.Enqueue(@"C:\whatsappPicTest\4.jpg");
         }
        private void startSideMethod(object sender, EventArgs e2)
        {
            
            
            Image image = this.sideImage;
            TimeSpan fadeOutTime = TimeSpan.FromSeconds(WhatsappProperties.Instance.SideImagefadingSpeedInSec);
            TimeSpan fadeInTime = TimeSpan.FromSeconds(WhatsappProperties.Instance.SideImagefadingSpeedInSec);
           

            var fadeInAnimation = new DoubleAnimation(1d, fadeInTime);
            var fadeOutAnimation = new DoubleAnimation(0d, fadeOutTime);
            if(queue.Count==0){
                image.BeginAnimation(Image.OpacityProperty, fadeOutAnimation);
                return;
            }
            string imgUrl = queue.Dequeue();
            BitmapImage logo = new BitmapImage();
            logo.BeginInit();
            logo.UriSource = new Uri(imgUrl);
            logo.EndInit();
            ImageSource source = logo;
            
            if (image.Source != null)
            {
                

                fadeOutAnimation.Completed += (o, e) =>
                {
                    image.Source = source;
                    image.BeginAnimation(Image.OpacityProperty, fadeInAnimation);
                };

                image.BeginAnimation(Image.OpacityProperty, fadeOutAnimation);
            }
            else
            {
                image.Opacity = 0d;
                image.Source = source;
                image.BeginAnimation(Image.OpacityProperty, fadeInAnimation);
            }
        }

       

        private void initWhatsAppConnect()
        {
            WhatsApp wa = new WhatsApp(Sender, Password, Nickname, true);
            wa.OnGetMessage += wa_OnGetMessage;
            //wa.OnGetPhoto += wa_OnGetPhoto;
            wa.OnGetMessageImage += wa_OnGetMessageImage;
            wa.OnConnectFailed += new WhatsEventBase.ExceptionDelegate(Instance_OnConnectFailed);
            WhatsAppApi.Helper.DebugAdapter.Instance.OnPrintDebug += Instance_OnPrintDebug;
            wa.Connect();
            
            try
            {
                wa.Login();
            }
            catch (Exception e)
            {
                MessageBox.Show(this, "Login failed resone: " + e, "Error", MessageBoxButton.OK, MessageBoxImage.Error);
                return;
            }
            if (wa.ConnectionStatus != WhatsAppApi.WhatsApp.CONNECTION_STATUS.LOGGEDIN)
            {
                MessageBox.Show(this, "Login failed resone: " + wa.ConnectionStatus, "Error", MessageBoxButton.OK, MessageBoxImage.Error);
                return;
            }
            ProcessChat(wa, target);
        }

        private void addImages()
        {
           
            ImgView imgView = new ImgView("0524376464", (ImageSource)new ImageSourceConverter().ConvertFromString(@"C:\whatsappPicTest\1.jpg"), "10:10");
            this.stackPanel1.Children.Add(imgView);
            ImgView imgView2 = new ImgView("0524376464", (ImageSource)new ImageSourceConverter().ConvertFromString(@"C:\whatsappPicTest\2.jpg"), "10:10");
            this.stackPanel1.Children.Add(imgView2);
            ImgView imgView3 = new ImgView("0524376464", (ImageSource)new ImageSourceConverter().ConvertFromString(@"C:\whatsappPicTest\3.jpg"), "10:10");
            this.stackPanel1.Children.Add(imgView3);
            ImgView imgView4 = new ImgView("0524376464", (ImageSource)new ImageSourceConverter().ConvertFromString(@"C:\whatsappPicTest\4.jpg"), "10:10");
            this.stackPanel1.Children.Add(imgView4);
            this.stackPanelScroller.ScrollToBottom();
        }

        private void addTexts()
        {
            TextView textView2 = new TextView("0524376464", "", "10:10");
            this.stackPanel1.Children.Add(textView2);
            for (int i = 0; i < 30; i++)
            {
                this.stackPanel1.Children.Add(new TextView("0524376464"+i, "מזל טוב ומבורך וובדיקה לטקסט ארוך מאוד מאוד מאוד לבדיקה שהוא טקסט ארוך מאוד מאוד מאוד מאוד נראה איך הוא יהיה", "10:10"));
                this.stackPanelScroller.ScrollToBottom();
            }
            this.stackPanelScroller.ScrollToBottom();
       }
       
        void wa_OnGetMessage(ProtocolTreeNode node, string from, string id, string name, string message, bool receipt_sent)
        {
            if (isCommandOpMsg(from, message))
            {
                handleCommandOpMag(from, message);
                return;
            }
            addText(from, getNikeName(node), message, DateTime.Now.ToString("HH:mm"));
        }
       
        private void addText(string phoneNumber,string nikeName ,string msgText, string hour)
        {
            phoneNumber = filterFromNumber(phoneNumber);
             bool isCanShowMsg = isCanShowMsgMet(phoneNumber, "TEXT");
             if (isCanShowMsg)
             {
                 TextView textView2 = null;
                 this.stackPanel1.Dispatcher.BeginInvoke(new Action(() => { textView2 = new TextView(phoneNumber + " - " + nikeName, msgText, hour); }));
                 this.stackPanel1.Dispatcher.BeginInvoke(new Action(() => { this.stackPanel1.Children.Add(textView2); }));
                 this.stackPanel1.Dispatcher.BeginInvoke(new Action(() => { this.stackPanelScroller.ScrollToBottom(); }));
             }
             addTextInfoToLog("Text", msgText, phoneNumber, isCanShowMsgMet(phoneNumber, "TEXT"));
            
            
        }
        
        void wa_OnGetMessageImage(string from, string id, string fileName, int size, string url, byte[] preview)
        {
            OnGetMedia(fileName, url, preview);
            addImg(from, fileName, DateTime.Now.ToString("HH:mm"));
        }
       
        static void OnGetMedia(string file, string url, byte[] data)
        {
            //save preview
            File.WriteAllBytes(string.Format("preview_{0}.jpg", file), data);
            //download
            using (WebClient wc = new WebClient())
            {
                
                if (!File.Exists(file)) { 
                    wc.DownloadFile(new Uri(url), file);
                }
            }
        }
        
        private void addImg(string phoneNumber, string imgFileName, string hour)
        {
            phoneNumber = filterFromNumber(phoneNumber) ;
            if (isCanShowMsgMet(phoneNumber, "IMG"))
            {
                ImgView imgView = null;
                this.stackPanel1.Dispatcher.BeginInvoke(new Action(() => { imgView = new ImgView(phoneNumber, (ImageSource)new ImageSourceConverter().ConvertFromString(imgFileName), hour); }));
                this.stackPanel1.Dispatcher.Invoke((Action)(() => { this.stackPanel1.Children.Add(imgView); }));
                this.stackPanel1.Dispatcher.Invoke((Action)(() => { this.stackPanelScroller.ScrollToBottom(); }));
                queue.Enqueue(Environment.CurrentDirectory+@"\"+imgFileName);
            }
            addTextInfoToLog("IMG", imgFileName, phoneNumber, isCanShowMsgMet(phoneNumber, "IMG"));
        }


        String filterFromNumber(String from)
        {
            char[] splitChar = { '@' };
            return from.Split(splitChar)[0];
        }
        private string getNikeName(ProtocolTreeNode node)
        {
            string nickName = "";
            try
            {
                List<KeyValue> attributeHashList = node.attributeHash.ToList();
                for (int i = 0; i < attributeHashList.Count; i++)
                {
                    if (attributeHashList[i] != null && attributeHashList[i].Key.Equals("notify"))
                    {
                        return attributeHashList[i].Value;
                    }
                }
            }
            catch (Exception) { }
            return nickName;
        }
        bool isCanShowMsgMet(string from, string type)
        {
            return NumberPropList.Instance.isCanShowMsg(from, type);
        }
        private void addTextInfoToLog(string type, string text, string phoneNumber, bool isShowen)
        {
            msgsLog.Info(isShowen + " " + type + " From: " + phoneNumber + ": " + text);
        }
        private bool isCommandOpMsg(string from, string message)
        {
            bool isCommandOpMsg = false;
            if (string.IsNullOrEmpty(message) || !message.StartsWith("op"))
            {
                return isCommandOpMsg;
            }

            bool isCanGetCommandsFromThisNumber = false;
            string commandsOpOnlyFrom = WhatsappProperties.Instance.CommandsOpOnlyFrom;
            if (!string.IsNullOrEmpty(commandsOpOnlyFrom))
            {
                string[] commandsOpOnlyFromNumbers = commandsOpOnlyFrom.Split(',');
                if (commandsOpOnlyFromNumbers != null && commandsOpOnlyFromNumbers.Length > 0)
                {
                    foreach (string number in commandsOpOnlyFromNumbers)
                    {
                        if (from.Contains(number))
                        {
                            isCanGetCommandsFromThisNumber = true;
                            break;
                        }
                    }
                }
            }
            else
            {
                isCanGetCommandsFromThisNumber = true;
            }

            if (isCanGetCommandsFromThisNumber)
            {
                string commandsOpPassword = WhatsappProperties.Instance.CommandsOpPassword;
                if (!string.IsNullOrEmpty(commandsOpPassword))
                {
                    string[] opMsgProp = message.Split(',');
                    if (opMsgProp.Length >= 3)
                    {
                        string pass = opMsgProp[1];
                        if (commandsOpPassword.Equals(pass))
                        {
                            return true;
                        }
                    }

                }
            }
            return isCommandOpMsg;
        }

        private void handleCommandOpMag(string from, string message)
        {
            if (string.IsNullOrEmpty(message))
            {
                return;
            }
            string[] messageProp = message.Split(',');
            if (messageProp != null && messageProp.Length >= 3)
            {
                string msgOp = messageProp[2];
                if ("clns".Equals(msgOp))
                {
                    if (messageProp.Length >= 4)
                    {
                        int i = 0;
                        bool result = int.TryParse(messageProp[3], out i);
                        if (result)
                        {
                            handelCleanScreenOpMsg(i);
                        }
                    }
                    else
                    {
                        handelCleanScreenOpMsg();
                    }
                }
            }

        }
        private void handelCleanScreenOpMsg()
        {
            handelCleanScreenOpMsg(-1);
        }

        private void handelCleanScreenOpMsg(int controlToRemoveCount)
        {
            if (controlToRemoveCount == -1)
            {
                this.stackPanel1.Dispatcher.Invoke((Action)(() => { this.stackPanel1.Children.Clear(); }));
            }
            else
            {
                int controlCount = 0;
                this.stackPanel1.Dispatcher.Invoke((Action)(() => { controlCount = this.stackPanel1.Children.Count; }));
                int controlToremove = controlCount - 1;
                while (controlCount > 0 && controlToremove >= 0 && controlToRemoveCount > 0)
                {
                    this.stackPanel1.Dispatcher.Invoke((Action)(() => { this.stackPanel1.Children.RemoveAt(controlToremove); }));
                    this.stackPanel1.Dispatcher.Invoke((Action)(() => { controlCount = this.stackPanel1.Children.Count; }));
                    controlToremove = controlToremove - 1;
                    controlToRemoveCount = controlToRemoveCount - 1;
                }
            }
        }
        private void startRunningText()
        {
            tbmarquee.Text = WhatsappProperties.Instance.RunnigText;
            tbmarquee.Foreground = WhatsappProperties.Instance.RunnigTextColor;
            tbmarquee.FontSize = WhatsappProperties.Instance.RunnigTextSize;
            
            double textGraphicalHeight = new FormattedText(tbmarquee.Text, System.Globalization.CultureInfo.CurrentCulture, System.Windows.FlowDirection.LeftToRight, new Typeface(tbmarquee.FontFamily.Source), tbmarquee.FontSize, tbmarquee.Foreground).Height;
            if (!WhatsappProperties.Instance.RunnigTextShow)
            {
                mainGrid.RowDefinitions[0].Height = new GridLength(0);
                tbmarquee.Text = "";
                return;
            }
            this.stackPanel1.Margin = new Thickness(WhatsappProperties.Instance.PaddingLeft, textGraphicalHeight, 0, 0);
            ThicknessAnimation ThickAnimation = new ThicknessAnimation();
            ThickAnimation.From = new Thickness(0, 0, 0, 0);
            ThickAnimation.To = new Thickness(System.Windows.SystemParameters.PrimaryScreenWidth, 0, 0, 0);
            ThickAnimation.RepeatBehavior = RepeatBehavior.Forever;
            ThickAnimation.Duration = new Duration(TimeSpan.FromSeconds(WhatsappProperties.Instance.RuningTextSpeed));
            mainGrid.RowDefinitions[0].Height = new GridLength(textGraphicalHeight);
            tbmarquee.BeginAnimation(TextBlock.PaddingProperty, ThickAnimation);
        }






        void Instance_OnConnectFailed(Exception ex)
        {
            MessageBox.Show(this, "Login failed", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
        }
        static void Instance_OnPrintDebug(object value)
        {
            Console.WriteLine(value);
        }
        
        private static void ProcessChat(WhatsApp wa, string dst)
        {
            var thRecv = new Thread(t =>
            {
                try
                {
                    while (wa != null)
                    {
                        try
                        {
                            wa.PollMessages();
                            Thread.Sleep(100);
                            continue;
                        }
                        catch (Exception)
                        {


                        }
                    }

                }
                catch (ThreadAbortException)
                {
                }
            }) { IsBackground = true };
            //thRecv.SetApartmentState(ApartmentState.STA);
            thRecv.Start();

            WhatsUserManager usrMan = new WhatsUserManager();
            var tmpUser = usrMan.CreateUser(dst, "User");

            while (false)
            {
                string line = Console.ReadLine();
                if (line == null && line.Length == 0)
                    continue;

                string command = line.Trim();
                switch (command)
                {
                    case "/query":
                        //var dst = dst//trim(strstr($line, ' ', FALSE));
                        Console.WriteLine("[] Interactive conversation with {0}:", tmpUser);
                        break;
                    case "/accountinfo":
                        Console.WriteLine("[] Account Info: {0}", wa.GetAccountInfo().ToString());
                        break;
                    case "/lastseen":
                        Console.WriteLine("[] Request last seen {0}", tmpUser);
                        wa.SendQueryLastOnline(tmpUser.GetFullJid());
                        break;
                    case "/exit":
                        wa = null;
                        thRecv.Abort();
                        return;
                    case "/start":
                        wa.SendComposing(tmpUser.GetFullJid());
                        break;
                    case "/pause":
                        wa.SendPaused(tmpUser.GetFullJid());
                        break;
                    default:
                        Console.WriteLine("[] Send message to {0}: {1}", tmpUser, line);
                        wa.SendMessage(tmpUser.GetFullJid(), line);
                        break;
                }
            }
        }














       
    }
}
