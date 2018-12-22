using System;
using System.Collections.Generic;
using System.Text;

namespace Blackmitten.Menzel
{
    public class LogBuilder
    {
        public static ILog Build()
        {
            return new Log();
        }
    }
}
