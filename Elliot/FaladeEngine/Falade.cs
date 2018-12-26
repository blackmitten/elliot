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
        bool _doDiags;

        public Falade(bool doDiags)
        {
            _doDiags = doDiags;
        }

        public void Stop()
        {

        }

        public Move GetBestMove(Board originalBoard)
        {
            Board board = new Board(originalBoard);
            Assert.IsTrue(board.GetFenString() == originalBoard.GetFenString());
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
                var undo = new List<Action>();
                if (_doDiags)
                {
                    string fenBefore = board.GetFenString();
                    board.Move(m, true, undo);
                    score = Evaluate(board);
                    string fenAfter = board.GetFenString();
                    board.UndoLastmove(undo);
                    string fenAfterUndo = board.GetFenString();
                    Assert.IsTrue(fenAfterUndo == fenBefore);
                }
                else
                {
                    board.Move(m, true, undo);
                    score = Evaluate(board);
                    board.UndoLastmove(undo);
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
            return Minimax(board, 4, double.MinValue, double.MaxValue, false, true);
        }

        private double Minimax(Board board, int depth, double alpha, double beta, bool maximizing, bool whitesTurn)
        {
            if (depth == 0)
            {
                return CalculateSidesScore(board, whitesTurn);
            }

            string fenBefore = "";
            if (_doDiags)
            {
                fenBefore = board.GetFenString();
            }

            var moves = board.GetAllMoves();
            if (maximizing)
            {
                double max = double.MinValue;
                foreach (var move in moves)
                {
                    var undo = new List<Action>();
                    if (_doDiags)
                    {
                        board.Move(move, true, undo);
                        max = Math.Max(max, Minimax(board, depth - 1, alpha, beta, !maximizing, whitesTurn));
                        board.UndoLastmove(undo);
                        var fenAfterUndo = board.GetFenString();
                        Assert.IsTrue(fenBefore == fenAfterUndo);
                        alpha = Math.Max(alpha, max);
                        if (alpha >= beta)
                        {
                            break;
                        }
                    }
                    else
                    {
                        board.Move(move, true, undo);
                        max = Math.Max(max, Minimax(board, depth - 1, alpha, beta, !maximizing, whitesTurn));
                        board.UndoLastmove(undo);
                        alpha = Math.Max(alpha, max);
                        if (alpha >= beta)
                        {
                            break;
                        }
                    }
                }
                return max;
            }
            else
            {
                double min = double.MaxValue;
                foreach (var move in moves)
                {
                    var undo = new List<Action>();
                    if (_doDiags)
                    {
                        board.Move(move, true, undo);
                        min = Math.Min(min, Minimax(board, depth - 1, alpha, beta, !maximizing, whitesTurn));
                        board.UndoLastmove(undo);
                        var fenAfterUndo = board.GetFenString();
                        Assert.IsTrue(fenBefore == fenAfterUndo);
                        beta = Math.Min(beta, min);
                        if (alpha >= beta)
                        {
                            break;
                        }
                    }
                    else
                    {
                        board.Move(move, true, undo);
                        min = Math.Min(min, Minimax(board, depth - 1, alpha, beta, !maximizing, whitesTurn));
                        board.UndoLastmove(undo);
                        beta = Math.Min(beta, min);
                        if (alpha >= beta)
                        {
                            break;
                        }
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

        public void Move(string move) => throw new NotImplementedException();
        public void Shutdown() => throw new NotImplementedException();
    }

}
