using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Html5WebSCSTrayApp
{
    class Exception500 : MyException
    {
        public new int errorcode = 500;
        public Exception500()
        {
        }

        public Exception500(string message)
        : base(message)
        {
        }

        public Exception500(string message, Exception inner)
        : base(message, inner)
        { }
    }
}
