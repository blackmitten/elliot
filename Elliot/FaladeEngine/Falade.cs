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

        public Move GetBestMove(Board board, bool doDiags)
        {
            var moves = board.GetAllMoves();
            if (moves.Count == 0)
            {
                return null;
            }
            double maxScore = double.MinValue;
            double minScore = double.MaxValue;
            Move maxScoreMove = null;
            Move minScoreMove = null;
            foreach (var m in moves)
            {
                double score;
                if (doDiags)
                {
                    string fenBefore = board.GetFenString();
                    board.Move(m, true);
                    score = board.CalculateWhitesScore();
                    string fenAfter = board.GetFenString();
                    board.UndoLastmove();
                    string fenAfterUndo = board.GetFenString();
                    Diags.Assert(fenAfterUndo == fenBefore);
                }
                else
                {
                    board.Move(m, true);
                    score = board.CalculateWhitesScore();
                    board.UndoLastmove();
                }
                if (score > maxScore)
                {
                    maxScore = score;
                    maxScoreMove = m;
                }
                else if (score < minScore)
                {
                    minScore = score;
                    minScoreMove = m;
                }
            }
            if (board.WhitesTurn)
            {
                return maxScoreMove;
            }
            else
            {
                return minScoreMove;
            }
        }

        public string Name { get; } = "Falade";

        public void Move(string move) => throw new NotImplementedException();
        public void Shutdown() => throw new NotImplementedException();
    }

}
