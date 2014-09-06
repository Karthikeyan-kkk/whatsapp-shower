using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Net;
using System.Runtime.CompilerServices;
using System.Security.Cryptography;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading;
using WhatsAppApi;
using WhatsAppApi.Account;
using WhatsAppApi.Helper;
using WhatsAppApi.Register;
using WhatsAppApi.Response;
using System.Windows.Forms;
using System.Collections.Generic;
using System.Drawing;

using System.Configuration;
using log4net;
using System.Drawing.Imaging;

namespace WindowsFormsApplication2
{
    public partial class WhatsappShower : Form
    {
        public static readonly Random random = new Random();
        private static readonly ILog msgsLog = log4net.LogManager.GetLogger("msgsLog");
        private static readonly ILog systemLog = log4net.LogManager.GetLogger("systemsLog");
        Dictionary<string, Color> dictionary = new Dictionary<string, Color>();
        
        
        string nickname = "test12";
        string sender = "972524376363"; // Mobile number with country code (but without + or 00)
        string password = "TicJAMworhafW+84w3vuA4yMS5o=";//v2 password
        string target = "972504219841";// Mobile number to send the message to
        bool showExample = false;
        System.Windows.Forms.Timer MarqueeTimer = new System.Windows.Forms.Timer();


        public WhatsappShower(string num, string pass, string nick)
        {
            sender = num;
            password = pass;
            nickname = nick;
            showExample = true;
            startWhatsappShower();
           
        }


        public void startWhatsappShower()
        {
            InitializeComponent();

           // String s = WhatsAppApi.Register.WhatsRegisterV2.GenerateIdentity("524376363", "147589"); 
            this.WindowState = FormWindowState.Maximized;
            
            if (WhatsappProperties.FullScreen)
            {
                this.FormBorderStyle = FormBorderStyle.None;
            }
           
            MarqueeTimer.Enabled = true;
            MarqueeTimer.Interval = WhatsappProperties.RuningTextSpeed;
            MarqueeTimer.Tick += new EventHandler(MarqueeUpdate);


            if (showExample)
            {
                init();
            }
            else
            {
                
                addText(" בדיקה לטקסט הארוך הזה שאני רוצה לבדוק איך הוא נראה", "05243763","עידן מור");
               
                addText(" עוד טקסט לבדיקה", "05243763");
                addText(" בדיקה לטקסט הארוך הזה שאני רוצה לבדוק איך הוא נראה", "05243763");
                addText(" בדיקה לטקסט הארוך הזה שאני רוצה לבדוק איך הוא נראה ולכן אני ממש בודק גם את האורך שלו האם זה אורך תקין והאם הוא באמת מקבל נכון את השורות החדשות או סתם עושה מה שבא לו", "05243763");
                addText(" עכשיו אני עושה בדיקה בנונית לראות איך זה מסתדר עם טקסט בגול בנוני לא ארוך ולא קצר מעניין איך זה יראה ", "05243763");
                addText(" זהעודטקסטבלירווחבכללאנירוצהלראותאיךהמערכתמסתדרעםהטקסטהזהבאמתזהמענייןאיךהיאתסתדרבלירווחבכלל", "05243763");
                addText(" זהעודטקסטבלירווחבכללאנירוצהלראותאיךהמערכתמסתדר עםרווחאחדועםהטקסטהזהבאמתזהמענייןאיךהיאתסתדרבלירווחבכלל", "05243763");
                addText(" סתם טסקטס לבדיקה...", "05243763");
                addText(" ...סתם קטסט לבדיקה ", "05243763");
                addText("מגניב", "05243763");


            }
        }
        void MarqueeUpdate(object sender, EventArgs e)
        {
           
            this.label1.Text = WhatsappProperties.RunnigText;
            this.label1.Font = WhatsappProperties.RunnigTextFont;
            this.label1.ForeColor = WhatsappProperties.RunnigTextColor;
            WhatsappProperties.StartRunnigLocation = WhatsappProperties.StartRunnigLocation + WhatsappProperties.RuningTextjumpingLocation;
            Size tableSize = this.tableLayoutPanel1.Size;
            Size labelSize = this.label1.Size;
            if (WhatsappProperties.StartRunnigLocation + labelSize.Width >= tableSize.Width)
            {
                WhatsappProperties.RuningTextjumpingLocation = WhatsappProperties.RuningTextjumpingLocation * -1;
                WhatsappProperties.StartRunnigLocation = WhatsappProperties.StartRunnigLocation + WhatsappProperties.RuningTextjumpingLocation + WhatsappProperties.RuningTextjumpingLocation;
            }
            if (WhatsappProperties.StartRunnigLocation == 0)
            {
                WhatsappProperties.RuningTextjumpingLocation = WhatsappProperties.RuningTextjumpingLocation * -1;
            }
           
            this.label1.Margin = new System.Windows.Forms.Padding(WhatsappProperties.StartRunnigLocation, WhatsappProperties.PaddingTop, 0, 0);
           
            Invalidate();

        }
        public WhatsappShower()
        {
            startWhatsappShower();
        }

        private void init()
        {
            var tmpEncoding = Encoding.UTF8;
           
           
           

            WhatsApp wa = new WhatsApp(sender, password, nickname, true);

            wa.OnGetMessage += wa_OnGetMessage;
            //wa.OnGetPhoto += wa_OnGetPhoto;
            wa.OnGetMessageImage += wa_OnGetMessageImage;
            wa.OnConnectFailed += new WhatsEventBase.ExceptionDelegate(Instance_OnConnectFailed);
            WhatsAppApi.Helper.DebugAdapter.Instance.OnPrintDebug += Instance_OnPrintDebug;
            wa.Connect();
            
           

       

            string datFile = getDatFileName(sender);
            byte[] nextChallenge = null;
            if (File.Exists(datFile))
            {
                try
                {
                    string foo = File.ReadAllText(datFile);
                    nextChallenge = Convert.FromBase64String(foo);
                }
                catch (Exception) { };
            }

            //wa.Login(nextChallenge);
            try
            {
                wa.Login();
            }
            catch (Exception e)
            {
                MessageBox.Show(this, "Login failed resone: " + e, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                Application.Exit();
                return;
               
            }



            if (wa.ConnectionStatus != WhatsAppApi.WhatsApp.CONNECTION_STATUS.LOGGEDIN)
            {
                MessageBox.Show(this, "Login failed resone: " + wa.ConnectionStatus, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }


            ProcessChat(wa, target);
           
        }
        void Instance_OnConnectFailed(Exception ex)
        {
           MessageBox.Show(this, "Login failed", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
        }
         void wa_OnGetMessage(ProtocolTreeNode node, string from, string id, string name, string message, bool receipt_sent)
        {
           
            string nickName = getNikeName(node);
            Console.WriteLine("Message from {0} {1}: {2}", name, from, message);
            from = filterFromNumber(from);
            if (isCommandOpMsg(from, message))
            {
                handleCommandOpMag(from, message);
                return;
            }
            addText(message, from, nickName);

        }

         private void handleCommandOpMag(string from, string message)
         {
             if(string.IsNullOrEmpty(message)){
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
                 this.panel1.Invoke(new MethodInvoker(delegate { this.panel1.Controls.Clear(); }));
             }
             else
             {
                 int controlCount = this.panel1.Controls.Count;
                 int controlToremove =  controlCount-1;
                 while (controlCount > 0 && controlToremove >= 0 && controlToRemoveCount>0)
                 {
                     this.panel1.Invoke(new MethodInvoker(delegate { this.panel1.Controls.RemoveAt(controlToremove); }));
                     controlCount = this.panel1.Controls.Count;
                     controlToremove = controlToremove -1;
                     controlToRemoveCount = controlToRemoveCount - 1;
                 }
             }
         }

         private bool isCommandOpMsg(string from, string message)
         {
             bool isCommandOpMsg = false;
             if (string.IsNullOrEmpty(message) || !message.StartsWith("op"))
             {
                 return isCommandOpMsg;
             }
            
             bool isCanGetCommandsFromThisNumber = false;
             string commandsOpOnlyFrom = WhatsappProperties.CommandsOpOnlyFrom;
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
                 string commandsOpPassword = WhatsappProperties.CommandsOpPassword;
                 if (!string.IsNullOrEmpty(commandsOpPassword))
                 {
                     string [] opMsgProp = message.Split(',');
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
             catch (Exception){}
             return nickName;
         }
        String filterFromNumber(String from){
            char[] splitChar = {'@'};
            return from.Split(splitChar)[0];
            
        }
        bool isCanShowMsgMet(string from,string type)
        {
            var reader = getCvsReader();
            if (reader == null)
            {
                return true;
            }
            
            while (!reader.EndOfStream)
            {
                var line = reader.ReadLine();
                var values = line.Split(',');
                if (values != null )
                {
                    if (values.Length > 0)
                    {
                        string phoneNumber = values[0];
                        if (!string.IsNullOrEmpty(from) && from.Equals(phoneNumber))
                        {
                            if ("TEXT".Equals(type, StringComparison.InvariantCultureIgnoreCase))
                            {
                                if (values.Length > 1)
                                {
                                    if (values[1].Equals("false", StringComparison.InvariantCultureIgnoreCase))
                                    {
                                        return false;
                                    }
                                }
                            }
                            if ("IMG".Equals(type, StringComparison.InvariantCultureIgnoreCase))
                            {
                                if (values.Length > 4)
                                {
                                    if (values[4].Equals("false", StringComparison.InvariantCultureIgnoreCase))
                                    {
                                        return false;
                                    }
                                }

                            }

                        }
                    }
                }

               // listB.Add(values[1]);
            }
            return true;
        }

        private static StreamReader getCvsReader()
        {
          
            try
            {
                var reader = new StreamReader(File.OpenRead(@"C:\Users\Idan\Google Drive\whatsAppShower\whatsAppShowePermissions.csv"));
                return reader;
            }
            catch (Exception)
            {

                return null;
            }
           
        }

        
          void wa_OnGetMessageImage(string from, string id, string fileName, int size, string url, byte[] preview)
          {
              
              from = filterFromNumber(from);
              Console.WriteLine("Got image from {0}", from, fileName);
              OnGetMedia(fileName, url, preview);
              if (isCanShowMsgMet(from, "IMG"))
              {   
                  addPicture(fileName, from);
              }
              addTextInfoToLog("IMG", fileName, from, isCanShowMsgMet(from, "IMG"));
          }
         
           static void OnGetMedia(string file, string url, byte[] data)
           {
               //save preview
               File.WriteAllBytes(string.Format("preview_{0}.jpg", file), data);
               //download
               using (WebClient wc = new WebClient())
               {
                   wc.DownloadFile(new Uri(url), file);
                   //wc.DownloadFileAsync(new Uri(url), file, null);
               }
           }
        static void Instance_OnPrintDebug(object value)
        {
            Console.WriteLine(value);
        }
        static string getDatFileName(string pn)
        {
            string filename = string.Format("{0}.next.dat", pn);
            return Path.Combine(Directory.GetCurrentDirectory(), filename);
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

        private void Form1_Load(object sender, EventArgs e)
        {

        }
        public void addText(String text, String phoneNumber)
        { addText(text, phoneNumber, ""); }

        public void addText(String text, String phoneNumber,string nickName)
        {
            bool isCanShowMsg = isCanShowMsgMet(phoneNumber,"TEXT");
            if (!isCanShowMsg)
            {
                return;
            }
            addTextInfoToLog("Text",text, phoneNumber, isCanShowMsgMet(phoneNumber,"TEXT"));
            text = text.Trim();
            int textSize = text.Length;
            double num3 = (double)textSize / (double)WhatsappProperties.CharPerRow;
            int textRowNumber = (int)Math.Round(num3);

            if (textRowNumber > 1)
            {
                text = modifayTextNewLine(text, WhatsappProperties.CharPerRow, textRowNumber);
            }
            Font fontss = WhatsappProperties.Font;
            string phoneName = phoneNumber;
            if (!string.IsNullOrEmpty(nickName))
            {
                phoneName = phoneNumber + " - " + nickName;
            }
            Image image = DrawText(text, WhatsappProperties.Font, phoneName, WhatsappProperties.PhonerFont, WhatsappProperties.HouerFont, Color.Gray, WhatsappProperties.TextColor, WhatsappProperties.TextBackGroundColor);

            PictureBox pictureBox = new PictureBox();
            pictureBox.Image = image;
            pictureBox.BackColor = System.Drawing.Color.Transparent;
            pictureBox.SizeMode = PictureBoxSizeMode.AutoSize;
            pictureBox.Margin = new System.Windows.Forms.Padding(WhatsappProperties.PaddingLeft, WhatsappProperties.PaddingTop, 0, 0);

            AddImageWithShadow(image, 3, 2, WhatsappProperties.ShadowColor, pictureBox);

            if (panel1.InvokeRequired)
            {
                this.panel1.Invoke(new MethodInvoker(delegate { this.panel1.Controls.Add(pictureBox); }));
            }
            else
            {
                
                this.panel1.Controls.Add(pictureBox);
            }
            saveToMsgHistory(pictureBox.Image);
            
            
         }

        private void saveToMsgHistory(Image image)
        {
            try
            {
                using (MemoryStream ms = new MemoryStream())
                {
                    image.Save(ms, ImageFormat.Jpeg);
                    byte[] imageBytes = ms.ToArray();
                    string imgAsBase64 = Convert.ToBase64String(imageBytes);

                }
            }
            catch (Exception e)
            {
                systemLog.Error("err saveToMsgHistory "+e);
            }
        }

        private void addTextInfoToLog(string type,string text, string phoneNumber, bool isShowen)
        {
            msgsLog.Info(isShowen + " " + type + " From: " + phoneNumber + ": " + text);
        }
        private Image DrawText(String text, Font font, String phoneNumber, Font phoneFont, Font houerFont, Color houerColor, Color textColor, Color backColor)
        {


            Brush phoneBrush = new SolidBrush(getPhoneColor(phoneNumber));
            Brush houerBrush = new SolidBrush(houerColor);
            Brush textBrush = new SolidBrush(textColor);

            String currentHouer = DateTime.Now.ToString("HH:mm");




            Image img = new Bitmap(1, 1);
            Graphics drawing = Graphics.FromImage(img);

            //measure the string to see how big the image needs to be
            String textForSize = phoneNumber + "\n" + text + " " + currentHouer;
            SizeF textSize = drawing.MeasureString(textForSize, font);

            //free up the dummy image and old graphics object
            img.Dispose();
            drawing.Dispose();

            //create a new image of the right size
            img = new Bitmap((int)textSize.Width + 15+20, (int)textSize.Height + 10);

            drawing = Graphics.FromImage(img);

            //paint the background
            drawing.Clear(backColor);

            //create a brush for the text
            SizeF sizeF = drawing.MeasureString(phoneNumber, phoneFont);
            StringFormatFlags stringFormatFlags = StringFormatFlags.DirectionRightToLeft;
            StringFormat stringFormat = new StringFormat(stringFormatFlags);
            drawing.DrawString(phoneNumber, phoneFont, phoneBrush, 10, 10);
          //drawing.DrawString(text, font, textBrush, (int)textSize.Width - sizeF.Width + 80, 30, stringFormat);
            drawing.DrawString(text, font, textBrush, img.Width-20, 30, stringFormat);


            drawing.Save();

            textBrush.Dispose();
            drawing.Dispose(); 

            drawing = Graphics.FromImage(img);

            drawing.DrawString(currentHouer, houerFont, houerBrush, img.Width - 10, img.Height - sizeF.Height + 8, stringFormat);
            drawing.Dispose();
            return img;

        }

        public void addPicture(String link, String from)
        {
            Image i2 = Image.FromFile(link);
            addPicture(i2, from);
        }
        public void addPicture(Image i2, String from)
        {

            i2 = ScaleImage(i2, WhatsappProperties.ImageMaxWidth, WhatsappProperties.ImageMaxHeight);
            Graphics drawing = Graphics.FromImage(i2);
            Brush phoneBrush = new SolidBrush(Color.White);
            drawing.DrawString(from, WhatsappProperties.PhonerFont, phoneBrush, 10, 10);
            String currentHouer = DateTime.Now.ToString("HH:mm");
            SizeF textSize = drawing.MeasureString(currentHouer, WhatsappProperties.Font);
            drawing.DrawString(currentHouer, WhatsappProperties.HouerFont, phoneBrush, i2.Width - textSize.Width, i2.Height - textSize.Height);
            PictureBox pictureBox = new PictureBox();
            pictureBox.SizeMode = PictureBoxSizeMode.AutoSize;
            pictureBox.Image = i2;
            pictureBox.Margin = new System.Windows.Forms.Padding(WhatsappProperties.PaddingLeft, WhatsappProperties.PaddingTop, 0, 0);
            this.panel1.Invoke(new MethodInvoker(delegate { this.panel1.Controls.Add(pictureBox); }));
            
        }
        public  Image ScaleImage(Image image, int maxWidth, int maxHeight)
        {
            var ratioX = (double)maxWidth / image.Width;
            var ratioY = (double)maxHeight / image.Height;
            var ratio = Math.Min(ratioX, ratioY);

            var newWidth = (int)(image.Width * ratio);
            var newHeight = (int)(image.Height * ratio);

            var newImage = new Bitmap(newWidth, newHeight);
            Graphics.FromImage(newImage).DrawImage(image, 0, 0, newWidth, newHeight);
            return newImage;
        }
        public String modifayTextNewLine(string text, int charNumber, int rowNumber)
        {
            try
            {
                String textToReturn = "";
                int startRunFrom = charNumber;
                int endRunTo = 0;
                for (int i = 0; i < rowNumber; i++)
                {
                    bool foundSpace = false;
                    for (int j = startRunFrom; j > endRunTo; j--)
                    {
                        if (text[j].Equals(' '))
                        {
                            foundSpace = true;
                            if (textToReturn.Length > 0)
                            {
                                int temp = text.Length;
                                //textToReturn = textToReturn + "\n" + text.Substring(endRunTo, startRunFrom);
                                text = text.Insert(j, "\n");
                            }
                            else
                            {
                                text = text.Insert(j, "\n");
                            }
                            endRunTo = startRunFrom;

                            startRunFrom = startRunFrom + charNumber;
                            if (startRunFrom > text.Length)
                            {
                                startRunFrom = text.Length - 1;
                            }
                            break;

                        };
                    }
                    if (!foundSpace)
                    {
                        text = text.Insert(startRunFrom, "\n");
                        endRunTo = startRunFrom;

                        startRunFrom = startRunFrom + charNumber;
                        if (startRunFrom > text.Length)
                        {
                            startRunFrom = text.Length - 1;
                        }
                    }
                }
            }
            catch (Exception)
            {
                
                
            }
            return text;
        }

        private void panel1_ControlAdded(object sender, ControlEventArgs e)
        {
            panel1.ScrollControlIntoView(e.Control);
        }

        private Color getPhoneColor(String phoneNumber)
        {

            if (dictionary.ContainsKey(phoneNumber))
            {
                return dictionary[phoneNumber];
            }
            Color phoneColor = Color.FromArgb(random.Next(0, 255), random.Next(0, 255), random.Next(0, 255));
            dictionary.Add(phoneNumber, phoneColor);
            return phoneColor;
        }

        private void AddImageWithShadow(Image img, int area, int thickness, Color clr, PictureBox PicBox)
        {
            int TopLeft = 0;
            int TopRight = 1;
            int BottomLeft = 2;
            int BottomRight = 3;
            Bitmap bm = new Bitmap(img.Width + thickness, img.Height + thickness);
            Graphics gr = Graphics.FromImage(bm);
            int ix = 0;
            int iy = 0;
            Rectangle rect = new Rectangle(thickness, thickness, img.Width, img.Height);

            if (area == TopLeft || area == TopRight)
            {
                iy = thickness;
                rect.Y = 0;
            }
            if (area == TopLeft || area == BottomLeft)
            {
                ix = thickness;
                rect.X = 0;
            }

            gr.FillRectangle(new SolidBrush(clr), rect);
            gr.DrawImage(img, ix, iy);
            if (PicBox.Image != null)
            {
                PicBox.Image.Dispose();
                PicBox.Image = new Bitmap(bm);
            }
        }

        private void panel1_DoubleClick(object sender, EventArgs e)
        {
            WhatsappProperties.initProperties();
            this.label1.Text = WhatsappProperties.RunnigText;
            MarqueeTimer.Interval = WhatsappProperties.RuningTextSpeed;
            if (WhatsappProperties.FullScreen)
            {
                this.FormBorderStyle = FormBorderStyle.None;
            }
            else
            {
                this.FormBorderStyle = FormBorderStyle.Sizable;
            }
        }

        void WhatsappShower_FormClosed(object sender, System.Windows.Forms.FormClosedEventArgs e)
        {
            Application.Exit();
        }



    }
}
