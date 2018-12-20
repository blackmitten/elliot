using Blackmitten.Elliot.Backend;
using Blackmitten.Menzel;
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
            foreach(var m in moves)
            {
                string fenBefore = board.GetFenString();
                board.Move(m, true);
                string fenAfter = board.GetFenString();
                board.UndoLastmove();
                string fenAfterUndo = board.GetFenString();
                Diags.Assert(fenAfterUndo == fenBefore);
            }
            int moveIndex = StaticRandom.Instance.Next % moves.Count;
            return moves[moveIndex];
        }

        public string Name { get; } = "Falade";

        public void Move(string move) => throw new NotImplementedException();
        public void Shutdown() => throw new NotImplementedException();
    }

}
