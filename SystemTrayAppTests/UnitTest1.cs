using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace SystemTrayAppTests
{
    [TestClass]
    public class UnitTest1
    {
        [TestMethod]
        public void TestCreateRandomStr()
        {
           string res= Html5WebSCSTrayApp.CryptData.CreateRandomStr();
            Console.WriteLine("CreateRandomStr:"+res);
        }
        [TestMethod]
        public void TestEncryptUserString()
        {
            string resEncrypt = Html5WebSCSTrayApp.CryptData.EncryptUserString("test");
            string res = Html5WebSCSTrayApp.CryptData.DecryptUserString(resEncrypt);
            Console.WriteLine("res:" + res);
        }
        [TestMethod]
        public void TestProtectString()
        {
            Html5WebSCSTrayApp.CryptData cryptData = new Html5WebSCSTrayApp.CryptData("cHVibGlj");

            string resEncrypt = cryptData.ProtectString("test");
            string res = cryptData.UnProtectString (resEncrypt);
            Console.WriteLine("res:" + res);
        }
    }
}
