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

        public ImgView()
        {
            InitializeComponent();
        }
        
        public ImgView(string phoneNumber, ImageSource imageSource, string hour)
        {
            InitializeComponent();
            this.phoneNumber = phoneNumber;
            this.imageSourceLink = imageSource;
            this.hour = hour;
            this.imgField.Source = imageSource;
            double screenWidth = System.Windows.SystemParameters.PrimaryScreenWidth;
            double imageMaxWidth = (screenWidth * WhatsappProperties.Instance.ImageMaxWidth) / 100;
            if (!string.IsNullOrEmpty(WhatsappProperties.Instance.ImageMaxWidthType) && "pix".Equals(WhatsappProperties.Instance.ImageMaxWidthType))
            {
                imageMaxWidth = WhatsappProperties.Instance.ImageMaxWidth;
            }
            this.imgField.MaxWidth = imageMaxWidth;
            this.phoneField.Text = PhoneNumber;
            this.phoneField.Foreground = NumberPropList.Instance.getPhoneColor(phoneNumber);
            this.phoneField.FontSize = WhatsappProperties.Instance.PhoneFontSize;

            this.hourField.Text = hour;
            this.hourField.Foreground = Brushes.Gray;
            this.hourField.FontSize = WhatsappProperties.Instance.HouerFontSize;
            this.HorizontalAlignment = HorizontalAlignment.Left;
            this.Margin = new Thickness(WhatsappProperties.Instance.PaddingLeft, WhatsappProperties.Instance.PaddingTop, 0, 0);
        }

        
    }
}
