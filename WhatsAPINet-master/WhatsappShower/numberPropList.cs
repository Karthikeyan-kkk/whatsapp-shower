using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;

namespace WhatsappShow
{
    class NumberPropList
    {

        private static NumberPropList instance;

        private NumberPropList() { }

        public static NumberPropList Instance
       {
          get 
          {
             if (instance == null)
             {
                 instance = new NumberPropList();
                 instance.loadCvs();
             }
             return instance;
          }
       }

        List<NumberProp> numberProps = new List<NumberProp>();

        internal List<NumberProp> NumberProps
        {
            get { return numberProps; }
            set { numberProps = value; }
        }

        public void loadCvs(){
            loadCvs("whatsAppShowePermissions.csv"); 
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

        public bool isCanShowMsg(string phone,string type) {
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
            if (NumberProps == null || NumberProps.Count < 1)
            {
                return -1;
            }
            for (int i = 0; i < NumberProps.Count; i++)
            {
                if (NumberProps[i].PhoneNumber.Contains(phone))
                {
                    return i;
                }
            }
            return -1;
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
