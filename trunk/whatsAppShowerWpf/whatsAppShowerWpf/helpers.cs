using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using WhatsAppApi.Helper;
using System.IO;
using log4net;

namespace whatsAppShowerWpf
{
    class Helpers
    {
        private static readonly ILog systemLog = log4net.LogManager.GetLogger("systemsLog");
        public static string filterFromNumber(String from)
        {
            char[] splitChar = { '@' };
            return from.Split(splitChar)[0];
        }
        public static string getNikeName(ProtocolTreeNode node)
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

        public static string formatPhoneNumber(String phoneNumber)
        {
            if (!string.IsNullOrEmpty(phoneNumber) && phoneNumber.StartsWith("972"))
            {
                phoneNumber = phoneNumber.Replace("972", "0");
                if (phoneNumber.Length == 10)
                {
                    phoneNumber = phoneNumber.Insert(3, "-");
                }

            }
            return phoneNumber;
        }


        public static string getImgFullPath(string file)
        {
            try
            {
                if (string.IsNullOrEmpty(WhatsappProperties.Instance.DownloadImgTo))
                {
                    return file;
                }
                if (!Directory.Exists(WhatsappProperties.Instance.DownloadImgTo))
                {
                    Directory.CreateDirectory(WhatsappProperties.Instance.DownloadImgTo);
                }
            }
            catch (Exception)
            {
                systemLog.Error("faild create DownloadImgTo Directory: " + WhatsappProperties.Instance.DownloadImgTo);
                return file;
            }
            return WhatsappProperties.Instance.DownloadImgTo + @"\" + file;
        }

    }
}
