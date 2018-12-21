using System;
using System.Collections.Generic;
using Blackmitten.Elliot.Backend;
using BlackMitten.Elliot.StockfishEngine;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Threading;
using System.Text;

namespace ElliotTests
{
    [TestClass]
    public class A_QuickTests
    {
        public static string _stockFishBinPath = @"C:\bin\stockfish\stockfish_9_x64.exe";

        [TestMethod]
        public void SquareEquality()
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

        [TestMethod]
        public void SquareCopyAndChange()
        {
            Square s1 = new Square(1, 1);
            Square s2 = s1;
            Assert.AreEqual(s1, s2);
            s2.x++;
            Assert.AreNotEqual(s1, s2);
        }

        [TestMethod]
        public void SquareOffset()
        {
            Square s6 = new Square(4, 4);
            Square s7 = new Square(2, 6);
            Square s8 = s6.Offset(-2, 2);
            Assert.AreNotEqual(s6, s7);
            Assert.AreEqual(s7, s8);
        }

        [TestMethod]
        public void SquareCopy()
        {
            Square s1 = new Square(4, 4);
            Square s2 = s1;
            Assert.AreEqual(s1, s2);
            Assert.AreNotSame(s1, s2);
        }

        [TestMethod]
        public void SquareConstructFromNotation()
        {
            Assert.AreEqual(new Square("a1"), new Square(1, 1));
            Assert.AreNotEqual(new Square("h1"), new Square(1, 1));
            Assert.AreEqual(new Square("h1"), new Square(8, 1));
            Assert.AreEqual(new Square("h8"), new Square(8, 8));
            Assert.AreEqual(new Square("a8"), new Square(1, 8));
        }

        [TestMethod]
        public void SquareToString()
        {
            Assert.AreEqual(new Square("a1").ToString(), "a1");
            Assert.AreEqual(new Square("h5").ToString(), "h5");
        }

        [TestMethod]
        public void InitGameFenString()
        {
            Board board = BoardFactory.InitNewGame();
            string fen = board.GetFenString();

            string startingFen = "rnbqkbnr/pppppppp/8/8/8/8/PPPPPPPP/RNBQKBNR w KQkq - 0 1";

            Assert.AreEqual(fen, startingFen);
        }

        [TestMethod]
        public void BoardFromFenString()
        {
            string fen = "rnbqkbnr/1ppppppp/8/p7/4P3/2N5/P1PP1PPP/R1BQKBNR b KQkq - 1 2";
            Board board = BoardFactory.BoardFromFenString(fen);
            string boardFen = board.GetFenString();
            Assert.AreEqual(fen, boardFen);
        }

        [TestMethod]
        public void UndoMove1()
        {
            string fen = "rnbqkbnr/1ppppppp/8/p7/4P3/2N5/P1PP1PPP/R1BQKBNR b KQkq - 1 2";
            Board board = BoardFactory.BoardFromFenString(fen);
            var undo = new List<Action>();
            board.Move(new Move(board, "b7b5"), true, undo);
            board.UndoLastmove( undo );
            string fenAfterUndo = board.GetFenString();
            Assert.AreEqual(fen, fenAfterUndo);
        }

        [TestMethod]
        public void UndoMove2()
        {
            string fen = "rnbqkbnr/1ppppppp/p7/8/4P3/8/P1PP1PPP/RNBQKBNR w KQkq - 0 2";
            Board board = BoardFactory.BoardFromFenString(fen);
            string fenCheck = board.GetFenString();
            Assert.IsTrue(fen == fenCheck);
            var undo1 = new List<Action>();
            var undo2 = new List<Action>();
            board.Move(new Move(board, "a2a3"), true, undo1);
            board.Move(new Move(board, "b7b6"), true, undo2);
            board.UndoLastmove(undo2);
            board.UndoLastmove(undo1);
            string fenAfterUndo = board.GetFenString();
            Assert.AreEqual(fen, fenAfterUndo);
        }

        [TestMethod]
        public void GetAllMoves()
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

        [TestMethod]
        public void SquareSubtraction()
        {
            Square s1 = new Square(2, 5);
            Square s2 = new Square(6, 1);
            Vector v = s1 - s2;
            Assert.IsTrue(v.x == -4);
            Assert.IsTrue(v.y == 4);
        }

        [TestMethod]
        public void TestThreatening()
        {
            List<string> strings = new List<string>();
            Board board = BoardFactory.InitNewGame();
            board.Move(new Move(board, new Square(4, 2), new Square(4, 4)), true, null);
            board.WhitesTurn = false;
            for (int y = 8; y >= 1; y--)
            {
                StringBuilder sb = new StringBuilder();
                for (int x = 1; x <= 8; x++)
                {
                    Square s = new Square(x, y);
                    sb.Append(board.IsSquareThreatened(s) ? "X" : "o");
                }
                strings.Add(sb.ToString());
            }
            Assert.IsTrue(board.IsSquareThreatened(new Square(8, 6)));
            int i = 1;
        }

        [TestMethod]
        public void TestStartingPositions()
        {
            Board b = BoardFactory.InitNewGame();
            IPiece whiteKing = b.GetPieceOnSquare(Square.WhiteKingStart);
            IPiece blackKing = b.GetPieceOnSquare(Square.BlackKingStart);
            Assert.IsTrue(whiteKing.White);
            Assert.IsTrue(whiteKing.IsKing);

            Assert.IsTrue(!blackKing.White);
            Assert.IsTrue(blackKing.IsKing);

        }



    }
}
