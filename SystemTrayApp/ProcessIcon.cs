using System;
using System.Diagnostics;
using System.Net;
using System.Windows.Forms;
using Html5WebSCSTrayApp.Properties;

namespace Html5WebSCSTrayApp
{
    /// <summary>
    /// 
    /// </summary>
    class ProcessIcon : IDisposable
    {
        private static readonly log4net.ILog log = log4net.LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);

        /// <summary>
        /// The NotifyIcon object.
        /// </summary>
        NotifyIcon ni;
        WebService wsHTTPRequestHandler;
        WebServer ws;

        /// <summary>
        /// Initializes a new instance of the <see cref="ProcessIcon"/> class.
        /// </summary>
        public ProcessIcon()
        {
            wsHTTPRequestHandler = new WebService();
            // Instantiate the NotifyIcon object.

            ni = new NotifyIcon();
        }

        /// <summary>
        /// Displays the icon in the system tray.
        /// </summary>
        public bool Display()
        {
            // Put the icon in the system tray and allow it react to mouse clicks.			
            ni.MouseClick += new MouseEventHandler(ni_MouseClick);
            ni.Icon = Resources.html5Paraf;
            ni.Text = "HTML5 and Digital Signatures: Signature Creation Service";
            ni.Visible = true;

            // Attach a context menu.

            ni.ContextMenuStrip = new ContextMenus().Create();
            try
            {
                ws = new WebServer(wsHTTPRequestHandler.SendResponse, Program.prefix);
                ws.Run();
            }
            catch (Exception e)
            {
                log.Error(e.Message, e);
                if (e.Message.Equals("Access is denied"))
                {
                    MessageBox.Show("Install & Setings Web Access SSLCert");
                    Html5WebSCSTrayApp.InstallSetup.runAs(Program.executablePath);
                }
                else
                {
                    MessageBox.Show(e.Message+"\n Exit", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    
                }
                return false;
            }
            return true;
        }

        /// <summary>
        /// Releases unmanaged and - optionally - managed resources
        /// </summary>
        public void Dispose()
        {
            // When the application closes, this will remove the icon from the system tray immediately.
            ni.Dispose();
        }

        /// <summary>
        /// Handles the MouseClick event of the ni control.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The <see cref="System.Windows.Forms.MouseEventArgs"/> instance containing the event data.</param>
        void ni_MouseClick(object sender, MouseEventArgs e)
        {
            // Handle mouse button clicks.
            if (e.Button == MouseButtons.Left)

            {
                ni.ContextMenuStrip.Update();
                /*
                if (browserStartProc == null)
                {
                    browserStartProc = Process.Start("http://localhost:53951/", null);
                }
                else
                {
                    browserStartProc.Refresh();
                }
                */
            }
        }
        void OnDestroy()
        {
            ws.Stop();
            Program.exit();
        }
    }
}