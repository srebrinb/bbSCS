using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Net;
using System.Reflection;
using System.Windows.Forms;
[assembly: log4net.Config.XmlConfigurator(Watch = true)]
namespace Html5WebSCSTrayApp
{
    /// <summary>
    /// 
    /// </summary>
    static class Program
    {
        private static readonly log4net.ILog log = log4net.LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);
        public static string productName = Application.ProductName;
        public static string executablePath = Application.ExecutablePath;


        public static string[] prefix = { "http://127.0.0.1:53951/", "https://127.0.0.1:53952/" };
        public static Properties.Settings settings = new Properties.Settings();
        public static void exit()
        {
            Application.Exit();
        }
        /// <summary>
        /// The main entry point for the application.
        /// </summary>
        [STAThread]
        static void Main()
        {
            log.InfoFormat("Starting {0} ver.{1}", Assembly.GetExecutingAssembly().GetName().Name, Assembly.GetExecutingAssembly().GetName().Version.ToString());
            Properties.Settings settings = new Properties.Settings();
            prefix[0] = settings.httpUri;
            prefix[1] = settings.httpsUri;
            /*
            List<string> webUrladdress = new List<string>();
            string[] confsUrls = (settings.httpUri + "," + settings.httpsUri).Split(',');
            foreach (string url in confsUrls)
            {
                webUrladdress.Add(url);
                Uri https = new Uri(url);
                IPAddress[] iPAddress = Dns.GetHostEntry(https.Host).AddressList;
                string ip = iPAddress[0].ToString();
                webUrladdress.Add(https.Scheme+"://"+ip+":"+ https.Port+"/");
            }
            prefix = webUrladdress.ToArray();
    */
            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);
            AppDomain.CurrentDomain.ProcessExit += new EventHandler(CurrentDomain_ProcessExit);


            if (Html5WebSCSTrayApp.InstallSetup.IsAdministrator())
            {
                string tump = Html5WebSCSTrayApp.InstallSetup.installCets();
                Html5WebSCSTrayApp.InstallSetup.setACLs(prefix);
                Uri https = new Uri(settings.httpsUri);
                //         string ip = Dns.GetHostEntry(https.Host).AddressList[0].ToString();
                Html5WebSCSTrayApp.InstallSetup.setCert(tump, https.Host + ":" + https.Port);
                try {
                    Process.Start(settings.httpsUri, null);
                }
                catch (Exception e) {
                    MessageBox.Show(e.Message, "Start " + settings.httpsUri, MessageBoxButtons.OK, MessageBoxIcon.Information);
                }
            }
            // Show the system tray icon.					
            using (ProcessIcon pi = new ProcessIcon())
            {
                try
                {
                    if (pi.Display())
                    {
                        Application.Run();
                    }
                    else
                    {
                        Application.Exit();
                    }
                }
                catch (Exception e)
                {
                    log.Error(e.Message, e);
                    MessageBox.Show(e.Message, "Application "+ Assembly.GetExecutingAssembly().GetName().Name, MessageBoxButtons.OK, MessageBoxIcon.Error);

                }


                // Make sure the application runs!

            }
        }
        static void CurrentDomain_ProcessExit(object sender, EventArgs e)
        {
            exit();
        }
    }
}