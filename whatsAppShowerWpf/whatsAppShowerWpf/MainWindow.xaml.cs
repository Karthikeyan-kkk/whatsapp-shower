﻿using System;
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
		string sender = ""; // Mobile number with country code (but without + or 00)
		string password = "";//v2 password
		string target = "";// Mobile number to send the message to
		private static readonly ILog msgsLog = log4net.LogManager.GetLogger("msgsLog");
		private static readonly ILog systemLog = log4net.LogManager.GetLogger("systemsLog");
		private DispatcherTimer sideImageTimer = new DispatcherTimer();
		private Queue<string> queue = new Queue<string>();

	   

		public string Nickname
		{
			get { return nickname;}
			set { nickname = value;}
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
		public Queue<string> Queue
		{
			get { return queue; }
			set { queue = value; }
		}
	   

		public MainWindow()
		{

			
		  Login login = new Login();
		  login.ShowDialog();
		  
		  InitializeComponent();
		  try
		  {
			  WhatsappProperties.Instance.initProperties();
		  }
		  catch (Exception e)
		  {
			  systemLog.Error("error initProperties Exception: " + e);
		  }
		  try
		  {
			  NumberPropList.Instance.loadFileProp();
		  }
		  catch (Exception e)
		  {
			  systemLog.Error("error loadFileProp Exception: " + e);
		  }
		  
		 
		  
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
			
		}

		private void initVisualProp()
		{
			double screenWidth = System.Windows.SystemParameters.PrimaryScreenWidth;
			double imageMaxWidth = (screenWidth * WhatsappProperties.Instance.MsgSectionWidth) / 100;
			if (!string.IsNullOrEmpty(WhatsappProperties.Instance.MsgSectionWidthType) && "pix".Equals(WhatsappProperties.Instance.MsgSectionWidthType))
			{
				imageMaxWidth = WhatsappProperties.Instance.MsgSectionWidth;
			}

			this.firstColumn.Width = new GridLength(imageMaxWidth);
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
			startSideImageShow();
		}

		private void startSideImageShow()
		{
			
			Image image = this.sideImage;
			TimeSpan fadeOutTime = TimeSpan.FromSeconds(WhatsappProperties.Instance.SideImagefadingSpeedInSec);
			TimeSpan fadeInTime = TimeSpan.FromSeconds(WhatsappProperties.Instance.SideImagefadingSpeedInSec);


			var fadeInAnimation = new DoubleAnimation(1d, fadeInTime);
			var fadeOutAnimation = new DoubleAnimation(0d, fadeOutTime);
			if (Queue.Count == 0)
			{
				
				image.BeginAnimation(Image.OpacityProperty, fadeOutAnimation);
				sideImageBorder.BeginAnimation(Image.OpacityProperty, fadeOutAnimation);
				return;
			}
			string imgUrl = Queue.Dequeue();
			BitmapImage logo = new BitmapImage();
			logo.BeginInit();
			logo.UriSource = new Uri(imgUrl);
			logo.EndInit();
			ImageSource source = logo;
			if (WhatsappProperties.Instance.SideImageWidth != 0) { 
				double screenWidth = System.Windows.SystemParameters.PrimaryScreenWidth;
				double imageMaxWidth = (screenWidth * WhatsappProperties.Instance.SideImageWidth) / 100;
				if (!string.IsNullOrEmpty(WhatsappProperties.Instance.SideImageWidthType) && "pix".Equals(WhatsappProperties.Instance.SideImageWidthType))
				{
					imageMaxWidth = WhatsappProperties.Instance.SideImageWidth;
				}
				image.Width = imageMaxWidth;
			}   
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
						 textView2 = new TextView(phoneNumber,"",msgText, hour);
					 }
					 else
					 {
						 textView2 = new TextView(phoneNumber,nikeName, msgText, hour);
					 }
					
					 this.stackPanel1.Children.Add(textView2);
					 this.stackPanelScroller.ScrollToBottom();
				 }));
				 
			 }
			 addTextInfoToLog("Text", msgText, phoneNumber, isCanShowMsgMet(phoneNumber, "TEXT"));
		 }
		
		private void addImg(string phoneNumber,string nickName,string imgFileName, string hour)
		{
			phoneNumber = Helpers.filterFromNumber(phoneNumber);
		   
			if (isCanShowMsgMet(phoneNumber, "IMG"))
			{
				phoneNumber = Helpers.formatPhoneNumber(phoneNumber);
				ImgView imgView = null;
				this.stackPanel1.Dispatcher.BeginInvoke(new Action(() => { imgView = new ImgView(phoneNumber,nickName, (ImageSource)new ImageSourceConverter().ConvertFromString(imgFileName), hour); }));
				this.stackPanel1.Dispatcher.Invoke((Action)(() => { this.stackPanel1.Children.Add(imgView); }));
				this.stackPanel1.Dispatcher.Invoke((Action)(() => { this.stackPanelScroller.ScrollToBottom(); }));
				Queue.Enqueue(Environment.CurrentDirectory + @"\" + imgFileName);
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
			try
			{
				Helpers helpers = new Helpers();
				helpers.buildRunningText(this);

				
			}
			catch (Exception e)
			{
				systemLog.Error("error in startRunningText: "+e);
			}
		  }


		private void stackPanel1MouseDown(object sender, MouseButtonEventArgs e)
		{
			
			if (e.ClickCount == 2)
			{
				WhatsappProperties.Instance.initProperties();
				initVisualProp();
				
			}
		}

	   private void mainGrid_MouseRightButtonDown(object sender, MouseButtonEventArgs e)
		{
			Settings.Instance.Show();
			Settings.Instance.Focus();
			Settings.Instance.OnUpdate += new Settings.OnUpdateEvent(UpdateUi);
		}

		public void UpdateUi(object source, EventArgs e)
		{

			double screenWidth = System.Windows.SystemParameters.PrimaryScreenWidth;
			double imageMaxWidth = (screenWidth * WhatsappProperties.Instance.MsgSectionWidth) / 100;
			if (!string.IsNullOrEmpty(WhatsappProperties.Instance.MsgSectionWidthType) && "pix".Equals(WhatsappProperties.Instance.MsgSectionWidthType))
			{
				imageMaxWidth = WhatsappProperties.Instance.MsgSectionWidth;
			}

			this.firstColumn.Width = new GridLength(imageMaxWidth);
			int paddingLeft = WhatsappProperties.Instance.PaddingLeft;
			UIElementCollection uIElementCollection = this.stackPanel1.Children;
			Helpers helper = new Helpers();
			int elementsToUpdate = 5;
			if (!string.IsNullOrEmpty(Settings.Instance.elementsToUpdate.Text))
			{
				int n;
				bool isNumeric = int.TryParse(Settings.Instance.elementsToUpdate.Text, out n);
				if (isNumeric) {
					if (n == -1)
					{
						elementsToUpdate = uIElementCollection.Count;
					}
					else {
						if (elementsToUpdate > uIElementCollection.Count)
						{
							elementsToUpdate = uIElementCollection.Count;
						}
						else
						{
							elementsToUpdate = n;
						}
						
					}
				}
			}

			for (int i = uIElementCollection.Count - 1; i > uIElementCollection.Count - 1 - elementsToUpdate; i--)
			{
				if(uIElementCollection[i].GetType() == typeof(TextView)){
					TextView textView = (TextView)uIElementCollection[i];
					TextView.buildTextView(textView);
				}
				if (uIElementCollection[i].GetType() == typeof(ImgView))
				{
					ImgView imgView = (ImgView)uIElementCollection[i];
					ImgView.buildImgView(imgView);
				}
			}
			
			helper.buildRunningText(this);
			if (!string.IsNullOrEmpty(WhatsappProperties.Instance.Backgroundimage))
			{
				try
				{
					BitmapImage bi = new BitmapImage();
					bi.BeginInit();
					Uri uri = new Uri(WhatsappProperties.Instance.Backgroundimage.Trim(), UriKind.RelativeOrAbsolute);
					if (File.Exists(uri.ToString()))
					{
						bi.UriSource = uri;
						bi.EndInit();
						this.mainGrid.Background.SetValue(ImageBrush.ImageSourceProperty, bi);
					}

				}
				catch (Exception e2)
				{
					systemLog.Error("no background img " + WhatsappProperties.Instance.Backgroundimage + " " + e2);

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

			Settings.Instance.updateStatusBar("Finish Updating...", Brushes.DarkGreen);
			

		}
		//Start tests items

		private void addImages()
		{
			
			string[] imageFilePaths = Directory.GetFiles(WhatsappProperties.Instance.DemoImageFolder, "*.*",SearchOption.AllDirectories);
			if (imageFilePaths != null && imageFilePaths.Length>0)
			{
				for (int i = 0; i < imageFilePaths.Length; i++)
				{
					string file = imageFilePaths[i];
					if (file.EndsWith(".png") || file.EndsWith(".jpg"))
					{
						if (File.Exists(file))
						{
							ImgView imgView = new ImgView("0524376464", "nickName", (ImageSource)new ImageSourceConverter().ConvertFromString(file), "10:10");
							this.stackPanel1.Children.Add(imgView);
							string fullPath = System.IO.Path.GetFullPath(file);
							Queue.Enqueue(fullPath);
						}
					}
				}
			}
			this.stackPanelScroller.ScrollToBottom();
		}

		private void addTexts()
		{
			TextView textView2 = new TextView("0524376464", "", "", "10:10");
			this.stackPanel1.Children.Add(textView2);
			for (int i = 0; i < 5; i++)
			{
				TextView textView = new TextView("0524376464" + i, "","מזל טוב ומבורך וובדיקה לטקסט ארוך מאוד מאוד מאוד לבדיקה שהוא טקסט ארוך מאוד מאוד מאוד מאוד נראה איך הוא יהיה", "10:10");
				this.stackPanel1.Children.Add(textView);
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

		void wa_OnGetMessageImage(string from, string id, string fileName, int fileSize, string url, byte[] preview, ProtocolTreeNode node)
		{
			OnGetMedia(Helpers.getImgFullPath(fileName), url, preview);

			addImg(from,Helpers.getNikeName(node),Helpers.getImgFullPath(fileName), DateTime.Now.ToString("HH:mm"));
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

			/*while (false)
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
			}*/
		}


		private void initWhatsAppConnect()
		{
			WhatsApp wa = new WhatsApp(Sender, Password, Nickname, true);
			
			wa.OnGetMessage += wa_OnGetMessage;
			//wa.OnGetPhoto += wa_OnGetPhoto;
			wa.OnGetMessageImage += new WhatsEventBase.OnGetMediaDelegate(wa_OnGetMessageImage);
			wa.OnConnectFailed += new WhatsEventBase.ExceptionDelegate(Instance_OnConnectFailed);
			wa.OnDisconnect += new WhatsEventBase.ExceptionDelegate(wa_OnDisconnect);
			WhatsAppApi.Helper.DebugAdapter.Instance.OnPrintDebug += Instance_OnPrintDebug;
			wa.OnConnectFailed += new WhatsEventBase.ExceptionDelegate(wa_OnDisconnect);
			wa.OnError += new WhatsEventBase.OnErrorDelegate(wa_OnError);
			wa.OnGetPaused += new WhatsEventBase.OnGetChatStateDelegate(wa_OnGetPaused);
			wa.OnLoginFailed += new WhatsEventBase.StringDelegate(wa_OnLoginFailed);
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

	   
	   
		void wa_OnLoginFailed(string data)
		{
			systemLog.Error("disconnect!!!");
		}

		void wa_OnGetPaused(string from)
		{
			systemLog.Error("disconnect!!!");
		}

		void wa_OnError(string id, string from, int code, string text)
		{
			systemLog.Error("disconnect!!!");
		}

		void wa_OnDisconnect(Exception ex)
		{
			systemLog.Error("disconnect!!!");
			initWhatsAppConnect();
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

