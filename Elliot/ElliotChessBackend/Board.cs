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
        List<IPiece> m_pieces = new List<IPiece>();

        public bool WhitesTurn { get; set; }

        public Board()
        {
        }

        public Board(Board board)
        {
            this.WhitesTurn = board.WhitesTurn;
            foreach (var piece in board.Pieces)
            {
                this.m_pieces.Add(piece.Copy());
            }
        }

        public static Board InitNewGame()
        {
            Board b = new Board();
            for (int i = 1; i < 9; i++)
            {
                b.m_pieces.Add(new Pawn(new Square(i, 7), false));
                b.m_pieces.Add(new Pawn(new Square(i, 2), true));
            }
            b.m_pieces.Add(new Rook(new Square(1, 8), false));
            b.m_pieces.Add(new Rook(new Square(8, 8), false));
            b.m_pieces.Add(new Knight(new Square(2, 8), false));
            b.m_pieces.Add(new Knight(new Square(7, 8), false));
            b.m_pieces.Add(new Bishop(new Square(3, 8), false));
            b.m_pieces.Add(new Bishop(new Square(6, 8), false));
            b.m_pieces.Add(new Queen(new Square(4, 8), false));
            b.m_pieces.Add(new King(new Square(5, 8), false));

            b.m_pieces.Add(new Rook(new Square(1, 1), true));
            b.m_pieces.Add(new Rook(new Square(8, 1), true));
            b.m_pieces.Add(new Knight(new Square(2, 1), true));
            b.m_pieces.Add(new Knight(new Square(7, 1), true));
            b.m_pieces.Add(new Bishop(new Square(3, 1), true));
            b.m_pieces.Add(new Bishop(new Square(6, 1), true));
            b.m_pieces.Add(new Queen(new Square(4, 1), true));
            b.m_pieces.Add(new King(new Square(5, 1), true));
            b.WhitesTurn = true;
            b.m_pieces = b.m_pieces.OrderBy(p => p.Pos.y).ThenBy(p => p.Pos.x).ToList();
            return b;
        }

        internal void Move(Move move)
        {
            IPiece piece = GetPieceOnSquare(move.Start);
            piece.Pos = move.End;
            WhitesTurn = !WhitesTurn;
        }

        public IList<IPiece> Pieces => m_pieces;

        /*
        public void MovePiece(Square startSquare, Square endSquare)
        {
            IPiece piece = GetPieceOnSquare(startSquare);
            piece.Pos = endSquare;
            WhitesTurn = !WhitesTurn;
        }
        */

        public IPiece GetPieceOnSquare(Square square)
        {
            foreach (var piece in m_pieces)
            {
                if (square == piece.Pos)
                {
                    return piece;
                }
            }
            return null;
        }

    }

}
