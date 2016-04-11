using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace PKCS11CertList
{
    class Program
    {
        static void Main(string[] args)
        {

            string res = "";
            if(args.Length>0)
                res = SmartCardSign.PKCS11CertUtils.getCertsList(args); //idprimepkcs11.dll
            else
              res =  SmartCardSign.PKCS11CertUtils.getCertsList(@"C:\work\PKCS11\64"); //idprimepkcs11.dll
            Console.WriteLine(res);
           // Console.WriteLine("Press Enter");
           // Console.ReadLine();
        }
    }
}
