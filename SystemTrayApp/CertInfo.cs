﻿using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.Serialization;
using System.Runtime.Serialization.Json;
using System.Security.Cryptography.X509Certificates;
using System.Text;

namespace Html5WebSCSTrayApp
{
    [Flags]
    public enum Profiles
    {
        none = 0,
        @base = 1,
        extensions = 2,
        chain = 4,
        certx509 = 8
    }
    [DataContract]
    class CertInfo
    {
        [DataMember]
        public string subject;
        [DataMember]
        public string serialNumber;
        [DataMember]
        public string issuer;
        [DataMember]
        public string thumbprint;
        [DataMember]
        public string dateTimeNotAfter;
        [DataMember]
        public string dateTimeNotBefore;
        [DataMember(EmitDefaultValue = false)]
        IList<Extension> extensions = new List<Extension>();
        public X509Certificate2 certificate;
        [DataMember(EmitDefaultValue = false)]
        public IList<string> chain = new List<string>();
        [DataMember(EmitDefaultValue = false)]
        public string certX509 = "";
        [DataMember]
        public bool valid;

        /*profiles:
        1) Base
        2) + Extensions
        4) + Chain
        default 5) Base + Chain
        */


        private Profiles profile = Profiles.@base | Profiles.chain;
        public CertInfo() { }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="profile">
        /// 1) Base
        /// 2) + Extensions
        /// 4) + Chain
        /// 8) + CertX509
        /// default 5) Base + Chain
        /// </param>
        public CertInfo(Profiles profile)
        {
            this.profile = profile;
        }
        public CertInfo getCertInfo(string strCert)
        {
            X509Certificate2 certificate = new X509Certificate2(System.Convert.FromBase64String(strCert));
            return getCertInfo(certificate);
        }
        public static Profiles getProfile(string profiles)
        {
            string[] profilesStrings = profiles.Split(',');
            Profiles profile = Profiles.none;
            foreach (string profileString in profilesStrings)
            {
                try
                {

                    Profiles profileValue = (Profiles)Enum.Parse(typeof(Profiles), profileString.ToLower().Trim());
                    if (Enum.IsDefined(typeof(Profiles), profileValue) | profileValue.ToString().Contains(","))
                    {
                        Console.WriteLine("Converted '{0}' to {1}.", profileValue, profileValue.ToString());
                        profile = profile | profileValue;
                    }
                    else
                        Console.WriteLine("{0} is not an underlying value of the Colors enumeration.", profileString);
                }
                catch (ArgumentException)
                {
                    Console.WriteLine("'{0}' is not a member of the Colors enumeration.", profileString);
                }
            }
            return profile;
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
                    resCert.subject = x509.Subject;
                    resCert.issuer = x509.Issuer;
                    resCert.thumbprint = x509.Thumbprint;
                    resCert.serialNumber = x509.GetSerialNumberString();

                    resCert.dateTimeNotBefore = x509.NotBefore.ToString("s");
                    resCert.dateTimeNotAfter = x509.NotAfter.ToString("s"); ;
                    resCert.valid = x509.Verify();
                    if (profile.HasFlag(Profiles.certx509))
                    {
                        resCert.certX509 = System.Convert.ToBase64String(x509.GetRawCertData());
                    }
                    if (profile.HasFlag(Profiles.extensions))
                    {
                        var extensions = x509.Extensions;
                        foreach (var extension in extensions)
                        {

                            resCert.extensions.Add(new Extension(extension));
                        }
                    }
                }
                i++;
                if (profile.HasFlag(Profiles.chain))
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
            if (chain.Count > 0)
            {
                return chain.ToArray();
            }
            else
            {
                string[] certs = { System.Convert.ToBase64String(certificate.GetRawCertData()) };
                return certs;
            }
        }
    }
}
