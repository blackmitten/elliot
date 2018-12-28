using Blackmitten.Menzel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Blackmitten.Elliot.Backend
{
    public class Undo
    {
        internal void UndoMove(Board board)
        {
            board.EnPassantTarget = EnPassantTarget;
            board.WhiteCanCastleKingside = WhiteCanCastleKingside;
            board.WhiteCanCastleQueenside = WhiteCanCastleQueenside;
            board.BlackCanCastleKingside = BlackCanCastleKingside;
            board.BlackCanCastleQueenside = BlackCanCastleQueenside;
            board.WhitesTurn = WhitesTurn;
            board.HalfMoveClock = HalfMoveClock;
            board.FullMoveClock = FullMoveClock;

            if (_pieceToMove != null)
            {
                board.Squares[_pieceToMove.Pos.x - 1, _pieceToMove.Pos.y - 1] = null;
                board.Squares[_squareToMovePieceTo.x - 1, _squareToMovePieceTo.y - 1] = _pieceToMove;
                _pieceToMove.Pos = _squareToMovePieceTo;

            }
            if (_capturedPieceToReplace != null)
            {
                board.AddPiece(_capturedPieceToReplace);
            }
            if (_promotedPawnToReplace != null)
            {
                board.AddPiece(_promotedPawnToReplace);
            }
            if (_squareToRemovePromotedPieceFrom.InBounds)
            {
                board.RemovePiece(board.GetPieceOnSquare(_squareToRemovePromotedPieceFrom));
            }
        }

        public Square EnPassantTarget { get; internal set; } = new Square();
        public bool WhiteCanCastleKingside { get; internal set; } = false;
        public bool WhiteCanCastleQueenside { get; internal set; } = false;
        public bool BlackCanCastleKingside { get; internal set; } = false;
        public bool BlackCanCastleQueenside { get; internal set; } = false;
        public bool WhitesTurn { get; internal set; }
        public int HalfMoveClock { get; internal set; }
        public int FullMoveClock { get; internal set; }

        IPiece _capturedPieceToReplace;
        Square _squareToReplaceCapturedPiece;
        internal void ReplaceCapturedPiece(IPiece capturedPiece, Square pos)
        {
            _capturedPieceToReplace = capturedPiece;
            _squareToReplaceCapturedPiece = pos;
        }

        IPiece _promotedPawnToReplace;
        Square _squareToReplacePromotedPawn;
        internal void ReplacePromotedPawn(IPiece piece, Square pos)
        {
            _promotedPawnToReplace = piece;
            _squareToReplacePromotedPawn = pos;
        }

        Square _squareToRemovePromotedPieceFrom;
        internal void RemovePromotedPieceOnSquare(Square pos) => _squareToRemovePromotedPieceFrom = pos;

        IPiece _pieceToMove;
        Square _squareToMovePieceTo;
        internal void MovePieceBackTo(IPiece piece, Square pos)
        {
            _pieceToMove = piece;
            _squareToMovePieceTo = pos;
        }
    }
}
