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
        public bool WhiteCanCastleKingside { get; set; } = true;
        public bool WhiteCanCastleQueenside { get; set; } = true;
        public bool BlackCanCastleKingside { get; set; } = true;
        public bool BlackCanCastleQueenside { get; set; } = true;
        public Square EnPassantTarget { get; set; } = new Square();
        public int HalfMoveClock { get; set; } = 0;
        public int FullMoveClock { get; set; } = 1;

        internal IPiece[,] Squares { get; } = new IPiece[8, 8];


        IPiece _blackKing;
        IPiece _whiteKing;

        public bool WhitesTurn { get; set; } = true;

        public IEnumerable<IPiece> BlackPieces => _blackPieces;
        public IEnumerable<IPiece> WhitePieces => _whitePieces;

        public Board()
        {
        }

        public Board(Board board)
        {
            WhitesTurn = board.WhitesTurn;
            foreach (var piece in board._whitePieces)
            {
                AddPiece(piece.Copy());
            }
            foreach (var piece in board._blackPieces)
            {
                AddPiece(piece.Copy());
            }

            Assert.IsTrue(_blackKing.IsKing && !_blackKing.White);
            Assert.IsTrue(_whiteKing.IsKing && _whiteKing.White);

            WhiteCanCastleKingside = board.WhiteCanCastleKingside;
            WhiteCanCastleQueenside = board.WhiteCanCastleQueenside;
            BlackCanCastleKingside = board.BlackCanCastleKingside;
            BlackCanCastleQueenside = board.BlackCanCastleQueenside;
            HalfMoveClock = board.HalfMoveClock;
            FullMoveClock = board.FullMoveClock;
            EnPassantTarget = board.EnPassantTarget;
        }

        public void UndoLastmove(Undo undo)
        {
            undo.UndoMove(this);
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
            MoveValidator validator = new MoveValidator();
            foreach (var move in generator.Moves)
            {
                if(validator.Validate(move))
                {
                    moves.Add(move);
                }
            }
            return moves;
        }

        public void Move(Move move, bool switchSides, Undo undo)
        {
            Square LastMoveEnPassantTaret = EnPassantTarget;
            if (EnPassantTarget.InBounds)
            {
                undo.EnPassantTarget = LastMoveEnPassantTaret;
            }
            EnPassantTarget = new Square();
            IPiece piece = GetPieceOnSquare(move.Start);
            IPiece capturedPiece = GetPieceOnSquare(move.End);
            bool done = false;

            undo.BlackCanCastleKingside = BlackCanCastleKingside;
            undo.BlackCanCastleQueenside = BlackCanCastleQueenside;
            undo.WhiteCanCastleKingside = WhiteCanCastleKingside;
            undo.WhiteCanCastleQueenside = WhiteCanCastleQueenside;
            if (capturedPiece != null)
            {
                undo.ReplaceCapturedPiece(capturedPiece, capturedPiece.Pos);
                RemovePiece(capturedPiece);
                if (capturedPiece.IsRook)
                {
                    if (move.End == Square.BlackKingsRookStart && BlackCanCastleKingside)
                    {
                        BlackCanCastleKingside = false;
                    }
                    else if (move.End == Square.BlackQueensRookStart && BlackCanCastleQueenside)
                    {
                        BlackCanCastleQueenside = false;
                    }
                    else if (move.End == Square.WhiteKingsRookStart && WhiteCanCastleKingside)
                    {
                        WhiteCanCastleKingside = false;
                    }
                    else if (move.End == Square.WhiteQueensRookStart && WhiteCanCastleQueenside)
                    {
                        WhiteCanCastleQueenside = false;
                    }
                }
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
                        undo.WhiteCanCastleKingside = true;
                    }
                    if (WhiteCanCastleQueenside)
                    {
                        undo.WhiteCanCastleQueenside = true;
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
                        undo.BlackCanCastleKingside = true;
                    }
                    if (BlackCanCastleQueenside)
                    {
                        undo.BlackCanCastleQueenside = true;
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
                        undo.WhiteCanCastleKingside = true;
                    }
                    WhiteCanCastleKingside = false;
                }
                else if (move.Start == Square.WhiteQueensRookStart)
                {
                    if (WhiteCanCastleQueenside)
                    {
                        undo.WhiteCanCastleQueenside = true;
                    }
                    WhiteCanCastleQueenside = false;
                }
                else if (move.Start == Square.BlackKingsRookStart)
                {
                    if (BlackCanCastleKingside)
                    {
                        undo.BlackCanCastleKingside = true;
                    }
                    BlackCanCastleKingside = false;
                }
                else if (move.Start == Square.BlackQueensRookStart)
                {
                    if (BlackCanCastleQueenside)
                    {
                        undo.BlackCanCastleQueenside = true;
                    }
                    BlackCanCastleQueenside = false;
                }
            }
            else if (piece.IsPawn)
            {
                if (move.Promoted != PieceType.None)
                {
                    done = true;
                    undo.ReplacePromotedPawn(piece, piece.Pos);
                    undo.RemovePromotedPieceOnSquare(move.End);
                    switch (move.Promoted)
                    {
                        case PieceType.None:
                            break;
                        case PieceType.Bishop:
                            RemovePiece(piece);
                            AddPiece(new Bishop(move.End, piece.White));
                            break;
                        case PieceType.Queen:
                            RemovePiece(piece);
                            AddPiece(new Queen(move.End, piece.White));
                            break;
                        case PieceType.Rook:
                            RemovePiece(piece);
                            AddPiece(new Rook(move.End, piece.White));
                            break;
                        case PieceType.Knight:
                            RemovePiece(piece);
                            AddPiece(new Knight(move.End, piece.White));
                            break;
                        default:
                            throw new NotImplementedException();
                    }
                }
                int dy = move.End.y - move.Start.y;
                if (LastMoveEnPassantTaret.x == move.End.x &&
                    LastMoveEnPassantTaret.y == move.End.y)
                {
                    capturedPiece = GetPieceOnSquare(move.End.Offset(0, -dy));
                    undo.ReplaceCapturedPiece(capturedPiece, capturedPiece.Pos);
                    RemovePiece(capturedPiece);
                }
                if (Math.Abs(dy) == 2)
                {
                    EnPassantTarget = new Square(move.End.x, move.End.y - dy / 2);
                }
            }
            if (!done)
            {
                undo.MovePieceBackTo(piece, piece.Pos);
                piece.Pos = move.End;
                Squares[move.Start.x - 1, move.Start.y - 1] = null;
                Squares[move.End.x - 1, move.End.y - 1] = piece;
            }

            undo.WhitesTurn = WhitesTurn;
            undo.HalfMoveClock = HalfMoveClock;
            undo.FullMoveClock = FullMoveClock;
            if (switchSides)
            {

                WhitesTurn = !WhitesTurn;
                HalfMoveClock++;
                if (WhitesTurn)
                {
                    FullMoveClock++;
                }
            }
            int halfMoveClock = HalfMoveClock;
            if (capturedPiece != null || piece.IsPawn)
            {
                HalfMoveClock = 0;
            }

        }

        public void RemovePiece(IPiece piece)
        {
            if (piece.White)
            {
                _whitePieces.Remove(piece);
                Squares[piece.Pos.x - 1, piece.Pos.y - 1] = null;

            }
            else
            {
                _blackPieces.Remove(piece);
                Squares[piece.Pos.x - 1, piece.Pos.y - 1] = null;
            }
        }

        public void AddPiece(IPiece piece)
        {
#if DEBUG
            Assert.IsTrue(!_whitePieces.Contains(piece));
            Assert.IsTrue(!_blackPieces.Contains(piece));
#endif
            if (piece.White)
            {
                _whitePieces.Add(piece);
                Squares[piece.Pos.x - 1, piece.Pos.y - 1] = piece;
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
                _blackPieces.Add(piece);
                Squares[piece.Pos.x - 1, piece.Pos.y - 1] = piece;
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
            IPiece pieceFromArray = Squares[square.x - 1, square.y - 1];
            IPiece pieceFromList = null;

#if DEBUG
            IList<IPiece> firstPieces;
            IList<IPiece> secondPieces;
            if (square.y > 4)
            {
                firstPieces = _blackPieces;
                secondPieces = _whitePieces;
            }
            else
            {
                firstPieces = _whitePieces;
                secondPieces = _blackPieces;
            }
            foreach (var piece in firstPieces)
            {
                if (square == piece.Pos)
                {
                    Assert.IsNull(pieceFromList);
                    pieceFromList = piece;
                }
            }
            foreach (var piece in secondPieces)
            {
                if (square == piece.Pos)
                {
                    Assert.IsNull(pieceFromList);
                    pieceFromList = piece;
                }
            }
            Assert.AreSame(pieceFromList, pieceFromArray);
#endif
            return Squares[square.x - 1, square.y - 1];
        }

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
            bool notWhitesTurn = !WhitesTurn;
            if (GetPieceOnSquareOnSide(square.Offset(1, 2), notWhitesTurn) as Knight != null ||
                GetPieceOnSquareOnSide(square.Offset(-1, 2), notWhitesTurn) as Knight != null ||
                GetPieceOnSquareOnSide(square.Offset(1, -2), notWhitesTurn) as Knight != null ||
                GetPieceOnSquareOnSide(square.Offset(-1, -2), notWhitesTurn) as Knight != null ||
                GetPieceOnSquareOnSide(square.Offset(2, 1), notWhitesTurn) as Knight != null ||
                GetPieceOnSquareOnSide(square.Offset(-2, 1), notWhitesTurn) as Knight != null ||
                GetPieceOnSquareOnSide(square.Offset(2, -1), notWhitesTurn) as Knight != null ||
                GetPieceOnSquareOnSide(square.Offset(-2, -1), notWhitesTurn) as Knight != null)
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

        public string ToLongString()
        {
            StringBuilder sb = new StringBuilder();
            FenCharPieceVisitor fenCharPieceVisitor = new FenCharPieceVisitor();
            for (int y = 8; y > 0; y--)
            {
                for (int x = 1; x <= 8; x++)
                {
                    IPiece piece = Squares[x - 1, y - 1];
                    if (piece == null)
                    {
                        sb.Append(". ");
                    }
                    else
                    {
                        sb.Append(fenCharPieceVisitor.GetFenChar(piece));
                        sb.Append(' ');
                    }
                }
                sb.AppendLine();
            }
            return sb.ToString();
        }
    }

}
