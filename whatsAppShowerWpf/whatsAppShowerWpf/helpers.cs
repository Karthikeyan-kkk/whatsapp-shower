using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using WhatsAppApi.Helper;

namespace whatsAppShowerWpf
{
    class Helpers
    {

        public static string filterFromNumber(String from)
        {
            char[] splitChar = { '@' };
            return from.Split(splitChar)[0];
        }
        public static string getNikeName(ProtocolTreeNode node)
        {
            string nickName = "";
            try
            {
                List<KeyValue> attributeHashList = node.attributeHash.ToList();
                for (int i = 0; i < attributeHashList.Count; i++)
                {
                    if (attributeHashList[i] != null && attributeHashList[i].Key.Equals("notify"))
                    {
                        return attributeHashList[i].Value;
                    }
                }
            }
            catch (Exception) { }
            return nickName;
        }
    }
}
