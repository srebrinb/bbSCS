using System;
using System.Diagnostics;
using System.Windows.Forms;

namespace Html5WebSCSTrayApp
{
    /// <summary>
    /// 
    /// </summary>
    static class Program
    {
        private static readonly log4net.ILog log = log4net.LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);

        public static string executablePath = Application.ExecutablePath;
        public static string[] prefix = { "http://127.0.0.1:53951/", "https://127.0.0.1:53952/" };
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

            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);
            AppDomain.CurrentDomain.ProcessExit += new EventHandler(CurrentDomain_ProcessExit);

            if (Html5WebSCSTrayApp.InstallSetup.IsAdministrator())
            {
                string tump = Html5WebSCSTrayApp.InstallSetup.installCets();
                Html5WebSCSTrayApp.InstallSetup.setACLs(prefix);
                Html5WebSCSTrayApp.InstallSetup.setCert(tump, "127.0.0.1:53952");
                Process.Start("https://localhost:53952/", null);

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
                    exit();
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