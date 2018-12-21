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
        PieceValuer _pieceValuer = new PieceValuer();

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
                    score = Evaluate(board);
                    string fenAfter = board.GetFenString();
                    board.UndoLastmove();
                    string fenAfterUndo = board.GetFenString();
                    Diags.Assert(fenAfterUndo == fenBefore);
                }
                else
                {
                    board.Move(m, true);
                    score = Evaluate( board );
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

        private double Evaluate(Board board)
        {
            return Minimax(board, 2, double.MinValue, double.MaxValue, board.WhitesTurn );
        }

        private double Minimax(Board board, int depth, double minValue, double maxValue, bool maximizing)
        {
            return CalculateWhitesScore(board);
        }

        double CalculateWhitesScore(Board board)
        {
            double score = 0;
            foreach (var piece in board.WhitePieces)
            {
                piece.Accept(_pieceValuer);
                score += _pieceValuer.Value;
            }
            foreach (var piece in board.BlackPieces)
            {
                piece.Accept(_pieceValuer);
                score += _pieceValuer.Value;
            }

            return score;
        }



        public string Name { get; } = "Falade";

        public void Move(string move) => throw new NotImplementedException();
        public void Shutdown() => throw new NotImplementedException();
    }

}
