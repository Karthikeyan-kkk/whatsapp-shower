using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Configuration;
using System.Drawing;
using System.IO;

namespace WindowsFormsApplication2
{
    public class WhatsappProperties
    {
        static int paddingTop = 10;

        public static int PaddingTop
        {
            get { return paddingTop; }
            set { paddingTop = value; }
        }


        static int paddingLeft = 20;

        public static int PaddingLeft
        {
            get { return paddingLeft; }
            set { paddingLeft = value; }
        }

        static int charPerRow = 50;

        public static int CharPerRow
        {
            get { return charPerRow; }
            set { charPerRow = value; }
        }

        static int imageMaxWidth = 80000000;

        public static int ImageMaxWidth
        {
            get { return imageMaxWidth; }
            set { imageMaxWidth = value; }
        }
        static int imageMaxHeight = 400;

        public static int ImageMaxHeight
        {
            get { return imageMaxHeight; }
            set { imageMaxHeight = value; }
        }

        static int runingTextSpeed = 50;

        public static int RuningTextSpeed
        {
            get { return runingTextSpeed; }
            set { runingTextSpeed = value; }
        }
        static int startRunnigLocation = 0;

        public static int StartRunnigLocation
        {
            get { return startRunnigLocation; }
            set { startRunnigLocation = value; }
        }
        static int runingTextjumpingLocation = 10;

        public static int RuningTextjumpingLocation
        {
            get { return runingTextjumpingLocation; }
            set { runingTextjumpingLocation = value; }
        }
        static string runnigText = "שלחו הודעת וואטסאפ למספר 0524376363";

        public static string RunnigText
        {
            get { return runnigText; }
            set { runnigText = value; }
        }

        static Single textFontSize = 18;

        public static Single TextFontSize
        {
            get { return WhatsappProperties.textFontSize; }
            set { WhatsappProperties.textFontSize = value; }
        }
        static Single phoneFontSize = 12;

        public static Single PhoneFontSize
        {
            get { return WhatsappProperties.phoneFontSize; }
            set { WhatsappProperties.phoneFontSize = value; }
        }
        static Single houerFontSize = 10;

        public static Single HouerFontSize
        {
            get { return WhatsappProperties.houerFontSize; }
            set { WhatsappProperties.houerFontSize = value; }
        }
        static Single runnigTextSize = 24;

        public static Single RunnigTextSize
        {
            get { return WhatsappProperties.runnigTextSize; }
            set { WhatsappProperties.runnigTextSize = value; }
        }

        static Color textBackGroundColor = Color.FromArgb(252, 251, 246);

        public static Color TextBackGroundColor
        {
            get { return textBackGroundColor; }
            set { textBackGroundColor = value; }
        }
        static Color shadowColor = Color.FromArgb(100, 0, 0, 0);

        public static Color ShadowColor
        {
            get { return shadowColor; }
            set { shadowColor = value; }
        }
        static Color textColor = Color.Black;

        public static Color TextColor
        {
            get { return textColor; }
            set { textColor = value; }
        }
        static Color houerColor = Color.Gray;

        public static Color HouerColor
        {
            get { return houerColor; }
            set { houerColor = value; }
        }
        static Color runnigTextColor = Color.Blue;

        public static Color RunnigTextColor
        {
            get { return runnigTextColor; }
            set { runnigTextColor = value; }
        }

        static String phoneNumber = "";

        public static String PhoneNumber
        {
            get { return phoneNumber; }
            set { phoneNumber = value; }
        }
        static String password = "";

        public static String Password
        {
            get { return password; }
            set { password = value; }
        }
        static String nickName = "";

        public static String NickName
        {
            get { return nickName; }
            set { nickName = value; }
        }
        static Dictionary<string, string> props = new Dictionary<string, string>();

        static public Dictionary<string, string> Props
        {
            get { return props; }
            set { props = value; }
        }

        static bool fullScreen = false;

        public static bool FullScreen
        {
            get { return WhatsappProperties.fullScreen; }
            set { WhatsappProperties.fullScreen = value; }
        }


        public static bool initProperties(){
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
        static Font runnigTextFont = new System.Drawing.Font("Choco", WhatsappProperties.RunnigTextSize, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(177)));

        public static Font RunnigTextFont
        {
            get { return new System.Drawing.Font("Choco", WhatsappProperties.RunnigTextSize, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(177))); }
            set { runnigTextFont = value; }
        }
        static Font phonerFont = new System.Drawing.Font("Arial", WhatsappProperties.PhoneFontSize, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(177)));

        public static Font PhonerFont
        {
            get { return new System.Drawing.Font("Arial", WhatsappProperties.PhoneFontSize, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(177))); }
            set { phonerFont = value; }
        }
        static Font houerFont = new System.Drawing.Font("Choco", WhatsappProperties.HouerFontSize, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(177)));

        public static Font HouerFont
        {
            get { return new System.Drawing.Font("Choco", WhatsappProperties.HouerFontSize, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(177))); }
            set { houerFont = value; }
        }
        static Font font = new System.Drawing.Font("Choco", WhatsappProperties.TextFontSize, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(177)));

        public static Font Font
        {
            get { return new System.Drawing.Font("Choco", WhatsappProperties.TextFontSize, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(177)));}
            set { font = value; }
        }

        private static void updateFiledFromPropsDictionary()
        {
            String prfixCode = "whatsappShow.";
            int propCount = Props.Count;

            for (int i = 0; i < propCount; i++)
            {
                String key = Props.Keys.ElementAt(i);
                if (key.Equals(prfixCode+"paddingTop"))
                {
                    PaddingTop = parseToInt(Props[key], 10);
                }
                if (key.Equals(prfixCode + "paddingLeft"))
                {
                    PaddingLeft = parseToInt(Props[key], 20);
                }
                if (key.Equals(prfixCode + "charPerRow"))
                {
                    CharPerRow = parseToInt(Props[key], 50);
                }
                if (key.Equals(prfixCode + "imageMaxWidth"))
                {
                    ImageMaxWidth = parseToInt(Props[key], 80000000);
                }
                if (key.Equals(prfixCode + "imageMaxHeight"))
                {
                    ImageMaxHeight = parseToInt(Props[key], 400);
                }
                if (key.Equals(prfixCode + "runingTextSpeed"))
                {
                    RuningTextSpeed = parseToInt(Props[key], 50);
                }
                if (key.Equals(prfixCode + "startRunnigLocation"))
                {
                    StartRunnigLocation = parseToInt(Props[key], 0);
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
                if (key.Equals(prfixCode + "textBackGroundColor"))
                {
                    TextBackGroundColor = parseToColor(Props[key], Color.FromArgb(252, 251, 246));
                }
                if (key.Equals(prfixCode + "shadowColor"))
                {
                    ShadowColor = parseToColor(Props[key], Color.FromArgb(100, 0, 0, 0));
                }
                if (key.Equals(prfixCode + "textColor"))
                {
                    TextColor = parseToColor(Props[key], Color.Black);
                }
                if (key.Equals(prfixCode + "houerColor"))
                {
                    HouerColor = parseToColor(Props[key], Color.Gray);
                }
                if (key.Equals(prfixCode + "runnigTextColor"))
                {
                    RunnigTextColor = parseToColor(Props[key], Color.White);
                }
                if (key.Equals(prfixCode + "fullScreen"))
                {
                    FullScreen = parseBoolean(Props[key], false);
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
