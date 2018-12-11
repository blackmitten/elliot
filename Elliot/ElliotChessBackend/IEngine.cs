using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Blackmitten.Elliot.Backend
{
    public interface IEngine
    {
        void Stop();
        Move GetBestMove(Board board);
        //        void Move(string move);
    }
}
