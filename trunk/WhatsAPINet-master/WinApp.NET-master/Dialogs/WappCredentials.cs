using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using WinAppNET.AppCode;
using WindowsFormsApplication2;

namespace WinAppNET.Dialogs
{
    public partial class WappCredentials : Form
    {
        public WappCredentials()
        {
            InitializeComponent();
        }

        private void btnLogin_Click(object sender, EventArgs e)
        {
            string username = this.txtPhonenumber.Text;
            string password = this.txtPassword.Text;
            if (!string.IsNullOrEmpty(username) && !String.IsNullOrEmpty(password))
            {
                System.Configuration.ConfigurationManager.AppSettings.Set("Username", username);
                System.Configuration.ConfigurationManager.AppSettings.Set("Password", password);
                //this.DialogResult = System.Windows.Forms.DialogResult.OK;
               // this.Dispose();
                WhatsappShower whatsappShower = new WhatsappShower(username, password, "sdas");
                WhatsappProperties.initProperties();
                whatsappShower.ShowDialog();
            }
        }

        private void btnRegister_Click(object sender, EventArgs e)
        {
            frmRegister regform = new frmRegister();
            regform.SetNumber(this.txtPhonenumber.Text);
            DialogResult regres = regform.ShowDialog();
            if (regres == System.Windows.Forms.DialogResult.OK)
            {
                if (!string.IsNullOrEmpty(regform.password))
                {
                    //found password!
                    this.onPasswordReceived(regform.phonenumber, regform.password);
                    return;
                }
                this.txtPhonenumber.Text = regform.phonenumber;

                //show code register form
                frmRegister2 reg2 = new frmRegister2();
                reg2.identity = regform.identity;
                reg2.phonenumber = regform.phonenumber;
                regres = reg2.ShowDialog();
                if (regres == System.Windows.Forms.DialogResult.OK && !string.IsNullOrEmpty(reg2.password))
                {
                    //success!
                    this.onPasswordReceived(reg2.phonenumber, reg2.password);
                    return;
                }
            }

            //nope nope nope
            MessageBox.Show("Failed registering your number");
        }

        private void onPasswordReceived(string username, string password)
        {
            this.txtPhonenumber.Text = username;
            this.txtPassword.Text = password;
        }
    }
}
