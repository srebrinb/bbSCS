using System;
using System.Windows.Forms;

namespace SystemTrayApp
{
    /// <summary>
    /// 
    /// </summary>
    static class Program
    {
        public static string executablePath = Application.ExecutablePath;
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
                Html5WebSCSTrayApp.InstallSetup.setCert(tump, "127.0.0.1:53952");

            }
            // Show the system tray icon.					
            using (ProcessIcon pi = new ProcessIcon())
            {
                pi.Display();

                // Make sure the application runs!
                Application.Run();
            }
        }
        static void CurrentDomain_ProcessExit(object sender, EventArgs e)
        {
            exit();
        }
    }
}