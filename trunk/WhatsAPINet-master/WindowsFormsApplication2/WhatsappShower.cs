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

namespace WindowsFormsApplication2
{
    public partial class WhatsappShower : Form
    {
        public static readonly Random random = new Random();
       
        Dictionary<string, Color> dictionary = new Dictionary<string, Color>();
        Font runnigTextFont = new System.Drawing.Font("Choco", WhatsappProperties.RunnigTextSize, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(177)));
        Font phonerFont = new System.Drawing.Font("Arial", WhatsappProperties.PhoneFontSize, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(177)));
        Font houerFont = new System.Drawing.Font("Choco", WhatsappProperties.HouerFontSize, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(177)));
        Font font = new System.Drawing.Font("Choco", WhatsappProperties.TextFontSize, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(177)));
        
        string nickname = "test12";
        string sender = "972524376363"; // Mobile number with country code (but without + or 00)
        string password = "TicJAMworhafW+84w3vuA4yMS5o=";//v2 password
        string target = "972504219841";// Mobile number to send the message to
        bool showExample = false;


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
            
            this.WindowState = FormWindowState.Maximized;
            System.Windows.Forms.Timer MarqueeTimer = new System.Windows.Forms.Timer();
            MarqueeTimer.Enabled = true;
            MarqueeTimer.Interval = WhatsappProperties.RuningTextSpeed;
            MarqueeTimer.Tick += new EventHandler(MarqueeUpdate);


            if (showExample)
            {
                init();
            }
            else
            {
                //WhatsappProperties.saveNickName("test");
                addText(" בדיקה לטקסט הארוך הזה שאני רוצה לבדוק איך הוא נראה", "0524376362");
               // addPicture("AmQcOXsWg7cQgVLk1VlHkmk52jZGSM2Cjix-_D2AzUIw.jpg", "524376363");
                //addPicture("AqnoddKDiXvuaYsynkBJSQH_L1rPH1eQ-i82OsU5UjKP.jpg", "524376363");
                addText(" עוד טקסט לבדיקה", "0524376362");
                addText(" בדיקה לטקסט הארוך הזה שאני רוצה לבדוק איך הוא נראה", "0524376362");
                addText(" בדיקה לטקסט הארוך הזה שאני רוצה לבדוק איך הוא נראה", "0524376362");


            }
        }
        void MarqueeUpdate(object sender, EventArgs e)
        {
            this.label1.Text = WhatsappProperties.RunnigText;
            this.label1.Font = runnigTextFont;
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

            wa.Login(nextChallenge);

            ProcessChat(wa, target);
           
        }
         void wa_OnGetMessage(ProtocolTreeNode node, string from, string id, string name, string message, bool receipt_sent)
        {
            Console.WriteLine("Message from {0} {1}: {2}", name, from, message);
            from = filterFormNumber(from);
            addText(message, from);

        }
        String filterFormNumber(String from){
            char[] splitChar = {'@'};
            return from.Split(splitChar)[0];
            
        }

          void wa_OnGetPhoto(string from, string id, byte[] data)
         {
             from = filterFormNumber(from);
             Console.WriteLine("Got full photo for {0}", from);
             File.WriteAllBytes(string.Format("{0}.jpg", from), data);
             MemoryStream ms = new MemoryStream(data);
             Image returnImage = Image.FromStream(ms);
             addPicture(returnImage, from);
            
         }
          void wa_OnGetMessageImage(string from, string id, string fileName, int size, string url, byte[] preview)
          {
              from = filterFormNumber(from);
              Console.WriteLine("Got image from {0}", from, fileName);
              OnGetMedia(fileName, url, preview);
              addPicture(fileName, from);
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
                        wa.PollMessages();
                        Thread.Sleep(100);
                        continue;
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
        {

            text = text.Trim();
            int textSize = text.Length;
            double num3 = (double)textSize / (double)WhatsappProperties.CharPerRow;
            int textRowNumber = (int)Math.Ceiling(num3);

            if (textRowNumber > 1)
            {
                text = modifayTextNewLine(text, WhatsappProperties.CharPerRow, textRowNumber);
            }

            Image image = DrawText(text, font, phoneNumber, phonerFont, houerFont, Color.Gray, WhatsappProperties.TextColor, WhatsappProperties.TextBackGroundColor);

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
            img = new Bitmap((int)textSize.Width + 15, (int)textSize.Height + 10);

            drawing = Graphics.FromImage(img);

            //paint the background
            drawing.Clear(backColor);

            //create a brush for the text


            drawing.DrawString(phoneNumber, phoneFont, phoneBrush, 10, 10);
            drawing.DrawString(text, font, textBrush, 10, 30);


            drawing.Save();

            textBrush.Dispose();
            drawing.Dispose();

            drawing = Graphics.FromImage(img);
            SizeF sizeF = drawing.MeasureString(phoneNumber, phoneFont);
            drawing.DrawString(currentHouer, houerFont, houerBrush, img.Width - sizeF.Width+60, img.Height - sizeF.Height + 6);
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
            drawing.DrawString(from, phonerFont, phoneBrush, 10, 10);
            String currentHouer = DateTime.Now.ToString("HH:mm");
            SizeF textSize = drawing.MeasureString(currentHouer, font);
            drawing.DrawString(currentHouer, houerFont, phoneBrush, i2.Width - textSize.Width, i2.Height - textSize.Height);
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





    }
}
