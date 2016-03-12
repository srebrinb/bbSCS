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

[assembly: log4net.Config.XmlConfigurator(Watch = true)]
namespace Html5WebSCSTrayApp
{
    class WebService
    {
        private static readonly log4net.ILog log = log4net.LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);

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
        public bool SendResponse(HttpListenerContext ctx)
        {
            HttpListenerRequest request = ctx.Request;
            //  HttpListenerResponse response=ctx.Response;
            log.InfoFormat("request ssl {2} {0} {1}", request.HttpMethod, request.RawUrl, ctx.Request.IsSecureConnection);

            
            
            var cookies = ctx.Request.Cookies;
            foreach (Cookie cookie in cookies)
            {
                Console.WriteLine(cookie.Name + ":" + cookie.Value);
                if (cookie.Name.Equals("ssession")) sessionid = cookie.Value;
            }
            if (sessionid=="")
            {
                sessionid = RandomString(32);
                Cookie sessCookie = new Cookie("ssession", sessionid);
                sessCookie.HttpOnly = true;
                sessCookie.Path = "/";
                sessCookie.Domain = request.UserHostName;
                if (ctx.Request.IsSecureConnection) sessCookie.Secure = true;
                ctx.Response.Cookies.Add(sessCookie);
            }
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
                
                if (segments[1].ToLower().StartsWith ("bytes"))
                {
                    buf = Encoding.UTF8.GetBytes("test ok");
                    ctx.Response.ContentLength64 = buf.Length;
                    ctx.Response.OutputStream.Write(buf, 0, buf.Length);
                    return true;
                }
                if ((request.HttpMethod.ToUpper().Equals("POST") || request.HttpMethod.ToUpper().Equals("GET")))
                {
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
                        payload = JsonConvert.DeserializeObject(documentContents);
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
                    string action = segments[1].ToLower().Replace("/", string.Empty);
                    
                    switch (action)
                    {
                        case ("sign"):
                            strOut = sSignerService.sign(payload);
                            break;
                        case ("certs"):
                            strOut = sSignerService.certs(payload);
                            break;
                        case ("selectcert"):
                            strOut = sSignerService.selectCert(payload);
                            break;
                        case ("validate"):
                            strOut = sSignerService.Validate (payload);
                            break;
                        case ("certinfo"):
                            strOut = sSignerService.certinfo(payload);
                            break;
                        case ("version"):
                            strOut = sSignerService.Version();
                            break;
                            //default:
                            //    strOut = sSignerService.unkonwAction(action);
                            //    break;
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
            ctx.Response.ContentLength64 = buf.Length;
            ctx.Response.OutputStream.Write(buf, 0, buf.Length);
            return true;
        }
        public void Options(HttpListenerContext ctx)
        {
            ctx.Response.StatusCode = 200;
            ctx.Response.AppendHeader("Access-Control-Allow-Methods", "GET, POST");
            ctx.Response.AppendHeader("Access-Control-Allow-Headers", "Accept, Content-Type");
            ctx.Response.AppendHeader("Access-Control-Allow-Origin", ctx.Request.Headers.Get("Origin"));
            ctx.Response.AppendHeader("Accept", "application/json, */*");
            log.InfoFormat("Origin {0}", ctx.Request.Headers.Get("Origin"));
        }
    }
}
