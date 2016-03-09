using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using System.Security.Cryptography.X509Certificates;
using System.Text;
using System.Threading.Tasks;
using SystemTrayApp;

namespace Html5WebSCSTrayApp
{
    class SignerService
    {
        private string version="1.0";
        public string getVersion()
        {
            return version;
        }
        CertInfo selectedCert = null;
        private X509Certificate2Collection lisyMyCerts(dynamic payload)
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
        public string selectCert(dynamic payload)
        {
            X509Certificate2Collection fcollection = lisyMyCerts(payload);
            if (fcollection.Count == 0)
            {
                var objResp = new JObject();
                objResp.Add("version", version);
                objResp.Add("status", "failed");
                objResp.Add("reasonCode", 404);
                objResp.Add("reasonText", "Cert not found");
                return objResp.ToString();
            }
            if (fcollection.Count > 1)
            {
                fcollection = X509Certificate2UI.SelectFromCollection(fcollection, "Certificate Select", "Select a certificate from the following list to get information on that certificate", X509SelectionFlag.SingleSelection);

            }
            CertInfo certInfo;
            string json = "";
            CertInfo ci=new CertInfo();
            foreach (X509Certificate2 x509 in fcollection)
            {
                certInfo = ci.getCertInfo(x509);
                json = certInfo.getJsonStr();
                selectedCert = certInfo;
                //x509.Reset();
            }
            return json;
        }
        public string certs(dynamic payload)
        {
            var objResp = new JObject();
            objResp.Add("version", version);
            X509Certificate2Collection fcollection = lisyMyCerts(payload);
            CertInfo certInfo;

            var arrCerts = new JArray();
            int count = 0;
            CertInfo ci = new CertInfo();
            foreach (X509Certificate2 x509 in fcollection)
            {
                certInfo = ci.getCertInfo(x509);
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
            CertInfo ci=new CertInfo();
            string json = "";
            foreach (X509Certificate2 x509 in scollection)
            {
                CertInfo certInfo = ci.getCertInfo(x509);
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
            objResp.Add("version", version);
            try
            {
                string content = payload.content;
                if (selectedCert == null) selectCert(payload);
                Signer si = new Signer(selectedCert.certificate);
                try
                {
                    if (payload.hashAlgorithm != null) si.hashAlgorithm = payload.hashAlgorithm;
                }
                catch { };
                try { 
                    if (payload.contentType != null) si.contentType = payload.contentType;
                }
                catch { }
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
        public string Version()
        {
            string resp= "{\"version\": \"1.0\",\"httpMethods\": \"GET, POST\",\"contentTypes\": \"data, digest\",\"signatureTypes\": \"signature\",\"selectorAvailable\": true,\"hashAlgorithms\": \"SHA1, SHA256, SHA384, SHA512\"}";
            return resp;
        }
        public string unkonwAction(string action)
        {
            var objResp = new JObject();
            objResp.Add("version", "1.0");
            objResp.Add("status", "failed");
            objResp.Add("reasonCode", 404);
            objResp.Add("reasonText", "Actions " + action + " not exist");
            var objRespActions = new JObject();
            objRespActions.Add("SelectCert","Select certificate for signer");
            objRespActions.Add("Sign", "Signnig contents");
            objRespActions.Add("Certs", "Get list certs");
            objRespActions.Add("Version", "The version check SCS");
            objResp.Add("Accept-Action", objRespActions);
            return objResp.ToString();
        }
    }
}
