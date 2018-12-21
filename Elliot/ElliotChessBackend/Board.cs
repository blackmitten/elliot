using Blackmitten.Menzel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Blackmitten.Elliot.Backend
{
    public class Board
    {
        List<IPiece> _blackPieces = new List<IPiece>();
        List<IPiece> _whitePieces = new List<IPiece>();

        IPiece _blackKing;
        IPiece _whiteKing;

        public bool WhitesTurn { get; set; } = true;

        public IEnumerable<IPiece> BlackPieces => _blackPieces;
        public IEnumerable<IPiece> WhitePieces => _whitePieces;

        public List<Action> _undoLastMove;

        public Board()
        {
        }

        public Board(Board board)
        {
            WhitesTurn = board.WhitesTurn;
            foreach (var piece in board._whitePieces)
            {
                AddPiece(piece.Copy(), null);
            }
            foreach (var piece in board._blackPieces)
            {
                AddPiece(piece.Copy(), null);
            }
        }

        public void UndoLastmove()
        {
            foreach (var undo in _undoLastMove)
            {
                undo();
            }
        }

        public IList<Move> GetAllMoves()
        {
            List<IPiece> pieces = WhitesTurn ? _whitePieces : _blackPieces;
            List<Move> moves = new List<Move>();
            MoveGenerator generator = new MoveGenerator();
            foreach (var piece in pieces)
            {
                piece.Accept(generator, this);
            }
            moves.AddRange(generator.Moves);
            MoveValidator validator = new MoveValidator();
            var invalidMoves = new List<Move>();
            foreach (var move in moves)
            {
                if (!validator.Validate(move))
                {
                    invalidMoves.Add(move);
                }
            }
            foreach (var move in invalidMoves)
            {
                moves.Remove(move);
            }
            return moves;
        }

        public void Move(Move move, bool switchSides, List<Action> undo = null)
        {
            if (undo == null)
            {
                _undoLastMove = new List<Action>();
                undo = _undoLastMove;
            }
            Square LastMoveEnPassantTaret = EnPassantTarget;
            if (EnPassantTarget.InBounds)
            {
                undo.Insert(0, () => EnPassantTarget = LastMoveEnPassantTaret);
            }
            else
            {
                undo.Insert(0, () => EnPassantTarget = new Square(0, 0));
            }
            EnPassantTarget = new Square();
            IPiece piece = GetPieceOnSquare(move.Start);
            IPiece capturedPiece = GetPieceOnSquare(move.End);
            if (capturedPiece != null)
            {
                RemovePiece(capturedPiece, undo);
            }
            if (piece.IsKing)
            {
                if (move.Start == Square.WhiteKingStart)
                {
                    if (move.End == Square.WhiteKingCastledQueenside)
                    {
                        Move(new Move(this, Square.WhiteQueensRookStart, Square.WhiteQueensRookCastled), false, undo);
                    }
                    else if (move.End == Square.WhiteKingCastledKingside)
                    {
                        Move(new Move(this, Square.WhiteKingsRookStart, Square.WhiteKingsRookCastled), false, undo);
                    }
                    if (WhiteCanCastleKingside)
                    {
                        undo.Insert(0, () => WhiteCanCastleKingside = true);
                    }
                    if (WhiteCanCastleQueenside)
                    {
                        undo.Insert(0, () => WhiteCanCastleQueenside = true);
                    }
                    WhiteCanCastleKingside = false;
                    WhiteCanCastleQueenside = false;
                }
                else if (move.Start == Square.BlackKingStart)
                {
                    if (move.End == Square.BlackKingCastledQueenside)
                    {
                        Move(new Move(this, Square.BlackQueensRookStart, Square.BlackQueensRookCastled), false, undo);
                    }
                    else if (move.End == Square.BlackKingCastledKingside)
                    {
                        Move(new Move(this, Square.BlackKingsRookStart, Square.BlackKingsRookCastled), false, undo);
                    }
                    if (BlackCanCastleKingside)
                    {
                        undo.Insert(0, () => BlackCanCastleKingside = true);
                    }
                    if (BlackCanCastleQueenside)
                    {
                        undo.Insert(0, () => BlackCanCastleQueenside = true);
                    }
                    BlackCanCastleKingside = false;
                    BlackCanCastleQueenside = false;
                }
            }
            else if (piece.IsRook)
            {
                if (move.Start == Square.WhiteKingsRookStart)
                {
                    if (WhiteCanCastleKingside)
                    {
                        undo.Insert(0, () => WhiteCanCastleKingside = true);
                    }
                    WhiteCanCastleKingside = false;
                }
                else if (move.Start == Square.WhiteQueensRookStart)
                {
                    if (WhiteCanCastleQueenside)
                    {
                        undo.Insert(0, () => WhiteCanCastleQueenside = true);
                    }
                    WhiteCanCastleQueenside = false;
                }
                else if (move.Start == Square.BlackKingsRookStart)
                {
                    if (BlackCanCastleKingside)
                    {
                        undo.Insert(0, () => BlackCanCastleKingside = true);
                    }
                    BlackCanCastleKingside = false;
                }
                else if (move.Start == Square.BlackQueensRookStart)
                {
                    if (BlackCanCastleQueenside)
                    {
                        undo.Insert(0, () => BlackCanCastleQueenside = true);
                    }
                    BlackCanCastleQueenside = false;
                }
            }
            else if (piece.IsPawn)
            {
                switch (move.Promoted)
                {
                    case PieceType.None:
                        break;
                    case PieceType.Bishop:
                        RemovePiece(piece, undo);
                        AddPiece(new Bishop(move.End, piece.White), undo);
                        break;
                    case PieceType.Queen:
                        RemovePiece(piece, undo);
                        AddPiece(new Queen(move.End, piece.White), undo);
                        break;
                    case PieceType.Rook:
                        RemovePiece(piece, undo);
                        AddPiece(new Rook(move.End, piece.White), undo);
                        break;
                    default:
                        throw new NotImplementedException();
                }
                int dy = move.End.y - move.Start.y;
                if (LastMoveEnPassantTaret.x == move.End.x &&
                    LastMoveEnPassantTaret.y == move.End.y)
                {
                    capturedPiece = GetPieceOnSquare(move.End.Offset(0, -dy));
                    RemovePiece(capturedPiece, undo);
                }
                if (Math.Abs(dy) == 2)
                {
                    EnPassantTarget = new Square(move.End.x, move.End.y - dy / 2);
                }
            }
            piece.Pos = move.End;
            undo.Add(() => piece.Pos = move.Start);
            if (switchSides)
            {
                undo.Add(() =>
                {
                    WhitesTurn = !WhitesTurn;
                    HalfMoveClock--;
                });
                WhitesTurn = !WhitesTurn;
                HalfMoveClock++;
                if (WhitesTurn)
                {
                    FullMoveClock++;
                    undo.Insert(0, () => FullMoveClock--);
                }
            }
            int halfMoveClock = HalfMoveClock;
            if (capturedPiece != null || piece.IsPawn)
            {
                undo.Insert(0, () => HalfMoveClock = halfMoveClock);
                HalfMoveClock = 0;
            }

        }

        public void RemovePiece(IPiece piece, List<Action> undo)
        {
            if (piece.White)
            {
                _whitePieces.Remove(piece);
                undo?.Insert(0, () =>
                {
                    _whitePieces.Add(piece);
                });
            }
            else
            {
                _blackPieces.Remove(piece);
                undo?.Insert(0, () =>
                {
                    _blackPieces.Add(piece);
                });
            }
        }

        public void AddPiece(IPiece piece, List<Action> undo)
        {
            if (piece.White)
            {
                _whitePieces.Add(piece);
                if (piece.IsKing)
                {
                    if (_whiteKing != null)
                    {
                        throw new InvalidOperationException("Already have a white king");
                    }
                    _whiteKing = piece;
                }
                undo?.Insert(0, () =>
                {
                    _whitePieces.Remove(piece);
                });
            }
            else
            {
                _blackPieces.Add(piece);
                if (piece.IsKing)
                {
                    if (_blackKing != null)
                    {
                        throw new InvalidOperationException("Already have a black king");
                    }
                    _blackKing = piece;
                }
                undo?.Insert(0, () =>
                {
                    _blackPieces.Remove(piece);
                });
            }
        }

        public IPiece GetPieceOnSquare(Square square, bool suppressOutOfBoundsException = false)
        {
            if (!square.InBounds)
            {
                if (suppressOutOfBoundsException)
                {
                    return null;
                }
                throw new InvalidOperationException("Out of bounds square in GetPieceOnSquare " + square.ToString());
            }
            foreach (var piece in _blackPieces)
            {
                if (square == piece.Pos)
                {
                    return piece;
                }
            }
            foreach (var piece in _whitePieces)
            {
                if (square == piece.Pos)
                {
                    return piece;
                }
            }
            return null;
        }

        public bool WhiteCanCastleKingside { get; set; } = true;
        public bool WhiteCanCastleQueenside { get; set; } = true;
        public bool BlackCanCastleKingside { get; set; } = true;
        public bool BlackCanCastleQueenside { get; set; } = true;
        public Square EnPassantTarget { get; set; } = new Square();
        public int HalfMoveClock { get; set; } = 0;
        public int FullMoveClock { get; set; } = 1;

        public bool CurrentPlayerInCheck
        {
            get
            {
                IPiece king = WhitesTurn ? _whiteKing : _blackKing;
                return IsSquareThreatened(king.Pos);
            }
        }

        public bool IsSquareThreatened(Square square)
        {
            if (GetPieceOnSquareOnSide(square.Offset(1, 2), !WhitesTurn) as Knight != null ||
                GetPieceOnSquareOnSide(square.Offset(-1, 2), !WhitesTurn) as Knight != null ||
                GetPieceOnSquareOnSide(square.Offset(1, -2), !WhitesTurn) as Knight != null ||
                GetPieceOnSquareOnSide(square.Offset(-1, -2), !WhitesTurn) as Knight != null ||
                GetPieceOnSquareOnSide(square.Offset(2, 1), !WhitesTurn) as Knight != null ||
                GetPieceOnSquareOnSide(square.Offset(-2, 1), !WhitesTurn) as Knight != null ||
                GetPieceOnSquareOnSide(square.Offset(2, -1), !WhitesTurn) as Knight != null ||
                GetPieceOnSquareOnSide(square.Offset(-2, -1), !WhitesTurn) as Knight != null)
            {
                return true;
            }
            if (IsSquareThreatenedFromDirection(square, 1, 1, piece => piece.IsDiagonalMover))
            {
                return true;
            }
            if (IsSquareThreatenedFromDirection(square, -1, 1, piece => piece.IsDiagonalMover))
            {
                return true;
            }
            if (IsSquareThreatenedFromDirection(square, 1, -1, piece => piece.IsDiagonalMover))
            {
                return true;
            }
            if (IsSquareThreatenedFromDirection(square, -1, -1, piece => piece.IsDiagonalMover))
            {
                return true;
            }
            if (IsSquareThreatenedFromDirection(square, 0, 1, piece => piece.IsStraightMover))
            {
                return true;
            }
            if (IsSquareThreatenedFromDirection(square, 0, -1, piece => piece.IsStraightMover))
            {
                return true;
            }
            if (IsSquareThreatenedFromDirection(square, 1, 0, piece => piece.IsStraightMover))
            {
                return true;
            }
            if (IsSquareThreatenedFromDirection(square, -1, 0, piece => piece.IsStraightMover))
            {
                return true;
            }

            return false;
        }

        private bool IsSquareThreatenedFromDirection(Square square, int dx, int dy, Func<IPiece, bool> pieceTest)
        {
            for (Square s = square.Offset(dx, dy); s.InBounds; s = s.Offset(dx, dy))
            {
                IPiece piece = GetPieceOnSquare(s);
                if (piece != null)
                {
                    if (piece.White == WhitesTurn)
                    {
                        return false;
                    }
                    else if (pieceTest(piece))
                    {
                        return true;
                    }
                    else if (piece.IsKing)
                    {
                        Vector v = square - s;
                        int distance = Math.Max(Math.Abs(v.x), Math.Abs(v.y));
                        return (distance == 1);
                    }
                    else if (piece.IsPawn)
                    {
                        int pawnsDirection = piece.White ? 1 : -1;
                        Vector v = square - s;
                        if (Math.Abs(v.x) == 1 && v.y == pawnsDirection)
                        {
                            return true;
                        }
                        return false;
                    }
                    else
                    {
                        return false;
                    }

                }
            }
            return false;
        }

        private IPiece GetPieceOnSquareOnSide(Square square, bool side)
        {
            IPiece piece = GetPieceOnSquare(square, true);
            if (piece != null && piece.White == side)
            {
                return piece;
            }
            return null;
        }

        public string GetFenString()
        {
            StringBuilder fen = new StringBuilder();
            FenCharPieceVisitor fenCharPieceVisitor = new FenCharPieceVisitor();

            for (int y = 8; y > 0; y--)
            {
                int x = 1;
                int emptySquares = 0;
                while (x <= 8)
                {
                    IPiece piece = GetPieceOnSquare(new Square(x, y));
                    if (piece != null)
                    {
                        if (emptySquares > 0)
                        {
                            fen.Append(emptySquares.ToString());
                            emptySquares = 0;
                        }
                        piece.Accept(fenCharPieceVisitor);
                        fen.Append(fenCharPieceVisitor.Char);
                    }
                    else
                    {
                        emptySquares++;
                    }
                    x++;
                }
                if (emptySquares > 0)
                {
                    fen.Append(emptySquares.ToString());
                    emptySquares = 0;
                }
                if (y > 1)
                {
                    fen.Append('/');
                }
            }

            if (WhitesTurn)
            {
                fen.Append(" w ");
            }
            else
            {
                fen.Append(" b ");
            }

            StringBuilder castling = new StringBuilder();
            if (WhiteCanCastleKingside)
            {
                castling.Append("K");
            }
            if (WhiteCanCastleQueenside)
            {
                castling.Append("Q");
            }
            if (BlackCanCastleKingside)
            {
                castling.Append("k");
            }
            if (BlackCanCastleQueenside)
            {
                castling.Append("q");
            }
            if (castling.Length == 0)
            {
                castling.Append("-");
            }
            fen.Append(castling);
            fen.Append(" ");
            if (EnPassantTarget.InBounds)
            {
                fen.Append(EnPassantTarget);
            }
            else
            {
                fen.Append("-");
            }
            fen.Append(" ");
            fen.Append(HalfMoveClock);
            fen.Append(" ");
            fen.Append(FullMoveClock);
            return fen.ToString();
        }

        public override string ToString() => GetFenString();

    }

}
