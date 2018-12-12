using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Blackmitten.Elliot.Backend
{
    class MoveGenerator : IPieceVisitor
    {
        public IList<Move> Moves { get; } = new List<Move>();

        public void Visit( Pawn pawn, object data )
        {
            var board = (Board) data;
            if (board.WhitesTurn != pawn.White)
            {
                throw new InvalidOperationException();
            }

            int? direction;
            bool? atStart;
            if (pawn.White)
            {
                direction = 1;
                atStart = pawn.Pos.y == 2;
            }
            else
            {
                direction = -1;
                atStart = pawn.Pos.y == 7;
            }

            var squareInFront = pawn.Pos.Offset(0, direction.Value);
            if (board.GetPieceOnSquare(squareInFront) == null)
            {
                Moves.Add(new Move(board, pawn.Pos, squareInFront));
            }

            if (atStart.Value)
            {
                var squareTwoInFront = pawn.Pos.Offset(0, 2 * direction.Value);
                if (board.GetPieceOnSquare(squareInFront) == null)
                {
                    Moves.Add(new Move(board, pawn.Pos, squareTwoInFront));
                }
            }

            var squareCaptureLeft = pawn.Pos.Offset(-1, direction.Value);
            if (squareCaptureLeft.InBounds)
            {
                var pieceCaptureLeft = board.GetPieceOnSquare(squareCaptureLeft);
                if (pieceCaptureLeft != null && pieceCaptureLeft.White != pawn.White)
                {
                    Moves.Add(new Move(board, pawn.Pos, squareCaptureLeft));
                }
            }

            var squareCaptureRight = pawn.Pos.Offset(1, direction.Value);
            if (squareCaptureRight.InBounds)
            {
                var pieceCaptureRight = board.GetPieceOnSquare(squareCaptureRight);
                if (pieceCaptureRight != null && pieceCaptureRight.White != pawn.White)
                {
                    Moves.Add(new Move(board, pawn.Pos, squareCaptureRight));
                }
            }

#warning TODO en-passant
        }

        public void Visit( Rook rook, object data )
        {
            Board board = (Board) data;
            if (board.WhitesTurn != rook.White)
            {
                throw new InvalidOperationException();
            }

            for (int x = rook.Pos.x; x <= 8; x++)
            {
                var s = new Square(x, rook.Pos.y);
                if (AddMoveIfPossible(rook, board, s))
                {
                    break;
                }
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="piece"></param>
        /// <param name="board"></param>
        /// <param name="square"></param>
        /// <returns>Returns true if no more moves</returns>
        private bool AddMoveIfPossible(IPiece piece, Board board, Square square)
        {
            var p = board.GetPieceOnSquare(square);
            if (p == null)
            {
                Moves.Add(new Move(board, piece.Pos, square));
            }
            else if (p.White == piece.White)
            {
                return true;
            }
            else
            {
                Moves.Add(new Move(board, piece.Pos, square));
                return true;
            }

            return false;
        }

        public void Visit( Knight knight, object data )
        {
            throw new NotImplementedException();
        }

        public void Visit( Bishop bishop, object data )
        {
            throw new NotImplementedException();
        }

        public void Visit( Queen queen, object data )
        {
            throw new NotImplementedException();
        }

        public void Visit( King king, object data )
        {
            throw new NotImplementedException();
        }
    }
}
