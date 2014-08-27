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
    public partial class ListChat : UserControl
    {
        public ListChat(WappMessage msg)
        {
            InitializeComponent();
            this.textBox1.RightToLeft = System.Windows.Forms.RightToLeft.No;
            if (!msg.from_me)
            {
                this.textBox1.Location = new Point(3, this.textBox1.Location.Y);
            }
            if (!String.IsNullOrEmpty(msg.author))
            {
                Contact c = ContactStore.GetContactByJid(msg.author);
                if (c != null)
                {
                    msg.author = c.FullName;
                }
                msg.data = String.Format("{0}\r\n{1}", msg.author, msg.data);
            }
            if (msg.type == "image")
            {
                if (!string.IsNullOrEmpty(msg.preview))
                {
                    MemoryStream ms = new MemoryStream(Convert.FromBase64String(msg.preview));
                    Image i = Image.FromStream(ms);
                    this.Height += i.Height;
                    this.Controls.Remove(this.textBox1);
                    PictureBox pb = new PictureBox();
                    pb.Width = i.Width;
                    pb.Height = i.Height;
                    pb.Image = i;
                    this.Controls.Add(pb);
                }
                
            }
            else
            {
                this.textBox1.Text = msg.data;
                Size size = TextRenderer.MeasureText(this.textBox1.Text, this.textBox1.Font, new Size(this.textBox1.Width, int.MaxValue), TextFormatFlags.WordBreak);
                this.Height = size.Height + 15;
            }
        }
    }
}
