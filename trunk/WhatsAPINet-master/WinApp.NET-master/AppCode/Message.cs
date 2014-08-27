using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using WhatsAppApi.Helper;

namespace WinAppNET.AppCode
{
    public class WappMessage
    {
        public int id;
        public String data;
        public bool from_me;
        public string jid;
        public string author = null;
        public DateTime timestamp;
        public string type;
        public string preview;

        public WappMessage(string message, string jid)
        {
            this.data = message;
            this.timestamp = DateTime.UtcNow;
            this.from_me = true;
            this.jid = jid;
        }

        public WappMessage(int id, string data, bool from_me, string jid, DateTime timestamp, string type, string preview)
        {
            this.id = id;
            this.data = data;
            this.from_me = from_me;
            this.jid = jid;
            this.timestamp = timestamp;
            this.type = type;
            this.preview = preview;
        }

        public WappMessage(string from, string message, bool fromMe)
        {
            this.from_me = fromMe;
            this.timestamp = DateTime.UtcNow;
            this.jid = from;
            this.data = message;
        }

        public override String ToString()
        {
            if (this.from_me)
                return "Me: " + this.data;
            else
                return "Him: " + this.data;
        }
    }
}
