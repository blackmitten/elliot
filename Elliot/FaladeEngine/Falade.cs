using BlackMitten.Elliot.Engine;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BlackMitten.Elliot.FaladeEngine
{
    
    public class Falade : IEngine
    {
        public void Stop()
        {

        }

        public string GetBestMove()
        {
            throw new NotImplementedException();
        }

        public void Move(string move) => throw new NotImplementedException();
    }
    
}
