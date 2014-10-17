using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using WhatsAppApi.Helper;
using System.IO;
using log4net;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media.Animation;

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

        public void buildImgView(ImgView imgView)
        {
            double screenWidth = System.Windows.SystemParameters.PrimaryScreenWidth;
            double imageMaxWidth = (screenWidth * WhatsappProperties.Instance.ImageMaxWidth) / 100;
            if (!string.IsNullOrEmpty(WhatsappProperties.Instance.ImageMaxWidthType) && "pix".Equals(WhatsappProperties.Instance.ImageMaxWidthType))
            {
                imageMaxWidth = WhatsappProperties.Instance.ImageMaxWidth;
            }
            imgView.imgField.MaxWidth = imageMaxWidth;
            imgView.phoneField.FontSize = WhatsappProperties.Instance.PhoneFontSize;
            imgView.hourField.FontSize = WhatsappProperties.Instance.HouerFontSize;
            imgView.Margin = new Thickness(WhatsappProperties.Instance.PaddingLeft, WhatsappProperties.Instance.PaddingTop, 0, 0);
        }

        public void buildTextView(TextView textView)
        {
            textView.Margin = new Thickness(WhatsappProperties.Instance.PaddingLeft, WhatsappProperties.Instance.PaddingTop, 0, 0);
            textView.textMsgField.FontSize = WhatsappProperties.Instance.TextFontSize;
            textView.fromField.FontSize = WhatsappProperties.Instance.PhoneFontSize;
            textView.hourField.FontSize = WhatsappProperties.Instance.HouerFontSize;
            double screenWidth = System.Windows.SystemParameters.PrimaryScreenWidth;
            double textMaxWidth = (screenWidth * WhatsappProperties.Instance.ImageMaxWidth) / 100;
            if (!string.IsNullOrEmpty(WhatsappProperties.Instance.TextMaxWidthType) && "pix".Equals(WhatsappProperties.Instance.TextMaxWidthType))
            {
                textMaxWidth = WhatsappProperties.Instance.ImageMaxWidth;
            }
            textView.textMsgField.MaxWidth = textMaxWidth;
        }
        public void buildRunningText(MainWindow mainWindow)
        {
            mainWindow.txtKron.Text = WhatsappProperties.Instance.RunnigText;
            mainWindow.txtKron.Foreground = WhatsappProperties.Instance.RunnigTextColor;
            mainWindow.txtKron.FontSize = WhatsappProperties.Instance.RunnigTextSize;
            mainWindow.txtKron2.Foreground = WhatsappProperties.Instance.RunnigTextColor;
            mainWindow.txtKron2.FontSize = WhatsappProperties.Instance.RunnigTextSize;
            if (mainWindow.stack.Resources["slide"] != null)
            {
                Storyboard storyboard = (Storyboard)mainWindow.stack.Resources["slide"];
                storyboard.SpeedRatio = WhatsappProperties.Instance.RuningTextSpeed;
                mainWindow.stack.BeginStoryboard(storyboard);
            }
        }

    }
}
