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
                m_whitePieces.Add(piece.Copy());
            }
            foreach (var piece in board.m_blackPieces)
            {
                m_blackPieces.Add(piece.Copy());
            }
        }

        public static Board InitNewGame()
        {
            Board b = new Board();
            for (int i = 1; i < 9; i++)
            {
                b.m_blackPieces.Add(new Pawn(new Square(i, 7), false));
                b.m_whitePieces.Add(new Pawn(new Square(i, 2), true));
            }
            b.m_blackPieces.Add(new Rook(new Square(1, 8), false));
            b.m_blackPieces.Add(new Rook(new Square(8, 8), false));
            b.m_blackPieces.Add(new Knight(new Square(2, 8), false));
            b.m_blackPieces.Add(new Knight(new Square(7, 8), false));
            b.m_blackPieces.Add(new Bishop(new Square(3, 8), false));
            b.m_blackPieces.Add(new Bishop(new Square(6, 8), false));
            b.m_blackPieces.Add(new Queen(new Square(4, 8), false));
            b.m_blackPieces.Add(new King(new Square(5, 8), false));

            b.m_whitePieces.Add(new Rook(new Square(1, 1), true));
            b.m_whitePieces.Add(new Rook(new Square(8, 1), true));
            b.m_whitePieces.Add(new Knight(new Square(2, 1), true));
            b.m_whitePieces.Add(new Knight(new Square(7, 1), true));
            b.m_whitePieces.Add(new Bishop(new Square(3, 1), true));
            b.m_whitePieces.Add(new Bishop(new Square(6, 1), true));
            b.m_whitePieces.Add(new Queen(new Square(4, 1), true));
            b.m_whitePieces.Add(new King(new Square(5, 1), true));
            b.WhitesTurn = true;
            b.m_whitePieces = b.m_whitePieces.OrderBy(p => p.Pos.y).ThenBy(p => p.Pos.x).ToList();
            b.m_blackPieces = b.m_blackPieces.OrderBy(p => p.Pos.y).ThenBy(p => p.Pos.x).ToList();
            return b;
        }

        internal void Move(Move move, bool switchSides = true)
        {
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
                        WhiteCanCastleKingside = false;
                        WhiteCanCastleQueenside = false;
                    }
                    else if (move.End == Square.WhiteKingCastledKingside)
                    {
                        Move(new Move(this, Square.WhiteKingsRookStart, Square.WhiteKingsRookCastled), false);
                        WhiteCanCastleKingside = false;
                        WhiteCanCastleQueenside = false;
                    }
                }
                else if (move.Start == Square.BlackKingStart)
                {
                    if (move.End == Square.BlackKingCastledQueenside)
                    {
                        Move(new Move(this, Square.BlackQueensRookStart, Square.BlackQueensRookCastled), false);
                        BlackCanCastleKingside = false;
                        BlackCanCastleQueenside = false;
                    }
                    else if (move.End == Square.BlackKingCastledKingside)
                    {
                        Move(new Move(this, Square.BlackKingsRookStart, Square.BlackKingsRookCastled), false);
                        BlackCanCastleKingside = false;
                        BlackCanCastleQueenside = false;
                    }
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
                    default:
                        throw new NotImplementedException();
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

        void Remove(IPiece piece)
        {
            if(piece.White)
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
            }
            else
            {
                m_blackPieces.Add(piece);
            }
        }

        public IPiece GetPieceOnSquare(Square square)
        {
            if (!square.InBounds)
            {
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
