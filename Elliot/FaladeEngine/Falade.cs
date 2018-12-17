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
            Thread.Sleep(200);
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
            var moves = board.GetAllMoves();
            if (moves.Count == 0)
            {
                return null;
            }
            int moveIndex = StaticRandom.Instance.Next % moves.Count;
            return moves[moveIndex];
        }

        public string Name { get; } = "Falade";

        public void Move(string move) => throw new NotImplementedException();
        public void Shutdown() => throw new NotImplementedException();
    }

}
