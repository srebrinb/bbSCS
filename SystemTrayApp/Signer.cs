using System;
using System.Collections.Generic;
using System.Linq;
using System.Security;
using System.Security.Cryptography;
using System.Security.Cryptography.X509Certificates;
using System.Text;

namespace SystemTrayApp
{
    class Signer
    {
        X509Certificate2 cert;
        public string hashAlgorithm="SHA1";
        public string contentType = "digest";
        SecureString securePwd = null;
        public Signer(X509Certificate2 cert)
        {
            this.cert = cert;
        }
        public void setPin(SecureString pw)
        {
            securePwd = pw;
        }
        public void setPin(char[] pw)
        {
            securePwd = new SecureString();
            for (int i=0;i<pw.Length;i++)
            {
                securePwd.AppendChar(pw[i]);
            }
        }
        public string sign(string content)
        {
            byte[] hash=null;
            if (contentType.Equals("digest"))
            {
                hash= System.Convert.FromBase64String(content);
            }
            if (contentType.Equals("data"))
            {
                if (hashAlgorithm.Equals("SHA1")) {
                    SHA1Managed sha = new SHA1Managed();
                    hash = sha.ComputeHash(System.Convert.FromBase64String(content));
                }
                if (hashAlgorithm.Equals("SHA256"))
                {
                    SHA256Managed sha = new SHA256Managed();
                    hash = sha.ComputeHash(System.Convert.FromBase64String(content));
                }
                if (hashAlgorithm.Equals("SHA384"))
                {
                    SHA384Managed sha = new SHA384Managed();
                    hash = sha.ComputeHash(System.Convert.FromBase64String(content));
                }
                if (hashAlgorithm.Equals("SHA512"))
                {
                    SHA512Managed sha = new SHA512Managed();
                    hash = sha.ComputeHash(System.Convert.FromBase64String(content));
                }
            }
            RSACryptoServiceProvider csp = (RSACryptoServiceProvider)cert.PrivateKey;
            CspKeyContainerInfo cspKeyContainerInfo = csp.CspKeyContainerInfo;
            CspParameters cspParametersTmp = new CspParameters();
            cspParametersTmp.KeyContainerName = cspKeyContainerInfo.KeyContainerName;
            //cspParametersTmp.KeyNumber = KeyNumber.Signature;
            cspParametersTmp.ProviderType = cspKeyContainerInfo.ProviderType;
            cspParametersTmp.ProviderName = cspKeyContainerInfo.ProviderName;
            if (securePwd!=null) {
                 cspParametersTmp.KeyPassword = securePwd;
            }
            RSACryptoServiceProvider rsaSignProvider = new RSACryptoServiceProvider(cspParametersTmp);

            byte[] sig = rsaSignProvider.SignHash(hash, CryptoConfig.MapNameToOID(hashAlgorithm));
            return System.Convert.ToBase64String(sig);
        }

    }
}
