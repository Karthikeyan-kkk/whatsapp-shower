﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using WhatsAppApi.Helper;
using System.IO;
using log4net;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media.Animation;
using System.Windows.Documents;
using System.Text.RegularExpressions;
using System.Windows.Media.Imaging;
using System.Globalization;
using System.Windows.Media;

namespace whatsAppShowerWpf
{
    class Helpers
    {
        private static readonly ILog systemLog = log4net.LogManager.GetLogger("systemsLog");
        public static string[] EmojiCodeSingles = { "203C", "21AA", "23F3", "25FD", "26AB", "2712", "2795", "2B1B", "2049", "231A", "24C2", "25FE", "26C5", "2714", "2796", "2B1C", "2139", "231B", "25AA", "2611", "26D4", "2716", "2797", "2194", "23EB", "25AB", "267B", "2705", "2744", "27B0", "2195", "23EC", "25FB", "2693", "2709", "2747", "2934", "21A9", "23F0", "25FC", "26AA", "270F", "274E", "2935", "3030" };
        public static string[] EmojiCodeAll = { "E415", "E057", "1F600", "E056", "E414", "E405", "E106", "E418", "E417", "1F617", "1F619", "E105", "E409", "1F61B", "E40D", "E404", "E403", "E40A", "E40E", "E058", "E406", "E413", "E412", "E411", "E408", "E401", "E40F", "1F605", "E108", "1F629", "1F62B", "E40B", "E107", "E059", "E416", "1F624", "E407", "1F606", "1F60B", "E40C", "1F60E", "1F634", "1F635", "E410", "1F61F", "1F626", "1F627", "1F608", "E11A", "1F62E", "1F62C", "1F610", "1F615", "1F62F", "1F636", "1F607", "E402", "1F611", "E516", "E517", "E152", "E51B", "E51E", "E51A", "E001", "E002", "E004", "E005", "E518", "E519", "E515", "E04E", "E51C", "1F63A", "1F638", "1F63B", "1F63D", "1F63C", "1F640", "1F63F", "1F639", "1F63E", "1F479", "1F47A", "1F648", "1F649", "1F64A", "E11C", "E10C", "E05A", "E11D", "E32E", "E335", "1F4AB", "1F4A5", "E334", "E331", "1F4A7", "E13C", "E330", "E41B", "E419", "E41A", "1F445", "E41C", "E00E", "E421", "E420", "E00D", "E010", "E011", "E41E", "E012", "E422", "E22E", "E22F", "E231", "E230", "E427", "E41D", "E00F", "E41F", "E14C", "E201", "E115", "E51F", "E428", "1F46A", "1F46C", "1F46D", "E111", "E425", "E429", "E424", "E423", "E253", "1F64B", "E31E", "E31F", "E31D", "1F470", "1F64E", "1F64D", "E426", "E503", "E10E", "E318", "E007", "1F45E", "E31A", "E13E", "E31B", "E006", "E302", "1F45A", "E319", "1F3BD", "1F456", "E321", "E322", "E11E", "E323", "1F45D", "1F45B", "1F453", "E314", "E43C", "E31C", "E32C", "E32A", "E32D", "E32B", "E022", "E023", "E328", "E327", "1F495", "1F496", "1F49E", "E329", "1F48C", "E003", "E034", "E035", "1F464", "1F465", "1F4AC", "E536", "1F4AD", "E052", "E52A", "E04F", "E053", "E524", "E52C", "E531", "E050", "E527", "E051", "E10B", "1F43D", "E52B", "E52F", "E109", "E528", "E01A", "E529", "E526", "1F43C", "E055", "E521", "E523", "1F425", "1F423", "E52E", "E52D", "1F422", "E525", "1F41D", "1F41C", "1F41E", "1F40C", "E10A", "E441", "E522", "E019", "E520", "E054", "1F40B", "1F404", "1F40F", "1F400", "1F403", "1F405", "1F407", "1F409", "E134", "1F410", "1F413", "1F415", "1F416", "1F401", "1F402", "1F432", "1F421", "1F40A", "E530", "1F42A", "1F406", "1F408", "1F429", "1F43E", "E306", "E030", "E304", "E110", "E032", "E305", "E303", "E118", "E447", "E119", "1F33F", "E444", "1F344", "E308", "E307", "1F332", "1F333", "1F330", "1F331", "1F33C", "1F310", "1F31E", "1F31D", "1F31A", "1F311", "1F312", "1F313", "1F314", "1F315", "1F316", "1F317", "1F318", "1F31C", "1F31B", "E04C", "1F30D", "1F30E", "1F30F", "1F30B", "1F30C", "1F320", "E32F", "E04A", "26C5", "E049", "E13D", "E04B", "2744", "E048", "E443", "1F301", "E44C", "E43E", "E436", "E437", "E438", "E43A", "E439", "E43B", "E117", "E440", "E442", "E446", "E445", "E11B", "E448", "E033", "E112", "1F38B", "E312", "1F38A", "E310", "E143", "1F52E", "E03D", "E008", "1F4F9", "E129", "E126", "E127", "E316", "1F4BE", "E00C", "E00A", "E009", "1F4DE", "1F4DF", "E00B", "E14B", "E12A", "E128", "E141", "1F509", "1F508", "1F507", "E325", "1F515", "E142", "E317", "23F3", "231B", "23F0", "231A", "E145", "E144", "1F50F", "1F510", "E03F", "1F50E", "E10F", "1F526", "1F506", "1F505", "1F50C", "1F50B", "E114", "1F6C1", "E13F", "1F6BF", "E140", "1F527", "1F529", "E116", "1F6AA", "E30E", "E311", "E113", "1F52A", "E30F", "E13B", "E12F", "1F4B4", "1F4B5", "1F4B7", "1F4B6", "1F4B3", "1F4B8", "E104", "1F4E7", "1F4E5", "1F4E4", "2709", "E103", "1F4E8", "1F4EF", "E101", "1F4EA", "1F4EC", "1F4ED", "E102", "1F4E6", "E301", "1F4C4", "1F4C3", "1F4D1", "1F4CA", "1F4C8", "1F4C9", "1F4DC", "1F4CB", "1F4C5", "1F4C6", "1F4C7", "1F4C1", "1F4C2", "E313", "1F4CC", "1F4CE", "2712", "270F", "1F4CF", "1F4D0", "1F4D5", "1F4D7", "1F4D8", "1F4D9", "1F4D3", "1F4D4", "1F4D2", "1F4DA", "E148", "1F516", "1F4DB", "1F52C", "1F52D", "1F4F0", "E502", "E324", "E03C", "E30A", "1F3BC", "E03E", "E326", "1F3B9", "1F3BB", "E042", "E040", "E041", "E12B", "1F3AE", "1F0CF", "1F3B4", "E12D", "1F3B2", "E130", "E42B", "E42A", "E018", "E016", "E015", "E42C", "1F3C9", "1F3B3", "E014", "1F6B5", "1F6B4", "E132", "1F3C7", "E131", "E013", "1F3C2", "E42D", "E017", "1F3A3", "E045", "E338", "E30B", "1F37C", "E047", "E30C", "E044", "1F379", "1F377", "E043", "1F355", "E120", "E33B", "1F357", "1F356", "E33F", "E341", "1F364", "E34C", "E344", "1F365", "E342", "E33D", "E33E", "E340", "E34D", "E343", "E33C", "E147", "E339", "1F369", "1F36E", "E33A", "1F368", "E43F", "E34B", "E046", "1F36A", "1F36B", "1F36C", "1F36D", "1F36F", "E345", "1F34F", "E346", "1F34B", "1F352", "1F347", "E348", "E347", "1F351", "1F348", "1F34C", "1F350", "1F34D", "1F360", "E34A", "E349", "1F33D", "E036", "1F3E1", "E157", "E038", "E153", "E155", "E14D", "E156", "E501", "E158", "E43D", "E037", "E504", "1F3E4", "E44A", "E146", "E505", "E506", "E122", "E508", "E509", "1F5FE", "E03B", "E04D", "E449", "E44B", "E51D", "1F309", "1F3A0", "E124", "E121", "E433", "E202", "E01C", "E135", "1F6A3", "2693", "E10D", "E01D", "E11F", "1F681", "1F682", "1F68A", "E039", "1F69E", "1F686", "E435", "E01F", "1F688", "E434", "1F69D", "E01E", "1F68B", "1F68E", "E159", "1F68D", "E42E", "1F698", "E01B", "E15A", "1F696", "1F69B", "E42F", "1F6A8", "E432", "1F694", "E430", "E431", "1F690", "E136", "1F6A1", "1F69F", "1F6A0", "1F69C", "E320", "E150", "E125", "1F6A6", "E14E", "E252", "E137", "E209", "E03A", "1F3EE", "E133", "E123", "1F5FF", "1F3AA", "1F3AD", "1F4CD", "1F6A9", "E50B", "E514", "E50E", "E513", "E50C", "E50D", "E511", "E50F", "E512", "E510", "E50A", "E21C", "E21D", "E21E", "E21F", "E220", "E221", "E222", "E223", "E224", "E225", "1F51F", "1F522", "E210", "1F523", "E232", "E233", "E235", "E234", "1F520", "1F521", "1F524", "E236", "E237", "E238", "E239", "2194", "2195", "1F504", "E23B", "E23A", "1F53C", "1F53D", "21A9", "21AA", "2139", "E23D", "E23C", "23EB", "23EC", "2935", "2934", "E24D", "1F500", "1F501", "1F502", "E212", "E213", "E214", "1F193", "1F196", "E20B", "E507", "E203", "E22C", "E22B", "E22A", "1F234", "1F232", "E226", "E227", "E22D", "E215", "E216", "E151", "E138", "E139", "E13A", "E309", "1F6B0", "1F6AE", "E14F", "E20A", "E208", "E217", "E218", "E228", "24C2", "1F6C2", "1F6C4", "1F6C5", "1F6C3", "1F251", "E315", "E30D", "1F191", "1F198", "E229", "1F6AB", "E207", "1F4F5", "1F6AF", "1F6B1", "1F6B3", "1F6B7", "1F6B8", "26D4", "E206", "2747", "274E", "2705", "E205", "E204", "E12E", "E250", "E251", "E532", "E533", "E534", "E535", "1F4A0", "E211", "267B", "E23F", "E240", "E241", "E242", "E243", "E244", "E245", "E246", "E247", "E248", "E249", "E24A", "E24B", "E23E", "E154", "E14A", "1F4B2", "E149", "E24E", "E24F", "E537", "E12C", "3030", "E24C", "1F51A", "1F519", "1F51B", "1F51C", "E333", "E332", "E021", "E020", "E337", "E336", "1F503", "E02F", "1F567", "E024", "1F55C", "E025", "1F55D", "E026", "1F55E", "E027", "1F55F", "E028", "1F560", "E029", "E02A", "E02B", "E02C", "E02D", "E02E", "1F561", "1F562", "1F563", "1F564", "1F565", "1F566", "2716", "2795", "2796", "2797", "E20E", "E20C", "E20F", "E20D", "1F4AE", "1F4AF", "2714", "2611", "1F518", "1F517", "27B0", "E031", "E21A", "E21B", "2B1B", "2B1C", "25FE", "25FD", "25AA", "25AB", "1F53A", "25FB", "25FC", "26AB", "26AA", "E219", "1F535", "1F53B", "1F536", "1F537", "1F538", "1F539", "2049", "203C" };
        public static string PatternEmojis = @"([\uE001-\uE537])|(\uD83C[\uDC00-\uDFFF]|\uD83D[\uDC00-\uDFFF])";
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

        public static string formatPhoneNumber(String phoneNumber)
        {
            if (!string.IsNullOrEmpty(phoneNumber) && phoneNumber.StartsWith("972"))
            {
                phoneNumber = phoneNumber.Replace("972", "0");
                if (phoneNumber.Length == 10)
                {
                    phoneNumber = phoneNumber.Insert(3, "-");
                }

            }
            return phoneNumber;
        }


        public static string getImgFullPath(string file)
        {
            try
            {
                if (string.IsNullOrEmpty(WhatsappProperties.Instance.DownloadImgTo))
                {
                    return file;
                }
                if (!Directory.Exists(WhatsappProperties.Instance.DownloadImgTo))
                {
                    Directory.CreateDirectory(WhatsappProperties.Instance.DownloadImgTo);
                }
            }
            catch (Exception)
            {
                systemLog.Error("faild create DownloadImgTo Directory: " + WhatsappProperties.Instance.DownloadImgTo);
                return file;
            }
            return WhatsappProperties.Instance.DownloadImgTo + @"\" + file;
        }

        

        
        public void buildRunningText(MainWindow mainWindow)
        {
            mainWindow.txtKron.Text = WhatsappProperties.Instance.RunnigText;
            mainWindow.txtKron.Foreground = WhatsappProperties.Instance.RunnigTextColor;
            mainWindow.txtKron.FontSize = WhatsappProperties.Instance.RunnigTextSize;
            mainWindow.txtKron2.Foreground = WhatsappProperties.Instance.RunnigTextColor;
            mainWindow.txtKron2.FontSize = WhatsappProperties.Instance.RunnigTextSize;
            if (mainWindow.stack.Resources["slide"] != null)
            {
                Storyboard storyboard = (Storyboard)mainWindow.stack.Resources["slide"];
                storyboard.SpeedRatio = WhatsappProperties.Instance.RuningTextSpeed;
                mainWindow.stack.BeginStoryboard(storyboard);
            }
            var formattedText = new FormattedText(
                    mainWindow.txtKron.Text,
                    CultureInfo.CurrentUICulture,
                    FlowDirection.LeftToRight,
                    new Typeface(mainWindow.txtKron.FontFamily, mainWindow.txtKron.FontStyle, mainWindow.txtKron.FontWeight, mainWindow.txtKron.FontStretch),
                    mainWindow.txtKron.FontSize,
                    Brushes.Black);
            mainWindow.firstRow.Height = new GridLength(formattedText.Height);
        }
        public static void parseEmjoi(string textMsg, FlowDocument flowDocument)
        {
            Paragraph para = new Paragraph();

            var mt = Regex.Matches(textMsg, GetEmojiCompletePattern());
            if (mt.Count > 0)
            {
                int pos = 0;

                for (int j = 0; j < mt.Count; j++)
                {
                    if (mt[j].Index > 0)
                    {
                        para.Inlines.Add((textMsg.Substring(pos, mt[j].Index - pos)));
                    }

                    string hex = "";
                    char[] bal = mt[j].Value.ToCharArray();

                    if (bal.Length == 1)
                    {

                        hex = string.Format("{0:X}", Convert.ToUInt32(mt[j].Value[0]));
                    }
                    else if (bal.Length == 2)
                    {
                        uint hi = Convert.ToUInt32(mt[j].Value[0]);
                        uint lo = Convert.ToUInt32(mt[j].Value[1]);
                        if (0xD800 <= hi && hi <= 0xDBFF)
                        {
                            var p = ((hi - 0xD800) * 0x400) + (lo - 0xDC00) + 0x10000;
                            hex = string.Format("{0:X}", Convert.ToUInt32(p));
                        }
                    }

                    pos = mt[j].Index + bal.Length;

                    BitmapImage bitmap = new BitmapImage(new Uri("emoji2/" + hex + ".png", UriKind.RelativeOrAbsolute));
                    Image image = new Image();
                    image.Source = bitmap;
                    image.Height = WhatsappProperties.Instance.TextFontSize;
                    if ("fromfd".Equals(flowDocument.Name))
                    {
                        image.Height = WhatsappProperties.Instance.PhoneFontSize;
                    }
                    para.Inlines.Add(image);
                }

                if (pos < textMsg.Length)
                {
                    para.Inlines.Add(textMsg.Substring(pos));
                }

            }
            else
            {
                para.Inlines.Add(textMsg);
            }
            flowDocument.Blocks.Add(para);
        }

        public static string GetEmojiCompletePattern()
        {
            string PatternEmojisComplete = PatternEmojis;

            if (EmojiCodeSingles != null && EmojiCodeSingles.Length > 0)
            {
                PatternEmojisComplete += @"|(";
                for (int x = 0; x < EmojiCodeSingles.Length; x++)
                {
                    PatternEmojisComplete += @"[\u" + EmojiCodeSingles[x] + "]";
                    if (x < EmojiCodeSingles.Length - 1)
                    {
                        PatternEmojisComplete += @"|";
                    }
                }
                PatternEmojisComplete += @")";
            }
            return PatternEmojisComplete;


        }
        public static Size MeasureString(string candidate, FontFamily fontFamily, FontStyle fontStyle, FontWeight fontWeight, FontStretch fontStretch,double fontSize)
        { 
            var formattedText = new FormattedText(
                candidate,
                CultureInfo.CurrentUICulture,
                FlowDirection.LeftToRight,
                new Typeface(fontFamily, fontStyle,fontWeight, fontStretch),fontSize,Brushes.Black);

            return new Size(formattedText.Width, formattedText.Height);
        }

    }
}
