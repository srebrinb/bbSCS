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
            if (args.Length ==0)
            {
                Console.Error.WriteLine("PKCS11CertList.exe <PATH to PKCS11 module DLL> or <Directory PKCS11 modules DLLs>");
                Console.Error.WriteLine("Result StdOut >out.json  Error SrdErr 2>");
                return;
            }
            SmartCardSign.PKCS11CertUtils certUtils = new SmartCardSign.PKCS11CertUtils();
            certUtils.getCerts(args);

            Console.Write(certUtils.getResult());
            Console.Error.Write(certUtils.getErrors());
           // Console.WriteLine("Press Enter");
           // Console.ReadLine();
        }
    }
}
