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

namespace SystemTrayApp
{
    class WebService
    {
        
        public bool SendResponse(HttpListenerContext ctx)
        {
            HttpListenerRequest request = ctx.Request;
            //  HttpListenerResponse response=ctx.Response;
            Console.WriteLine(request.HttpMethod + " " + request.RawUrl);

            Options(ctx);
            dynamic payload;
            string strOut = "";
            string[] segments = request.Url.Segments;
            if (segments[1].ToLower().StartsWith("ui"))
            {
                ctx.Response.ContentType = "text/html";
                strOut = "HTML5 and Digital Signatures: Signature Creation Service<br/>Generate digital signatures without browser extensions.";
                byte[] bufOut = Encoding.UTF8.GetBytes(strOut);
                ctx.Response.ContentLength64 = bufOut.Length;
                ctx.Response.OutputStream.Write(bufOut, 0, bufOut.Length);
                return true;
            }
            if (segments[1].ToLower().StartsWith("install"))
            {

                if (!Html5WebSCSTrayApp.InstallSetup.IsAdministrator())
                {
                    ctx.Response.ContentType = "text/html";
                    strOut = "Installing<br/>Reset app";
                    byte[] bufOut = Encoding.UTF8.GetBytes(strOut);
                    ctx.Response.ContentLength64 = bufOut.Length;
                    ctx.Response.OutputStream.Write(bufOut, 0, bufOut.Length);
                    // ctx.Response.Close();
                    Html5WebSCSTrayApp.InstallSetup.runAs(Program.executablePath);
                    Program.exit();
                    return false;
                }
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
                    payload = JsonConvert.DeserializeObject(documentContents);
                }
                else
                {
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
                SignerService sSignerService = new SignerService();
                switch (action)
                {
                    case ("sign"):
                        strOut = sSignerService.sign(payload);
                        break;
                    case ("certs"):
                        strOut = sSignerService.cert(payload);
                        break;
                    case ("selectcert"):
                        strOut = sSignerService.selectCert(payload);
                        break;
                    case ("version"):
                        strOut = sSignerService.Version();
                        break;
                    default:
                        strOut = sSignerService.unkonwAction(action);
                        break;
                }

            }

            byte[] buf = Encoding.UTF8.GetBytes(strOut);
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
        }
    }
}
