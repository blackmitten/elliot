using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Blackmitten.Elliot.Backend
{
    public class HumanPlayer : IPlayer
    {
        public HumanPlayer(bool white)
        {
            White = white;
        }

        public bool White { get; }

        public void Play()
        {

        }

    }
}
