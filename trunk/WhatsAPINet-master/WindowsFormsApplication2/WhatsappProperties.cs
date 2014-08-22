using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Configuration;

namespace WindowsFormsApplication2
{
    public class WhatsappProperties
    {
        public static string getPhonenumber()
        {
            return ConfigurationManager.AppSettings["phonenumber"];
        }
        public static string getPassword()
        {
            return ConfigurationManager.AppSettings["password"];
        }
        public static string getNickName()
        {
            return ConfigurationManager.AppSettings["nickName"];
        }
        public static void savePhonenumber(String phoneNumber)
        {
           saveConfig("phonenumber", phoneNumber);
        }
        public static void savePassword(string password)
        {
            saveConfig("password", password);
        }
        public static void saveNickName(string nickName)
        {
            saveConfig("nickName", nickName);
        }
        private static void saveConfig(string key,string val){
            Configuration config = ConfigurationManager.OpenExeConfiguration(ConfigurationUserLevel.None);
            config.AppSettings.Settings[key].Value = val;
            config.Save(ConfigurationSaveMode.Modified);
            ConfigurationManager.RefreshSection("appSettings");
        }
    }
}
