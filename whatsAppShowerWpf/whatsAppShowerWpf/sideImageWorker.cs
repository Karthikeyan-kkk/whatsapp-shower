using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace whatsAppShowerWpf
{
    class sideImageWorker
    {
        MainWindow mainWindows;

        public MainWindow MainWindows
        {
            get { return mainWindows; }
            set { mainWindows = value; }
        }
        public sideImageWorker(MainWindow MainWindowsParam)
        {
            this.MainWindows = MainWindowsParam;
        }
        public void DoWork()
        {
            while (!_shouldStop)
            {
                //int replaceSideImageInSec = WhatsappProperties.Instance.ReplaceSideImageInSec;
                
            }
            Console.WriteLine("worker thread: terminating gracefully.");
        }
        public void RequestStop()
        {
            _shouldStop = true;
        }
        // Volatile is used as hint to the compiler that this data 
        // member will be accessed by multiple threads. 
        private volatile bool _shouldStop;
    }
}
