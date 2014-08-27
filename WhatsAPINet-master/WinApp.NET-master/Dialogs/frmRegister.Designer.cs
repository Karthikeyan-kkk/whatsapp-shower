using System.Windows.Forms;
namespace WinAppNET.Dialogs
{
    partial class frmRegister
    {
        /// <summary>
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows Form Designer generated code

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(frmRegister));
            this.Label1 = new System.Windows.Forms.Label();
            this.txtNumber = new System.Windows.Forms.TextBox();
            this.btnRequest = new System.Windows.Forms.Button();
            this.cmbMethod = new System.Windows.Forms.ComboBox();
            this.Label2 = new System.Windows.Forms.Label();
            this.txtPersonalPass = new System.Windows.Forms.TextBox();
            this.SuspendLayout();
            // 
            // Label1
            // 
            this.Label1.AutoSize = true;
            this.Label1.Location = new System.Drawing.Point(12, 15);
            this.Label1.Name = "Label1";
            this.Label1.Size = new System.Drawing.Size(76, 13);
            this.Label1.TabIndex = 0;
            this.Label1.Text = "Phone number";
            // 
            // txtNumber
            // 
            this.txtNumber.Location = new System.Drawing.Point(114, 12);
            this.txtNumber.Name = "txtNumber";
            this.txtNumber.Size = new System.Drawing.Size(170, 20);
            this.txtNumber.TabIndex = 1;
            // 
            // btnRequest
            // 
            this.btnRequest.Location = new System.Drawing.Point(186, 64);
            this.btnRequest.Name = "btnRequest";
            this.btnRequest.Size = new System.Drawing.Size(98, 21);
            this.btnRequest.TabIndex = 2;
            this.btnRequest.Text = "Request code";
            this.btnRequest.Click += new System.EventHandler(this.btnRequest_Click);
            // 
            // cmbMethod
            // 
            this.cmbMethod.FormattingEnabled = true;
            this.cmbMethod.ItemHeight = 13;
            this.cmbMethod.Location = new System.Drawing.Point(15, 64);
            this.cmbMethod.Name = "cmbMethod";
            this.cmbMethod.Size = new System.Drawing.Size(121, 21);
            this.cmbMethod.TabIndex = 3;
            // 
            // Label2
            // 
            this.Label2.AutoSize = true;
            this.Label2.Location = new System.Drawing.Point(12, 41);
            this.Label2.Name = "Label2";
            this.Label2.Size = new System.Drawing.Size(96, 13);
            this.Label2.TabIndex = 4;
            this.Label2.Text = "Personal password";
            // 
            // txtPersonalPass
            // 
            this.txtPersonalPass.Location = new System.Drawing.Point(114, 38);
            this.txtPersonalPass.Name = "txtPersonalPass";
            this.txtPersonalPass.PasswordChar = '*';
            this.txtPersonalPass.Size = new System.Drawing.Size(170, 20);
            this.txtPersonalPass.TabIndex = 5;
            // 
            // frmRegister
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(295, 101);
            this.Controls.Add(this.txtPersonalPass);
            this.Controls.Add(this.Label2);
            this.Controls.Add(this.cmbMethod);
            this.Controls.Add(this.btnRequest);
            this.Controls.Add(this.txtNumber);
            this.Controls.Add(this.Label1);
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.Name = "frmRegister";
            this.Text = "Register: Step 1";
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private TextBox txtNumber;
        private Button btnRequest;
        private ComboBox cmbMethod;
        private Label Label1;
        private Label Label2;
        private TextBox txtPersonalPass;
    }
}