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
using System.Windows.Media.Effects;
using System.Windows.Media.Animation;
using System.IO;
using System.Net;
using System.Threading;
using WhatsAppApi;
using WhatsAppApi.Helper;
using WhatsAppApi.Account;
using System.Windows.Threading;
using log4net;
using System.Globalization;


namespace whatsAppShowerWpf
{
	/// <summary>
	/// Interaction logic for MainWindow.xaml
	/// </summary>
	public partial class MainWindow : Window
	{
		string nickname = "";
		bool showExample = false;
		string sender = "972524376363"; // Mobile number with country code (but without + or 00)
		string password = "TicJAMworhafW+84w3vuA4yMS5o=";//v2 password
		string target = "972504219841";// Mobile number to send the message to
		private static readonly ILog msgsLog = log4net.LogManager.GetLogger("msgsLog");
		private static readonly ILog systemLog = log4net.LogManager.GetLogger("systemsLog");
		private static readonly ILog msgHistoryLog = log4net.LogManager.GetLogger("msgHistory");
		private DispatcherTimer sideImageTimer = new DispatcherTimer();
		private Duration runingTextSpeedDuration = new Duration(TimeSpan.FromSeconds(20));
	   

		public Duration RuningTextSpeedDuration
		{
			get { return new Duration(TimeSpan.FromSeconds(WhatsappProperties.Instance.RuningTextSpeed)); }
			set { runingTextSpeedDuration = value; }
		}
		Queue<string> queue = new Queue<string>();

		public string Nickname
		{
			get { return nickname; }
			set { nickname = value;
		   
			Storyboard storyboard = new Storyboard();
			storyboard.SetSpeedRatio(1);
			canvas.BeginStoryboard(storyboard); }
		}
		public string Sender
		{
			get { return sender; }
			set { sender = value; }
		}
		public string Password
		{
			get { return password; }
			set { password = value; }
		}
		public string Target
		{
			get { return target; }
			set { target = value; }
		}
		public bool ShowExample
		{
			get { return showExample; }
			set { showExample = value; }
		}
	   

		public MainWindow()
		{
		   
		  
		  Login login = new Login();
		  login.ShowDialog();
		  
		  InitializeComponent();
		  bool showDemo = (login.showDemo.IsChecked == true);
		  if (login.DialogResult.HasValue && login.DialogResult.Value && !showDemo)
		  {
			  googleSpreadSheetsHandler googleSpreadSheetsHandler = new googleSpreadSheetsHandler();
			  string tokenFromGSheet = googleSpreadSheetsHandler.isCanLoginIn();
			  
			  if (string.IsNullOrEmpty(tokenFromGSheet))
			  {
				  string phoneToken = WhatsappProperties.Instance.PhoneToken;
				  if (string.IsNullOrEmpty(phoneToken))
				  {
					  systemLog.Error("error token");
					  Application.Current.Shutdown();
				  }
				  else
				  {
					  tokenFromGSheet = phoneToken;
				  }
			  }
			  string phoneNumber = login.txtUserName.Text;
			  this.Sender = phoneNumber;
			  this.Password = tokenFromGSheet;
			  initWhatsAppConnect();
			}
			else
			{
				if (login.DialogResult.HasValue && login.DialogResult.Value == false)
				{
					Application.Current.Shutdown();
				}
				else
				{
					addTexts();
					addImages();
					addImageSide();
				}
			}

			
			   


		  if (WhatsappProperties.Instance.ShowSideImages)
		  {
			  sideImageTimer.Tick += new EventHandler(startSideMethod);
			  sideImageTimer.Interval = TimeSpan.FromSeconds(WhatsappProperties.Instance.SideImagefadingSpeedInSec + WhatsappProperties.Instance.SideImagefadingSpeedInSec);
			  sideImageTimer.Start();
		  }
			initVisualProp();
			this.WindowState =  WindowState.Maximized;
			startRunningText();
		}

		private void initVisualProp()
		{
			if (!string.IsNullOrEmpty(WhatsappProperties.Instance.Backgroundimage))
			{
				try
				{
					BitmapImage bi =new BitmapImage();
					bi.BeginInit();
					Uri uri = new Uri(WhatsappProperties.Instance.Backgroundimage.Trim(), UriKind.RelativeOrAbsolute);
					if (File.Exists(uri.ToString()))
					{
						bi.UriSource = uri;
						bi.EndInit();
						this.mainGrid.Background.SetValue(ImageBrush.ImageSourceProperty, bi);
					}
					
				}
				catch (Exception e)
				{
					systemLog.Error("no background img " + WhatsappProperties.Instance.Backgroundimage + " " + e);
					
				}
				
			}
			if (WhatsappProperties.Instance.FullScreen)
			{
				this.WindowStyle = WindowStyle.None;
			}
			else
			{
				this.WindowStyle = WindowStyle.ThreeDBorderWindow;
			}
			startRunningText();
		}

		
		private void startSideMethod(object sender, EventArgs e2)
		{
			//(sender as DispatcherTimer).Stop();
			startSideImageShow();
		}

		private void startSideImageShow()
		{
			
			Image image = this.sideImage;
			TimeSpan fadeOutTime = TimeSpan.FromSeconds(WhatsappProperties.Instance.SideImagefadingSpeedInSec);
			TimeSpan fadeInTime = TimeSpan.FromSeconds(WhatsappProperties.Instance.SideImagefadingSpeedInSec);


			var fadeInAnimation = new DoubleAnimation(1d, fadeInTime);
			var fadeOutAnimation = new DoubleAnimation(0d, fadeOutTime);
			if (queue.Count == 0)
			{
				
				image.BeginAnimation(Image.OpacityProperty, fadeOutAnimation);
				sideImageBorder.BeginAnimation(Image.OpacityProperty, fadeOutAnimation);
				return;
			}
			string imgUrl = queue.Dequeue();
			BitmapImage logo = new BitmapImage();
			logo.BeginInit();
			logo.UriSource = new Uri(imgUrl);
			logo.EndInit();
			ImageSource source = logo;

			if (image.Source != null)
			{


				fadeOutAnimation.Completed += (o, e) =>
				{
					image.Source = source;
					image.BeginAnimation(Image.OpacityProperty, fadeInAnimation);
					sideImageBorder.BeginAnimation(Image.OpacityProperty, fadeInAnimation);
				};
				image.BeginAnimation(Image.OpacityProperty, fadeOutAnimation);
				sideImageBorder.BeginAnimation(Image.OpacityProperty, fadeOutAnimation);
			}
			else
			{
				image.Opacity = 0d;
				image.Source = source;
				image.BeginAnimation(Image.OpacityProperty, fadeInAnimation);
				sideImageBorder.BeginAnimation(Image.OpacityProperty, fadeInAnimation);
			   
			}
		}

		
		private void addText(string phoneNumber,string nikeName ,string msgText, string hour)
		{
            bool isCanShowMsg = isCanShowMsgMet(phoneNumber, "TEXT");
			phoneNumber = Helpers.filterFromNumber(phoneNumber);
			 if (isCanShowMsg)
			 {
				 TextView textView2 = null;
				 phoneNumber = Helpers.formatPhoneNumber(phoneNumber);
				 this.stackPanel1.Dispatcher.BeginInvoke(new Action(() => {
					 if (string.IsNullOrEmpty(nikeName))
					 {
						 textView2 = new TextView(phoneNumber, msgText, hour);
					 }
					 else
					 {
						 textView2 = new TextView(phoneNumber + " - " + nikeName, msgText, hour);
					 }
					
					 this.stackPanel1.Children.Add(textView2);
					 this.stackPanelScroller.ScrollToBottom();
				 }));
				 
			 }
			 addTextInfoToLog("Text", msgText, phoneNumber, isCanShowMsgMet(phoneNumber, "TEXT"));
		 }
		
		private void addImg(string phoneNumber, string imgFileName, string hour)
		{
			phoneNumber = Helpers.filterFromNumber(phoneNumber);
		   
			if (isCanShowMsgMet(phoneNumber, "IMG"))
			{
				phoneNumber = Helpers.formatPhoneNumber(phoneNumber);
				ImgView imgView = null;
				this.stackPanel1.Dispatcher.BeginInvoke(new Action(() => { imgView = new ImgView(phoneNumber, (ImageSource)new ImageSourceConverter().ConvertFromString(imgFileName), hour); }));
				this.stackPanel1.Dispatcher.Invoke((Action)(() => { this.stackPanel1.Children.Add(imgView); }));
				this.stackPanel1.Dispatcher.Invoke((Action)(() => { this.stackPanelScroller.ScrollToBottom(); }));
				queue.Enqueue(Environment.CurrentDirectory+@"\"+imgFileName);
			}
			addTextInfoToLog("IMG", imgFileName, phoneNumber, isCanShowMsgMet(phoneNumber, "IMG"));
		}

		bool isCanShowMsgMet(string from, string type)
		{
			return NumberPropList.Instance.isCanShowMsg(from, type);
		}
		private void addTextInfoToLog(string type, string text, string phoneNumber, bool isShowen)
		{
			msgsLog.Info(isShowen + " " + type + " From: " + phoneNumber + ": " + text);
		}
		private void startRunningText()
		{
			
			txtKron.Text = WhatsappProperties.Instance.RunnigText;
			txtKron.Foreground = WhatsappProperties.Instance.RunnigTextColor;
			txtKron.FontSize = WhatsappProperties.Instance.RunnigTextSize;
			txtKron2.Foreground = WhatsappProperties.Instance.RunnigTextColor;
			txtKron2.FontSize = WhatsappProperties.Instance.RunnigTextSize;

			var formattedText = new FormattedText(
				txtKron.Text,
				CultureInfo.CurrentUICulture,
				FlowDirection.LeftToRight,
				new Typeface(this.txtKron.FontFamily, this.txtKron.FontStyle, this.txtKron.FontWeight, this.txtKron.FontStretch),
				this.txtKron.FontSize,
				Brushes.Black);
			this.firstRow.Height = new GridLength(formattedText.Height);


			
		}
	   
		private void stackPanel1MouseDown(object sender, MouseButtonEventArgs e)
		{
			
			if (e.ClickCount == 2)
			{
				WhatsappProperties.Instance.initProperties();
				initVisualProp();
				
			}
		}


		//Start tests items

		private void addImageSide()
		{
			queue.Enqueue(@"C:\whatsappPicTest\5.jpg");
			queue.Enqueue(@"C:\whatsappPicTest\1.jpg");
			queue.Enqueue(@"C:\whatsappPicTest\2.jpg");
			queue.Enqueue(@"C:\whatsappPicTest\3.jpg");
			queue.Enqueue(@"C:\whatsappPicTest\4.jpg");

		}

		private void addImages()
		{

			ImgView imgView = new ImgView("0524376464", (ImageSource)new ImageSourceConverter().ConvertFromString(@"C:\whatsappPicTest\1.jpg"), "10:10");
			this.stackPanel1.Children.Add(imgView);
			ImgView imgView2 = new ImgView("0524376464", (ImageSource)new ImageSourceConverter().ConvertFromString(@"C:\whatsappPicTest\2.jpg"), "10:10");
			this.stackPanel1.Children.Add(imgView2);
			ImgView imgView3 = new ImgView("0524376464", (ImageSource)new ImageSourceConverter().ConvertFromString(@"C:\whatsappPicTest\3.jpg"), "10:10");
			this.stackPanel1.Children.Add(imgView3);
			ImgView imgView4 = new ImgView("0524376464", (ImageSource)new ImageSourceConverter().ConvertFromString(@"C:\whatsappPicTest\4.jpg"), "10:10");
			this.stackPanel1.Children.Add(imgView4);
			this.stackPanelScroller.ScrollToBottom();
		}

		private void addTexts()
		{
			TextView textView2 = new TextView("0524376464", "", "10:10");
			this.stackPanel1.Children.Add(textView2);
			for (int i = 0; i < 5; i++)
			{
				this.stackPanel1.Children.Add(new TextView("0524376464" + i, "מזל טוב ומבורך וובדיקה לטקסט ארוך מאוד מאוד מאוד לבדיקה שהוא טקסט ארוך מאוד מאוד מאוד מאוד נראה איך הוא יהיה", "10:10"));
				this.stackPanelScroller.ScrollToBottom();
			}
			this.stackPanelScroller.ScrollToBottom();
		}

		//End test items




		//Start whatsApp events

		void wa_OnGetMessage(ProtocolTreeNode node, string from, string id, string name, string message, bool receipt_sent)
		{
			if (isCommandOpMsg(from, message))
			{
				handleCommandOpMag(from, message);
				return;
			}
			addText(from, Helpers.getNikeName(node), message, DateTime.Now.ToString("HH:mm"));
		}

		void wa_OnGetMessageImage(string from, string id, string fileName, int size, string url, byte[] preview)
		{
			OnGetMedia(Helpers.getImgFullPath(fileName), url, preview);

			addImg(from, Helpers.getImgFullPath(fileName), DateTime.Now.ToString("HH:mm"));
		}

		static void OnGetMedia(string file, string url, byte[] data)
		{
			//save preview
			//File.WriteAllBytes(string.Format("preview_{0}.jpg", file), data);
			//download
			using (WebClient wc = new WebClient())
			{
				string fullPath = file;
				if (!File.Exists(fullPath))
				{
					wc.DownloadFile(new Uri(url), fullPath);
				}
			}
		}

		//End whatsApp events







		// Start whatsApp Connection
		void Instance_OnConnectFailed(Exception ex)
		{
			MessageBox.Show(this, "Login failed", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
		}
		static void Instance_OnPrintDebug(object value)
		{
			Console.WriteLine(value);
		}
		


		private static void ProcessChat(WhatsApp wa, string dst)
		{
			var thRecv = new Thread(t =>
			{
				try
				{
					while (wa != null)
					{
						try
						{
							wa.PollMessages();
							Thread.Sleep(100);
							continue;
						}
						catch (Exception)
						{


						}
					}

				}
				catch (ThreadAbortException)
				{
				}
			}) { IsBackground = true };
			thRecv.Start();

			WhatsUserManager usrMan = new WhatsUserManager();
			var tmpUser = usrMan.CreateUser(dst, "User");

			while (false)
			{
				string line = Console.ReadLine();
				if (line == null && line.Length == 0)
					continue;

				string command = line.Trim();
				switch (command)
				{
					case "/query":
						//var dst = dst//trim(strstr($line, ' ', FALSE));
						Console.WriteLine("[] Interactive conversation with {0}:", tmpUser);
						break;
					case "/accountinfo":
						Console.WriteLine("[] Account Info: {0}", wa.GetAccountInfo().ToString());
						break;
					case "/lastseen":
						Console.WriteLine("[] Request last seen {0}", tmpUser);
						wa.SendQueryLastOnline(tmpUser.GetFullJid());
						break;
					case "/exit":
						wa = null;
						thRecv.Abort();
						return;
					case "/start":
						wa.SendComposing(tmpUser.GetFullJid());
						break;
					case "/pause":
						wa.SendPaused(tmpUser.GetFullJid());
						break;
					default:
						Console.WriteLine("[] Send message to {0}: {1}", tmpUser, line);
						wa.SendMessage(tmpUser.GetFullJid(), line);
						break;
				}
			}
		}


		private void initWhatsAppConnect()
		{
			WhatsApp wa = new WhatsApp(Sender, Password, Nickname, true);
			wa.OnGetMessage += wa_OnGetMessage;
			//wa.OnGetPhoto += wa_OnGetPhoto;
			wa.OnGetMessageImage += wa_OnGetMessageImage;
			wa.OnConnectFailed += new WhatsEventBase.ExceptionDelegate(Instance_OnConnectFailed);
			WhatsAppApi.Helper.DebugAdapter.Instance.OnPrintDebug += Instance_OnPrintDebug;
			wa.Connect();

			try
			{
				wa.Login();
			}
			catch (Exception e)
			{
				MessageBox.Show(this, "Login failed resone: " + e, "Error", MessageBoxButton.OK, MessageBoxImage.Error);
				return;
			}
			if (wa.ConnectionStatus != WhatsAppApi.WhatsApp.CONNECTION_STATUS.LOGGEDIN)
			{
				MessageBox.Show(this, "Login failed resone: " + wa.ConnectionStatus, "Error", MessageBoxButton.OK, MessageBoxImage.Error);
				return;
			}
			ProcessChat(wa, target);
		}
		// End whatsApp Connection









		//Start op msgs

		private bool isCommandOpMsg(string from, string message)
		{
			bool isCommandOpMsg = false;
			if (string.IsNullOrEmpty(message) || !message.StartsWith("op"))
			{
				return isCommandOpMsg;
			}

			bool isCanGetCommandsFromThisNumber = false;
			string commandsOpOnlyFrom = WhatsappProperties.Instance.CommandsOpOnlyFrom;
			if (!string.IsNullOrEmpty(commandsOpOnlyFrom))
			{
				string[] commandsOpOnlyFromNumbers = commandsOpOnlyFrom.Split(',');
				if (commandsOpOnlyFromNumbers != null && commandsOpOnlyFromNumbers.Length > 0)
				{
					foreach (string number in commandsOpOnlyFromNumbers)
					{
						if (from.Contains(number))
						{
							isCanGetCommandsFromThisNumber = true;
							break;
						}
					}
				}
			}
			else
			{
				isCanGetCommandsFromThisNumber = true;
			}

			if (isCanGetCommandsFromThisNumber)
			{
				string commandsOpPassword = WhatsappProperties.Instance.CommandsOpPassword;
				if (!string.IsNullOrEmpty(commandsOpPassword))
				{
					string[] opMsgProp = message.Split(',');
					if (opMsgProp.Length >= 3)
					{
						string pass = opMsgProp[1];
						if (commandsOpPassword.Equals(pass))
						{
							return true;
						}
					}

				}
			}
			return isCommandOpMsg;
		}

		private void handleCommandOpMag(string from, string message)
		{
			if (string.IsNullOrEmpty(message))
			{
				return;
			}
			string[] messageProp = message.Split(',');
			if (messageProp != null && messageProp.Length >= 3)
			{
				string msgOp = messageProp[2];
				if ("clns".Equals(msgOp))
				{
					if (messageProp.Length >= 4)
					{
						int i = 0;
						bool result = int.TryParse(messageProp[3], out i);
						if (result)
						{
							handelCleanScreenOpMsg(i);
						}
					}
					else
					{
						handelCleanScreenOpMsg();
					}
				}
			}

		}
		private void handelCleanScreenOpMsg()
		{
			handelCleanScreenOpMsg(-1);
		}

		private void handelCleanScreenOpMsg(int controlToRemoveCount)
		{
			if (controlToRemoveCount == -1)
			{
				this.stackPanel1.Dispatcher.Invoke((Action)(() => { this.stackPanel1.Children.Clear(); }));
			}
			else
			{
				
				int controlCount = 0;
				this.stackPanel1.Dispatcher.Invoke((Action)(() => { controlCount = this.stackPanel1.Children.Count; }));
				int controlToremove = controlCount - 1;
				while (controlCount > 0 && controlToremove >= 0 && controlToRemoveCount > 0)
				{
					this.stackPanel1.Dispatcher.Invoke((Action)(() => { this.stackPanel1.Children.RemoveAt(controlToremove); }));
					this.stackPanel1.Dispatcher.Invoke((Action)(() => { controlCount = this.stackPanel1.Children.Count; }));
					controlToremove = controlToremove - 1;
					controlToRemoveCount = controlToRemoveCount - 1;
				}
			}
		}

		//End op msgs



		
	}
	
	
}

