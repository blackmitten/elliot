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
        List<IPiece> m_blackPieces = new List<IPiece>();
        List<IPiece> m_whitePieces = new List<IPiece>();
        IPiece _blackKing;
        IPiece _whiteKing;

        public bool WhitesTurn { get; set; } = true;

        public IEnumerable<IPiece> BlackPieces => m_blackPieces;
        public IEnumerable<IPiece> WhitePieces => m_whitePieces;


        public Board()
        {
        }

        public Board(Board board)
        {
            WhitesTurn = board.WhitesTurn;
            foreach (var piece in board.m_whitePieces)
            {
                Add(piece.Copy());
            }
            foreach (var piece in board.m_blackPieces)
            {
                Add(piece.Copy());
            }
        }

        public IList<Move> GetAllMoves()
        {
            List<IPiece> pieces = WhitesTurn ? m_whitePieces : m_blackPieces;
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

        public void Move(Move move, bool switchSides = true)
        {
            Square LastMoveEnPassantTaret = EnPassantTarget;
            EnPassantTarget = new Square();
            IPiece piece = GetPieceOnSquare(move.Start);
            IPiece capturedPiece = GetPieceOnSquare(move.End);
            if (capturedPiece != null)
            {
                Remove(capturedPiece);
            }
            if (piece.IsKing)
            {
                if (move.Start == Square.WhiteKingStart)
                {
                    if (move.End == Square.WhiteKingCastledQueenside)
                    {
                        Move(new Move(this, Square.WhiteQueensRookStart, Square.WhiteQueensRookCastled), false);
                    }
                    else if (move.End == Square.WhiteKingCastledKingside)
                    {
                        Move(new Move(this, Square.WhiteKingsRookStart, Square.WhiteKingsRookCastled), false);
                    }
                    WhiteCanCastleKingside = false;
                    WhiteCanCastleQueenside = false;
                }
                else if (move.Start == Square.BlackKingStart)
                {
                    if (move.End == Square.BlackKingCastledQueenside)
                    {
                        Move(new Move(this, Square.BlackQueensRookStart, Square.BlackQueensRookCastled), false);
                    }
                    else if (move.End == Square.BlackKingCastledKingside)
                    {
                        Move(new Move(this, Square.BlackKingsRookStart, Square.BlackKingsRookCastled), false);
                    }
                    BlackCanCastleKingside = false;
                    BlackCanCastleQueenside = false;
                }
            }
            else if (piece.IsRook)
            {
                if (move.Start == Square.WhiteKingsRookStart)
                {
                    WhiteCanCastleKingside = false;
                }
                else if (move.Start == Square.WhiteQueensRookStart)
                {
                    WhiteCanCastleQueenside = false;
                }
                else if (move.Start == Square.BlackKingsRookStart)
                {
                    BlackCanCastleKingside = false;
                }
                else if (move.Start == Square.BlackQueensRookStart)
                {
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
                        Remove(piece);
                        Add(new Bishop(move.End, piece.White));
                        break;
                    case PieceType.Queen:
                        Remove(piece);
                        Add(new Queen(move.End, piece.White));
                        break;
                    case PieceType.Rook:
                        Remove(piece);
                        Add(new Rook(move.End, piece.White));
                        break;
                    default:
                        throw new NotImplementedException();
                }
                int dy = move.End.y - move.Start.y;
                if (LastMoveEnPassantTaret.x == move.End.x &&
                    LastMoveEnPassantTaret.y == move.End.y)
                {
                    capturedPiece = GetPieceOnSquare(move.End.Offset(0, -dy));
                    Remove(capturedPiece);
                }
                if (Math.Abs(dy) == 2)
                {
                    EnPassantTarget = new Square(move.End.x, move.End.y - dy / 2);
                }
            }
            piece.Pos = move.End;
            if (switchSides)
            {
                WhitesTurn = !WhitesTurn;
                if (WhitesTurn)
                {
                    FullMoveClock++;
                }
                HalfMoveClock++;
            }
            if (capturedPiece != null || piece.IsPawn)
            {
                HalfMoveClock = 0;
            }

        }

        public void Remove(IPiece piece)
        {
            if (piece.White)
            {
                m_whitePieces.Remove(piece);
            }
            else
            {
                m_blackPieces.Remove(piece);
            }
        }

        public void Add(IPiece piece)
        {
            if (piece.White)
            {
                m_whitePieces.Add(piece);
                if (piece.IsKing)
                {
                    if (_whiteKing != null)
                    {
                        throw new InvalidOperationException("Already have a white king");
                    }
                    _whiteKing = piece;
                }
            }
            else
            {
                m_blackPieces.Add(piece);
                if (piece.IsKing)
                {
                    if (_blackKing != null)
                    {
                        throw new InvalidOperationException("Already have a black king");
                    }
                    _blackKing = piece;
                }
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
            foreach (var piece in m_blackPieces)
            {
                if (square == piece.Pos)
                {
                    return piece;
                }
            }
            foreach (var piece in m_whitePieces)
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
