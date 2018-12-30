using Blackmitten.Elliot.Backend;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ElliotTools
{
    class Program
    {
        static void Main(string[] args)
        {
            string fen = args[0];
            Console.WriteLine(fen);
            Board board = BoardFactory.BoardFromFenString(fen);
            Console.WriteLine(board.ToLongString());
        }
    }
}
