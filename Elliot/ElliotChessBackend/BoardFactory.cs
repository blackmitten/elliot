using Blackmitten.Elliot.Backend.Exceptions;
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
                b.AddPiece(new Pawn(new Square(i, 7), false));
                b.AddPiece(new Pawn(new Square(i, 2), true));
            }
            b.AddPiece(new Rook(new Square(1, 8), false));
            b.AddPiece(new Rook(new Square(8, 8), false));
            b.AddPiece(new Knight(new Square(2, 8), false));
            b.AddPiece(new Knight(new Square(7, 8), false));
            b.AddPiece(new Bishop(new Square(3, 8), false));
            b.AddPiece(new Bishop(new Square(6, 8), false));
            b.AddPiece(new Queen(new Square(4, 8), false));
            b.AddPiece(new King(new Square(5, 8), false));

            b.AddPiece(new Rook(new Square(1, 1), true));
            b.AddPiece(new Rook(new Square(8, 1), true));
            b.AddPiece(new Knight(new Square(2, 1), true));
            b.AddPiece(new Knight(new Square(7, 1), true));
            b.AddPiece(new Bishop(new Square(3, 1), true));
            b.AddPiece(new Bishop(new Square(6, 1), true));
            b.AddPiece(new Queen(new Square(4, 1), true));
            b.AddPiece(new King(new Square(5, 1), true));
            b.WhitesTurn = true;
            //            b.m_whitePieces = b.m_whitePieces.OrderBy(p => p.Pos.y).ThenBy(p => p.Pos.x).ToList();
            //            b.m_blackPieces = b.m_blackPieces.OrderBy(p => p.Pos.y).ThenBy(p => p.Pos.x).ToList();
            return b;
        }

        public static Board BuildEnPassantTest()
        {
            Board board = new Board();
            board.AddPiece(new King(new Square(1, 1), true));
            board.AddPiece(new Pawn(new Square(6, 4), true));
            board.AddPiece(new King(new Square(1, 8), false));
            board.AddPiece(new Pawn(new Square(7, 4), false));
            board.BlackCanCastleKingside = false;
            board.BlackCanCastleQueenside = false;
            board.WhiteCanCastleKingside = false;
            board.WhiteCanCastleQueenside = false;
            board.WhitesTurn = false;
            board.EnPassantTarget = new Square(6, 3);
            return board;
        }

        public static Board BoardFromFenString(string fen)
        {
            Board board = new Board();
            string[] bits = fen.Split(' ');
            string[] rows = bits[0].Split('/');
            if (rows.Length != 8)
            {
                throw new InvalidFenException();
            }
            int x = 1;
            int y = 9;
            for (int rowIndex = 0; rowIndex < 8; rowIndex++)
            {
                x = 1;
                y--;
                string rowString = rows[rowIndex];
                for (int rowCharIndex = 0; rowCharIndex < rowString.Length; rowCharIndex++)
                {
                    char c = rowString[rowCharIndex];
                    switch (c)
                    {
                        case 'r':
                            board.AddPiece(new Rook(new Square(x++, y), false));
                            break;
                        case 'n':
                            board.AddPiece(new Knight(new Square(x++, y), false));
                            break;
                        case 'b':
                            board.AddPiece(new Bishop(new Square(x++, y), false));
                            break;
                        case 'q':
                            board.AddPiece(new Queen(new Square(x++, y), false));
                            break;
                        case 'k':
                            board.AddPiece(new King(new Square(x++, y), false));
                            break;
                        case 'p':
                            board.AddPiece(new Pawn(new Square(x++, y), false));
                            break;
                        case 'R':
                            board.AddPiece(new Rook(new Square(x++, y), true));
                            break;
                        case 'N':
                            board.AddPiece(new Knight(new Square(x++, y), true));
                            break;
                        case 'B':
                            board.AddPiece(new Bishop(new Square(x++, y), true));
                            break;
                        case 'Q':
                            board.AddPiece(new Queen(new Square(x++, y), true));
                            break;
                        case 'K':
                            board.AddPiece(new King(new Square(x++, y), true));
                            break;
                        case 'P':
                            board.AddPiece(new Pawn(new Square(x++, y), true));
                            break;
                        default:
                            if (char.IsDigit(c))
                            {
                                int n = int.Parse(c.ToString());
                                x += n;
                            }
                            else
                            {
                                throw new NotImplementedException();
                            }
                            break;
                    }
                }
            }
            switch (bits[1])
            {
                case "w": board.WhitesTurn = true; break;
                case "b": board.WhitesTurn = false; break;
                default:
                    throw new InvalidOperationException();
            }
            board.WhiteCanCastleKingside = bits[2].Contains("K");
            board.WhiteCanCastleQueenside = bits[2].Contains("Q");
            board.BlackCanCastleKingside = bits[2].Contains("k");
            board.BlackCanCastleQueenside = bits[2].Contains("q");

            if (bits[3] == "-")
            {
                board.EnPassantTarget = new Square(0, 0);
            }
            else
            {
                board.EnPassantTarget = new Square(bits[3]);
            }
            board.FullMoveClock = int.Parse(bits[5]);
            board.HalfMoveClock = int.Parse(bits[4]);

            return board;
        }
    }
}
