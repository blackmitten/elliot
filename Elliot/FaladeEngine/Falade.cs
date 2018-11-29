using Blackmitten.Elliot.Backend;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace BlackMitten.Elliot.FaladeEngine
{

    public class Falade : IEngine
    {

        public void Stop()
        {

        }

        public Move GetBestMove(Board board)
        {
            Thread.Sleep(1000);
            Move move = null;
            foreach (var piece in board.Pieces)
            {
                if (piece.White == board.WhitesTurn)
                {
                    Square s;
                    do
                    {
                        s = Square.Random;

                    } while (board.GetPieceOnSquare(s) != null);
                    move = new Move(piece.Pos, s);
                    break;
                }
            }
            return move;
        }

        public void Move(string move) => throw new NotImplementedException();
    }

}
