using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Security.Cryptography;
using System.Security.Cryptography.X509Certificates;
using System.Text;

namespace Html5WebSCSTrayApp
{
    [DataContract]
    class Extension
    {
        [DataMember]
        public string Oid;
        [DataMember]
        public string Name;
        [DataMember]
        public string Value;
        [DataMember]
        public string RawValue;
        public Extension(X509Extension ext )
        {
            Oid = ext.Oid.Value;
            Name = ext.Oid.FriendlyName;
            Value = parse(ext);// BitConverter.ToString(ext.RawData).Replace('-',':');
            RawValue = System.Convert.ToBase64String(ext.RawData);
        }
         string parse(X509Extension extension)
        {
            string res = "";
            
            if (extension.Oid.FriendlyName == "Key Usage")
            {
                X509KeyUsageExtension ext = (X509KeyUsageExtension)extension;
                res+=(ext.KeyUsages);
            }

            if (extension.Oid.FriendlyName == "Basic Constraints")
            {
                X509BasicConstraintsExtension ext = (X509BasicConstraintsExtension)extension;
                res += "CertificateAuthority="+(ext.CertificateAuthority)+",";
                res += "HasPathLengthConstraint=" + (ext.HasPathLengthConstraint) + ",";
                res += "PathLengthConstraint=" + (ext.PathLengthConstraint);
            }

            if (extension.Oid.FriendlyName == "Subject Key Identifier")
            {
                X509SubjectKeyIdentifierExtension ext = (X509SubjectKeyIdentifierExtension)extension;
                res += (ext.SubjectKeyIdentifier);
            }
            
            if (extension.Oid.FriendlyName == "Enhanced Key Usage")
            {
                X509EnhancedKeyUsageExtension ext = (X509EnhancedKeyUsageExtension)extension;
                OidCollection oids = ext.EnhancedKeyUsages;
                foreach (Oid oid in oids)
                {
                    res += (oid.FriendlyName + "(" + oid.Value + "),");
                }
                res.Trim(',');
            }
            if (extension.Oid.FriendlyName == "Authority Key Identifier")
            {
                AsnEncodedData asndata = new AsnEncodedData(extension.Oid, extension.RawData);
                string[] tmp = asndata.Format(true).Split('\n');
                res = tmp[0].Replace("KeyID=",string.Empty).Replace(" ", string.Empty).ToUpper().Trim('\r');


            }
            if (res=="")
            {
                AsnEncodedData asndata = new AsnEncodedData(extension.Oid, extension.RawData);
                res = asndata.Format(true);
                res.Replace("\u000d\u000a", ";");
                res.Replace("\r\n", ";");
                Console.WriteLine(res);
                // SHA1 sha1=SHA1.Create();
                //  res= BitConverter.ToString(sha1.ComputeHash(extension.RawData)).Replace("-", string.Empty);

            }
            return res;
        } 
    }
}
