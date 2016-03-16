﻿using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;

namespace Html5WebSCSTrayApp
{
    class Utils
    {
        static void ReadTail(string filename)
        {
            using (FileStream fs = File.Open(filename, FileMode.Open, FileAccess.Read, FileShare.ReadWrite))
            {
                // Seek 1024 bytes from the end of the file
                fs.Seek(-1024, SeekOrigin.End);
                // read 1024 bytes
                byte[] bytes = new byte[1024];
                fs.Read(bytes, 0, 1024);
                // Convert bytes to string
                string s = Encoding.Default.GetString(bytes);
                // or string s = Encoding.UTF8.GetString(bytes);
                // and output to console
                Console.WriteLine(s);
            }
        }
    }
}
