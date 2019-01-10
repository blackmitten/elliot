using Blackmitten.Elliot.Backend;
using Blackmitten.Menzel;
using System;
using System.Collections;
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
        bool _stopping = false;

        public Falade(int depth = 4)
        {
            Depth = depth;
        }

        public void Stop()
        {
            _stopping = true;
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
            SortMoves(board, moves, board.WhitesTurn);
            double maxScore = double.MinValue;
            double minScore = double.MaxValue;
            Move maxScoreMove = moves[0];
            Move minScoreMove = moves[0];
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

        private void SortMoves(Board board, List<Move> moves, bool maximising)
        {
            return;
            moves.Sort((m1, m2) =>
            {
                try
                {
                    double sign = maximising ? -1 : 1;

                    Undo u1 = new Undo();
                    board.Move(m1, true, u1);
                    double score1 = sign * CalculateWhitesScore(board);
                    board.UndoLastmove(u1);

                    Undo u2 = new Undo();
                    board.Move(m2, true, u2);
                    double score2 = sign * CalculateWhitesScore(board);
                    board.UndoLastmove(u2);



                    if (score1 > score2)
                    {
                        return 1;
                    }
                    else if (score2 < score1)
                    {
                        return -1;
                    }
                    else
                    {
                        return 0;
                    }
                }
                catch (Exception ex)
                {
                    throw;
                }
            });
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
            SortMoves(board, moves, board.WhitesTurn);
            if (moves.Count == 0)
            {
                if (board.WhitesTurn)
                {
                    if (board.WhiteInCheck)
                    {
                        return -100000 - depth;
                    }
                    else
                    {
                        return 0;
                    }
                }
                else
                {
                    if (board.BlackInCheck)
                    {
                        return 100000 + depth;
                    }
                    else
                    {
                        return 0;
                    }
                }
            }
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
                    if (alpha >= beta || _stopping)
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
                    if (alpha >= beta || _stopping)
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
            return score;
        }



        public string Name { get; } = "Falade";

    }

}
