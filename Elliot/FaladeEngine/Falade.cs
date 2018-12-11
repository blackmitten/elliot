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

            IEnumerable<IPiece> pieces;
            if (board.WhitesTurn)
            {
                pieces = board.WhitePieces;
            }
            else
            {
                pieces = board.BlackPieces;
            }
            foreach (var piece in pieces)
            {
                if (piece.White == board.WhitesTurn)
                {
                    Square s;
                    do
                    {
                        s = Square.Random;

                    } while (board.GetPieceOnSquare(s) != null);
                    move = new Move(board, piece.Pos, s);
                    break;
                }
            }
            return move;
        }

        public void Move(string move) => throw new NotImplementedException();
        public void Shutdown() => throw new NotImplementedException();
    }

}
