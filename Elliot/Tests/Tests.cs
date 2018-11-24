using BlackMitten.Elliot.Engine;
using BlackMitten.Elliot.StockfishEngine;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace BlackMitten.Elliot.Tests
{
    class Program
    {
        static void Main( string[ ] args )
        {
            string stockfishFilename = args[ 0 ];
            Test1( stockfishFilename );

            Console.WriteLine("Tests complete");
            Console.ReadLine();
        }

        private static void Test1( string stockfishFileName )
        {
            IEngine stockfish = StockfishBuilder.Build( stockfishFileName );

            string moves = "";
            for (;;)
            {
                string bestMove = stockfish.GetBestMove();
                Console.WriteLine(bestMove);
                moves += " " + bestMove;
                stockfish.Move(moves);
            }


            stockfish.Stop();
            //            Thread.Sleep(10000);
        }
    }
}
