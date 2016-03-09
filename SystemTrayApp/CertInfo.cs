using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.Serialization;
using System.Runtime.Serialization.Json;
using System.Security.Cryptography.X509Certificates;
using System.Text;

namespace SystemTrayApp
{
    [DataContract]
    class CertInfo
    {
        [DataMember]
        public string Subject;
        [DataMember]
        public string SerialNumber;
        [DataMember]
        public string Issuer;
        [DataMember]
        public string Thumbprint;
        [DataMember]
        public DateTime DateTimeNotAfter;
        [DataMember]
        public DateTime DateTimeNotBefore;
        [DataMember]
        IList<Extension>  Extensions=new List<Extension>();
        public X509Certificate2 certificate;
        [DataMember]
        public IList<string> chain = new List<string>();
        [DataMember]
        public bool Valid;
        /*profiles:
        1) Base
        2) + Extensions
        4) + Chain
        default 5) Base + Chain
        */

        private int profile=5;
        public CertInfo() { }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="profile">
        /// 1) Base
        /// 2) + Extensions
        /// 4) + Chain
        /// default 5) Base + Chain
        /// </param>
        public CertInfo(int profile)
        {
            this.profile = profile;
        }
        public CertInfo getCertInfo(X509Certificate2 certificate)
        {
            CertInfo resCert = new CertInfo();
            resCert.certificate = certificate;
            X509Chain ch = new X509Chain();
            ch.Build(certificate);
            var chainElements = ch.ChainElements;
            X509ChainElementEnumerator enumerator = chainElements.GetEnumerator();
            int i = 1;

            while (enumerator.MoveNext())
            {
                var x509ChainElement = enumerator.Current;
                var x509 = x509ChainElement.Certificate;
                if (i == 1)
                {
                    resCert.Subject = x509.Subject;
                    resCert.Issuer = x509.Issuer;
                    resCert.Thumbprint = x509.Thumbprint;
                    resCert.SerialNumber = x509.GetSerialNumberString();
                    
                    resCert.DateTimeNotBefore = x509.NotBefore;
                    resCert.DateTimeNotAfter = x509.NotAfter;
                    resCert.Valid = x509.Verify();
                    if ((profile & 2) == 2) {
                        var extensions = x509.Extensions;
                        foreach (var extension in extensions)
                        {
                            resCert.Extensions.Add(new Extension(extension));
                        }
                    }
                }
                i++;
                if ((profile & 4) == 4)
                {
                    resCert.chain.Add(System.Convert.ToBase64String(x509.GetRawCertData()));
                }
            }
            return resCert;
        }
        
        public JObject getJson()
        {
            return JObject.FromObject(this);


        }
        public string getJsonStr()
        {
            MemoryStream stream1 = new MemoryStream();
            DataContractJsonSerializer ser = new DataContractJsonSerializer(typeof(CertInfo));
            ser.WriteObject(stream1, this);
            stream1.Position = 0;
            StreamReader sr = new StreamReader(stream1);
            string json = sr.ReadToEnd();
            return json;
        }
      
        public string[] getChain()
        {
            return chain.ToArray();
        }
    }
}
