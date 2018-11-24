using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Blackmitten.Elliot.Backend
{
    public interface IPiece
    {
        Square CurrentPosition { get; set; }
        bool White { get; }
    }
}
