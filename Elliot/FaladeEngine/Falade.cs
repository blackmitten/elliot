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
        public int Depth { get; set; } = 4;

        public Falade(int depth = 4)
        {
            Depth = depth;
        }

        public void Stop()
        {

        }

        public Move GetBestMove(Board originalBoard)
        {
            Board board = new Board(originalBoard);
#if DIAGNOSTIC
            Assert.IsTrue(board.GetFenString() == originalBoard.GetFenString());
#endif
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
                var undo = new Undo();
#if DIAGNOSTIC
                string fenBefore = board.GetFenString();
#endif
                board.Move(m, true, undo);
                score = Evaluate(board);
#if DIAGNOSTIC
                string fenAfter = board.GetFenString();
#endif
                board.UndoLastmove(undo);
#if DIAGNOSTIC
                string fenAfterUndo = board.GetFenString();
                Assert.IsTrue(fenAfterUndo == fenBefore);
#endif
                if (score > maxScore)
                {
                    maxScore = score;
                    maxScoreMove = m;
                }
                if (score < minScore)
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
            return Minimax(board, Depth, double.MinValue, double.MaxValue, board.WhitesTurn);
        }

        private double Minimax(Board board, int depth, double alpha, double beta, bool maximizing)
        {
            if (depth == 0)
            {
                return CalculateWhitesScore(board);
            }

#if DIAGNOSTIC
            string fenBefore = board.GetFenString();
#endif

            var moves = board.GetAllMoves();
            if (maximizing)
            {
                double max = double.MinValue;
                foreach (var move in moves)
                {
                    var undo = new Undo();
                    board.Move(move, true, undo);
                    max = Math.Max(max, Minimax(board, depth - 1, alpha, beta, !maximizing));
                    board.UndoLastmove(undo);
#if DIAGNOSTIC
                    var fenAfterUndo = board.GetFenString();
                    Assert.IsTrue(fenBefore == fenAfterUndo);
#endif
                    alpha = Math.Max(alpha, max);
                    if (alpha >= beta)
                    {
                        break;
                    }
                }
                return max;
            }
            else
            {
                double min = double.MaxValue;
                foreach (var move in moves)
                {
                    var undo = new Undo();
                    board.Move(move, true, undo);
                    min = Math.Min(min, Minimax(board, depth - 1, alpha, beta, !maximizing));
                    board.UndoLastmove(undo);
#if DIAGNOSTIC
                    var fenAfterUndo = board.GetFenString();
                    Assert.IsTrue(fenBefore == fenAfterUndo);
#endif
                    beta = Math.Min(beta, min);
                    if (alpha >= beta)
                    {
                        break;
                    }
                }
                return min;
            }
        }

        double CalculateSidesScore(Board board, bool white)
        {
            double sign = white ? 1 : -1;
            return sign * CalculateWhitesScore(board);
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
            if(board.WhiteInCheck)
            {
                score += 10;
            }
            if(board.BlackInCheck)
            {
                score -= 10;
            }

            return score;
        }



        public string Name { get; } = "Falade";

        public void Move(string move) => throw new NotImplementedException();
        public void Shutdown() => throw new NotImplementedException();
    }

}
