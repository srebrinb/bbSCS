using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using System.Text;

namespace Html5WebSCSTrayApp
{
    public class CryptData
    {
        static char padChar = (char)32;
        private byte[] s_aditionalEntropy;
        public CryptData(string Entropy)
        {
            byte[] s_aditionalEntropy = System.Convert.FromBase64String(Entropy);
        }
        public string ProtectString(string strData)
        {
            
            byte[] toEncrypt = UnicodeEncoding.ASCII.GetBytes(strData);
            return System.Convert.ToBase64String(Protect(toEncrypt));
        }
        public string UnProtectString(string strData)
        {
            byte[] toDecrypt = System.Convert.FromBase64String(strData);
            return UnicodeEncoding.ASCII.GetString(Unprotect(toDecrypt));
        }
        public static string EncryptUserString(string strData)
        {
            //TODO Fix padding data          
            strData = strData.PadRight(16, padChar);
            byte[] toEncrypt = UnicodeEncoding.ASCII.GetBytes(strData);
            EncryptInMemoryData(toEncrypt,MemoryProtectionScope.SameLogon);
            return System.Convert.ToBase64String(toEncrypt);
        }
        public static string DecryptUserString(string strData)
        {
            byte[] toDecrypt = System.Convert.FromBase64String(strData);
            DecryptInMemoryData(toDecrypt, MemoryProtectionScope.SameLogon);
            return UnicodeEncoding.ASCII.GetString(toDecrypt).Trim(padChar);
        }
        public static void EncryptInMemoryData(byte[] Buffer, MemoryProtectionScope Scope)
        {
            if (Buffer.Length <= 0)
                throw new ArgumentException("Buffer");
            if (Buffer == null)
                throw new ArgumentNullException("Buffer");

            
            // Encrypt the data in memory. The result is stored in the same same array as the original data.
            ProtectedMemory.Protect(Buffer, Scope);
        }

        public static void DecryptInMemoryData(byte[] Buffer, MemoryProtectionScope Scope)
        {
            if (Buffer.Length <= 0)
                throw new ArgumentException("Buffer");
            if (Buffer == null)
                throw new ArgumentNullException("Buffer");


            // Decrypt the data in memory. The result is stored in the same same array as the original data.
            ProtectedMemory.Unprotect(Buffer, Scope);

        }
        public  byte[] Protect(byte[] data)
        {
            try
            {
                // Encrypt the data using DataProtectionScope.CurrentUser. The result can be decrypted
                //  only by the same current user.
                return ProtectedData.Protect(data, s_aditionalEntropy, DataProtectionScope.CurrentUser);
            }
            catch (CryptographicException e)
            {
                Console.WriteLine("Data was not encrypted. An error occurred.");
                Console.WriteLine(e.ToString());
                return null;
            }
        }

        public  byte[] Unprotect(byte[] data)
        {
            try
            {
                //Decrypt the data using DataProtectionScope.CurrentUser.
                return ProtectedData.Unprotect(data, s_aditionalEntropy, DataProtectionScope.CurrentUser);
            }
            catch (CryptographicException e)
            {
                Console.WriteLine("Data was not decrypted. An error occurred.");
                Console.WriteLine(e.ToString());
                return null;
            }
        }
        public static string CreateRandomStr(int lenKey)
        {
            return System.Convert.ToBase64String(CreateRandom(lenKey));
        }
        public static string CreateRandomStr()
        {
            return System.Convert.ToBase64String(CreateRandom());
        }
        public static byte[] CreateRandom()
        {
            return CreateRandom(16);
        }
        public static byte[] CreateRandom(int lenKey)
        {
            // Create a byte array to hold the random value.
            byte[] entropy = new byte[lenKey];

            // Create a new instance of the RNGCryptoServiceProvider.
            // Fill the array with a random value.
            new RNGCryptoServiceProvider().GetBytes(entropy);

            // Return the array.
            return entropy;


        }
    }
}
