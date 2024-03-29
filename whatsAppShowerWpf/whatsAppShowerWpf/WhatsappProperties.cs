﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Configuration;
using System.IO;
using System.Windows.Media;
using System.Windows;

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

        int msgSectionWidth = 300;

        public int MsgSectionWidth
        {
            get { return msgSectionWidth; }
            set { msgSectionWidth = value; }
        }
        string msgSectionWidthType = "per";

        public string MsgSectionWidthType
        {
            get { return msgSectionWidthType; }
            set { msgSectionWidthType = value; }
        }
        
        
        int paddingTop = 10;

        public  int PaddingTop
        {
            get { return paddingTop; }
            set {paddingTop = value; }
        }

        int paddingLeft = 20;

        public  int PaddingLeft
        {
            get { return paddingLeft; }
            set { paddingLeft = value; }
        }

        int imageMaxWidth = 800;

        public  int ImageMaxWidth
        {
            get { return imageMaxWidth; }
            set { imageMaxWidth = value; }
        }
        int imageMaxHeight = 400;

        public int ImageMaxHeight
        {
            get { return imageMaxHeight; }
            set { imageMaxHeight = value; }
        }

        string imageMaxWidthType = "per";

        
        public string ImageMaxWidthType
        {
            get { return imageMaxWidthType; }
            set { imageMaxWidthType = value; }
        }

        int maxTextWidth = 400;

        public int MaxTextWidth
        {
            get { return maxTextWidth; }
            set { maxTextWidth = value; }
        }

        Single textFontSize = 18;

        public Single TextFontSize
        {
            get { return WhatsappProperties.Instance.textFontSize; }
            set { WhatsappProperties.Instance.textFontSize = value; }
        }

        Brush textColor = Brushes.White;

        public Brush TextColor
        {
            get { return textColor; }
            set { textColor = value; }
        }

        Brush textBackgroundColor = Brushes.Black;

        public Brush TextBackgroundColor
        {
            get { return textBackgroundColor; }
            set { textBackgroundColor = value; }
        }



        

        string textMaxWidthType = "per";

        public string TextMaxWidthType
        {
            get { return textMaxWidthType; }
            set { textMaxWidthType = value; }
        }



        double runingTextSpeed = 0.01;

        public double RuningTextSpeed
        {
            get { return runingTextSpeed; }
            set { runingTextSpeed = value; }
        }
        
        string runnigText = "שלחו הודעת וואטסאפ למספר 0524376363";

        public  string RunnigText
        {
            get { return runnigText; }
            set { runnigText = value; }
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

        Brush hourColor = Brushes.Gray;

        public Brush HourColor
        {
            get { return hourColor; }
            set { hourColor = value; }
        }

        Single runnigTextSize = 24;

        public  Single RunnigTextSize
        {
            get { return WhatsappProperties.Instance.runnigTextSize; }
            set { WhatsappProperties.Instance.runnigTextSize = value; }
        }
       
        Brush runnigTextColor = Brushes.Yellow;

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


        string premissionsFileName = "whatsAppShowePermissions.csv";

        public string PremissionsFileName
        {
            get { return premissionsFileName; }
            set { premissionsFileName = value; }
        }

        string premissionsDir = Environment.CurrentDirectory;

        public string PremissionsDir
        {
            get { return premissionsDir; }
            set { premissionsDir = value; }
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

        string sideImageWidthType = "per";

        public string SideImageWidthType
        {
            get { return sideImageWidthType; }
            set { sideImageWidthType = value; }
        }

        int sideImageWidth = 0;

        public int SideImageWidth
        {
            get { return sideImageWidth; }
            set { sideImageWidth = value; }
        }



        string demoImageFolder = "demoImgs";

        public string DemoImageFolder
        {
            get { return demoImageFolder; }
            set { demoImageFolder = value; }
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
                        if (!props.ContainsKey(key.Trim())) { 
                            props.Add(key.Trim(), val.Trim());
                        }
                    }
                }
            syncProp(true);
            return true;
        }
        
        
        public  void syncProp(bool fromFile)
        {
            String prfixCode = "whatsappShow.";
            int propCount = Props.Count;

            for (int i = 0; i < propCount; i++)
            {
                String key = Props.Keys.ElementAt(i);
                if (key.Equals(prfixCode + "msgSectionWidth"))
                {
                    if (fromFile)
                    {
                        MsgSectionWidth = parseToInt(Props[key], 300);
                    }
                    else
                    {
                        saveToFile("msgSectionWidth", MsgSectionWidth + "");
                    }
                }
                if (key.Equals(prfixCode + "msgSectionWidthType"))
                {
                    if (fromFile)
                    {
                        msgSectionWidthType = parsString(Props[key], "per");
                    }
                    else
                    {
                        saveToFile("msgSectionWidthType", MsgSectionWidthType + "");
                    }
                }

                if (key.Equals(prfixCode + "paddingTop"))
                {
                    if (fromFile)
                    {
                        PaddingTop = parseToInt(Props[key], 10);
                    }
                    else
                    {
                        saveToFile("paddingTop", PaddingTop+"");
                    }
                }
                if (key.Equals(prfixCode + "paddingLeft"))
                {
                    if (fromFile)
                    {
                        PaddingLeft = parseToInt(Props[key], 20);
                    }
                    else
                    {
                        saveToFile("paddingLeft", PaddingLeft + "");
                    }
                }
               if (key.Equals(prfixCode + "imageMaxWidth"))
                {
                    if (fromFile)
                    {
                        ImageMaxWidth = parseToInt(Props[key], 80000000);
                    }
                    else
                    {
                        saveToFile("imageMaxWidth", ImageMaxWidth + "");
                    }
                }
                if (key.Equals(prfixCode + "imageMaxWidthType"))
                {
                    if (fromFile)
                    {
                        ImageMaxWidthType = parsString(Props[key], "per");
                    }
                    else
                    {
                        saveToFile("imageMaxWidthType", ImageMaxWidthType + "");
                    }
                }
                if (key.Equals(prfixCode + "textMaxWidthType"))
                {
                    if (fromFile)
                    {
                        TextMaxWidthType = parsString(Props[key], "per");
                    }
                    else
                    {
                        saveToFile("textMaxWidthType", TextMaxWidthType + "");
                    }
                }
                if (key.Equals(prfixCode + "imageMaxHeight"))
                {
                    if (fromFile)
                    {
                        ImageMaxHeight = parseToInt(Props[key], 400);
                    }
                    else
                    {
                        saveToFile("imageMaxHeight", ImageMaxHeight + "");
                    }
                }
                if (key.Equals(prfixCode + "runingTextSpeed"))
                {
                    if (fromFile)
                    {
                        RuningTextSpeed = parseToDouble(Props[key], 0.01);
                    }
                    else
                    {
                        saveToFile("runingTextSpeed", RuningTextSpeed + "");
                    }
                }
                if (key.Equals(prfixCode + "runnigText"))
                {
                    if (fromFile)
                    {
                        RunnigText = parsString(Props[key], " ");
                    }
                    else
                    {
                        saveToFile("runnigText", RunnigText + "");
                    }
                }
                if (key.Equals(prfixCode + "runnigTextColor"))
                {
                    if (fromFile)
                    {
                        RunnigTextColor = parseToBrush(Props[key],Brushes.White);
                    }
                    else
                    {
                        saveToFile("runnigTextColor", RunnigTextColor + "");
                    }
                }
                if (key.Equals(prfixCode + "textFontSize"))
                {
                    if (fromFile)
                    {
                        TextFontSize = parseToInt(Props[key], 18);
                    }
                    else
                    {
                        saveToFile("textFontSize", TextFontSize + "");
                    }
                }
                if (key.Equals(prfixCode + "textColor"))
                {
                    if (fromFile)
                    {
                        TextColor = parseToBrush(Props[key], Brushes.Black);
                    }
                    else
                    {
                        saveToFile("textColor", TextColor + "");
                    }
                }
                if (key.Equals(prfixCode + "textBackgroundColor"))
                {
                    if (fromFile)
                    {
                        TextBackgroundColor = parseToBrush(Props[key], Brushes.Black);
                    }
                    else
                    {
                        saveToFile("textBackgroundColor", TextBackgroundColor + "");
                    }
                }
                if (key.Equals(prfixCode + "phoneFontSize"))
                {
                    if (fromFile)
                    {
                        PhoneFontSize = parseToInt(Props[key], 18);
                    }
                    else
                    {
                        saveToFile("phoneFontSize", PhoneFontSize + "");
                    }
                }
                if (key.Equals(prfixCode + "houerFontSize"))
                {
                    if (fromFile)
                    {
                        HouerFontSize = parseToInt(Props[key], 10);
                    }
                    else
                    {
                        saveToFile("houerFontSize", HouerFontSize + "");
                    }
                }

                if (key.Equals(prfixCode + "hourColor"))
                {
                    if (fromFile)
                    {
                        HourColor = parseToBrush(Props[key], Brushes.Gray);
                    }
                    else
                    {
                        saveToFile("hourColor", HourColor + "");
                    }
                }

                if (key.Equals(prfixCode + "runnigTextSize"))
                {
                    if (fromFile)
                    {
                        RunnigTextSize = parseToInt(Props[key], 24);
                    }
                    else
                    {
                        saveToFile("runnigTextSize", RunnigTextSize + "");
                    }
                }
                if (key.Equals(prfixCode + "fullScreen"))
                {
                    if (fromFile)
                    {
                        FullScreen = parseBoolean(Props[key], false);
                    }
                    else
                    {
                        saveToFile("fullScreen", FullScreen + "");
                    }
                }
                if (key.Equals(prfixCode + "commandsOpOnlyFrom"))
                {
                    if (fromFile)
                    {
                        CommandsOpOnlyFrom = parsString(Props[key], "");
                    }
                    else
                    {
                        saveToFile("commandsOpOnlyFrom", CommandsOpOnlyFrom + "");
                    }
                }
                if (key.Equals(prfixCode + "commandsOpPassword"))
                {
                    if (fromFile)
                    {
                        CommandsOpPassword = parsString(Props[key], "");
                    }
                    else
                    {
                        saveToFile("commandsOpPassword", CommandsOpPassword + "");
                    }
                }
                if (key.Equals(prfixCode + "maxTextWidth"))
                {
                    if (fromFile)
                    {
                        MaxTextWidth = parseToInt(Props[key], 400);
                    }
                    else
                    {
                        saveToFile("maxTextWidth", MaxTextWidth + "");
                    }
                }
                if (key.Equals(prfixCode + "sideImageRunEveryInSec"))
                {
                    if (fromFile)
                    {
                        SideImageRunEveryInSec = parseToInt(Props[key], 10);
                    }
                    else
                    {
                        saveToFile("sideImageRunEveryInSec", SideImageRunEveryInSec + "");
                    }
                }
                if (key.Equals(prfixCode + "sideImagefadingSpeedInSec"))
                {
                    if (fromFile)
                    {
                        SideImagefadingSpeedInSec = parseToInt(Props[key], 10);
                    }
                    else
                    {
                        saveToFile("sideImagefadingSpeedInSec", SideImagefadingSpeedInSec + "");
                    }
                }
                if (key.Equals(prfixCode + "showSideImages"))
                {
                    if (fromFile)
                    {
                        ShowSideImages = parseBoolean(Props[key], true);
                    }
                    else
                    {
                        saveToFile("showSideImages", ShowSideImages + "");
                    }
                }
                if (key.Equals(prfixCode + "sideImageWidthType"))
                {
                    if (fromFile)
                    {
                        SideImageWidthType = parsString(Props[key], "per");
                    }
                    else
                    {
                        saveToFile("sideImageWidthType", SideImageWidthType + "");
                    }
                }
                if (key.Equals(prfixCode + "sideImageWidth"))
                {
                    if (fromFile)
                    {
                        SideImageWidth = parseToInt(Props[key], 600);
                    }
                    else
                    {
                        saveToFile("sideImageWidth", SideImageWidth + "");
                    }
                }
                if (key.Equals(prfixCode + "backgroundimage"))
                {
                    if (fromFile)
                    {
                        Backgroundimage = parsString(Props[key], "");
                    }
                    else
                    {
                        saveToFile("backgroundimage", Backgroundimage + "");
                    }
                }
                if (key.Equals(prfixCode + "demoImageFolder"))
                {
                    if (fromFile)
                    {
                        DemoImageFolder = parsString(Props[key], "demoImgs");
                    }
                    else
                    {
                        saveToFile("demoImageFolder", DemoImageFolder + "");
                    }
                }
                if (key.Equals(prfixCode + "downloadImgTo"))
                {
                    if (fromFile)
                    {
                        DownloadImgTo = parsString(Props[key], "");
                    }
                    else
                    {
                        saveToFile("downloadImgTo", DownloadImgTo + "");
                    }
                }
                if (key.Equals(prfixCode + "appToken"))
                {
                    if (fromFile)
                    {
                        AppToken = parsString(Props[key], "");
                    }
                    else
                    {
                        saveToFile("appToken", AppToken + "");
                    }
                }
                if (key.Equals(prfixCode + "phoneToken"))
                {
                    if (fromFile)
                    {
                        PhoneToken = parsString(Props[key], "");
                    }
                    else
                    {
                        saveToFile("phoneToken", PhoneToken + "");
                    }
                }
                if (key.Equals(prfixCode + "premissionsFileName"))
                {
                    if (fromFile)
                    {
                        PremissionsFileName = parsString(Props[key], "whatsAppShowePermissions.csv");
                    }
                    else
                    {
                        saveToFile("premissionsFileName", PremissionsFileName + "");
                    }
                }
                if (key.Equals(prfixCode + "premissionsDir"))
                {
                    if (fromFile)
                    {
                        PremissionsDir = parsString(Props[key], Environment.CurrentDirectory);
                    }
                    else
                    {
                        saveToFile("premissionsDir", PremissionsDir + "");
                    }
                }
            }

        }

       

        private static bool parseBoolean(string val, bool valToReturn)
        {
            if (!string.IsNullOrEmpty(val) && ("true".Equals(val, StringComparison.InvariantCultureIgnoreCase) || "TRUE".Equals(val, StringComparison.InvariantCultureIgnoreCase)))
            {
                return true;
            }
            return false;
        }

        private static Brush parseToBrush(string colorName, Brush brush)
        {

            try
            {
                return (SolidColorBrush)(new BrushConverter().ConvertFrom(colorName));
            }
            catch (Exception e)
            {
                
                
            }
            
            return brush;
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
        private double parseToDouble(string val, double valToReturn)
        {
            try
            {
                return Convert.ToDouble(val);
            }
            catch (Exception)
            {

                return valToReturn;
            }
        }

        public void saveToFile()
        {
            string filename = "whatsappShow.property";
            String prfixCode = "whatsappShow.";



            string text = File.ReadAllText(filename);
            foreach (var row in File.ReadAllLines("whatsappShow.property"))
            {
                if (!String.IsNullOrEmpty(row))
                {
                    if (row.Trim().Contains("paddingTop"))
                    {
                        text = text.Replace(row, prfixCode + "paddingTop = " + PaddingTop);
                    }

                }
            }

            File.WriteAllText(filename, text);

        }

        public void saveToFile(string propName, string value)
        {
            string filename = "whatsappShow.property";
            String prfixCode = "whatsappShow.";



            string text = File.ReadAllText(filename);
            foreach (var row in File.ReadAllLines("whatsappShow.property")) { 
                if (!String.IsNullOrEmpty(row))
                {
                    if (row.Trim().Contains(prfixCode + propName + " = "))
                    {
                        text = text.Replace(row, prfixCode + propName+" = " + value);
                    }
                   
                }
            }

            File.WriteAllText(filename, text);
            
          
        }

    }
}
