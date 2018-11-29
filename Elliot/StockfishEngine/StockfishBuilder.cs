using Blackmitten.Elliot.Backend;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BlackMitten.Elliot.StockfishEngine
{
    public static class StockfishBuilder
    {
        public static IEngine Build(string filename)
        {
            return new Stockfish(filename);
        }
    }
}
