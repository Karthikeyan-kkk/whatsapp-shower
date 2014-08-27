using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;

namespace WinAppNET.AppCode
{
    class Helper
    {
        public class ChatWindowParameters
        {
            public string jid;
            public bool stealFocus;
            public bool onTop;
            public ChatWindowParameters(string jid, bool stealFocus, bool onTop)
            {
                this.jid = jid;
                this.stealFocus = stealFocus;
                this.onTop = onTop;
            }
        }

        public static void CreateFolderTree()
        {
            string targetdir = Directory.GetCurrentDirectory() + "\\data";
            if (!Directory.Exists(targetdir))
            {
                Directory.CreateDirectory(targetdir);
            }
            string foo = targetdir + "\\profilecache";
            if (!Directory.Exists(foo))
            {
                Directory.CreateDirectory(foo);
            }
            foo = targetdir + "\\sqlite";
            if (!Directory.Exists(foo))
            {
                Directory.CreateDirectory(foo);
            }
            foo = targetdir + "\\media";
            if (!Directory.Exists(foo))
            {
                Directory.CreateDirectory(foo);
            }
        }
    }
}
