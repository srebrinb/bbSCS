using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Html5WebSCSTrayApp
{
    class Exception500 : MyException
    {
       
        public Exception500()
        {
            errorcode = 500;
        }

        public Exception500(string message)
        : base(message)
        {
            errorcode = 500;
        }

        public Exception500(string message, Exception inner)
        : base(message, inner)
        {
            errorcode = 500;
        }
    }
}
