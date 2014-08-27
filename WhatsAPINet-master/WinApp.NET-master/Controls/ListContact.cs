using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using WinAppNET.AppCode;
using System.IO;

namespace WinAppNET.Controls
{
    public partial class ListContact : UserControl
    {
        public Contact contact;
        public ListContact(string jid)
        {
            InitializeComponent();
            int vertScrollWidth = SystemInformation.VerticalScrollBarWidth;
            this.Width -= vertScrollWidth;
            this.contact = ContactStore.GetContactByJid(jid);
            if (this.contact != null)
            {
                this.lblName.Text = this.contact.FullName;
                string imgpath = ChatWindow.getCacheImagePath(jid);
                if (File.Exists(imgpath))
                {
                    try
                    {
                        Image i = Image.FromFile(imgpath);
                        Bitmap b = new Bitmap(i, new Size(48, 48));
                        this.pictureBox1.Image = b;
                        i.Dispose();
                    }
                    catch (Exception e)
                    {
                        throw e;
                    }
                }
            }
        }
    }
}
