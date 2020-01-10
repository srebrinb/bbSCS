using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;

using System.Security.Cryptography;
using System.Security.Permissions;
using System.IO;
using System.Security.Cryptography.X509Certificates;
using System.Runtime.Serialization.Json;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using Html5WebSCSTrayApp;
using Html5WebSCSTrayApp.Properties;
using System.Text.RegularExpressions;
using System.Windows.Forms;


namespace Html5WebSCSTrayApp
{
    class WebService
    {
        internal NotifyIcon TrayNotifyIcon = null;
        public WebService() { }
        public WebService(NotifyIcon TrayNotifyIcon)
        {
            this.TrayNotifyIcon = TrayNotifyIcon;
        }
        private static readonly log4net.ILog log = log4net.LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);
        private static string sessionCookieName = "PSess";
        private static Dictionary<string, string> MimeTypesForExtensions = new Dictionary<string, string>
        {
            { ".html", "text/html" },
            { ".css", "text/css" },
            { ".js", "application/javascript" },
            { ".jpg", "image/jpeg" },
            { ".jpeg", "image/jpeg" },
            { ".png", "image/png" },
            { ".gif", "image/gif" },
            { ".svg", "image/svg+xml" }
        };
        private SignerService sSignerService = new SignerService();
        private string sessionid = "";
        public static string RandomString(int length)
        {
            const string chars = "ABCDEFGHIJKLMNOPQRSTUVWXYZabcdefghijklmnopqrstuvwxyz0123456789";
            var random = new Random();
            return new string(Enumerable.Repeat(chars, length)
              .Select(s => s[random.Next(s.Length)]).ToArray());
        }
        public static byte[] ReadFully(Stream input)
        {
            using (MemoryStream ms = new MemoryStream())
            {
                input.CopyTo(ms);
                return ms.ToArray();
            }
        }
        private static string getCaller(HttpListenerRequest request, string formater="{0} {1} {2}")
        {
            string winTitle = Utils.GetActiveWindowTitle();
            string Origin = request.Headers.Get("Origin");
            string Referer = request.Headers.Get("Referer");
            string CallerName = string.Format(formater, winTitle , Origin,  Referer);
            return CallerName.Trim();
        }
        private static string getCallerId(HttpListenerRequest request)
        {
            string Origin = getCaller(request,"{1}{2}");// request.Headers.Get("Origin");
            string UserAgent = request.UserAgent;
            string Machine = Program.executablePath;
            SHA1 sha1 = SHA1.Create();
            byte[] id = sha1.ComputeHash(Encoding.UTF8.GetBytes(Origin + UserAgent + Machine));
            return System.Convert.ToBase64String(id);
        }
        private void TrayNotifyIconInfo(string Text)
        {
            if (TrayNotifyIcon != null) TrayNotifyIcon.ShowBalloonTip(2000, "", Text, ToolTipIcon.Info);
        }
        private void TrayNotifyIconInfo(string Title, string Text)
        {
            if (TrayNotifyIcon != null) TrayNotifyIcon.ShowBalloonTip(2000, Title, Text, ToolTipIcon.Info);
        }
        public bool SendResponse(HttpListenerContext ctx)
        {
            HttpListenerRequest request = ctx.Request;
            log4net.NDC.Remove();
            log4net.NDC.Push("Request");
            log.InfoFormat("{0} {2} {1}", request.HttpMethod, request.RawUrl, (ctx.Request.IsSecureConnection ? "https" : "http"));
            log.InfoFormat("Caller {0}",getCaller(request,"Active Windows '{0}' {1} {2}"));
            if (log.IsDebugEnabled)
            {
                var headers = request.Headers;
                foreach (var header in headers)
                {
                    log.Debug(header.ToString() + ": " + headers.Get(header.ToString()));
                }

            }
            bool newSession = true;
            var cookies = request.Cookies;
            foreach (Cookie cookie in cookies)
            {
                Console.WriteLine(cookie.Name + ": " + cookie.Value);
                if (cookie.Name.Equals(sessionCookieName)) sessionid = cookie.Value;
            }
            string originId = getCallerId(request);
            string[] tm = sessionid.Split('-');
            if (tm.Length == 2 && tm[1] == originId)
            {
                newSession = false;
                log.DebugFormat("old session {0}", sessionid);
            }
            if (sessionid == "" || newSession)
            {
                sessionid = RandomString(32) + "-" + originId;
                Cookie sessCookie = new Cookie(sessionCookieName, sessionid);
                sessCookie.HttpOnly = true;
                sessCookie.Path = "/";
                sessCookie.Expires = DateTime.Now.AddMinutes(30);
                sessCookie.Domain = request.UserHostName;
                if (ctx.Request.IsSecureConnection) sessCookie.Secure = true;
                ctx.Response.Cookies.Add(sessCookie);
                log.DebugFormat("start new session {0}", sessionid);
            }
            //TODO 
            //if (false && newSession)
            //{
            //    TrayNotifyIconInfo("New Session", getCaller(request) + "\r\n" + request.RawUrl);
            //}
            sSignerService.newSession = newSession;
            sSignerService.hWndCaller =  Utils.GetForegroundWindow();

            Options(ctx);
            if (request.HttpMethod.ToUpper().Equals("OPTIONS"))
            {
                return true;
            }
            dynamic payload;
            string strOut = "";
            byte[] buf = Encoding.UTF8.GetBytes(strOut);
            string[] segments = request.Url.Segments;
            if (segments.Length > 1)
            {
                if (segments[1].ToLower().StartsWith("install"))
                {

                    if (!Html5WebSCSTrayApp.InstallSetup.IsAdministrator())
                    {
                        ctx.Response.ContentType = "text/html";
                        using (FileStream fsSource = new FileStream("htmls/installCert.html", FileMode.Open, FileAccess.Read))
                        {
                            buf = ReadFully(fsSource);
                        }

                        ctx.Response.ContentLength64 = buf.Length;
                        ctx.Response.OutputStream.Write(buf, 0, buf.Length);
                        // ctx.Response.Close();
                        Html5WebSCSTrayApp.InstallSetup.runAs(Program.executablePath);
                        Program.exit();
                        return false;
                    }
                }

                if (segments[1].ToLower().StartsWith("bytes"))
                {
                    buf = Encoding.UTF8.GetBytes("test ok");
                    ctx.Response.ContentLength64 = buf.Length;
                    ctx.Response.OutputStream.Write(buf, 0, buf.Length);
                    return true;
                }
                if ((request.HttpMethod.ToUpper().Equals("POST") || request.HttpMethod.ToUpper().Equals("GET")))
                {
                    log4net.NDC.Pop();
                    string action = segments[1].ToLower().Replace("/", string.Empty);
                    if (sSignerService.checkAcrionExits(action))
                    {
                        log4net.NDC.Push("API "+action);
                    }
                    
                    if (request.HttpMethod.ToUpper().Equals("POST"))
                    {
                        string documentContents;
                        using (Stream receiveStream = request.InputStream)
                        {
                            using (StreamReader readStream = new StreamReader(receiveStream, Encoding.UTF8))
                            {
                                documentContents = readStream.ReadToEnd();
                            }
                        }

                        log.Debug(documentContents);
                        try
                        {
                            payload = JsonConvert.DeserializeObject(documentContents);
                        }
                        catch (Exception e)
                        {
                            throw new Exception400("POST Contents wrong JSON.", e);
                        }
                    }
                    else
                    {
                        log.Debug(request.RawUrl);
                        payload = new DynamicDictionary();
                        payload.selector = new DynamicDictionary();
                        foreach (var param in request.QueryString)
                        {

                            Console.WriteLine(param.ToString().ToLower());
                            payload.dictionary[param.ToString().ToLower()] = request.QueryString.Get(param.ToString());
                            if (param.ToString().ToLower() == "thumbprint")
                            {
                                payload.selector.thumbprint = request.QueryString.Get(param.ToString());
                            }
                        }
                    }
                    ctx.Response.ContentType = "application/json";
                    

                    switch (action)
                    {
                        case ("sign"):
                            strOut = sSignerService.sign(payload);
                            break;
                        case ("certs"):
                            strOut = sSignerService.certs(payload);
                            break;
                        case ("selectcert"):
                        case ("cert"):
                            strOut = sSignerService.selectCert(payload);
                            break;
                        case ("validate"):
                            strOut = sSignerService.Validate(payload);
                            break;
                        case ("certinfo"):
                            strOut = sSignerService.certinfo(payload);
                            break;
                        case ("version"):
                            strOut = sSignerService.Version();
                            break;
                        case ("protect"):
                            strOut = sSignerService.ProtectPin(payload);
                            break;

                        case ("help"):
                            strOut = sSignerService.unkonwAction(action);
                            break;
                    }

                }
                if (segments[1].ToLower().Equals("ui"))
                {
                    ctx.Response.Redirect("/");
                }
                
                buf = Encoding.UTF8.GetBytes(strOut);

            }
            if (buf.Length == 0)
            {
                log4net.NDC.Pop();
                log4net.NDC.Push("Web UI");
                try
                {
                    string filename = "htmls";
                    if (segments[segments.Length - 1].EndsWith("/"))
                    {
                        segments[segments.Length - 1] += "index.html";
                    }
                    if (segments[segments.Length - 1].EndsWith(".ico"))
                    {

                        Resources.html5Paraf.Save(ctx.Response.OutputStream);
                        return true;
                    }
                    else {
                        filename += String.Join("", segments);

                        string extension = Path.GetExtension(filename);
                        string mimeType = "text/html";
                        MimeTypesForExtensions.TryGetValue(extension.ToLower(), out mimeType);
                        ctx.Response.ContentType = mimeType;
                        using (FileStream fsSource = new FileStream(filename, FileMode.Open, FileAccess.Read))
                        {
                            buf = ReadFully(fsSource);
                        }
                    }
                }
                catch (Exception e)
                {
                    ctx.Response.StatusCode = 404;
                    buf = Encoding.UTF8.GetBytes(e.Message);
                }
            }
            else
            {
              
                log4net.NDC.Push("Response");
                if (log.IsDebugEnabled)
                {
                    log.Debug(strOut);
                    var headers = ctx.Response.Headers;
                    foreach (var header in headers)
                    {
                        log.Debug(header.ToString() + ": " + headers.Get(header.ToString()));
                    }

                }
            }
            
            ctx.Response.ContentLength64 = buf.Length;
            try
            {
                ctx.Response.OutputStream.Write(buf, 0, buf.Length);
            }
            catch (Exception e) { }
            log4net.NDC.Clear();
            return true;
        }
        public void Options(HttpListenerContext ctx)
        {

            ctx.Response.StatusCode = 200;
            ctx.Response.AppendHeader("Access-Control-Allow-Methods", "GET, POST");
            ctx.Response.AppendHeader("Access-Control-Allow-Headers", "Accept, Content-Type");
            if (ctx.Request.Headers.Get("Origin") != "")
            {
                ctx.Response.AppendHeader("Access-Control-Allow-Origin", ctx.Request.Headers.Get("Origin"));
                log.DebugFormat("Origin: ", ctx.Request.Headers.Get("Origin"));
            }
            ctx.Response.AppendHeader("Accept", "application/json, */*");


        }
    }
}
