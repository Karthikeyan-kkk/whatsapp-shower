using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Configuration;
using System.IO;
using System.Windows.Media;

namespace whatsAppShowerWpf
{

    

    public class WhatsappProperties
    {

        private static WhatsappProperties instance;


        public static WhatsappProperties Instance
        {
            get
            {
                if (instance == null)
                {
                    instance = new WhatsappProperties();
                    instance.initProperties();
                }
                return instance;
            }
        }

         int paddingTop = 10;

        public  int PaddingTop
        {
            get { return paddingTop; }
            set { paddingTop = value; }
        }


         int paddingLeft = 20;

        public  int PaddingLeft
        {
            get { return paddingLeft; }
            set { paddingLeft = value; }
        }

        int imageMaxWidth = 80000000;

        public  int ImageMaxWidth
        {
            get { return imageMaxWidth; }
            set { imageMaxWidth = value; }
        }

        string imageMaxWidthType = "per";

        public string ImageMaxWidthType
        {
            get { return imageMaxWidthType; }
            set { imageMaxWidthType = value; }
        }

        string textMaxWidthType = "per";

        public string TextMaxWidthType
        {
            get { return textMaxWidthType; }
            set { textMaxWidthType = value; }
        }

         int imageMaxHeight = 400;

        public  int ImageMaxHeight
        {
            get { return imageMaxHeight; }
            set { imageMaxHeight = value; }
        }

         int runingTextSpeed = 50;

        public  int RuningTextSpeed
        {
            get { return runingTextSpeed; }
            set { runingTextSpeed = value; }
        }
         
         int runingTextjumpingLocation = 10;

        public  int RuningTextjumpingLocation
        {
            get { return runingTextjumpingLocation; }
            set { runingTextjumpingLocation = value; }
        }
         string runnigText = "שלחו הודעת וואטסאפ למספר 0524376363";

        public  string RunnigText
        {
            get { return runnigText; }
            set { runnigText = value; }
        }

         Single textFontSize = 18;

        public  Single TextFontSize
        {
            get { return WhatsappProperties.Instance.textFontSize; }
            set { WhatsappProperties.Instance.textFontSize = value; }
        }
         Single phoneFontSize = 12;

        public  Single PhoneFontSize
        {
            get { return WhatsappProperties.Instance.phoneFontSize; }
            set { WhatsappProperties.Instance.phoneFontSize = value; }
        }
         Single houerFontSize = 10;

        public  Single HouerFontSize
        {
            get { return WhatsappProperties.Instance.houerFontSize; }
            set { WhatsappProperties.Instance.houerFontSize = value; }
        }
        Single runnigTextSize = 24;

        public  Single RunnigTextSize
        {
            get { return WhatsappProperties.Instance.runnigTextSize; }
            set { WhatsappProperties.Instance.runnigTextSize = value; }
        }

        Brush runnigTextColor = Brushes.White;

        public Brush RunnigTextColor
        {
            get { return runnigTextColor; }
            set { runnigTextColor = value; }
        }

         String phoneNumber = "";

        public  String PhoneNumber
        {
            get { return phoneNumber; }
            set { phoneNumber = value; }
        }
         String password = "";

        public  String Password
        {
            get { return password; }
            set { password = value; }
        }
         String nickName = "";

        public  String NickName
        {
            get { return nickName; }
            set { nickName = value; }
        }

        string appToken= "";

        public string AppToken
        {
            get { return appToken; }
            set { appToken = value; }
        }


        static Dictionary<string, string> props = new Dictionary<string, string>();

        static public Dictionary<string, string> Props
        {
            get { return props; }
            set { props = value; }
        }

         bool fullScreen = false;

        public  bool FullScreen
        {
            get { return fullScreen; }
            set { fullScreen = value; }
        }

        bool runnigTextShow = true;

        public bool RunnigTextShow
        {
            get { return runnigTextShow; }
            set { runnigTextShow = value; }
        }

       string commandsOpOnlyFrom = "";

        public  string CommandsOpOnlyFrom
        {
            get { return commandsOpOnlyFrom; }
            set { commandsOpOnlyFrom = value; }
        }

         string commandsOpPassword = "";

        public  string CommandsOpPassword
        {
            get { return commandsOpPassword; }
            set { commandsOpPassword = value; }
        }

        int maxTextWidth = 400;

        public int MaxTextWidth
        {
            get { return maxTextWidth; }
            set { maxTextWidth = value; }
        }

        int sideImagefadingSpeedInSec = 10;

        public int SideImagefadingSpeedInSec
        {
            get { return sideImagefadingSpeedInSec; }
            set { sideImagefadingSpeedInSec = value; }
        }

        
        int sideImageRunEveryInSec = 10;

        public int SideImageRunEveryInSec
        {
            get { return sideImageRunEveryInSec; }
            set { sideImageRunEveryInSec = value; }
        }

        bool showSideImages = true;

        public bool ShowSideImages
        {
            get { return showSideImages; }
            set { showSideImages = value; }
        }

        string backgroundimage = "";

        public string Backgroundimage
        {
            get { return backgroundimage; }
            set { backgroundimage = value; }
        }

        string downloadImgTo = "";

        public string DownloadImgTo
        {
            get { return downloadImgTo; }
            set { downloadImgTo = value; }
        }


        string phoneToken = "";

        public string PhoneToken
        {
            get { return phoneToken; }
            set { phoneToken = value; }
        }

        public  bool initProperties()
        {
            props.Clear();
            foreach (var row in File.ReadAllLines("whatsappShow.property"))
                if (!String.IsNullOrEmpty(row))
                {
                    String prop = row.Trim();
                    if (!String.IsNullOrEmpty(prop))
                    {
                        String key = prop.Split('=')[0];
                        String val = string.Join("=", prop.Split('=').Skip(1).ToArray());
                        props.Add(key.Trim(), val.Trim());
                    }
                }
            updateFiledFromPropsDictionary();
            return true;
        }
        

        private  void updateFiledFromPropsDictionary()
        {
            String prfixCode = "whatsappShow.";
            int propCount = Props.Count;

            for (int i = 0; i < propCount; i++)
            {
                String key = Props.Keys.ElementAt(i);
                if (key.Equals(prfixCode + "paddingTop"))
                {
                   PaddingTop = parseToInt(Props[key], 10);
                }
                if (key.Equals(prfixCode + "paddingLeft"))
                {
                    PaddingLeft = parseToInt(Props[key], 20);
                }
               if (key.Equals(prfixCode + "imageMaxWidth"))
                {
                    ImageMaxWidth = parseToInt(Props[key], 80000000);
                }
                if (key.Equals(prfixCode + "imageMaxWidthType"))
                {
                    ImageMaxWidthType = parsString(Props[key], "per");
                }
                if (key.Equals(prfixCode + "textMaxWidthType"))
                {
                    TextMaxWidthType = parsString(Props[key], "per");
                }
                if (key.Equals(prfixCode + "imageMaxHeight"))
                {
                    ImageMaxHeight = parseToInt(Props[key], 400);
                }
                if (key.Equals(prfixCode + "runingTextSpeed"))
                {
                    RuningTextSpeed = parseToInt(Props[key], 50);
                }
                if (key.Equals(prfixCode + "runingTextjumpingLocation"))
                {
                    RuningTextjumpingLocation = parseToInt(Props[key], 10);
                }
                if (key.Equals(prfixCode + "runnigText"))
                {
                    RunnigText = parsString(Props[key], " ");
                }
                if (key.Equals(prfixCode + "textFontSize"))
                {
                    TextFontSize = parseToInt(Props[key], 18);
                }
                if (key.Equals(prfixCode + "phoneFontSize"))
                {
                    PhoneFontSize = parseToInt(Props[key], 18);
                }
                if (key.Equals(prfixCode + "houerFontSize"))
                {
                    HouerFontSize = parseToInt(Props[key], 10);
                }
                if (key.Equals(prfixCode + "runnigTextSize"))
                {
                    RunnigTextSize = parseToInt(Props[key], 24);
                }
                if (key.Equals(prfixCode + "fullScreen"))
                {
                    FullScreen = parseBoolean(Props[key], false);
                }
                if (key.Equals(prfixCode + "commandsOpOnlyFrom"))
                {
                    commandsOpOnlyFrom = parsString(Props[key], "");
                }
                if (key.Equals(prfixCode + "commandsOpPassword"))
                {
                    commandsOpPassword = parsString(Props[key], "");
                }
                if (key.Equals(prfixCode + "maxTextWidth"))
                {
                    MaxTextWidth = parseToInt(Props[key], 400);
                }
                if (key.Equals(prfixCode + "runnigTextShow"))
                {
                    RunnigTextShow = parseBoolean(Props[key], true);
                }
                if (key.Equals(prfixCode + "sideImageRunEveryInSec"))
                {
                    SideImageRunEveryInSec = parseToInt(Props[key], 10);
                }
                if (key.Equals(prfixCode + "sideImagefadingSpeedInSec"))
                {
                    SideImagefadingSpeedInSec = parseToInt(Props[key], 10);
                }
                if (key.Equals(prfixCode + "showSideImages"))
                {
                    ShowSideImages = parseBoolean(Props[key], true);
                }
                if (key.Equals(prfixCode + "backgroundimage"))
                {
                    Backgroundimage = parsString(Props[key], "");
                }
                if (key.Equals(prfixCode + "downloadImgTo"))
                {
                    DownloadImgTo = parsString(Props[key], "");
                }
                if (key.Equals(prfixCode + "appToken"))
                {
                    AppToken = parsString(Props[key], "");
                }
                if (key.Equals(prfixCode + "phoneToken"))
                {
                    phoneToken = parsString(Props[key], "");
                }
            }

        }

        private static bool parseBoolean(string val, bool valToReturn)
        {
            if (!string.IsNullOrEmpty(val) && ("true".Equals(val) || "TRUE".Equals(val)))
            {
                return true;
            }
            return false;
        }

        private static Color parseToColor(string colorName, Color color)
        {


            //Color colorFromArgb = Color.FromArgb(Convert.ToInt32(colorName.Split(',')[0]), Convert.ToInt32(colorName.Split(',')[1]), Convert.ToInt32(colorName.Split(',')[2]), Convert.ToInt32(colorName.Split(',')[3]));
            //if (colorFromArgb != null)
            // {
            //     return colorFromArgb;
            // }
            return color;
        }

        private static string parsString(string val, string valToReturn)
        {
            if (string.IsNullOrEmpty(val))
            {
                return valToReturn;
            }
            return val;
        }

        private static int parseToInt(string val, int valToReturn)
        {
            try
            {
                return Convert.ToInt32(val);
            }
            catch (Exception)
            {

                return valToReturn;
            }
        }

    }
}
