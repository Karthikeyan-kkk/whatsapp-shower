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
    public partial class frmRegister : Form
    {
        private string[] methods = { "sms", "voice" };
        private string _phonenumber;
        public string phonenumber
        {
            get
            {
                return this._phonenumber;
            }
        }
        public string password = string.Empty;
        public string identity;

        public void SetNumber(string phonenumber)
        {
            this._phonenumber = phonenumber;
            this.txtNumber.Text = phonenumber;
        }

        public frmRegister()
        {
            InitializeComponent();
            this.cmbMethod.DataSource = methods;
        }

        private void btnRequest_Click(object sender, EventArgs e)
        {
            this._phonenumber = this.txtNumber.Text;
            if (!string.IsNullOrEmpty(this.phonenumber))
            {
                try
                {
                    this.identity = WhatsAppApi.Register.WhatsRegisterV2.GenerateIdentity(this.phonenumber, this.txtPersonalPass.Text);
                    string method = this.cmbMethod.Text;
                    string response = string.Empty;
                    if (WhatsAppApi.Register.WhatsRegisterV2.RequestCode(this.phonenumber, out this.password, out response, method, this.identity))
                    {
                        this.DialogResult = System.Windows.Forms.DialogResult.OK;
                    }
                    else
                    {
                        this.DialogResult = System.Windows.Forms.DialogResult.OK;
                        MessageBox.Show(String.Format("Could not request code:\r\n{0}", response));

                    }
                }
                catch (Exception)
                {
                    return;
                }
            }
        }
    }
}
