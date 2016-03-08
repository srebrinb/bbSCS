using System;
using System.Diagnostics;
using System.Net;
using System.Windows.Forms;
using Html5WebSCSTrayApp.Properties;

namespace SystemTrayApp
{
	/// <summary>
	/// 
	/// </summary>
	class ProcessIcon : IDisposable
	{
		/// <summary>
		/// The NotifyIcon object.
		/// </summary>
		NotifyIcon ni;
        WebService wsHTTPRequestHandler;
        Process browserStartProc=null;
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
		public void Display()
		{
			// Put the icon in the system tray and allow it react to mouse clicks.			
			ni.MouseClick += new MouseEventHandler(ni_MouseClick);
            ni.Icon = Resources.html5Paraf;
			ni.Text = "HTML5 and Digital Signatures: Signature Creation Service";
			ni.Visible = true;

            // Attach a context menu.
            
			ni.ContextMenuStrip = new ContextMenus().Create();
            //
            string[] prefix = { "http://localhost:53951/", "https://localhost:53952/" };
            WebServer ws = new WebServer(wsHTTPRequestHandler.SendResponse , prefix);
            ws.Run();

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
                if (browserStartProc == null)
                {
                    browserStartProc = Process.Start("http://localhost:53951/UI", null);
                }
                else
                {
                    browserStartProc.Refresh();
                }
			}
		}
	}
}