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

        public void Visit(Rook rook, object data) => throw new InvalidMoveException("not implemented");
        public void Visit(Knight knight, object data) => throw new InvalidMoveException("not implemented");

        public void Visit(Bishop bishop, object data)
        {
            Move move = (Move)data;
            Board board = move.Board;
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
            int xdir = Math.Sign(dx);
            int ydir = Math.Sign(dy);
            Square s = move.Start;
            while (s != move.End)
            {
                s = s.Offset(xdir, ydir);
                IPiece capturedPiece = board.GetPieceOnSquare(s);
                if (capturedPiece != null)
                {
                    if (s == move.End)
                    {
                        if (capturedPiece.White == bishop.White)
                        {
                            throw new InvalidMoveException("Can only take a piece of other side");
                        }
                    }
                    else
                    {
                        throw new InvalidMoveException("There's a piece in the way of this bishop");
                    }
                }
            }
        }

        public void Visit(Queen queen, object data) => throw new InvalidMoveException("not implemented");
        public void Visit(King king, object data) => throw new InvalidMoveException("not implemented");
    }
}
