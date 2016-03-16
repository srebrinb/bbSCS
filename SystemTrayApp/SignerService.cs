using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using System.Security.Cryptography.X509Certificates;
using System.Text;
using System.Threading.Tasks;
using Html5WebSCSTrayApp;
using System.Reflection;

namespace Html5WebSCSTrayApp
{
    class SignerService
    {
        private static readonly log4net.ILog log = log4net.LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);
        public bool newSession = true;
        private string version = "1.0";
        public string getVersion()
        {
            return version;
        }
        CertInfo selectedCert = null;

        public dynamic profiles = "base";

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
        [RestAction("selectcert", "Select certificate for signer")]
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
            if (fcollection.Count == 0)
            {
                throw new Exception("The Certificate Select was cancelled by the user");
            }
            try
            {
                if (payload.profile != null) profiles = payload.profile;
            }
            catch { }
            Profiles profile = CertInfo.getProfile(profiles);
            CertInfo ci = new CertInfo(profile);
            CertInfo certInfo = new CertInfo();

            string json = "";

            foreach (X509Certificate2 x509 in fcollection)
            {
                certInfo = ci.getCertInfo(x509);
                json = certInfo.getJsonStr();
                selectedCert = certInfo;
                //x509.Reset();
            }
            //TODO return status 200
            return json;
        }
        [RestAction("certs", "Get list certs")]
        public string certs(dynamic payload)
        {
            var objResp = new JObject();
            objResp.Add("version", version);
            X509Certificate2Collection fcollection = lisyMyCerts(payload);
            CertInfo certInfo;

            var arrCerts = new JArray();
            int count = 0;
            try
            {
                if (payload.profile != null) profiles = payload.profile;
            }
            catch { }
            Profiles profile = CertInfo.getProfile(profiles);
            CertInfo ci = new CertInfo(profile);
            foreach (X509Certificate2 x509 in fcollection)
            {
                certInfo = ci.getCertInfo(x509);
                arrCerts.Add(certInfo.getJson());
                count++;
            }
            objResp.Add("count", count);
            objResp.Add("certs", arrCerts);
            objResp.Add("status", "ok");
            objResp.Add("reasonCode", 200);
            return objResp.ToString();
        }

        public static byte[] StringToByteArray(string hex)
        {
            return Enumerable.Range(0, hex.Length)
                             .Where(x => x % 2 == 0)
                             .Select(x => Convert.ToByte(hex.Substring(x, 2), 16))
                             .ToArray();
        }
        [RestAction("sign", "Signnig contents")]
        public string sign(dynamic payload)
        {
            log4net.NDC.Push("Sign");
            var objResp = new JObject();
            objResp.Add("version", version);
            try
            {
                bool multisign = false;
                string content = "";
                List<string> contents = new List<string>();
                log4net.NDC.Push("Get Content");

                try
                {
                    if (payload.content != null)
                    {
                        multisign = false;
                        content = payload.content;
                    }
                    else if (payload.contents != null)
                    {
                        multisign = true;
                        JArray arrContents = payload.contents;
                        foreach (string tmpCont in arrContents)
                        {
                            contents.Add(tmpCont);
                        }
                    }
                    else
                    {
                        throw new Exception("Not exist 'content' or 'contents'");
                    }
                }
                catch (Exception e)
                {
                    throw new Exception("Get content to sign."+ e.Message);
                }


                bool forceSelectCert = false;
                bool forcePINRquest = false;
                try
                {
                    if (payload.forceSelectCert != null)
                    {
                        string strForceSelectCert = payload.forceSelectCert;
                        strForceSelectCert = strForceSelectCert.ToLower();
                        if (strForceSelectCert == "true" || strForceSelectCert == "yes")
                        {
                            forceSelectCert = true;
                        }
                    }
                }
                catch { };
                try
                {
                    if (payload.forcePINRquest != null)
                    {
                        string strforcePINRquest = payload.forcePINRquest;
                        strforcePINRquest = strforcePINRquest.ToLower();
                        if (strforcePINRquest == "true" || strforcePINRquest == "yes")
                        {
                            forcePINRquest = true;
                        }
                    }
                }
                catch { };
                if (selectedCert == null || forceSelectCert || newSession) selectCert(payload);
                log4net.NDC.Pop();
                log4net.NDC.Push("init Signer");
                Signer si = new Signer(selectedCert.certificate);
                si.forceClearPINCache = newSession | forcePINRquest;
                try
                {
                    if (payload.hashAlgorithm != null) si.HashAlgorithm = payload.hashAlgorithm;
                }
                catch { };
                try
                {
                    if (payload.contentType != null) si.contentType = payload.contentType;
                }
                catch { }
                try
                {
                    if (payload.protectedPin != null) si.setProtectedPin((string)payload.protectedPin);
                }
                catch (Exception e)
                {
                    log.Error("setPin", e);
                }

                if (multisign)
                {
                    log4net.NDC.Push("multisign");
                    JArray signatures = new JArray();
                    foreach (string tmpCont in contents)
                    {
                        signatures.Add(si.sign(tmpCont));
                    }
                    objResp.Add("signatures", signatures);
                }
                else {
                    log4net.NDC.Push("sign");
                    objResp.Add("signature", si.sign(content));
                }
                var arrChain = new JArray(selectedCert.getChain());

                objResp.Add("status", "ok");
                objResp.Add("reasonCode", 200);
                objResp.Add("reasonText", "Signature generated");

                objResp.Add("signatureAlgorithm", si.HashAlgorithm + "withRSA");
                objResp.Add("chain", arrChain);
            }
            catch (Exception e)
            {
                selectedCert = null;
                objResp.Add("status", "failed");
                objResp.Add("reasonCode", 500);
                objResp.Add("reasonText", e.Message);
                log.Error("sign error", e);
            }
            return objResp.ToString();
        }
        [RestAction("validate", "Validate signature", Methods = "POST")]
        public string Validate(dynamic payload)
        {
            var objResp = new JObject();
            objResp.Add("version", version);
            try
            {
                string content = payload.content;
                string signature = payload.signature;
                JArray arr = payload.chain;
                string cert = arr[0].ToString();
                Signer si = new Signer(cert);
                try
                {
                    string signatureAlgorithm = "SHA1withRSA";
                    if (payload.signatureAlgorithm != null) signatureAlgorithm = payload.signatureAlgorithm;
                    si.HashAlgorithm = signatureAlgorithm.Replace("withRSA", string.Empty);
                }
                catch { };
                try
                {
                    if (payload.contentType != null) si.contentType = payload.contentType;
                }
                catch { }
                bool res = si.verify(content, signature);
                objResp.Add("status", "ok");
                objResp.Add("reasonCode", 200);
                objResp.Add("result", res);
            }
            catch (Exception e)
            {
                selectedCert = null;
                objResp.Add("status", "failed");
                objResp.Add("reasonCode", 500);
                objResp.Add("reasonText", e.Message);
                Console.WriteLine(e.Message + "\n" + e.StackTrace + "\n" + e.Source);
            }
            return objResp.ToString();

        }
        [RestAction("certinfo")]
        public string certinfo(dynamic payload)
        {
            var objResp = new JObject();
            objResp.Add("version", version);
            try
            {
                string cert = "";
                string profiles = "base";
                if (payload.certX509 != null) cert = payload.certX509;
                try
                {
                    if (payload.profile != null) profiles = payload.profile;
                }
                catch { }
                Profiles profile = CertInfo.getProfile(profiles);
                CertInfo certinfo = new CertInfo(profile);

                return certinfo.getCertInfo(cert).getJsonStr();

            }
            catch (Exception e)
            {
                selectedCert = null;
                objResp.Add("status", "failed");
                objResp.Add("reasonCode", 500);
                objResp.Add("reasonText", e.Message);
                Console.WriteLine(e.Message + "\n" + e.StackTrace + "\n" + e.Source);
            }
            return objResp.ToString();
        }
        public string ProtectPinDemo()
        {
            return "{res:true}";
        }
        [RestAction("ProtectPin", Protect = true)]
        public string ProtectPin(dynamic payload)
        {
            var objResp = new JObject();
            objResp.Add("version", version);
            try
            {
                string pin = payload.pin;
                objResp.Add("protectedPin", CryptData.EncryptUserString(pin));
            }
            catch (Exception e)
            {
                selectedCert = null;
                objResp.Add("status", "failed");
                objResp.Add("reasonCode", 500);
                objResp.Add("reasonText", e.Message);
                Console.WriteLine(e.Message + "\n" + e.StackTrace + "\n" + e.Source);
            }
            return objResp.ToString();
        }
        [RestAction("version")]
        public string Version()
        {
            string resp = "{\"version\": \"1.0\",\"httpMethods\": \"GET, POST\",\"contentTypes\": \"data, digest\",\"signatureTypes\": \"signature\",\"selectorAvailable\": true,\"hashAlgorithms\": \"SHA1, SHA256, SHA384, SHA512\"}";
            return resp;
        }
        public string unkonwAction(string action)
        {
            /*
            var objResp = new JObject();
            objResp.Add("version", "1.0");
            objResp.Add("status", "failed");
            objResp.Add("reasonCode", 404);
            objResp.Add("reasonText", "Actions " + action + " not exist");
            var objRespActions = new JObject();
            objRespActions.Add("SelectCert", "Select certificate for signer");
            objRespActions.Add("Sign", "Signnig contents");
            objRespActions.Add("Certs", "Get list certs");
            objRespActions.Add("Version", "The version check SCS");
            objRespActions.Add("validate", "Validate signature");
            objResp.Add("Accept-Action", objRespActions);
            return objResp.ToString();
            */

            var objResp = new JObject();
            objResp.Add("version", "1.0");
            objResp.Add("status", "failed");
            objResp.Add("reasonCode", 404);
            objResp.Add("reasonText", "Actions " + action + " not exist");
            JArray jacction = new JArray();
            foreach (MethodInfo method in (typeof(SignerService)).GetMethods())
            {
                if (RestActionAttribute.IsRestActionAttribute(method))
                {

                    RestActionAttribute restAction = RestActionAttribute.getRestActionAttribute(method);
                    var objRespActions = new JObject();
                    objRespActions.Add("Name", restAction.Name);
                    objRespActions.Add("Desc", restAction.Desc);
                    objRespActions.Add("Methods", restAction.Methods);
                    if (restAction.Protect) objRespActions.Add("Protect", "Only SSL");
                    jacction.Add(objRespActions);
                }
            }
            objResp.Add("Accept-Actions", jacction);
            return objResp.ToString();
        }
    }
}
