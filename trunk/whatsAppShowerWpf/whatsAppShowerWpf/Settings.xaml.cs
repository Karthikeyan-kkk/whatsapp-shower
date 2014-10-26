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
using System.Reflection;
using log4net;
using Xceed.Wpf.Toolkit;

namespace whatsAppShowerWpf
{
    /// <summary>
    /// Interaction logic for Settings.xaml
    /// </summary>
    public partial class Settings : Window
    {
        private static Settings instance;
        private static readonly ILog systemLog = log4net.LogManager.GetLogger("systemsLog");
        private static List<BindingExpression> bindingExpressions = new List<BindingExpression>();

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


        public static List<BindingExpression> BindingExpressions
        {
            get { return Settings.bindingExpressions; }
            set { Settings.bindingExpressions = value; }
        }


        public Settings()
        {
            Closing += new System.ComponentModel.CancelEventHandler(Settings_Closing);
            InitializeComponent();

            Type type = WhatsappProperties.Instance.GetType();
            PropertyInfo[] properties = type.GetProperties();
            int marginTop = 0;

            foreach (PropertyInfo property in properties)
            {
                if (property.Name.Equals("Instance") || property.Name.Equals("ImageMaxHeight") || property.Name.Equals("RunnigTextColor") || property.Name.Equals("PhoneNumber") || property.Name.Equals("Password") || property.Name.Equals("AppToken") || property.Name.Equals("NickName") || property.Name.Equals("PhoneToken") || property.Name.Equals("SideImageWidth") || property.Name.Equals("SideImageWidthType"))
                {
                    continue;
                }

                Label testLabel = new Label();
                testLabel.Margin = new Thickness(0, marginTop, 0, 0);
                testLabel.Name = property.Name + "Label";
                testLabel.Content = property.Name;
                testLabel.Height = 28;
                testLabel.HorizontalAlignment = HorizontalAlignment.Left;
                testLabel.VerticalAlignment = VerticalAlignment.Top;
                this.flowGrid.Children.Add(testLabel);
                Grid.SetColumn(testLabel, 0);

                string userType = property.Name;
                if (property.Name.EndsWith("Color"))
                {
                    userType = "Color";
                }
                if (property.Name.EndsWith("Type"))
                {
                    userType = "Type";
                }
                UIElement uIElement = null;
                switch (userType)
                {
                    case "Color":
                        uIElement = createColorPicker(marginTop, property.Name);
                        break;
                    case "Type":
                        uIElement = createSizeType(marginTop, property.Name);
                        break;
                    default:
                        uIElement = createTextBox(marginTop, property.Name);
                        break;
                }



                this.flowGrid.Children.Add(uIElement);
                Grid.SetColumn(uIElement, 1);

                marginTop = marginTop + 40;



            }
        }

        private UIElement createSizeType(int marginTop, string propName)
        {
            ListBox listBox = new ListBox();
            listBox.Margin = new Thickness(0, marginTop, 0, 0);
            listBox.Height = 36;
            listBox.HorizontalAlignment = HorizontalAlignment.Left;
            listBox.Name = propName + "textBox";
            listBox.VerticalAlignment = VerticalAlignment.Top;
            listBox.Width = 120;
            listBox.Items.Insert(0, "pix");
            listBox.Items.Insert(1, "per");

            Binding binding = new Binding(propName);
            binding.Source = WhatsappProperties.Instance;
            binding.UpdateSourceTrigger = UpdateSourceTrigger.Explicit;
            listBox.SetBinding(ListBox.SelectedValueProperty, binding);
            BindingExpression be = listBox.GetBindingExpression(ListBox.SelectedValueProperty);
            BindingExpressions.Add(be);

            return listBox;
        }

        private UIElement createColorPicker(int marginTop, string propName)
        {
            ColorPicker colorPicker = new ColorPicker();
            colorPicker.Margin = new Thickness(0, marginTop, 0, 0);
            colorPicker.Height = 36;
            colorPicker.HorizontalAlignment = HorizontalAlignment.Left;
            colorPicker.Name = propName + "textBox";
            colorPicker.VerticalAlignment = VerticalAlignment.Top;
            colorPicker.Width = 120;
            return colorPicker;
        }

        private UIElement createTextBox(int marginTop, string propName)
        {
            TextBox textBox = new TextBox();
            textBox.Margin = new Thickness(0, marginTop, 0, 0);
            textBox.Height = 36;
            textBox.HorizontalAlignment = HorizontalAlignment.Left;
            textBox.Name = propName + "textBox";
            textBox.VerticalAlignment = VerticalAlignment.Top;
            textBox.Width = 120;

            Binding binding = new Binding(propName);
            binding.Source = WhatsappProperties.Instance;
            binding.UpdateSourceTrigger = UpdateSourceTrigger.Explicit;
            Control control = (Control)textBox;
            control.SetBinding(TextBox.TextProperty, binding);
            BindingExpression be = control.GetBindingExpression(TextBox.TextProperty);
            BindingExpressions.Add(be);

            return textBox;
        }

        void Settings_Closing(object sender, System.ComponentModel.CancelEventArgs e)
        {
            instance = null;
        }
        public delegate void OnUpdateEvent(object source, EventArgs e);
        public event OnUpdateEvent OnUpdate;
        private void button_Click(object sender, RoutedEventArgs e)
        {

            updateStatusBar("Updating...", Brushes.Black);
            try
            {
                foreach (BindingExpression be in BindingExpressions)
                {
                    be.UpdateSource();
                }

                WhatsappProperties.Instance.syncProp(false);
                EventArgs eventArgs = new EventArgs();
                if (OnUpdate != null)
                {
                    OnUpdate(this, eventArgs);
                }
            }
            catch (Exception ex)
            {
                updateStatusBar("Error", Brushes.Red);
                systemLog.Error("error in updateProperties: " + ex);
            }

        }
        public void updateStatusBar(string msg, Brush Color)
        {
            statusBarTop.Text = msg;
            statusBarTop.Foreground = Color;

            statusBarBottom.Text = msg;
            statusBarBottom.Foreground = Color;
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
