using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using System.Globalization;
using System.Text.RegularExpressions;
using System.Windows.Media.Animation;

namespace whatsAppShowerWpf
{
        
    public partial class TextView : UserControl
    {

        public static string[] EmojiCodeSingles = { "203C", "21AA", "23F3", "25FD", "26AB", "2712", "2795", "2B1B", "2049", "231A", "24C2", "25FE", "26C5", "2714", "2796", "2B1C", "2139", "231B", "25AA", "2611", "26D4", "2716", "2797", "2194", "23EB", "25AB", "267B", "2705", "2744", "27B0", "2195", "23EC", "25FB", "2693", "2709", "2747", "2934", "21A9", "23F0", "25FC", "26AA", "270F", "274E", "2935", "3030" };
        public static string[] EmojiCodeAll = { "E415", "E057", "1F600", "E056", "E414", "E405", "E106", "E418", "E417", "1F617", "1F619", "E105", "E409", "1F61B", "E40D", "E404", "E403", "E40A", "E40E", "E058", "E406", "E413", "E412", "E411", "E408", "E401", "E40F", "1F605", "E108", "1F629", "1F62B", "E40B", "E107", "E059", "E416", "1F624", "E407", "1F606", "1F60B", "E40C", "1F60E", "1F634", "1F635", "E410", "1F61F", "1F626", "1F627", "1F608", "E11A", "1F62E", "1F62C", "1F610", "1F615", "1F62F", "1F636", "1F607", "E402", "1F611", "E516", "E517", "E152", "E51B", "E51E", "E51A", "E001", "E002", "E004", "E005", "E518", "E519", "E515", "E04E", "E51C", "1F63A", "1F638", "1F63B", "1F63D", "1F63C", "1F640", "1F63F", "1F639", "1F63E", "1F479", "1F47A", "1F648", "1F649", "1F64A", "E11C", "E10C", "E05A", "E11D", "E32E", "E335", "1F4AB", "1F4A5", "E334", "E331", "1F4A7", "E13C", "E330", "E41B", "E419", "E41A", "1F445", "E41C", "E00E", "E421", "E420", "E00D", "E010", "E011", "E41E", "E012", "E422", "E22E", "E22F", "E231", "E230", "E427", "E41D", "E00F", "E41F", "E14C", "E201", "E115", "E51F", "E428", "1F46A", "1F46C", "1F46D", "E111", "E425", "E429", "E424", "E423", "E253", "1F64B", "E31E", "E31F", "E31D", "1F470", "1F64E", "1F64D", "E426", "E503", "E10E", "E318", "E007", "1F45E", "E31A", "E13E", "E31B", "E006", "E302", "1F45A", "E319", "1F3BD", "1F456", "E321", "E322", "E11E", "E323", "1F45D", "1F45B", "1F453", "E314", "E43C", "E31C", "E32C", "E32A", "E32D", "E32B", "E022", "E023", "E328", "E327", "1F495", "1F496", "1F49E", "E329", "1F48C", "E003", "E034", "E035", "1F464", "1F465", "1F4AC", "E536", "1F4AD", "E052", "E52A", "E04F", "E053", "E524", "E52C", "E531", "E050", "E527", "E051", "E10B", "1F43D", "E52B", "E52F", "E109", "E528", "E01A", "E529", "E526", "1F43C", "E055", "E521", "E523", "1F425", "1F423", "E52E", "E52D", "1F422", "E525", "1F41D", "1F41C", "1F41E", "1F40C", "E10A", "E441", "E522", "E019", "E520", "E054", "1F40B", "1F404", "1F40F", "1F400", "1F403", "1F405", "1F407", "1F409", "E134", "1F410", "1F413", "1F415", "1F416", "1F401", "1F402", "1F432", "1F421", "1F40A", "E530", "1F42A", "1F406", "1F408", "1F429", "1F43E", "E306", "E030", "E304", "E110", "E032", "E305", "E303", "E118", "E447", "E119", "1F33F", "E444", "1F344", "E308", "E307", "1F332", "1F333", "1F330", "1F331", "1F33C", "1F310", "1F31E", "1F31D", "1F31A", "1F311", "1F312", "1F313", "1F314", "1F315", "1F316", "1F317", "1F318", "1F31C", "1F31B", "E04C", "1F30D", "1F30E", "1F30F", "1F30B", "1F30C", "1F320", "E32F", "E04A", "26C5", "E049", "E13D", "E04B", "2744", "E048", "E443", "1F301", "E44C", "E43E", "E436", "E437", "E438", "E43A", "E439", "E43B", "E117", "E440", "E442", "E446", "E445", "E11B", "E448", "E033", "E112", "1F38B", "E312", "1F38A", "E310", "E143", "1F52E", "E03D", "E008", "1F4F9", "E129", "E126", "E127", "E316", "1F4BE", "E00C", "E00A", "E009", "1F4DE", "1F4DF", "E00B", "E14B", "E12A", "E128", "E141", "1F509", "1F508", "1F507", "E325", "1F515", "E142", "E317", "23F3", "231B", "23F0", "231A", "E145", "E144", "1F50F", "1F510", "E03F", "1F50E", "E10F", "1F526", "1F506", "1F505", "1F50C", "1F50B", "E114", "1F6C1", "E13F", "1F6BF", "E140", "1F527", "1F529", "E116", "1F6AA", "E30E", "E311", "E113", "1F52A", "E30F", "E13B", "E12F", "1F4B4", "1F4B5", "1F4B7", "1F4B6", "1F4B3", "1F4B8", "E104", "1F4E7", "1F4E5", "1F4E4", "2709", "E103", "1F4E8", "1F4EF", "E101", "1F4EA", "1F4EC", "1F4ED", "E102", "1F4E6", "E301", "1F4C4", "1F4C3", "1F4D1", "1F4CA", "1F4C8", "1F4C9", "1F4DC", "1F4CB", "1F4C5", "1F4C6", "1F4C7", "1F4C1", "1F4C2", "E313", "1F4CC", "1F4CE", "2712", "270F", "1F4CF", "1F4D0", "1F4D5", "1F4D7", "1F4D8", "1F4D9", "1F4D3", "1F4D4", "1F4D2", "1F4DA", "E148", "1F516", "1F4DB", "1F52C", "1F52D", "1F4F0", "E502", "E324", "E03C", "E30A", "1F3BC", "E03E", "E326", "1F3B9", "1F3BB", "E042", "E040", "E041", "E12B", "1F3AE", "1F0CF", "1F3B4", "E12D", "1F3B2", "E130", "E42B", "E42A", "E018", "E016", "E015", "E42C", "1F3C9", "1F3B3", "E014", "1F6B5", "1F6B4", "E132", "1F3C7", "E131", "E013", "1F3C2", "E42D", "E017", "1F3A3", "E045", "E338", "E30B", "1F37C", "E047", "E30C", "E044", "1F379", "1F377", "E043", "1F355", "E120", "E33B", "1F357", "1F356", "E33F", "E341", "1F364", "E34C", "E344", "1F365", "E342", "E33D", "E33E", "E340", "E34D", "E343", "E33C", "E147", "E339", "1F369", "1F36E", "E33A", "1F368", "E43F", "E34B", "E046", "1F36A", "1F36B", "1F36C", "1F36D", "1F36F", "E345", "1F34F", "E346", "1F34B", "1F352", "1F347", "E348", "E347", "1F351", "1F348", "1F34C", "1F350", "1F34D", "1F360", "E34A", "E349", "1F33D", "E036", "1F3E1", "E157", "E038", "E153", "E155", "E14D", "E156", "E501", "E158", "E43D", "E037", "E504", "1F3E4", "E44A", "E146", "E505", "E506", "E122", "E508", "E509", "1F5FE", "E03B", "E04D", "E449", "E44B", "E51D", "1F309", "1F3A0", "E124", "E121", "E433", "E202", "E01C", "E135", "1F6A3", "2693", "E10D", "E01D", "E11F", "1F681", "1F682", "1F68A", "E039", "1F69E", "1F686", "E435", "E01F", "1F688", "E434", "1F69D", "E01E", "1F68B", "1F68E", "E159", "1F68D", "E42E", "1F698", "E01B", "E15A", "1F696", "1F69B", "E42F", "1F6A8", "E432", "1F694", "E430", "E431", "1F690", "E136", "1F6A1", "1F69F", "1F6A0", "1F69C", "E320", "E150", "E125", "1F6A6", "E14E", "E252", "E137", "E209", "E03A", "1F3EE", "E133", "E123", "1F5FF", "1F3AA", "1F3AD", "1F4CD", "1F6A9", "E50B", "E514", "E50E", "E513", "E50C", "E50D", "E511", "E50F", "E512", "E510", "E50A", "E21C", "E21D", "E21E", "E21F", "E220", "E221", "E222", "E223", "E224", "E225", "1F51F", "1F522", "E210", "1F523", "E232", "E233", "E235", "E234", "1F520", "1F521", "1F524", "E236", "E237", "E238", "E239", "2194", "2195", "1F504", "E23B", "E23A", "1F53C", "1F53D", "21A9", "21AA", "2139", "E23D", "E23C", "23EB", "23EC", "2935", "2934", "E24D", "1F500", "1F501", "1F502", "E212", "E213", "E214", "1F193", "1F196", "E20B", "E507", "E203", "E22C", "E22B", "E22A", "1F234", "1F232", "E226", "E227", "E22D", "E215", "E216", "E151", "E138", "E139", "E13A", "E309", "1F6B0", "1F6AE", "E14F", "E20A", "E208", "E217", "E218", "E228", "24C2", "1F6C2", "1F6C4", "1F6C5", "1F6C3", "1F251", "E315", "E30D", "1F191", "1F198", "E229", "1F6AB", "E207", "1F4F5", "1F6AF", "1F6B1", "1F6B3", "1F6B7", "1F6B8", "26D4", "E206", "2747", "274E", "2705", "E205", "E204", "E12E", "E250", "E251", "E532", "E533", "E534", "E535", "1F4A0", "E211", "267B", "E23F", "E240", "E241", "E242", "E243", "E244", "E245", "E246", "E247", "E248", "E249", "E24A", "E24B", "E23E", "E154", "E14A", "1F4B2", "E149", "E24E", "E24F", "E537", "E12C", "3030", "E24C", "1F51A", "1F519", "1F51B", "1F51C", "E333", "E332", "E021", "E020", "E337", "E336", "1F503", "E02F", "1F567", "E024", "1F55C", "E025", "1F55D", "E026", "1F55E", "E027", "1F55F", "E028", "1F560", "E029", "E02A", "E02B", "E02C", "E02D", "E02E", "1F561", "1F562", "1F563", "1F564", "1F565", "1F566", "2716", "2795", "2796", "2797", "E20E", "E20C", "E20F", "E20D", "1F4AE", "1F4AF", "2714", "2611", "1F518", "1F517", "27B0", "E031", "E21A", "E21B", "2B1B", "2B1C", "25FE", "25FD", "25AA", "25AB", "1F53A", "25FB", "25FC", "26AB", "26AA", "E219", "1F535", "1F53B", "1F536", "1F537", "1F538", "1F539", "2049", "203C" };
        public static string PatternEmojis = @"([\uE001-\uE537])|(\uD83C[\uDC00-\uDFFF]|\uD83D[\uDC00-\uDFFF])";

        public static string ImageError64 = "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";
        public static string ImageUpdload = "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";

        string from;
        string textMsg;
        string hour;

        public string From
        {
            get { return from; }
            set { from = value; }
        }
        
        public string TextMsg
        {
            get { return textMsg; }
            set { textMsg = value; }
        }
      
        public string Hour
        {
            get { return hour; }
            set { hour = value; }
        }


        public TextView()
        {
            InitializeComponent();
        }
        public TextView(string from, string textMsg)
        {
            InitializeComponent();
            string currentHouer = DateTime.Now.ToString("HH:mm");
            new TextView(from, textMsg, currentHouer);
        }
        
        public TextView(string from , string textMsg,string hour)
        {
            
            InitializeComponent();
            parseEmjoi(textMsg,this.fld);
            //this.textMsgField.Text = textMsg;
            this.textMsgField.FontSize = WhatsappProperties.Instance.TextFontSize;
            double screenWidth = System.Windows.SystemParameters.PrimaryScreenWidth;
            double textMaxWidth = (screenWidth * WhatsappProperties.Instance.ImageMaxWidth) / 100;
            if (!string.IsNullOrEmpty(WhatsappProperties.Instance.TextMaxWidthType) && "pix".Equals(WhatsappProperties.Instance.TextMaxWidthType))
            {
                textMaxWidth = WhatsappProperties.Instance.ImageMaxWidth;
            }
            this.textMsgField.MaxWidth = textMaxWidth;

            parseEmjoi(from, this.fromfd);
            //this.fromField.Text = from;
            this.fromField.Width = MeasureString(from).Width;
            this.fromField.Foreground = NumberPropList.Instance.getPhoneColor(from);
            this.fromField.FontSize = WhatsappProperties.Instance.PhoneFontSize;

            this.hourField.Text = hour;
            this.hourField.Foreground = Brushes.Gray;
            this.hourField.FontSize = WhatsappProperties.Instance.HouerFontSize;
            //double sd = this.textMsgField.DesiredSize.Width;
            if (from.Length > textMsg.Length)
            {
                
               // this.Width = MeasureString(from).Width + 30;
            }
            else
            {
               // this.Width = MeasureString(textMsg).Width + 30;

            }
            TextRange textRange = new TextRange(
                // TextPointer to the start of content in the RichTextBox.
                this.textMsgField.Document.ContentStart,
                        // TextPointer to the end of content in the RichTextBox.
                this.textMsgField.Document.ContentEnd
            );

            // The Text property on a TextRange object returns a string 
            // representing the plain text content of the TextRange. 
            string s= textRange.Text;
            this.HorizontalAlignment = HorizontalAlignment.Left;

            this.Margin = new Thickness(WhatsappProperties.Instance.PaddingLeft, WhatsappProperties.Instance.PaddingTop, 0, 0);
            this.Width = Double.NaN;
        }

        

        private Size MeasureString(string candidate)
        {
            var formattedText = new FormattedText(
                candidate,
                CultureInfo.CurrentUICulture,
                FlowDirection.LeftToRight,
                new Typeface(this.textMsgField.FontFamily, this.textMsgField.FontStyle, this.textMsgField.FontWeight, this.textMsgField.FontStretch),
                this.textMsgField.FontSize,
                Brushes.Black);

            return new Size(formattedText.Width, formattedText.Height);
        }



        private void parseEmjoi(string textMsg, FlowDocument flowDocument)
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

       


        
    }
}
