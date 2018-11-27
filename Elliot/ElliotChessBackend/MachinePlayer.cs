using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Blackmitten.Elliot.Backend
{
    public class MachinePlayer:IPlayer
    {
        public MachinePlayer(bool white)
        {
            White = white;
        }

        public bool Human => false;

        public bool White { get; }

        public void Play()
        {
            throw new NotImplementedException();
        }
    }
}
