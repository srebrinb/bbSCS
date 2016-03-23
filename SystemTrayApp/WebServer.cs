using System;
using System.Net;
using System.Threading;
using System.Linq;
using System.Text;
using Newtonsoft.Json.Linq;
using System.Windows.Forms;

namespace Html5WebSCSTrayApp
{
    public class WebServer
    {
        private static readonly log4net.ILog log = log4net.LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);
        internal NotifyIcon TrayNotifyIcon=null;
        private readonly HttpListener _listener = new HttpListener();
        private readonly Func<HttpListenerContext, bool> _responderMethod;

        public WebServer(string[] prefixes, Func<HttpListenerContext, bool> method)
        {
            if (!HttpListener.IsSupported)
                throw new NotSupportedException(
                    "Needs Windows XP SP2, Server 2003 or later.");

            // URI prefixes are required, for example 
            // "http://localhost:8080/index/".
            if (prefixes == null || prefixes.Length == 0)
                throw new ArgumentException("prefixes");

            // A responder method is required
            if (method == null)
                throw new ArgumentException("method");

            foreach (string s in prefixes)
            {
                _listener.Prefixes.Add(s);
                log.Info(" listener Add (" + s + ")");
            }
            _responderMethod = method;

            _listener.Start();


        }

        public WebServer(Func<HttpListenerContext, bool> method, params string[] prefixes)
            : this(prefixes, method)
        { }

        public void Run()
        {
            ThreadPool.QueueUserWorkItem((o) =>
            {

                log.Info("Webserver running...");
                try
                {
                    while (_listener.IsListening)
                    {
                        ThreadPool.QueueUserWorkItem((c) =>
                        {
                            var ctx = c as HttpListenerContext;
                            try
                            {

                                _responderMethod(ctx);

                            }
                            catch (MyException e)
                            {
                                //if (ctx.Response.Headers.Get("Accept").IndexOf("json")>0)
                                /*ctx.Response.StatusCode = 500;
                                byte[] buf = Encoding.UTF8.GetBytes(e.Message);
                                ctx.Response.ContentLength64 = buf.Length;
                                ctx.Response.OutputStream.Write(buf, 0, buf.Length);
    */
                                var objResp = new JObject();
                                objResp.Add("status", "failed");
                                objResp.Add("reasonCode", e.errorcode);
                                if (e.InnerException!=null && !e.Message.Equals(e.InnerException.Message)) {
                                    objResp.Add("reasonText", e.Message + "\r\n" + e.InnerException.Message);
                                }
                                else
                                {
                                    objResp.Add("reasonText", e.Message);
                                }
                                log.Error(e.Message, e);
                                ctx.Response.StatusCode = 200;
                                ctx.Response.ContentType = "application/json";
                                byte[] buf = Encoding.UTF8.GetBytes(objResp.ToString());
                                ctx.Response.ContentLength64 = buf.Length;
                                ctx.Response.OutputStream.Write(buf, 0, buf.Length);
                                if (TrayNotifyIcon != null) TrayNotifyIcon.ShowBalloonTip(3000, "", e.Message, ToolTipIcon.Error);
                            } // suppress any exceptions
                            finally
                            {
                                // always close the stream
                                ctx.Response.OutputStream.Close();
                            }
                        }, _listener.GetContext());
                    }
                }
                catch { } // suppress any exceptions
            });
        }

        public void Stop()
        {
            _listener.Stop();
            _listener.Close();
        }
    }
}