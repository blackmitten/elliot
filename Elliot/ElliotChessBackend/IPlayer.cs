using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Blackmitten.Elliot.Backend
{
    public interface IPlayer
    {
        bool Human { get; }
        bool White { get; }
        string Name { get; }
        Move Play(Board board);
        void Kill();
    }
}
