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
    /// Interaction logic for SettingRow.xaml
    /// </summary>
    public partial class SettingRow : UserControl
    {

        
        public SettingRow()
        {
            InitializeComponent();
        }
        public string LabelText
        {
            get
            {
                return (string)label1.Content;
            }

            set
            {
               label1.Content =  value;
            }
        }
       
        public string TextBoxText
        {
            get
            {
                return textBox1.Text;
            }

            set
            {
                textBox1.Text = value;
            }
        }
    }
   
}
