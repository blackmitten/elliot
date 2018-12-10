using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Blackmitten.Elliot.Backend
{
    public class MoveValidator : IMoveValidator, IPieceVisitor
    {

        public void Validate(Move move)
        {
            string moveString = move.ToLongString();

            IPiece piece = move.Board.GetPieceOnSquare(move.Start);
            piece.Accept(this, move);
            //throw new InvalidMoveException(moveString);
        }

        public void Visit(Pawn pawn, object data)
        {
            Move move = (Move)data;
            Board board = move.Board;
            int direction = pawn.White ? 1 : -1;
            int dx = move.End.x - move.Start.x;
            int dy = move.End.y - move.Start.y;
            int squaresAdvanced = dy * direction;

            if (dx == 0)
            {
                if (squaresAdvanced == 1)
                {
                    if (board.GetPieceOnSquare(move.End) != null)
                    {
                        throw new InvalidMoveException("Pawns can't take forwards");
                    }
                }
                else if (squaresAdvanced == 2)
                {
                    if (board.GetPieceOnSquare(new Square(move.Start.x, move.Start.y + direction)) != null)
                    {
                        throw new InvalidMoveException("Pawns can't take forwards");
                    }
                    if (board.GetPieceOnSquare(move.End) != null)
                    {
                        throw new InvalidMoveException("Pawns can't take forwards");
                    }
                }
                else
                {
                    throw new InvalidMoveException("Pawns can only move 1 or 2 spaces forwards");
                }
            }
            else if (Math.Abs(dx) == 1 && squaresAdvanced == 1)
            {
                IPiece capturedPiece = board.GetPieceOnSquare(move.End);
                if (capturedPiece == null)
                {
                    throw new InvalidMoveException("Can only move diagonally when taking");
                }
                else if (capturedPiece.White == pawn.White)
                {
                    throw new InvalidMoveException("Can only take a piece of other side");
                }
            }
            else
            {
                throw new InvalidMoveException("Can only move forward 1 or 2 spaces, 1 to either side while taking");
            }

        }

        public void Visit(Rook rook, object data)
        {
            Move move = (Move)data;
            int dx = move.End.x - move.Start.x;
            int dy = move.End.y - move.Start.y;
            if (dx != 0 && dy != 0)
            {
                throw new InvalidMoveException("Rooks can only move horizontally/vertically");
            }
            if (dx == 0 && dy == 0)
            {
                throw new InvalidMoveException("Rook didn't move");
            }
            CheckNothingInTheWay(rook, move);
        }

        public void Visit(Knight knight, object data)
        {
            Move move = (Move)data;
            int absDx = Math.Abs(move.End.x - move.Start.x);
            int absDy = Math.Abs(move.End.y - move.Start.y);
            if (!(absDx == 1 && absDy == 2) && !(absDx == 2 && absDy == 1))
            {
                throw new InvalidMoveException("Knight must move in knightly fashion");
            }
            IPiece capturedPiece = move.Board.GetPieceOnSquare(move.End);
            if (capturedPiece != null)
            {
                if (capturedPiece.White == knight.White)
                {
                    throw new InvalidMoveException("Can only take a piece of other side");
                }
            }
        }

        public void Visit(Bishop bishop, object data)
        {
            Move move = (Move)data;
            int dx = move.End.x - move.Start.x;
            int dy = move.End.y - move.Start.y;
            if (Math.Abs(dx) != Math.Abs(dy))
            {
                throw new InvalidMoveException("Bishops only move diagonally");
            }
            if (dx == 0)
            {
                throw new InvalidMoveException("Bishop didn't move");
            }
            CheckNothingInTheWay(bishop, move);

        }

        public void Visit(Queen queen, object data)
        {
            Move move = (Move)data;
            int dx = move.End.x - move.Start.x;
            int dy = move.End.y - move.Start.y;
            if (dx != 0 && dy != 0)
            {
                if (Math.Abs(dx) != Math.Abs(dy))
                {
                    throw new InvalidMoveException("Queens can only move diagonally/horizontally/vertically");
                }
            }
            if (dx == 0 && dy == 0)
            {
                throw new InvalidMoveException("Queen didn't move");
            }
            CheckNothingInTheWay(queen, move);
        }

        public void Visit(King king, object data)
        {
            Move move = (Move)data;
            int absDx = Math.Abs(move.End.x - move.Start.x);
            int absDy = Math.Abs(move.End.y - move.Start.y);
            if (absDx == 0 && absDy == 0)
            {
                throw new InvalidMoveException("King didn't move");
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
                    throw new InvalidMoveException("King can only move one space");
                }
            }
            IPiece capturedPiece = move.Board.GetPieceOnSquare(move.End);
            if (capturedPiece != null)
            {
                if (capturedPiece.White == king.White)
                {
                    throw new InvalidMoveException("Can only take a piece of other side");
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
                            throw new InvalidMoveException("Can only take a piece of other side");
                        }
                    }
                    else
                    {
                        throw new InvalidMoveException("There's a piece in the way of this " + piece.Name);
                    }
                }
            }
        }
    }
}
