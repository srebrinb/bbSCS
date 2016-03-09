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

namespace SystemTrayApp
{
    class WebService
    {
        CertInfo selectedCert = null;
        public X509Certificate2Collection lisyMyCerts(dynamic payload)
        {
            X509Store store = new X509Store("MY", StoreLocation.CurrentUser);
            store.Open(OpenFlags.ReadOnly | OpenFlags.OpenExistingOnly);
            X509Certificate2Collection collection = (X509Certificate2Collection)store.Certificates;
            X509Certificate2Collection fcollection = new X509Certificate2Collection(collection);
            dynamic selector = null;
            try
            {
                selector = payload.selector;
            }
            catch { }
            if (selector != null)
            {

                bool validate = false;
                try
                {
                    if (selector.validate != null && selector.validate == "true")
                    {
                        validate = (selector.validate == "true" ? true : false);
                        fcollection = (X509Certificate2Collection)fcollection.Find(X509FindType.FindByTimeValid, DateTime.Now, validate);
                    }
                }
                catch { }
                try
                {
                    if (selector.issuers != null)
                    {

                        X509Certificate2Collection tmpcollection = new X509Certificate2Collection();
                        foreach (string issuer in selector.issuers)
                        {
                            X509Certificate2Collection issuerColl = (X509Certificate2Collection)fcollection.Find(X509FindType.FindByIssuerName, issuer, validate);
                            tmpcollection.AddRange(issuerColl);
                        }
                        fcollection = tmpcollection;
                    }
                }
                catch { }
                try
                {
                    if (selector.thumbprint != null)
                    {
                        string thumbprint = selector.thumbprint;
                        thumbprint = thumbprint.Replace("\u200e", string.Empty).Replace("\u200f", string.Empty).Replace(" ", string.Empty).Replace(":", string.Empty).Replace("-", string.Empty);
                        thumbprint = thumbprint.ToLower();
                        fcollection = (X509Certificate2Collection)fcollection.Find(X509FindType.FindByThumbprint, thumbprint, validate);
                    }
                }
                catch { }
            }
            store.Close();
            return fcollection;
        }
        private string selectCert(dynamic payload)
        {
            X509Certificate2Collection fcollection = lisyMyCerts(payload);
            if (fcollection.Count == 0)
            {
                var objResp = new JObject();
                objResp.Add("version", "1.0");
                objResp.Add("status", "failed");
                objResp.Add("reasonCode", 500);
                objResp.Add("reasonText", "Cert not faund");
                return objResp.ToString();
            }
            if (fcollection.Count > 1)
            {
                fcollection = X509Certificate2UI.SelectFromCollection(fcollection, "Certificate Select", "Select a certificate from the following list to get information on that certificate", X509SelectionFlag.SingleSelection);

            }
            CertInfo certInfo;
            string json = "";
            foreach (X509Certificate2 x509 in fcollection)
            {
                certInfo = new CertInfo(x509);
                json = certInfo.getJsonStr();
                selectedCert = certInfo;
                //x509.Reset();
            }
            return json;
        }
        private string getListCerts(dynamic payload)
        {
            var objResp = new JObject();
            objResp.Add("version", "1.0");
            X509Certificate2Collection fcollection = lisyMyCerts(payload);
            CertInfo certInfo;
            string json = "";

            var arrCerts = new JArray();
            int count = 0;
            foreach (X509Certificate2 x509 in fcollection)
            {
                certInfo = new CertInfo(x509);
                arrCerts.Add(certInfo.getJson());
                count++;
            }
            objResp.Add("count", count);            
            objResp.Add("certs", arrCerts);
            return objResp.ToString();

        }
        private string selectCert()
        {
            X509Store store = new X509Store("MY", StoreLocation.CurrentUser);
            store.Open(OpenFlags.ReadOnly | OpenFlags.OpenExistingOnly);

            X509Certificate2Collection collection = (X509Certificate2Collection)store.Certificates;
            X509Certificate2Collection fcollection = (X509Certificate2Collection)collection.Find(X509FindType.FindByTimeValid, DateTime.Now, false);

            X509Certificate2Collection scollection = X509Certificate2UI.SelectFromCollection(fcollection, "Test Certificate Select", "Select a certificate from the following list to get information on that certificate", X509SelectionFlag.SingleSelection);
            CertInfo certInfo;
            string json = "";
            foreach (X509Certificate2 x509 in scollection)
            {
                certInfo = new CertInfo(x509);
                json = certInfo.getJsonStr();
                selectedCert = certInfo;
                //x509.Reset();
            }
            store.Close();

            return json;
        }
        public static byte[] StringToByteArray(string hex)
        {
            return Enumerable.Range(0, hex.Length)
                             .Where(x => x % 2 == 0)
                             .Select(x => Convert.ToByte(hex.Substring(x, 2), 16))
                             .ToArray();
        }
        public string sign(string hashB64data)
        {
            byte[] hash;
            try
            {
                hash = StringToByteArray(hashB64data);
            }
            catch
            {
                hash = System.Convert.FromBase64String(hashB64data);
            }
            //  byte[] hash = System.Convert.FromBase64String(hashB64data);
            if (selectedCert == null) selectCert();
            RSACryptoServiceProvider csp = (RSACryptoServiceProvider)selectedCert.certificate.PrivateKey;
            CspKeyContainerInfo cspKeyContainerInfo = csp.CspKeyContainerInfo;
            CspParameters cspParametersTmp = new CspParameters();
            cspParametersTmp.KeyContainerName = cspKeyContainerInfo.KeyContainerName;
            //cspParametersTmp.KeyNumber = KeyNumber.Signature;
            cspParametersTmp.ProviderType = cspKeyContainerInfo.ProviderType;
            cspParametersTmp.ProviderName = cspKeyContainerInfo.ProviderName;
            // cspParametersTmp.KeyPassword = securePwd;
            RSACryptoServiceProvider rsaSignProvider = new RSACryptoServiceProvider(cspParametersTmp);

            byte[] sig = rsaSignProvider.SignHash(hash, CryptoConfig.MapNameToOID("SHA1"));
            return System.Convert.ToBase64String(sig);
        }
        public string sign(dynamic payload)
        {
            var objResp = new JObject();
            objResp.Add("version", "1.0");
            try
            {
                string content = payload.content;
                if (selectedCert == null) selectCert(payload);
                if (selectedCert == null) throw new Exception("Cert not found");
                Signer si = new Signer(selectedCert.certificate);
                if (payload.hashAlgorithm != null) si.hashAlgorithm = payload.hashAlgorithm;
                if (payload.contentType != null) si.contentType = payload.contentType;

                objResp.Add("signature", si.sign(content));
                var arrChain = new JArray(selectedCert.getChain());

                objResp.Add("status", "ok");
                objResp.Add("reasonCode", 200);
                objResp.Add("reasonText", "Signature generated");

                objResp.Add("signatureAlgorithm", si.hashAlgorithm + "withRSA");
                objResp.Add("chain", arrChain);
            }
            catch (Exception e)
            {
                selectedCert = null;
                objResp.Add("status", "failed");
                objResp.Add("reasonCode", 500);
                objResp.Add("reasonText", e.Message);
            }
            return objResp.ToString();
        }
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
                switch (action)
                {
                    case ("sign"):
                        strOut = sign(payload);
                        break;
                    case ("certs"):
                        strOut = getListCerts(payload);
                        break;
                    case ("selectcert"):
                        strOut = selectCert(payload);
                        break;
                    default:
                        var objResp = new JObject();
                        objResp.Add("version", "1.0");
                        objResp.Add("status", "failed");
                        objResp.Add("reasonCode", 404);
                        objResp.Add("reasonText", "Actions " + action + " not exist");

                        strOut = objResp.ToString();

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
