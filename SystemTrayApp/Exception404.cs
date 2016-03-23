using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Html5WebSCSTrayApp
{
    class Exception404 : MyException
    {
       
        public Exception404()
        {
            errorcode = 404;
        }

        public Exception404(string message)
        : base(message)
        {
            errorcode = 404;
        }

        public Exception404(string message, Exception inner)
        : base(message, inner)
        {
            errorcode = 404;
        }

    }
}
