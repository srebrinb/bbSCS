using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Html5WebSCSTrayApp
{
    class MyException : Exception
    {
        public int errorcode = 500;
        public MyException()
        {
        }

        public MyException(string message)
        : base(message)
        {
        }

        public MyException(string message, Exception inner)
        : base(message, inner)
        { }
    }
    }
