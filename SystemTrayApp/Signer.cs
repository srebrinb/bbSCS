using System;
using System.Runtime.InteropServices;
using System.Security;
using System.Security.Cryptography;
using System.Security.Cryptography.X509Certificates;

namespace Html5WebSCSTrayApp
{
    class Signer
    {
        X509Certificate2 cert;
        private string hashAlgorithm = "SHA1";
        public string contentType = "digest";
        SecureString securePwd = null;
        private HashAlgorithm hasher = new SHA1Managed();
        public bool forceClearPINCache = true;
        public string signatureAlgorithm;
        internal IntPtr hWndCaller=IntPtr.Zero;

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
        public Signer(string strCert)
        {
            this.cert = new X509Certificate2(System.Convert.FromBase64String(strCert));
        }
        public Signer(X509Certificate2 cert)
        {
            this.cert = cert;
        }
        public void setProtectedPin(string protectPW)
        {

            setPin(CryptData.DecryptUserString(protectPW).ToCharArray());
        }
        public void setPin(SecureString pw)
        {
            securePwd = pw;
        }
        public void setPin(char[] pw)
        {
            securePwd = new SecureString();
            for (int i = 0; i < pw.Length; i++)
            {
                securePwd.AppendChar(pw[i]);
            }
        }
        private byte[] processContent(string content)
        {
            byte[] res = new byte[0];
            switch (contentType.ToLower())
            {
                case "digest":
                    res = System.Convert.FromBase64String(content);
                    break;
                case "data":
                    res = hasher.ComputeHash(System.Convert.FromBase64String(content));
                    break;
            }
            return res;
        }
        [DllImport("Advapi32.dll", SetLastError = true)]
        public static extern bool CryptSetProvParam(IntPtr hProv, uint dwParam, IntPtr pvData, uint dwFlags);

        [DllImport("advapi32.dll", CharSet = CharSet.Auto, SetLastError = true)]
        public static extern bool CryptAcquireContext(ref IntPtr hProv,
            string pszContainer, string pszProvider, uint dwProvType, uint dwFlags);

        [DllImport("advapi32.dll", CharSet = CharSet.Auto, SetLastError = true)]
        public static extern void CryptReleaseContext(IntPtr hProv, uint dwFlags);

        static public bool ClearPINCache2(RSACryptoServiceProvider key)
        {
            const uint PP_KEYEXCHANGE_PIN = 32;
            const uint PP_SIGNATURE_PIN = 33;
            bool bretval = false;

            IntPtr hProv = IntPtr.Zero;

            if (CryptAcquireContext(ref hProv, key.CspKeyContainerInfo.KeyContainerName,
                key.CspKeyContainerInfo.ProviderName, (uint)key.CspKeyContainerInfo.ProviderType, 0))
            {
                if ((CryptSetProvParam(hProv, PP_KEYEXCHANGE_PIN, IntPtr.Zero, 0) == true) &&
                    (CryptSetProvParam(hProv, PP_SIGNATURE_PIN, IntPtr.Zero, 0) == true))
                {
                    bretval = true;
                }
            }

            if (hProv != IntPtr.Zero)
            {
                CryptReleaseContext(hProv, 0);
                hProv = IntPtr.Zero;
            }

            return bretval;
        }
        public string sign(string content)
        {
            if (content.Trim().Equals(string.Empty))
            {
                throw new Exception("Not exist 'content' or 'contents'");
            }
            byte[] hashContent = processContent(content);

            RSACryptoServiceProvider csp = (RSACryptoServiceProvider)cert.PrivateKey;
            CspKeyContainerInfo cspKeyContainerInfo = csp.CspKeyContainerInfo;
            CspParameters cspParametersTmp = new CspParameters();
            cspParametersTmp.KeyContainerName = cspKeyContainerInfo.KeyContainerName;
            cspParametersTmp.ProviderType = cspKeyContainerInfo.ProviderType;
            cspParametersTmp.ProviderName = cspKeyContainerInfo.ProviderName;
            if (securePwd != null)
            {
                cspParametersTmp.KeyPassword = securePwd;
            }
            cspParametersTmp.Flags = CspProviderFlags.UseUserProtectedKey;
            /*
            if (hWndCaller != IntPtr.Zero)
            {
                cspParametersTmp.ParentWindowHandle= hWndCaller;
            }
            */
            csp.Clear();
            RSACryptoServiceProvider rsaSignProvider = new RSACryptoServiceProvider(cspParametersTmp);
            //RSACryptoServiceProvider rsaSignProvider = (RSACryptoServiceProvider)cert.PrivateKey;
            if (forceClearPINCache && securePwd == null) ClearPINCache2(rsaSignProvider);
            byte[] sig = rsaSignProvider.SignHash(hashContent, CryptoConfig.MapNameToOID(HashAlgorithm));
            signatureAlgorithm = rsaSignProvider.SignatureAlgorithm;
            rsaSignProvider.Clear();
            rsaSignProvider.Dispose();
            Properties.Settings setting = new Properties.Settings();
            if (setting.PINCache) ClearPINCache2(rsaSignProvider);
            return System.Convert.ToBase64String(sig);
        }
        public bool verify(string content, string signature)
        {
            byte[] hashedData = null;
            byte[] signatureBytes = null;
            try
            {
                hashedData = processContent(content);
            }
            catch (Exception e)
            {
                throw new Exception400("Wrong content.", e);
            }
            try
            {
                signatureBytes = System.Convert.FromBase64String(signature);
            }
            catch (Exception e)
            {
                throw new Exception400("Wrong signature.", e);
            }
            RSACryptoServiceProvider rsaVerifyProvider = (RSACryptoServiceProvider)cert.PublicKey.Key;
            return rsaVerifyProvider.VerifyHash(hashedData, CryptoConfig.MapNameToOID(HashAlgorithm), signatureBytes);
        }


    }
}
