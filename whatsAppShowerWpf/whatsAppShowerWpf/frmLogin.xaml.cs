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

namespace WPFLoginWindowCS
{
  /// <summary>
  /// Interaction logic for frmLogin.xaml
  /// </summary>
  public partial class frmLogin : Window
  {
    public frmLogin()
    {
      InitializeComponent();
    }

    private void btnCancel_Click(object sender, RoutedEventArgs e)
    {
      DialogResult = false;
      this.Close();
    }

    private void btnLogin_Click(object sender, RoutedEventArgs e)
    {
        string phoneNumber = this.txtUserName.Text;
        string token = this.txtPassword.Password;
        if (string.IsNullOrEmpty(phoneNumber) || string.IsNullOrEmpty(token))
        {
            DialogResult = false;
        }
        else { 
            DialogResult = true;
        }
        this.Close();
    }
  }
}
