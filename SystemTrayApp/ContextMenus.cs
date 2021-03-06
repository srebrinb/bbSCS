﻿using System;
using System.Diagnostics;
using System.Windows.Forms;
using Html5WebSCSTrayApp.Properties;
using System.Drawing;
using log4net;
using log4net.Core;

namespace Html5WebSCSTrayApp
{
    /// <summary>
    /// 
    /// </summary>
    class ContextMenus
    {
        /// <summary>
        /// Is the About box displayed?
        /// </summary>
        bool isAboutLoaded = false;

        /// <summary>
        /// Creates this instance.
        /// </summary>
        /// <returns>ContextMenuStrip</returns>
        public ContextMenuStrip Create()
        {
            // Add the default menu options.
            ContextMenuStrip menu = new ContextMenuStrip();
            ToolStripMenuItem item;
            ToolStripSeparator sep;
            ToolStripLabel label = new ToolStripLabel();
            label.Text = Application.ProductName;

            menu.Items.Add(label);

            sep = new ToolStripSeparator();
            menu.Items.Add(sep);

            item = new ToolStripMenuItem();
            item.Text = "View Web";
            item.Click += new EventHandler(OpenWeb_Click);
            menu.Items.Add(item);
            item = new ToolStripMenuItem();
            item.Text = "View SSL Web";
            item.Click += new EventHandler(OpenSSL_Click);
            menu.Items.Add(item);
            menu.Items.Add(new ToolStripSeparator());
            item = new ToolStripMenuItem();
            item.Text = "About";
            item.Click += new EventHandler(About_Click);
            item.Image = Resources.About;
            menu.Items.Add(item);
            item = new ToolStripMenuItem();
            item.Text = "Log";
            item.Click += new EventHandler(ViewLog_Click);
            menu.Items.Add(item);
            item = new ToolStripMenuItem();
            item.Text = "Install SSL cert";
            item.Click += new EventHandler(InstallSSLCert_Click);
            item.Image = Resources.ssl_certificates;
            menu.Items.Add(item);
            ToolStripMenuItem itemComboBox = new ToolStripMenuItem();
            itemComboBox.Text = "Startup";
            itemComboBox.CheckOnClick = true;
            itemComboBox.Checked = InstallSetup.GetStartup();
            //      itemComboBox.Click += new System.EventHandler(Startup_Click);
            itemComboBox.CheckedChanged += new System.EventHandler(Startup_Click);
            menu.Items.Add(itemComboBox);
            itemComboBox = new ToolStripMenuItem();
            itemComboBox.Text = "Debug";
            itemComboBox.CheckOnClick = true;
            itemComboBox.Checked = isDebug();
            itemComboBox.CheckedChanged += new System.EventHandler(Debug_Click);
            menu.Items.Add(itemComboBox);
            menu.Items.Add(new ToolStripSeparator());
            itemComboBox = new ToolStripMenuItem();
            itemComboBox.Text = "PINCache";
            itemComboBox.CheckOnClick = true;
            itemComboBox.Checked = isPINCache();
            itemComboBox.CheckedChanged += new System.EventHandler(PINCache_Click);
            menu.Items.Add(itemComboBox);
            menu.Items.Add(new ToolStripSeparator());
            // Exit.
            item = new ToolStripMenuItem();
            item.Text = "Exit";
            item.Click += new System.EventHandler(Exit_Click);
            item.Image = Resources.Exit;
            menu.Items.Add(item);

            return menu;
        }

        /// <summary>
        /// Handles the Click event of the Explorer control.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The <see cref="System.EventArgs"/> instance containing the event data.</param>
        void InstallSSLCert_Click(object sender, EventArgs e)
        {
            Html5WebSCSTrayApp.InstallSetup.runAs(Program.executablePath);
            Application.Exit();
        }
        void OpenWeb_Click(object sender, EventArgs e)
        {
            Process.Start(Program.settings.httpUri, null);
        }
        void OpenSSL_Click(object sender, EventArgs e)
        {
            Process.Start(Program.settings.httpsUri, null);
        }
        ViewTailLogFile.ViewLog viewLog = null;
        void ViewLog_Click(object sender, EventArgs e)
        {
            // Process.Start("log4net.log", null);
            if (viewLog == null || viewLog.getClosed())
            {
                viewLog = new ViewTailLogFile.ViewLog("log4net.log");
            }


            viewLog.Show();
            Application.DoEvents();
        }
        /// <summary>
        /// Handles the Click event of the About control.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The <see cref="System.EventArgs"/> instance containing the event data.</param>
        void About_Click(object sender, EventArgs e)
        {
            About_Show();

        }

        void About_Show()
        {
            if (!isAboutLoaded)
            {
                isAboutLoaded = true;
                new AboutBox().ShowDialog();
                isAboutLoaded = false;
            }
        }
        /// <summary>
        /// Processes a menu item.
        /// </summary>
        /// <param name="sender">The sender.</param>
        /// <param name="e">The <see cref="System.EventArgs"/> instance containing the event data.</param>
        void Exit_Click(object sender, EventArgs e)
        {
            // Quit without further ado.
            Application.Exit();
        }
        void Startup_Click(object sender, EventArgs e)
        {
            InstallSetup.SetStartup(!InstallSetup.GetStartup());
        }
        bool isDebug()
        {
            var repo = ((log4net.Repository.Hierarchy.Hierarchy)LogManager.GetRepository());
            Console.WriteLine("Log Level is" + repo.Root.Level.Name);
            return repo.Root.Level <= Level.Debug;
        }
        void Debug_Click(object sender, EventArgs e)
        {
            var repo = ((log4net.Repository.Hierarchy.Hierarchy)LogManager.GetRepository());
            if (isDebug())
            {
                repo.Root.Level = Level.Info;
            }
            else
            {
                repo.Root.Level = Level.Debug;
            }
            repo.RaiseConfigurationChanged(EventArgs.Empty);
        }
        bool isPINCache()
        {
            Properties.Settings setting = new Properties.Settings();
            return setting.PINCache;
        }
        void PINCache_Click(object sender, EventArgs e)
        {
            Properties.Settings.Default.PINCache = !Properties.Settings.Default.PINCache ;
            Properties.Settings.Default.Save();
        }
    }
}