using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Html5WebSCSTrayApp
{
    class Exception404 : MyException
    {
        public new int errorcode = 404;
        public Exception404()
        {
        }

        public Exception404(string message)
        : base(message)
        {
        }

        public Exception404(string message, Exception inner)
        : base(message, inner)
        { }

    }
}
