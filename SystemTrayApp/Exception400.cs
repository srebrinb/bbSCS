using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Html5WebSCSTrayApp
{
    class Exception400: MyException
    {
        
        public Exception400()
        {
            errorcode = 400;
        }

        public Exception400(string message)
        : base(message)
        {
            errorcode = 400;
        }

        public Exception400(string message, Exception inner)
        : base(message, inner)
        { errorcode = 400; }
    }
}
