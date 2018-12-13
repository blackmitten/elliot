using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Blackmitten.Elliot.Backend
{
    public class StaticRandom
    {
        Random _random = new Random(28091978);
        static StaticRandom _instance;

        private StaticRandom()
        {
        }

        public static StaticRandom Instance
        {
            get
            {
                if (_instance == null)
                {
                    _instance = new StaticRandom();
                }
                return _instance;
            }
        }

        public int Next => _random.Next();
    }
}
