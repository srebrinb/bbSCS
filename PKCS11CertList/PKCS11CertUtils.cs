using System;
using System.Collections.Generic;
using System.Linq;
using Net.Pkcs11Interop.Common;
using Net.Pkcs11Interop.HighLevelAPI;
using System.Security.Cryptography.X509Certificates;
using Newtonsoft.Json.Linq;
using System.IO;

namespace SmartCardSign
{
    class PKCS11CertUtils
    {
        JArray jsonArrayRes;
        PKCS11CertUtils()
        {
            jsonArrayRes= new JArray();
        }

        public static string getCertsList(string[] pkcs11DLLs)
        {
            JArray jsonArrayOut = new JArray();
            foreach (string pkcs11DLL in pkcs11DLLs)
            {

                JArray jsonArrayTmp = getCertsListJSON(pkcs11DLL);
                for (int i = 0; i < jsonArrayTmp.Count; i++)
                {
                    jsonArrayOut.Add(jsonArrayTmp[i]);
                }
            }
            return jsonArrayOut.ToString();
        }
        public static string getCertsList(string pkcs11DLLPath)
        {
            JArray jsonArrayOut = new JArray();
            if (File.Exists(pkcs11DLLPath))
            {
                  jsonArrayOut = getCertsListJSON(pkcs11DLLPath);
            }
            else if (Directory.Exists(pkcs11DLLPath))
            {
                string[] fileEntries = Directory.GetFiles(pkcs11DLLPath);
                return getCertsList(fileEntries);


            }
                return jsonArrayOut.ToString();
        }

        public int addCertFromPKCS11(string pkcs11DLL)
        {
            JArray jsonArrayOut = new JArray();
            CertInfo certInfo = new CertInfo();
            Pkcs11 pkcs11 = null;
            int returnedCerts = 0;
            try
            {
                try
                {
                    pkcs11 = new Pkcs11(pkcs11DLL, false);
                }
                catch
                {
                    pkcs11 = null;
                    return returnedCerts;
                }
                List<Slot> slots = pkcs11.GetSlotList(false);
                foreach (Slot slot in slots)
                {
                    TokenInfo tokenInfo;
                    try
                    {
                        tokenInfo = slot.GetTokenInfo();
                    }
                    catch
                    {
                        continue;
                    }
                    using (Session session = slot.OpenSession(false))
                    {

                        Dictionary<string, X509Certificate2> Certificates = new Dictionary<string, X509Certificate2>();
                        List<ObjectAttribute> template = new List<ObjectAttribute>();
                        template.Add(new ObjectAttribute(CKA.CKA_CLASS, CKO.CKO_CERTIFICATE));
                        template.Add(new ObjectAttribute(CKA.CKA_CERTIFICATE_TYPE, CKC.CKC_X_509));
                        List<ObjectHandle> oObjCollection = session.FindAllObjects(template);
                        byte[] ckaId = null;
                        foreach (var item in oObjCollection)
                        {
                            var oAttriVal = session.GetAttributeValue(item, new List<CKA>() { CKA.CKA_VALUE }).FirstOrDefault();
                            var oAttriKey = session.GetAttributeValue(item, new List<CKA>() { CKA.CKA_ID }).FirstOrDefault();

                            var ckaLabel = session.GetAttributeValue(item, new List<CKA>() { CKA.CKA_LABEL }).FirstOrDefault();
                            //  Certificates[oAttriKey.GetValueAsString()] = new X509Certificate2(oAttriVal.GetValueAsByteArray());
                            JObject jobj = JObject.FromObject(certInfo.getCertInfo(new X509Certificate2(oAttriVal.GetValueAsByteArray())));

                            JObject jTokenInfo = new JObject();
                            jTokenInfo.Add("SlotID", tokenInfo.SlotId);
                            jTokenInfo.Add("ckaLabel", ckaLabel.GetValueAsString());
                            jTokenInfo.Add("Label", tokenInfo.Label);
                            jTokenInfo.Add("ManufacturerId", tokenInfo.ManufacturerId);
                            jTokenInfo.Add("Model", tokenInfo.Model);
                            jTokenInfo.Add("SerialNumber", tokenInfo.SerialNumber);
                            jobj.Add("tokenInfo", jTokenInfo);
                            //Console.WriteLine(tokenInfo.SlotId + ":" + ckaLabel.GetValueAsString() + " - " + Certificates[oAttriKey.GetValueAsString()].Thumbprint);
                            jsonArrayOut.Add(jobj);
                            returnedCerts++;
                        }
                    }
                    slot.CloseAllSessions();
                }
            }
            catch (Exception e)
            {
                JObject error = JObject.FromObject(e);
                jsonArrayOut.Add(error);
            }
            finally
            {
                if (pkcs11 != null) pkcs11.Dispose();
            }

            return returnedCerts;
        }
        public static JArray getCertsListJSON(string pkcs11DLL)
        {

            JArray jsonArrayOut = new JArray() ;
            CertInfo certInfo = new CertInfo();
            Pkcs11 pkcs11 = null;
            try
            {
                try
                {
                    pkcs11 = new Pkcs11(pkcs11DLL, false);
                }
                catch
                {
                    pkcs11 = null;
                    return jsonArrayOut;
                }
                List<Slot> slots = pkcs11.GetSlotList(false);
                foreach (Slot slot in slots)
                {
                    TokenInfo tokenInfo;
                    try {
                        tokenInfo = slot.GetTokenInfo();
                    }
                    catch
                    {
                        continue;
                    }
                    using (Session session = slot.OpenSession(false))
                    {

                        Dictionary<string, X509Certificate2> Certificates = new Dictionary<string, X509Certificate2>();
                        List<ObjectAttribute> template = new List<ObjectAttribute>();
                        template.Add(new ObjectAttribute(CKA.CKA_CLASS, CKO.CKO_CERTIFICATE));
                        template.Add(new ObjectAttribute(CKA.CKA_CERTIFICATE_TYPE, CKC.CKC_X_509));
                        List<ObjectHandle> oObjCollection = session.FindAllObjects(template);
                        byte[] ckaId = null;
                        foreach (var item in oObjCollection)
                        {
                            var oAttriVal = session.GetAttributeValue(item, new List<CKA>() { CKA.CKA_VALUE }).FirstOrDefault();
                            var oAttriKey = session.GetAttributeValue(item, new List<CKA>() { CKA.CKA_ID }).FirstOrDefault();

                            var ckaLabel = session.GetAttributeValue(item, new List<CKA>() { CKA.CKA_LABEL }).FirstOrDefault();
                          //  Certificates[oAttriKey.GetValueAsString()] = new X509Certificate2(oAttriVal.GetValueAsByteArray());
                            JObject jobj = JObject.FromObject(certInfo.getCertInfo(new X509Certificate2(oAttriVal.GetValueAsByteArray())));
                           
                            JObject jTokenInfo =new JObject();
                            jTokenInfo.Add("SlotID", tokenInfo.SlotId);
                            jTokenInfo.Add("ckaLabel", ckaLabel.GetValueAsString());
                            jTokenInfo.Add("Label", tokenInfo.Label);
                            jTokenInfo.Add("ManufacturerId", tokenInfo.ManufacturerId);
                            jTokenInfo.Add("Model", tokenInfo.Model);
                            jTokenInfo.Add("SerialNumber", tokenInfo.SerialNumber);
                            jobj.Add("tokenInfo", jTokenInfo);
                            //Console.WriteLine(tokenInfo.SlotId + ":" + ckaLabel.GetValueAsString() + " - " + Certificates[oAttriKey.GetValueAsString()].Thumbprint);
                            jsonArrayOut.Add(jobj);
                        }
                    }
                    slot.CloseAllSessions();
                }
            }
            catch(Exception e)
            {
                JObject error =  JObject.FromObject (e);
                jsonArrayOut.Add(error);
            }
            finally
            {
                if (pkcs11 != null) pkcs11.Dispose();
            }

            
            return jsonArrayOut;
        }
        public static void Pkcs11CertList()
        {
            using (Pkcs11 pkcs11 = new Pkcs11(@"C:\work\PKCS11\idprimepkcs11.dll", true))
            {
                List<Slot> slots = pkcs11.GetSlotList(true);

                foreach (Slot slot in slots)
                {
                    TokenInfo tokenInfo = slot.GetTokenInfo();
                    Console.WriteLine(tokenInfo.HardwareVersion);
                    Console.WriteLine(tokenInfo.ManufacturerId);
                    Console.WriteLine(tokenInfo.SlotId);
                    Console.WriteLine(tokenInfo.SerialNumber);
                    using (Session session = slot.OpenSession(false))
                    {

                        Dictionary<string, X509Certificate2> Certificates = new Dictionary<string, X509Certificate2>();
                        List<ObjectAttribute> template = new List<ObjectAttribute>();
                        template.Add(new ObjectAttribute(CKA.CKA_CLASS, CKO.CKO_CERTIFICATE));
                        template.Add(new ObjectAttribute(CKA.CKA_CERTIFICATE_TYPE, CKC.CKC_X_509));
                        List<ObjectHandle> oObjCollection = session.FindAllObjects(template);
                        byte[] ckaId = null;
                        foreach (var item in oObjCollection)
                        {
                            var oAttriVal = session.GetAttributeValue(item, new List<CKA>() { CKA.CKA_VALUE }).FirstOrDefault();
                            var oAttriKey = session.GetAttributeValue(item, new List<CKA>() { CKA.CKA_ID }).FirstOrDefault();
                            var ckaLabel = session.GetAttributeValue(item, new List<CKA>() { CKA.CKA_LABEL }).FirstOrDefault();
                            Certificates[oAttriKey.GetValueAsString()] = new X509Certificate2(oAttriVal.GetValueAsByteArray());
                        }
                        session.CloseSession();
                    }

                }
            }
        }
    }
}
