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

namespace whatsAppShowerWpf
{
    /// <summary>
    /// Interaction logic for ImgView.xaml
    /// </summary>
    /// 
    
    public partial class ImgView : UserControl
    {
        private string phoneNumber;
        private string nickName;
        private ImageSource imageSourceLink;
        private string hour;
        private double maximumSize;

        public double MaximumSize
        {
            get { return maximumSize; }
            set { maximumSize = value; }
        }

        public string PhoneNumber
        {
            get { return phoneNumber; }
            set { phoneNumber = value; }
        }
        public ImageSource ImageSourceLink
        {
            get { return imageSourceLink; }
            set { imageSourceLink = value; }
        }
        public string Hour
        {
            get { return hour; }
            set { hour = value; }
        }

        public string NickName
        {
            get { return nickName; }
            set { nickName = value; }
        }

        public ImgView()
        {
            InitializeComponent();
        }
        
        public ImgView(string phoneNumber, string nickName , ImageSource imageSource, string hour)
        {
            InitializeComponent();
            this.PhoneNumber = phoneNumber;
            this.NickName = nickName;
            this.ImageSourceLink = imageSource;
            this.Hour = hour;
            
            this.imgField.Source = ImageSourceLink;
            string from = PhoneNumber;
            if (string.IsNullOrEmpty(nickName))
            {
                from = PhoneNumber;
            }
            else
            {
                from = PhoneNumber + " - " + nickName;
                
            }
           
            Helpers.parseEmjoi(from, this.fromfd);
            this.phoneField.Foreground = NumberPropList.Instance.getPhoneColor(phoneNumber);
            this.hourField.Text = Hour;
            this.hourField.Foreground = Brushes.Gray;
            this.HorizontalAlignment = HorizontalAlignment.Left;
            buildImgView(this);
            this.phoneField.Width = Helpers.MeasureString(from, this.phoneField.FontFamily, this.phoneField.FontStyle, this.phoneField.FontWeight, this.phoneField.FontStretch, this.phoneField.FontSize).Width + 50;
            
        }



        public static void buildImgView(ImgView imgView)
        {
            if (WhatsappProperties.Instance.ImageMaxWidth != 0)
            {
                double screenWidth = System.Windows.SystemParameters.PrimaryScreenWidth;
                double imageMaxWidth = (screenWidth * WhatsappProperties.Instance.ImageMaxWidth) / 100;
                if (!string.IsNullOrEmpty(WhatsappProperties.Instance.ImageMaxWidthType) && "pix".Equals(WhatsappProperties.Instance.ImageMaxWidthType))
                {
                    imageMaxWidth = WhatsappProperties.Instance.ImageMaxWidth;
                }
                imgView.imgField.MaxWidth = imageMaxWidth;
            }
            else
            {
                double screenWidth = System.Windows.SystemParameters.PrimaryScreenWidth;
                
                double imageMaxWidth = ((screenWidth * WhatsappProperties.Instance.MsgSectionWidth) / 100);
                imageMaxWidth = imageMaxWidth - WhatsappProperties.Instance.PaddingLeft - 20;
                if (!string.IsNullOrEmpty(WhatsappProperties.Instance.ImageMaxWidthType) && "pix".Equals(WhatsappProperties.Instance.ImageMaxWidthType))
                {
                    imageMaxWidth = WhatsappProperties.Instance.MsgSectionWidth;
                }
                imgView.imgField.MaxWidth = imageMaxWidth;
               
            }
            
            imgView.phoneField.FontSize = WhatsappProperties.Instance.PhoneFontSize;
            imgView.hourField.FontSize = WhatsappProperties.Instance.HouerFontSize;
            imgView.Margin = new Thickness(WhatsappProperties.Instance.PaddingLeft, WhatsappProperties.Instance.PaddingTop, 0, 0);
        }

        
    }
}
