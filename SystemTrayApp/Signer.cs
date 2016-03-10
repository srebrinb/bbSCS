using System.Security;
using System.Security.Cryptography;
using System.Security.Cryptography.X509Certificates;

namespace SystemTrayApp
{
    class Signer
    {
        X509Certificate2 cert;
        private string hashAlgorithm = "SHA1";
        public string contentType = "digest";
        SecureString securePwd = null;
        private HashAlgorithm hasher= new SHA1Managed();

        public string HashAlgorithm
        {
            get
            {
                return hashAlgorithm;
            }

            set
            {
                hashAlgorithm = value.ToUpper();
                switch (HashAlgorithm)
                {
                    case "SHA1":
                        hasher = new SHA1Managed();
                        break;
                    case "SHA256":
                        hasher = new SHA256Managed();
                        break;
                    case "SHA512":
                        hasher = new SHA512Managed();
                        break;
                    case "SHA384":
                        hasher = new SHA384Managed();
                        break;
                }
            }
        }
        public Signer(string strCert) {
            this.cert = new X509Certificate2(System.Convert.FromBase64String(strCert));
        }
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
        private byte[] processContent(string content)
        {
            byte[] res= new byte[0];
            switch (contentType.ToLower())
            {
                case "digest":
                    res= System.Convert.FromBase64String(content);
                    break;
                case "data":
                    res = hasher.ComputeHash(System.Convert.FromBase64String(content));
                    break;
            }
            return res;
        }
        public string sign(string content)
        {
            byte[] hashContent = processContent(content);
            
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

            byte[] sig = rsaSignProvider.SignHash(hashContent, CryptoConfig.MapNameToOID(HashAlgorithm));
            return System.Convert.ToBase64String(sig);
        }
        public bool verify(string content,string signature)
        {
            byte[] hashedData = processContent(content);
            byte[]  signatureBytes = System.Convert.FromBase64String(signature);
            
            RSACryptoServiceProvider rsaVerifyProvider = (RSACryptoServiceProvider)cert.PublicKey.Key;

            return rsaVerifyProvider.VerifyHash(hashedData, CryptoConfig.MapNameToOID(HashAlgorithm), signatureBytes);

        }

    }
}
