
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading;
using System.Windows.Forms;
using WinAppNET.AppCode;

namespace WinAppNET.Dialogs
{
    public partial class frmGoogleSync : Form
    {
        protected string email;
        protected string password;

        public frmGoogleSync()
        {
            InitializeComponent();
        }

        delegate void setLabelTextDelegate(string text, Color color);

        protected void setLabelText(string text, Color color)
        {
            if (this.InvokeRequired)
            {
                setLabelTextDelegate d = new setLabelTextDelegate(setLabelText);
                this.Invoke(d, new object[]{text, color});
            }
            else
            {
                this.lblError.Text = text;
                this.lblError.ForeColor = color;
            }
        }

        private void btnSync_Click(object sender, EventArgs e)
        {
            this.setLabelText("Logging in...", Color.Black);
            this.email = this.txtEmail.Text;
            this.password = this.txtPassword.Text;
            if (string.IsNullOrEmpty(this.email))
            {
                this.lblError.Text = "Please enter your email address";
                this.lblError.ForeColor = Color.Red;
                return;
            }
            if (string.IsNullOrEmpty(this.password))
            {
                this.lblError.Text = "Please enter your password";
                this.lblError.ForeColor = Color.Red;
                return;
            }

            Thread t = new Thread(new ThreadStart(ExecuteImport));
            t.IsBackground = true;
            t.Start();
        }

        delegate void remoteDelegate();

        protected void doExit()
        {
            if (this.InvokeRequired)
            {
                remoteDelegate d = new remoteDelegate(doExit);
                this.Invoke(d);
            }
            else
            {
                this.Dispose();
            }
        }

        protected void showProgressBar(int total)
        {
            if (this.InvokeRequired)
            {
                progressDelegate r = new progressDelegate(showProgressBar);
                this.Invoke(r, new object[] { total });
            }
            else
            {
                this.btnSync.Hide();
                this.progressBar.Show();
                this.progressBar.Minimum = 0;
                this.progressBar.Maximum = total;
            }
        }

        delegate void progressDelegate(int progress);

        protected void setProgress(int progress)
        {
            if (this.InvokeRequired)
            {
                progressDelegate d = new progressDelegate(setProgress);
                this.Invoke(d, new object[] { progress });
            }
            else
            {
                this.progressBar.Value = progress;
            }
        }

        protected void ExecuteImport()
        {}
    }
}
