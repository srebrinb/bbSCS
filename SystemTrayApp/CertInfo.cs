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
        IList<Extension>  Extensions=new List<Extension>();
        public X509Certificate2 certificate;
        [DataMember]
        IList<string> chain = new List<string>();
        public CertInfo(X509Certificate2 certificate)
        {
            this.certificate = certificate;
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
                    this.Subject = x509.Subject;
                    this.Issuer = x509.Issuer;
                    this.Thumbprint = x509.Thumbprint;
                    this.SerialNumber = x509.GetSerialNumberString();
                    var extensions = x509.Extensions;

                    foreach (var extension in extensions)
                    {

                        //Extensions += extension.Oid.FriendlyName + " " + BitConverter.ToString(extension.RawData);
                        Extensions.Add(new Extension(extension));
                    }
                }
                i++;
                chain.Add(System.Convert.ToBase64String(x509.GetRawCertData()));
            }
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
