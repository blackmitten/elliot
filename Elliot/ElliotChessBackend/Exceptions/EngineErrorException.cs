using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Blackmitten.Elliot.Backend
{
    public class EngineErrorException : ElliotChessException
    {
        public EngineErrorException(string message) : base(message)
        {

        }
    }
}
