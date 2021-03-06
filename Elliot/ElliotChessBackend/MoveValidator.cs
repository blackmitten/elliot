﻿using Blackmitten.Menzel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Blackmitten.Elliot.Backend
{
    public class MoveValidator : IMoveValidator, IPieceVisitor
    {
        bool? _valid;

        public bool Validate(Move move)
        {
            if (move.Start == move.End)
            {
                return false;
            }
            _valid = null;

            IPiece piece = move.Board.GetPieceOnSquare(move.Start);
            if (piece.White != move.Board.WhitesTurn)
            {
                return false;
            }
            piece.Accept(this, move);

#if DIAGNOSTIC
            string fenBefore = move.Board.GetFenString();
#endif
            var undo = new Undo();
            move.Board.Move(move, false, undo);
#if DIAGNOSTIC
            string fenAfter = move.Board.GetFenString();
#endif
            if (move.Board.CurrentPlayerInCheck)
            {
                _valid = false;
            }
            move.Board.UndoLastmove(undo);
#if DIAGNOSTIC
            string fenAfterUndo = move.Board.GetFenString();
            Assert.IsTrue(fenBefore != fenAfter);
            Assert.IsTrue(fenBefore == fenAfterUndo);
#endif
            return _valid.Value;
        }

        void IPieceVisitor.Visit(Pawn pawn, object data)
        {
            Move move = (Move)data;
            Board board = move.Board;
            int direction = pawn.White ? 1 : -1;
            int dx = move.End.x - move.Start.x;
            int dy = move.End.y - move.Start.y;
            int squaresAdvanced = dy * direction;

            _valid = true;
            if (dx == 0)
            {
                if (squaresAdvanced == 1)
                {
                    if (board.GetPieceOnSquare(move.End) != null)
                    {
                        _valid = false;
                        return;
                    }
                }
                else if (squaresAdvanced == 2)
                {
                    if (board.GetPieceOnSquare(new Square(move.Start.x, move.Start.y + direction)) != null)
                    {
                        _valid = false;
                        return;
                    }
                    else if (board.GetPieceOnSquare(move.End) != null)
                    {
                        _valid = false;
                        return;
                    }
                }
                else
                {
                    _valid = false;
                    return;
                }
            }
            else if (Math.Abs(dx) == 1 && squaresAdvanced == 1)
            {
                IPiece capturedPiece = board.GetPieceOnSquare(move.End);
                if (board.EnPassantTarget == move.End)
                {
                    capturedPiece = board.GetPieceOnSquare(board.EnPassantTarget.Offset(0, -direction));
                }
                if (capturedPiece == null)
                {
                    _valid = false;
                    return;
                }
                else if (capturedPiece.White == pawn.White)
                {
                    _valid = false;
                    return;
                }
                else
                {
                    move.Capturing = true;
                }
            }
            else
            {
                _valid = false;
                return;
            }

        }

        void IPieceVisitor.Visit(Rook rook, object data)
        {
            Move move = (Move)data;
            int dx = move.End.x - move.Start.x;
            int dy = move.End.y - move.Start.y;

            _valid = true;
            if (dx != 0 && dy != 0)
            {
                _valid = false;
                return;
            }
            if (dx == 0 && dy == 0)
            {
                _valid = false;
                return;
            }
            CheckNothingInTheWay(rook, move);
        }

        void IPieceVisitor.Visit(Knight knight, object data)
        {
            Move move = (Move)data;
            int absDx = Math.Abs(move.End.x - move.Start.x);
            int absDy = Math.Abs(move.End.y - move.Start.y);

            _valid = true;
            if (!(absDx == 1 && absDy == 2) && !(absDx == 2 && absDy == 1))
            {
                _valid = false;
                return;
            }
            IPiece capturedPiece = move.Board.GetPieceOnSquare(move.End);
            if (capturedPiece != null)
            {
                if (capturedPiece.White == knight.White)
                {
                    _valid = false;
                    return;
                }
                else
                {
                    move.Capturing = true;
                }
            }
        }

        void IPieceVisitor.Visit(Bishop bishop, object data)
        {
            Move move = (Move)data;
            int dx = move.End.x - move.Start.x;
            int dy = move.End.y - move.Start.y;

            _valid = true;
            if (Math.Abs(dx) != Math.Abs(dy))
            {
                _valid = false;
                return;
            }
            if (dx == 0)
            {
                _valid = false;
                return;
            }
            CheckNothingInTheWay(bishop, move);

        }

        void IPieceVisitor.Visit(Queen queen, object data)
        {
            Move move = (Move)data;
            int dx = move.End.x - move.Start.x;
            int dy = move.End.y - move.Start.y;

            _valid = true;
            if (dx != 0 && dy != 0)
            {
                if (Math.Abs(dx) != Math.Abs(dy))
                {
                    _valid = false;
                    return;
                }
            }
            if (dx == 0 && dy == 0)
            {
                _valid = false;
                return;
            }
            CheckNothingInTheWay(queen, move);
        }

        void IPieceVisitor.Visit(King king, object data)
        {
            Move move = (Move)data;
            int absDx = Math.Abs(move.End.x - move.Start.x);
            int absDy = Math.Abs(move.End.y - move.Start.y);

            _valid = true;
            if (absDx == 0 && absDy == 0)
            {
                _valid = false;
                return;
            }
            if (absDx > 1 || absDy > 1)
            {
                Board board = move.Board;
                bool castleOk = false;
                if (king.White &&
                    move.Start == Square.WhiteKingStart)
                {
                    if (move.End == Square.WhiteKingCastledKingside &&
                        board.WhiteCanCastleKingside)
                    {
                        if (board.GetPieceOnSquare(new Square(6, 1)) == null &&
                            board.GetPieceOnSquare(new Square(7, 1)) == null)
                        {
                            castleOk = true;
                        }
                    }
                    else if (move.End == Square.WhiteKingCastledQueenside &&
                        board.WhiteCanCastleQueenside)
                    {
                        if (board.GetPieceOnSquare(new Square(4, 1)) == null &&
                            board.GetPieceOnSquare(new Square(3, 1)) == null &&
                            board.GetPieceOnSquare(new Square(2, 1)) == null)
                        {
                            castleOk = true;
                        }
                    }
                }
                else if (!king.White &&
                    move.Start == Square.BlackKingStart)
                {
                    if (move.End == Square.BlackKingCastledKingside &&
                        board.BlackCanCastleKingside)
                    {
                        if (board.GetPieceOnSquare(new Square(6, 8)) == null &&
                            board.GetPieceOnSquare(new Square(7, 8)) == null)
                        {
                            castleOk = true;
                        }
                    }
                    else if (move.End == Square.BlackKingCastledQueenside &&
                        board.BlackCanCastleQueenside)
                    {
                        if (board.GetPieceOnSquare(new Square(4, 8)) == null &&
                            board.GetPieceOnSquare(new Square(3, 8)) == null &&
                            board.GetPieceOnSquare(new Square(2, 8)) == null)
                        {
                            castleOk = true;
                        }
                    }
                }
                if (!castleOk)
                {
                    _valid = false;
                    return;
                }
            }
            IPiece capturedPiece = move.Board.GetPieceOnSquare(move.End);
            if (capturedPiece != null)
            {
                if (capturedPiece.White == king.White)
                {
                    _valid = false;
                    return;
                }
                else
                {
                    move.Capturing = true;
                }
            }
        }

        void CheckNothingInTheWay(IPiece piece, Move move)
        {
            int dx = move.End.x - move.Start.x;
            int dy = move.End.y - move.Start.y;
            int xdir = Math.Sign(dx);
            int ydir = Math.Sign(dy);
            Square s = move.Start;
            while (s != move.End)
            {
                s = s.Offset(xdir, ydir);
                IPiece capturedPiece = move.Board.GetPieceOnSquare(s);
                if (capturedPiece != null)
                {
                    if (s == move.End)
                    {
                        if (capturedPiece.White == piece.White)
                        {
                            _valid = false;
                            return;
                        }
                        else
                        {
                            move.Capturing = true;
                        }
                    }
                    else
                    {
                        _valid = false;
                        return;
                    }
                }
            }
        }
    }
}
