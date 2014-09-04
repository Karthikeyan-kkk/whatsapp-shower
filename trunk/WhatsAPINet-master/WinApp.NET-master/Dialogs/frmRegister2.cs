using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using WinAppNET.AppCode;

namespace WinAppNET.Dialogs
{
    public partial class frmRegister2 : Form
    {
        public string phonenumber;
        public string identity;
        public string password;
         
        public frmRegister2()
        {
            InitializeComponent();
        }

        private void btnSubmit_Click(object sender, EventArgs e)
        {
            if(!String.IsNullOrEmpty(this.txtCode.Text) && this.txtCode.Text.Length == 6)
            {
                try
                {
                    this.password = WhatsAppApi.Register.WhatsRegisterV2.RegisterCode(this.phonenumber, this.txtCode.Text, this.identity);
                    if (!string.IsNullOrEmpty(this.password))
                    {
                        this.DialogResult = System.Windows.Forms.DialogResult.OK;
                    }
                    else
                    {
                        MessageBox.Show("Verification code not accepted");
                    }
                }
                catch(Exception)
                {}
            }
        }
    }
}
