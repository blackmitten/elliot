using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Blackmitten.Elliot.Backend
{
    public class Board
    {
        IList<IPiece> m_pieces = new List<IPiece>();

        public bool WhitesTurn { get; set; }

        public static Board InitNewGame()
        {
            Board b = new Board();
            for (int i = 1; i < 9; i++)
            {
                b.m_pieces.Add(new Pawn(new Square(i, 2), false));
                b.m_pieces.Add(new Pawn(new Square(i, 7), true));
            }
            b.m_pieces.Add(new Rook(new Square(1, 1), false));
            b.m_pieces.Add(new Rook(new Square(8, 1), false));
            b.m_pieces.Add(new Knight(new Square(2, 1), false));
            b.m_pieces.Add(new Knight(new Square(7, 1), false));
            b.m_pieces.Add(new Bishop(new Square(3, 1), false));
            b.m_pieces.Add(new Bishop(new Square(6, 1), false));
            b.m_pieces.Add(new Queen(new Square(4, 1), false));
            b.m_pieces.Add(new King(new Square(5, 1), false));

            b.m_pieces.Add(new Rook(new Square(1, 8), true));
            b.m_pieces.Add(new Rook(new Square(8, 8), true));
            b.m_pieces.Add(new Knight(new Square(2, 8), true));
            b.m_pieces.Add(new Knight(new Square(7, 8), true));
            b.m_pieces.Add(new Bishop(new Square(3, 8), true));
            b.m_pieces.Add(new Bishop(new Square(6, 8), true));
            b.m_pieces.Add(new Queen(new Square(4, 8), true));
            b.m_pieces.Add(new King(new Square(5, 8), true));
            b.WhitesTurn = true;
            return b;
        }

        public IList<IPiece> Pieces => m_pieces;

    }
}
