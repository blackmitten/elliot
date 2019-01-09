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
            if (fen.StartsWith("{") && fen.EndsWith("}"))
            {
                fen = fen.Substring(1, fen.Length - 2);
            }
            Board board = BoardFactory.BoardFromFenString(fen);
            Console.WriteLine(board.ToLongString());
        }
    }
}
