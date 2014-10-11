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
using System.Windows.Shapes;

namespace whatsAppShowerWpf
{
    /// <summary>
    /// Interaction logic for Settings.xaml
    /// </summary>
    public partial class Settings : Window
    {
        private static Settings instance;


        public static Settings Instance
        {
            get
            {
                if (instance == null)
                {
                    instance = new Settings();
                }
                return instance;
            }
        }
       
        public Settings()
        {
            Closing += new System.ComponentModel.CancelEventHandler(Settings_Closing);
            InitializeComponent();

           


            
            Label testLabel = new Label();
            testLabel.Name="aa";
            testLabel.Content = "testLabel";
            testLabel.Height=28;
            testLabel.HorizontalAlignment = HorizontalAlignment.Left;
            testLabel.VerticalAlignment = VerticalAlignment.Top;
            this.flowGrid.Children.Add(testLabel);
            Grid.SetColumn(testLabel, 0);

            TextBox textBox = new TextBox();
            textBox.Height = 36;
            textBox.HorizontalAlignment = HorizontalAlignment.Left;
            textBox.Name = "textBox";
            textBox.VerticalAlignment = VerticalAlignment.Top;
            textBox.Width = 120;

           



            this.flowGrid.Children.Add(textBox);
            Grid.SetColumn(textBox, 1);

            var binding = new Binding("textBox");
            binding.Source = this.flowGrid;
            //      binding.Path = new PropertyPath(ListBox.SelectedValueProperty);
            var bound = this.flowGrid.SetBinding(textBox.Text, binding);




            
        }

        void Settings_Closing(object sender, System.ComponentModel.CancelEventArgs e)
        {
            instance = null;
        }

        private void button_Click(object sender, RoutedEventArgs e)
        {
            //WhatsappProperties.Instance.PaddingTop = validateNumber(this.paddingTopsettingRow.textBox1.Text, WhatsappProperties.Instance.PaddingTop);
           // WhatsappProperties.Instance.PaddingLeft = validateNumber(this.paddingLeftsettingRow.textBox1.Text, WhatsappProperties.Instance.PaddingLeft);


            WhatsappProperties.Instance.syncProp(false);
        }

        private int validateNumber(string newParam, int paramToReturn)
        {
            int n;
            bool isNumeric = int.TryParse(newParam, out n);
            if (isNumeric)
            {
                return n;
            }
            return paramToReturn;
        }
    }
}
