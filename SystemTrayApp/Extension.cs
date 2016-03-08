using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Security.Cryptography.X509Certificates;
using System.Text;

namespace SystemTrayApp
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
            Value = BitConverter.ToString(ext.RawData).Replace('-',':');
            RawValue = System.Convert.ToBase64String(ext.RawData);
        }
    }
}
