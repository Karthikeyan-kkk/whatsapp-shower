using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using WinAppNET.AppCode;
using System.Threading;
using WinAppNET.Controls;

namespace WinAppNET
{
    public partial class ContactsSelector : Form
    {
        protected Contact[] _initialContacts;
        protected BindingList<Contact> _matchingContacts = new BindingList<Contact>();
        public string SelectedJID = String.Empty;

        public ContactsSelector()
        {
            InitializeComponent();
            this._initialContacts = ContactStore.GetAllContacts();
            foreach(Contact c in this._initialContacts)
            {
                this._matchingContacts.Add(c);
            }
            this.redraw();
        }

        private void redraw()
        {
            this.flowLayoutPanel1.Controls.Clear();
            foreach (Contact contact in this._matchingContacts)
            {
                ListContact lc = new ListContact(contact.jid);
                lc.DoubleClick += this.listContact_DoubleClick;
                this.flowLayoutPanel1.Controls.Add(lc);
            }
        }

        private void textBox1_TextChanged(object sender, EventArgs e)
        {
            foreach (ListContact lc in this.flowLayoutPanel1.Controls)
            {
                if(lc.contact.ToString().ToLower().Contains(this.textBox1.Text.ToLower()))
                {
                    lc.Show();
                }
                else
                {
                    lc.Hide();
                }
            }
        }

        private void listContact_DoubleClick(object sender, EventArgs e)
        {
            ListContact lc = sender as ListContact;
            if (lc != null)
            {
                this.SelectedJID = lc.contact.jid;
                this.DialogResult = DialogResult.OK;
                this.Close();
            }
        }
    }
}
