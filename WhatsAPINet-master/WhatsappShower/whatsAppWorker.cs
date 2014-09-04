using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using WhatsAppApi;
using WhatsAppApi.Account;
using WhatsAppApi.Helper;
using WhatsAppApi.Register;
using WhatsAppApi.Response;

namespace WindowsFormsApplication2
{
    class whatsAppWorker
    {

            
            public static string nickname = "test12";
            public static string sender = "972524376363"; // Mobile number with country code (but without + or 00)
            public static string password = "gSTzBnqBiveRSIX5ii8e8d38mxM=";//v2 password
            public static string target = "972504219841";// Mobile number to send the message to

            WhatsApp wa = new WhatsApp(sender, password, nickname, true);
            
    }
}
