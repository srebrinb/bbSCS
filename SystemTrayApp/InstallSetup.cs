﻿using Html5WebSCSTrayApp.Properties;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Reflection;
using System.Runtime.InteropServices;
using System.Security.Cryptography.X509Certificates;
using System.Security.Principal;
using System.Text;
using System.Threading.Tasks;


namespace Html5WebSCSTrayApp
{
    class InstallSetup
    {
        private static readonly log4net.ILog log = log4net.LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);

        public static string installCets()
        {
            string thump = "";
            X509Store store = new X509Store("Root", StoreLocation.LocalMachine);
            store.Open(OpenFlags.ReadWrite);
            X509Certificate2 rootCert = new X509Certificate2(Resources.vSrebrinCA);
            store.Add(rootCert);
            store.Close();
            log.InfoFormat("Install certs CA {0} thmp:{1}", rootCert.Subject, rootCert.Thumbprint);
            rootCert.Reset();
            X509Store mystore = new X509Store("My", StoreLocation.LocalMachine);
            mystore.Open(OpenFlags.ReadWrite);
            X509Certificate2 Cert = new X509Certificate2(Resources.localhost, "", X509KeyStorageFlags.MachineKeySet | X509KeyStorageFlags.PersistKeySet);
            mystore.Add(Cert);
            mystore.Close();
            thump= Cert.Thumbprint;
            
            log.InfoFormat("Install certs Cert {0} thmp:{1}", Cert.Subject,thump);
            Cert.Reset();
            return thump;
        }
        public static void setCert(string thump,string address)
        {
            string args=String.Format( "http add sslcert ipport={0} certhash={1} appid={{{2}}}"
                           ,address,thump.ToLower(),getGUID());


            execNetsh(args);

        }
        public static void execNetsh(string cmdArgs)
        {
            log.InfoFormat("netsh {0}", cmdArgs);
            ProcessStartInfo proc = new ProcessStartInfo();
            proc.UseShellExecute = false;
            proc.WorkingDirectory = Environment.CurrentDirectory;
            proc.FileName = "netsh";
            proc.Arguments = cmdArgs;
            //  proc.Verb = "runas";
            proc.CreateNoWindow = true;
            proc.WindowStyle = ProcessWindowStyle.Hidden;
              proc.RedirectStandardOutput = true;
            proc.RedirectStandardError = true;
            Process p = new Process();
            p.StartInfo = proc;
            string output = "";
            string error = "";
            try
            {
                p.Start();

                // To avoid deadlocks, always read the output stream first and then wait.
                output = p.StandardOutput.ReadToEnd();
                error = p.StandardError.ReadToEnd();
                p.WaitForExit();
                log.Debug (output);

            }
            catch (Exception ex)
            {
                log.Error(cmdArgs,ex);
                log.Debug(error);
                return;
            }
        }
        public static void setACLs(string[] address)
        {
            foreach (string addres in address)
                setACL(addres);
        }
        public static void setACL(string address)
        {
            string args = String.Format("http add urlacl {0} user=\\Everyone", address);
            execNetsh(args);
        }
        public static void runAs(string cmd)
        {
            // Elevate the process if it is not run as administrator.
            if (!IsAdministrator())
            {
                // Launch itself as administrator
                ProcessStartInfo proc = new ProcessStartInfo();
                proc.UseShellExecute = true;
                proc.WorkingDirectory = Environment.CurrentDirectory;
                proc.FileName = cmd;
                proc.Arguments = "doSetupYes";
                proc.Verb = "runas";
                try
                {
                    Process.Start(proc);
                }
                catch
                {
                    // The user refused the elevation.
                    // Do nothing and return directly ...
                    return;
                }
                

            }

        }
        public static string getGUID()
        {
            Assembly assembly = Assembly.GetExecutingAssembly();

            //The following line (part of the original answer) is misleading.
            //**Do not** use it unless you want to return the System.Reflection.Assembly type's GUID.
            Console.WriteLine(assembly.GetType().GUID.ToString());


            // The following is the correct code.
            var attribute = (GuidAttribute)assembly.GetCustomAttributes(typeof(GuidAttribute), true)[0];
            return attribute.Value;
        }
        public static bool IsAdministrator()
        {
            WindowsIdentity identity = WindowsIdentity.GetCurrent();

            if (identity != null)
            {
                WindowsPrincipal principal = new WindowsPrincipal(identity);
                return principal.IsInRole(WindowsBuiltInRole.Administrator);
            }

            return false;
        }
    }

}
