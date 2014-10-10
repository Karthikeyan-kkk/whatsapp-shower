using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;
using System.Windows.Media;
using log4net;
using System.Data.OleDb;



namespace whatsAppShowerWpf
{
    class NumberPropList
    {

        private static NumberPropList instance;
        private static readonly ILog systemLog = log4net.LogManager.GetLogger("systemsLog");
        private NumberPropList() { }
        Dictionary<string, SolidColorBrush> dictionary = new Dictionary<string, SolidColorBrush>();
        public static readonly Random random = new Random();

        public static NumberPropList Instance
        {
            get
            {
                if (instance == null)
                {
                    try
                    {
                        instance = new NumberPropList();
                        instance.loadFileProp();
                        if (Directory.Exists(WhatsappProperties.Instance.PremissionsDir))
                        {
                            FileSystemWatcher watcher = new FileSystemWatcher();
                            watcher.NotifyFilter = NotifyFilters.LastWrite;
                            watcher.Path = WhatsappProperties.Instance.PremissionsDir;
                            watcher.Filter = WhatsappProperties.Instance.PremissionsFileName;
                            watcher.Changed += new FileSystemEventHandler(watcher_Changed);
                            watcher.EnableRaisingEvents = true;
                        }
                    }
                    catch (Exception e)
                    {
                        systemLog.Error("fail start instance NumberPropList... "+ e);
                    }
                }
                return instance;
            }
        }

        static void watcher_Changed(object sender, FileSystemEventArgs e)
        {
            systemLog.Info("whatsAppShowePermissions file changed updating...");
            Instance.loadFileProp();
            systemLog.Info("whatsAppShowePermissions file changed finish updating...");
        }


        public Dictionary<string, SolidColorBrush> Dictionary
        {
            get { return dictionary; }
            set { dictionary = value; }
        }

        List<NumberProp> numberProps = new List<NumberProp>();

        internal List<NumberProp> NumberProps
        {
            get { return numberProps; }
            set { numberProps = value; }
        }

        public void loadFileProp()
        {
            string fullFilePath = WhatsappProperties.Instance.PremissionsDir + @"\" + WhatsappProperties.Instance.PremissionsFileName;
            if (!File.Exists(fullFilePath))
            {
                systemLog.Info("no PremissionsFileName to load found : " + WhatsappProperties.Instance.PremissionsFileName);
                return;
            }
            if (WhatsappProperties.Instance.PremissionsFileName.EndsWith("xls"))
            {
                loadXls(WhatsappProperties.Instance.PremissionsDir + @"\" + WhatsappProperties.Instance.PremissionsFileName);
                return;
            }
            if (WhatsappProperties.Instance.PremissionsFileName.EndsWith("cvs"))
            {
                loadCvs(WhatsappProperties.Instance.PremissionsDir + @"\" + WhatsappProperties.Instance.PremissionsFileName);
                return;
            }
            systemLog.Info("no PremissionsFileName surfix found : " + WhatsappProperties.Instance.PremissionsFileName);
        }

        private void loadXls(string file)
        {
            numberProps.Clear();
            string con= "Provider=Microsoft.ACE.OLEDB.12.0;Data Source=" + file + ";Extended Properties=\"Excel 12.0 Xml;HDR=YES;\"";
            using(OleDbConnection connection = new OleDbConnection(con))
            {
                connection.Open();
                OleDbCommand command = new OleDbCommand("select * from [whatsAppShowePermissions3$]", connection);
                using(OleDbDataReader dr = command.ExecuteReader())
                {
                     while(dr.Read())
                     {
                        


                         string phoneNumber = "";
                         bool isCanShowText = true;
                         bool isCanShowImg = true;
                         int textNumberInSeconde = 0;
                         int imgNumberInSeconde = 0;
                         long lastTextMsg = 0;
                         long lastImgMsg = 0;
                         if (dr.FieldCount > 0)
                         {
                             phoneNumber = dr.GetString(0);
                        }

                         if (dr.FieldCount > 1 && !dr.IsDBNull(1))
                         {
                             if ("false".Equals(dr.GetString(1), StringComparison.InvariantCultureIgnoreCase))
                             {
                                 isCanShowText = false;
                             }

                         }
                         if (dr.FieldCount > 2 && !dr.IsDBNull(2))
                         {
                             int i = 0;
                             string s = dr.GetString(2);
                             bool result = int.TryParse(s, out i);
                             if (result)
                             {
                                 textNumberInSeconde = i;
                             }

                         }
                         if (dr.FieldCount > 3 && !dr.IsDBNull(3))
                         {
                             int i = 0;
                             string s = dr.GetString(3);
                             bool result = int.TryParse(s, out i);
                             if (result)
                             {
                                 lastTextMsg = i;
                             }

                         }
                         if (dr.FieldCount > 4 && !dr.IsDBNull(4))
                         {
                             if ("false".Equals(dr.GetString(4), StringComparison.InvariantCultureIgnoreCase))
                             {
                                 isCanShowImg = false;
                             }

                         }


                         if (dr.FieldCount > 6 && !dr.IsDBNull(6))
                         {
                             int i = 0;
                             string s = dr.GetString(4);
                             bool result = int.TryParse(s, out i);
                             if (result)
                             {
                                 lastTextMsg = i;
                             }

                         }
                         if (dr.FieldCount > 5 && !dr.IsDBNull(5))
                         {
                             int i = 0;
                             string s = dr.GetString(6);
                             bool result = int.TryParse(s, out i);
                             if (result)
                             {
                                 imgNumberInSeconde = i;
                             }

                         }

                         NumberProp numberProp = new NumberProp();
                         numberProp.PhoneNumber = phoneNumber;
                         numberProp.IsCanShowText = isCanShowText;
                         numberProp.IsCanShowImg = isCanShowImg;
                         numberProp.TextNumberIsSeconde = textNumberInSeconde;
                         numberProp.ImgNumberIsSeconde = imgNumberInSeconde;
                         numberProp.LastTextMsg = lastTextMsg;
                         numberProp.LastImgMsg = lastImgMsg;
                         NumberProps.Add(numberProp);





                     }
                }
            }
        }

        void loadCvs(string fullPathfileName)
        {
            numberProps.Clear();
            var reader = getCvsReader(fullPathfileName);
            if (reader == null)
            {
                return;
            }

            while (!reader.EndOfStream)
            {
                var line = reader.ReadLine();
                var values = line.Split(',');
                if (values != null)
                {
                    string phoneNumber = "";
                    bool isCanShowText = true;
                    bool isCanShowImg = true;
                    int textNumberInSeconde = 0;
                    int imgNumberInSeconde = 0;
                    long lastTextMsg = 0;
                    long lastImgMsg = 0;
                    if (values.Length > 0)
                    {
                        phoneNumber = values[0];

                    }

                    if (values.Length > 1)
                    {
                        if ("false".Equals(values[1], StringComparison.InvariantCultureIgnoreCase))
                        {
                            isCanShowText = false;
                        }

                    }
                    if (values.Length > 2)
                    {
                        int i = 0;
                        string s = values[2];
                        bool result = int.TryParse(s, out i);
                        if (result)
                        {
                            textNumberInSeconde = i;
                        }

                    }
                    if (values.Length > 3)
                    {
                        int i = 0;
                        string s = values[3];
                        bool result = int.TryParse(s, out i);
                        if (result)
                        {
                            lastTextMsg = i;
                        }

                    }
                    if (values.Length > 4)
                    {
                        if ("false".Equals(values[4], StringComparison.InvariantCultureIgnoreCase))
                        {
                            isCanShowImg = false;
                        }

                    }


                    if (values.Length > 6)
                    {
                        int i = 0;
                        string s = values[4];
                        bool result = int.TryParse(s, out i);
                        if (result)
                        {
                            lastTextMsg = i;
                        }

                    }
                    if (values.Length > 5)
                    {
                        int i = 0;
                        string s = values[6];
                        bool result = int.TryParse(s, out i);
                        if (result)
                        {
                            imgNumberInSeconde = i;
                        }

                    }

                    NumberProp numberProp = new NumberProp();
                    numberProp.PhoneNumber = phoneNumber;
                    numberProp.IsCanShowText = isCanShowText;
                    numberProp.IsCanShowImg = isCanShowImg;
                    numberProp.TextNumberIsSeconde = textNumberInSeconde;
                    numberProp.ImgNumberIsSeconde = imgNumberInSeconde;
                    numberProp.LastTextMsg = lastTextMsg;
                    numberProp.LastImgMsg = lastImgMsg;
                    NumberProps.Add(numberProp);

                }

            }

        }
        private static StreamReader getCvsReader(string fullPathfileName)
        {
            try
            {
                var reader = new StreamReader(File.OpenRead(fullPathfileName));
                return reader;
            }
            catch (Exception)
            {

                return null;
            }

        }

        public bool isCanShowMsg(string phone, string type)
        {
            bool isCanShowMsg = true;
            if (string.IsNullOrEmpty(phone))
            {
                return isCanShowMsg;
            }
            int index = findNumberPropIndex(phone);
            if (index == -1)
            {
                return true;
            }
            if (NumberProps != null && NumberProps.Count > 0)
            {
                NumberProp theNumberProp = NumberProps[index];
                if ("IMG".Equals(type))
                {
                    return theNumberProp.IsCanShowImg;
                }
                if ("TEXT".Equals(type))
                {
                    return theNumberProp.IsCanShowText;
                }
            }
            return isCanShowMsg;
        }

        private int findNumberPropIndex(string phone)
        {
            if (NumberProps == null || NumberProps.Count < 1 || string.IsNullOrEmpty(phone))
            {
                return -1;
            }
            for (int i = 0; i < NumberProps.Count; i++)
            {
                if (phone.Contains(NumberProps[i].PhoneNumber))
                {
                    return i;
                }
            }
            return -1;
        }

        public SolidColorBrush getPhoneColor(String phoneNumber)
        {

            if (dictionary.ContainsKey(phoneNumber))
            {
                return dictionary[phoneNumber];
            }
            byte[] colorBytes = new byte[3];
            random.NextBytes(colorBytes);
            SolidColorBrush phoneColor = new SolidColorBrush(System.Windows.Media.Color.FromRgb( colorBytes[0], colorBytes[1], colorBytes[2]));
            dictionary.Add(phoneNumber, phoneColor);
            return phoneColor;
        }
    }

    class NumberProp
    {
        string phoneNumber;
        bool isCanShowText;
        bool isCanShowImg;
        int textNumberIsSeconde;
        int imgNumberIsSeconde;
        long lastTextMsg;
        long lastImgMsg;


        public string PhoneNumber
        {
            get { return phoneNumber; }
            set { phoneNumber = value; }
        }
        public bool IsCanShowImg
        {
            get { return isCanShowImg; }
            set { isCanShowImg = value; }
        }
        public bool IsCanShowText
        {
            get { return isCanShowText; }
            set { isCanShowText = value; }
        }
        public int TextNumberIsSeconde
        {
            get { return textNumberIsSeconde; }
            set { textNumberIsSeconde = value; }
        }
        public int ImgNumberIsSeconde
        {
            get { return imgNumberIsSeconde; }
            set { imgNumberIsSeconde = value; }
        }
        public long LastTextMsg
        {
            get { return lastTextMsg; }
            set { lastTextMsg = value; }
        }
        public long LastImgMsg
        {
            get { return lastImgMsg; }
            set { lastImgMsg = value; }
        }
        

    }
}
