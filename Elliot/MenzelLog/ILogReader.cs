using System;
using System.Collections.Generic;
using System.Text;

namespace Blackmitten.Menzel
{
    public interface ILogReader
    {
        IList<string> Read();
    }
}
