using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Blackmitten.Elliot.Backend
{
    public static class BoardFactory
    {
        public static Board InitNewGame()
        {
            Board b = new Board();
            for (int i = 1; i < 9; i++)
            {
                b.AddPiece(new Pawn(new Square(i, 7), false), null);
                b.AddPiece(new Pawn(new Square(i, 2), true), null);
            }
            b.AddPiece(new Rook(new Square(1, 8), false), null);
            b.AddPiece(new Rook(new Square(8, 8), false), null);
            b.AddPiece(new Knight(new Square(2, 8), false), null);
            b.AddPiece(new Knight(new Square(7, 8), false), null);
            b.AddPiece(new Bishop(new Square(3, 8), false), null);
            b.AddPiece(new Bishop(new Square(6, 8), false), null);
            b.AddPiece(new Queen(new Square(4, 8), false), null);
            b.AddPiece(new King(new Square(5, 8), false), null);

            b.AddPiece(new Rook(new Square(1, 1), true), null);
            b.AddPiece(new Rook(new Square(8, 1), true), null);
            b.AddPiece(new Knight(new Square(2, 1), true), null);
            b.AddPiece(new Knight(new Square(7, 1), true), null);
            b.AddPiece(new Bishop(new Square(3, 1), true), null);
            b.AddPiece(new Bishop(new Square(6, 1), true), null);
            b.AddPiece(new Queen(new Square(4, 1), true), null);
            b.AddPiece(new King(new Square(5, 1), true), null);
            b.WhitesTurn = true;
//            b.m_whitePieces = b.m_whitePieces.OrderBy(p => p.Pos.y).ThenBy(p => p.Pos.x).ToList();
//            b.m_blackPieces = b.m_blackPieces.OrderBy(p => p.Pos.y).ThenBy(p => p.Pos.x).ToList();
            return b;
        }

        public static Board BuildEnPassantTest()
        {
            Board board = new Board();
            board.AddPiece(new King(new Square(1, 1), true), null);
            board.AddPiece(new Pawn(new Square(6, 4), true), null);
            board.AddPiece(new King(new Square(1, 8), false), null);
            board.AddPiece(new Pawn(new Square(7, 4), false), null);
            board.BlackCanCastleKingside = false;
            board.BlackCanCastleQueenside = false;
            board.WhiteCanCastleKingside = false;
            board.WhiteCanCastleQueenside = false;
            board.WhitesTurn = false;
            board.EnPassantTarget = new Square(6, 3);
            return board;
        }

    }
}
