using System;
using System.Collections.Generic;
using Blackmitten.Elliot.Backend;
using BlackMitten.Elliot.StockfishEngine;
using System.Threading;
using System.Text;
using Blackmitten.Menzel;

namespace ElliotTests
{
    public class A_QuickTests
    {

        public static void SquareEquality()
        {
            Square s1 = new Square(0, 0);
            Square s2 = new Square(0, 1);
            Square s3 = new Square(0, 1);
            Square s4 = new Square(1, 1);
            Square s5 = new Square(1, 1);

            Assert.AreEqual(s1, s1);
            Assert.AreNotEqual(s1, s2);
            Assert.AreEqual(s2, s3);
            Assert.AreNotEqual(s3, s4);
            Assert.AreEqual(s4, s5);
        }

        public static void SquareCopyAndChange()
        {
            Square s1 = new Square(1, 1);
            Square s2 = s1;
            Assert.AreEqual(s1, s2);
            s2.x++;
            Assert.AreNotEqual(s1, s2);
        }

        public static void SquareOffset()
        {
            Square s6 = new Square(4, 4);
            Square s7 = new Square(2, 6);
            Square s8 = s6.Offset(-2, 2);
            Assert.AreNotEqual(s6, s7);
            Assert.AreEqual(s7, s8);
        }

        public static void SquareCopy()
        {
            Square s1 = new Square(4, 4);
            Square s2 = s1;
            Assert.AreEqual(s1, s2);
            Assert.AreNotSame(s1, s2);
        }

        public static void SquareConstructFromNotation()
        {
            Assert.AreEqual(new Square("a1"), new Square(1, 1));
            Assert.AreNotEqual(new Square("h1"), new Square(1, 1));
            Assert.AreEqual(new Square("h1"), new Square(8, 1));
            Assert.AreEqual(new Square("h8"), new Square(8, 8));
            Assert.AreEqual(new Square("a8"), new Square(1, 8));
        }

        public static void SquareToString()
        {
            Assert.AreEqual(new Square("a1").ToString(), "a1");
            Assert.AreEqual(new Square("h5").ToString(), "h5");
        }

        public static void InitGameFenString()
        {
            Board board = BoardFactory.InitNewGame();
            string fen = board.GetFenString();

            string startingFen = "rnbqkbnr/pppppppp/8/8/8/8/PPPPPPPP/RNBQKBNR w KQkq - 0 1";

            Assert.AreEqual(fen, startingFen);
        }

        public static void BoardFromFenString()
        {
            string fen = "rnbqkbnr/1ppppppp/8/p7/4P3/2N5/P1PP1PPP/R1BQKBNR b KQkq - 1 2";
            Board board = BoardFactory.BoardFromFenString(fen);
            string boardFen = board.GetFenString();
            Assert.AreEqual(fen, boardFen);
        }

        public static void UndoMove1()
        {
            string fen = "rnbqkbnr/1ppppppp/8/p7/4P3/2N5/P1PP1PPP/R1BQKBNR b KQkq - 1 2";
            Board board = BoardFactory.BoardFromFenString(fen);
            var undo = new Undo();
            board.Move(new Move(board, "b7b5"), true, undo);
            board.UndoLastmove( undo );
            string fenAfterUndo = board.GetFenString();
            Assert.AreEqual(fen, fenAfterUndo);
        }

        public static void UndoMove2()
        {
            string fen = "rnbqkbnr/1ppppppp/p7/8/4P3/8/P1PP1PPP/RNBQKBNR w KQkq - 0 2";
            Board board = BoardFactory.BoardFromFenString(fen);
            string fenCheck = board.GetFenString();
            Assert.IsTrue(fen == fenCheck);
            var undo1 = new Undo();
            var undo2 = new Undo();
            board.Move(new Move(board, "a2a3"), true, undo1);
            board.Move(new Move(board, "b7b6"), true, undo2);
            board.UndoLastmove(undo2);
            board.UndoLastmove(undo1);
            string fenAfterUndo = board.GetFenString();
            Assert.AreEqual(fen, fenAfterUndo);
        }

        public static void GetAllMoves()
        {
            Board board = BoardFactory.InitNewGame();
            var moves = board.GetAllMoves();

            Assert.IsTrue(moves.Count == 20);
            MoveValidator moveValidator = new MoveValidator();
            foreach(var move in moves)
            {
                moveValidator.Validate(move);
            }
        }

        public static void SquareSubtraction()
        {
            Square s1 = new Square(2, 5);
            Square s2 = new Square(6, 1);
            Vector v = s1 - s2;
            Assert.IsTrue(v.x == -4);
            Assert.IsTrue(v.y == 4);
        }

        public static void TestGetPieceOnSquare()
        {
            Board board = BoardFactory.InitNewGame();
            Assert.IsTrue(TestPiece(board.GetPieceOnSquare(new Square(1, 1)), (piece) => piece.White && piece.IsRook));
            Assert.IsTrue(TestPiece(board.GetPieceOnSquare(new Square(2, 1)), (piece) => piece.White && piece.IsKnight));
            Assert.IsTrue(TestPiece(board.GetPieceOnSquare(new Square(3, 1)), (piece) => piece.White && piece.IsBishop));
            Assert.IsTrue(TestPiece(board.GetPieceOnSquare(new Square(4, 1)), (piece) => piece.White && piece.IsQueen));
            Assert.IsTrue(TestPiece(board.GetPieceOnSquare(new Square(5, 1)), (piece) => piece.White && piece.IsKing));
            Assert.IsTrue(TestPiece(board.GetPieceOnSquare(new Square(6, 1)), (piece) => piece.White && piece.IsBishop));
            Assert.IsTrue(TestPiece(board.GetPieceOnSquare(new Square(7, 1)), (piece) => piece.White && piece.IsKnight));
            Assert.IsTrue(TestPiece(board.GetPieceOnSquare(new Square(8, 1)), (piece) => piece.White && piece.IsRook));
            Assert.IsTrue(TestPiece(board.GetPieceOnSquare(new Square(1, 8)), (piece) => !piece.White && piece.IsRook));
            Assert.IsTrue(TestPiece(board.GetPieceOnSquare(new Square(2, 8)), (piece) => !piece.White && piece.IsKnight));
            Assert.IsTrue(TestPiece(board.GetPieceOnSquare(new Square(3, 8)), (piece) => !piece.White && piece.IsBishop));
            Assert.IsTrue(TestPiece(board.GetPieceOnSquare(new Square(4, 8)), (piece) => !piece.White && piece.IsQueen));
            Assert.IsTrue(TestPiece(board.GetPieceOnSquare(new Square(5, 8)), (piece) => !piece.White && piece.IsKing));
            Assert.IsTrue(TestPiece(board.GetPieceOnSquare(new Square(6, 8)), (piece) => !piece.White && piece.IsBishop));
            Assert.IsTrue(TestPiece(board.GetPieceOnSquare(new Square(7, 8)), (piece) => !piece.White && piece.IsKnight));
            Assert.IsTrue(TestPiece(board.GetPieceOnSquare(new Square(8, 8)), (piece) => !piece.White && piece.IsRook));
            for (int x = 1; x <= 8; x++)
            {
                Assert.IsTrue(TestPiece(board.GetPieceOnSquare(new Square(x, 2)), (piece) => piece.White && piece.IsPawn));
                for (int y = 3; y <= 6; y++)
                {
                    Assert.IsTrue(board.GetPieceOnSquare(new Square(x, y)) == null);
                }
                Assert.IsTrue(TestPiece(board.GetPieceOnSquare(new Square(x, 7)), (piece) => !piece.White && piece.IsPawn));
            }

            var ui = new MockUI();
            MoveHelper(board, "e2e4", ui);
            MoveHelper(board, "d7d5", ui);
            MoveHelper(board, "d2d4", ui);
        }

        static void MoveHelper(Board board, string moveString, IUserInterface ui)
        {
            var move = new Move(board, moveString);
            var validator = new MoveValidator();
            var fenCharPieceVisitor = new FenCharPieceVisitor();
            if (validator.Validate(move))
            {
                var piece0 = board.GetPieceOnSquare(move.Start);
                Undo undo = new Undo();
                board.Move(move, true, undo);
                Assert.IsTrue(board.GetPieceOnSquare(move.Start) == null);
                Assert.IsTrue(TestPiece(board.GetPieceOnSquare(move.End), (piece) => fenCharPieceVisitor.GetFenChar(piece) == fenCharPieceVisitor.GetFenChar(piece0)));
            }
            else
            {
                throw new InvalidMoveException(moveString);
            }
            ui.Board = board;
        }

        static bool TestPiece(IPiece piece, Func<IPiece,bool> test)
        {
            return test(piece);
        }



        public static void TestThreatening()
        {
            List<string> strings = new List<string>();
            Board board = BoardFactory.InitNewGame();
            board.Move(new Move(board, new Square(4, 2), new Square(4, 4)), true, new Undo());
            board.WhitesTurn = false;
            for (int y = 8; y >= 1; y--)
            {
                StringBuilder sb = new StringBuilder();
                for (int x = 1; x <= 8; x++)
                {
                    Square s = new Square(x, y);
                    sb.Append(board.IsSquareThreatened(s,true) ? "X" : "o" );
                }
                strings.Add(sb.ToString());
            }
            Assert.IsTrue(board.IsSquareThreatened(new Square(8, 6), true));
        }

        public static void TestStartingPositions()
        {
            Board b = BoardFactory.InitNewGame();
            IPiece whiteKing = b.GetPieceOnSquare(Square.WhiteKingStart);
            IPiece blackKing = b.GetPieceOnSquare(Square.BlackKingStart);
            Assert.IsTrue(whiteKing.White);
            Assert.IsTrue(whiteKing.IsKing);

            Assert.IsTrue(!blackKing.White);
            Assert.IsTrue(blackKing.IsKing);

        }

        public static void TestCheck()
        {
            Board b = BoardFactory.BoardFromFenString("4k3/8/8/8/4K2R/8/8/R7 b - - 7 4");
            Assert.IsTrue(!b.BlackInCheck);
            Assert.IsTrue(!b.WhiteInCheck);
        }


    }
}
