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

        private void AddMoveIfValid( Move move )
        {
            Moves.Add( move );
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="piece"></param>
        /// <param name="board"></param>
        /// <param name="square"></param>
        /// <returns>Returns true if square was occupied</returns>
        private bool AddMoveIfPossible( IPiece piece, Board board, Square square )
        {
            if ( !square.InBounds )
            {
                return true;
            }
            var p = board.GetPieceOnSquare( square );
            if ( p == null )
            {
                AddMoveIfValid( new Move( board, piece.Pos, square ) );
            }
            else if ( p.White == piece.White )
            {
                return true;
            }
            else
            {
                AddMoveIfValid( new Move( board, piece.Pos, square ) );
                return true;
            }

            return false;
        }

        public void Visit( Pawn pawn, object data )
        {
            var board = ( Board ) data;
            if ( board.WhitesTurn != pawn.White )
            {
                throw new InvalidOperationException();
            }

            int? direction;
            bool? atStart;
            if ( pawn.White )
            {
                direction = 1;
                atStart = pawn.Pos.y == 2;
            }
            else
            {
                direction = -1;
                atStart = pawn.Pos.y == 7;
            }

            var squareInFront = pawn.Pos.Offset( 0, direction.Value );
            if ( squareInFront.InBounds && board.GetPieceOnSquare( squareInFront ) == null )
            {
                AddMoveIfValid( new Move( board, pawn.Pos, squareInFront ) );
            }

            if ( atStart.Value )
            {
                var squareTwoInFront = pawn.Pos.Offset( 0, 2 * direction.Value );
                if ( board.GetPieceOnSquare( squareTwoInFront ) == null )
                {
                    AddMoveIfValid( new Move( board, pawn.Pos, squareTwoInFront ) );
                }
            }

            var squareCaptureLeft = pawn.Pos.Offset( -1, direction.Value );
            if ( squareCaptureLeft.InBounds )
            {
                var pieceCaptureLeft = board.GetPieceOnSquare( squareCaptureLeft );
                if ( pieceCaptureLeft != null && pieceCaptureLeft.White != pawn.White )
                {
                    AddMoveIfValid( new Move( board, pawn.Pos, squareCaptureLeft ) );
                }
            }

            var squareCaptureRight = pawn.Pos.Offset( 1, direction.Value );
            if ( squareCaptureRight.InBounds )
            {
                var pieceCaptureRight = board.GetPieceOnSquare( squareCaptureRight );
                if ( pieceCaptureRight != null && pieceCaptureRight.White != pawn.White )
                {
                    AddMoveIfValid( new Move( board, pawn.Pos, squareCaptureRight ) );
                }
            }

#warning TODO en-passant
        }

        public void Visit( Rook rook, object data )
        {
            Board board = ( Board ) data;
            if ( board.WhitesTurn != rook.White )
            {
                throw new InvalidOperationException();
            }

            for ( Square s = rook.Pos.Offset( 0, 1 ); s.InBounds; s = s.Offset( 0, 1 ) )
            {
                if ( AddMoveIfPossible( rook, board, s ) )
                {
                    break;
                }
            }
            for ( Square s = rook.Pos.Offset( 0, -1 ); s.InBounds; s = s.Offset( 0, -1 ) )
            {
                if ( AddMoveIfPossible( rook, board, s ) )
                {
                    break;
                }
            }
            for ( Square s = rook.Pos.Offset( 1, 0 ); s.InBounds; s = s.Offset( 1, 0 ) )
            {
                if ( AddMoveIfPossible( rook, board, s ) )
                {
                    break;
                }
            }
            for ( Square s = rook.Pos.Offset( -1, 0 ); s.InBounds; s = s.Offset( -1, 0 ) )
            {
                if ( AddMoveIfPossible( rook, board, s ) )
                {
                    break;
                }
            }
        }

        public void Visit( Knight knight, object data )
        {
            Board board = ( Board ) data;
            if ( board.WhitesTurn != knight.White )
            {
                throw new InvalidOperationException();
            }

            AddMoveIfPossible( knight, board, knight.Pos.Offset( 1, 2 ) );
            AddMoveIfPossible( knight, board, knight.Pos.Offset( -1, 2 ) );
            AddMoveIfPossible( knight, board, knight.Pos.Offset( 1, -2 ) );
            AddMoveIfPossible( knight, board, knight.Pos.Offset( -1, -2 ) );
            AddMoveIfPossible( knight, board, knight.Pos.Offset( 2, 1 ) );
            AddMoveIfPossible( knight, board, knight.Pos.Offset( -2, 1 ) );
            AddMoveIfPossible( knight, board, knight.Pos.Offset( 2, -1 ) );
            AddMoveIfPossible( knight, board, knight.Pos.Offset( -2, -1 ) );
        }

        public void Visit( Bishop bishop, object data )
        {
            Board board = ( Board ) data;
            if ( board.WhitesTurn != bishop.White )
            {
                throw new InvalidOperationException();
            }

            for ( Square s = bishop.Pos.Offset( 1, 1 ); s.InBounds; s = s.Offset( 1, 1 ) )
            {
                if ( AddMoveIfPossible( bishop, board, s ) )
                {
                    break;
                }
            }
            for ( Square s = bishop.Pos.Offset( -1, 1 ); s.InBounds; s = s.Offset( -1, 1 ) )
            {
                if ( AddMoveIfPossible( bishop, board, s ) )
                {
                    break;
                }
            }
            for ( Square s = bishop.Pos.Offset( 1, -1 ); s.InBounds; s = s.Offset( 1, -1 ) )
            {
                if ( AddMoveIfPossible( bishop, board, s ) )
                {
                    break;
                }
            }
            for ( Square s = bishop.Pos.Offset( -1, -1 ); s.InBounds; s = s.Offset( -1, -1 ) )
            {
                if ( AddMoveIfPossible( bishop, board, s ) )
                {
                    break;
                }
            }
        }

        public void Visit( Queen queen, object data )
        {
            Board board = ( Board ) data;
            if ( board.WhitesTurn != queen.White )
            {
                throw new InvalidOperationException();
            }

            for ( Square s = queen.Pos.Offset( 1, 1 ); s.InBounds; s = s.Offset( 1, 1 ) )
            {
                if ( AddMoveIfPossible( queen, board, s ) )
                {
                    break;
                }
            }
            for ( Square s = queen.Pos.Offset( -1, 1 ); s.InBounds; s = s.Offset( -1, 1 ) )
            {
                if ( AddMoveIfPossible( queen, board, s ) )
                {
                    break;
                }
            }
            for ( Square s = queen.Pos.Offset( 1, -1 ); s.InBounds; s = s.Offset( 1, -1 ) )
            {
                if ( AddMoveIfPossible( queen, board, s ) )
                {
                    break;
                }
            }
            for ( Square s = queen.Pos.Offset( -1, -1 ); s.InBounds; s = s.Offset( -1, -1 ) )
            {
                if ( AddMoveIfPossible( queen, board, s ) )
                {
                    break;
                }
            }
            for ( Square s = queen.Pos.Offset( 0, 1 ); s.InBounds; s = s.Offset( 0, 1 ) )
            {
                if ( AddMoveIfPossible( queen, board, s ) )
                {
                    break;
                }
            }
            for ( Square s = queen.Pos.Offset( 0, -1 ); s.InBounds; s = s.Offset( 0, -1 ) )
            {
                if ( AddMoveIfPossible( queen, board, s ) )
                {
                    break;
                }
            }
            for ( Square s = queen.Pos.Offset( 1, 0 ); s.InBounds; s = s.Offset( 1, 0 ) )
            {
                if ( AddMoveIfPossible( queen, board, s ) )
                {
                    break;
                }
            }
            for ( Square s = queen.Pos.Offset( -1, 0 ); s.InBounds; s = s.Offset( -1, 0 ) )
            {
                if ( AddMoveIfPossible( queen, board, s ) )
                {
                    break;
                }
            }

        }

        public void Visit( King king, object data )
        {
            Board board = ( Board ) data;
            if ( board.WhitesTurn != king.White )
            {
                throw new InvalidOperationException();
            }

            AddMoveIfPossible( king, board, king.Pos.Offset( 0, 1 ) );
            AddMoveIfPossible( king, board, king.Pos.Offset( 0, -1 ) );
            AddMoveIfPossible( king, board, king.Pos.Offset( 1, 1 ) );
            AddMoveIfPossible( king, board, king.Pos.Offset( 1, -1 ) );
            AddMoveIfPossible( king, board, king.Pos.Offset( -1, 1 ) );
            AddMoveIfPossible( king, board, king.Pos.Offset( -1, -1 ) );
            AddMoveIfPossible( king, board, king.Pos.Offset( 1, 0 ) );
            AddMoveIfPossible( king, board, king.Pos.Offset( -1, 0 ) );
#warning TODO castling
        }


    }
}
