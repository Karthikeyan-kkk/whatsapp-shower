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
using System.Windows.Media.Animation;

namespace whatsAppShowerWpf
{
    /// <summary>
    /// Interaction logic for Login.xaml
    /// </summary>
    /// 
    
    public partial class Login : Window
    {
        public Login()
    {
      InitializeComponent();
     
    }

    private void btnCancel_Click(object sender, RoutedEventArgs e)
    {
      DialogResult = false;
      Application.Current.Shutdown();
    }

    private void btnLogin_Click(object sender, RoutedEventArgs e)
    {
      bool showDemo = (this.showDemo.IsChecked == true);
      if (!showDemo && string.IsNullOrEmpty(this.txtUserName.Text))
      {
          System.Windows.MessageBox.Show("Phone number can't be empty");
          return;
      }
      DialogResult = true;
      this.Close();
    }

    private void showDemo_Checked(object sender, RoutedEventArgs e)
    {
        bool showDemo = (this.showDemo.IsChecked == true);
        if (showDemo)
        {
            this.txtUserName.Text = "";
            this.txtUserName.IsEnabled = false;
        }
        else
        {
            this.txtUserName.IsEnabled = true;
        }
    }
  }
    }

