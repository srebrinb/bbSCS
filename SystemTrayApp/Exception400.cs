using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Html5WebSCSTrayApp
{
    class Exception400: MyException
    {
        public new int errorcode = 404;
        public Exception400()
        {
        }

        public Exception400(string message)
        : base(message)
        {
        }

        public Exception400(string message, Exception inner)
        : base(message, inner)
        { }
    }
}
