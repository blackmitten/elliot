using Blackmitten.Elliot.Backend;
using BlackMitten.Elliot.StockfishEngine;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Threading;

namespace ElliotTests
{
    [TestClass]
    public class UnitTest1
    {
        string _stockFishBinPath = @"C:\bin\stockfish\stockfish_9_x64.exe";

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
        public void FenString()
        {
            Board board = BoardFactory.InitNewGame();
            string fen = board.GetFenString();

            string startingFen = "rnbqkbnr/pppppppp/8/8/8/8/PPPPPPPP/RNBQKBNR w KQkq - 0 1";

            Assert.AreEqual(fen, startingFen);
        }

        [TestMethod]
        public void EnPassant()
        {
            //   a b c d e f g h
            // 8   ■   k   ■   ■
            // 7 ■   ■   ■   ■
            // 6   ■   ■   ■   ■
            // 5 ■   ■   ■   ■
            // 4   ■   ■   p P ■
            // 3 ■   ■   ■   ■
            // 2   ■   ■   ■   ■
            // 1 ■   ■ K ■   ■
            //   1 2 3 4 5 6 7 8

            MockUI ui = new MockUI();

            IPlayer whiteStockfish = new MachinePlayer(true, ui, new Stockfish(_stockFishBinPath, 10));
            IPlayer blackStockfish = new MachinePlayer(false, ui, new Stockfish(_stockFishBinPath, 10));

            Board board = BoardFactory.BuildEnPassantTest();

            Game game = new Game(whiteStockfish, blackStockfish, ui, new MockLog(), new MockValidator(), board);

            game.PlaySingleMove();

        }

        [TestMethod]
        public void PromotePawn()
        {
            //   a b c d e f g h
            // 8   ■   k   ■   ■
            // 7 ■ p ■   ■   ■
            // 6   ■   ■   ■   ■
            // 5 ■   ■   ■   ■
            // 4   ■   ■   ■   ■
            // 3 ■   ■   ■   ■
            // 2   ■   ■   ■   ■
            // 1 ■   ■ K ■   ■
            //   1 2 3 4 5 6 7 8

            MockUI ui = new MockUI();

            IPlayer whiteStockfish = new MachinePlayer(true, ui, new Stockfish(_stockFishBinPath, 10));
            IPlayer blackStockfish = new MachinePlayer(false, ui, new Stockfish(_stockFishBinPath, 10));

            Board board = new Board();
            board.Add(new King(new Square(4, 1), true));
            board.Add(new Pawn(new Square(2, 7), true));
            board.Add(new King(new Square(4, 8), false));
            board.BlackCanCastleKingside = false;
            board.BlackCanCastleQueenside = false;
            board.WhiteCanCastleKingside = false;
            board.WhiteCanCastleQueenside = false;

            Game game = new Game(whiteStockfish, blackStockfish, ui, new MockLog(), new MockValidator(), board);

            game.PlaySingleMove();

            Assert.IsTrue(board.GetPieceOnSquare(new Square(2, 8)).IsQueen);
        }

        [TestMethod]
        public void PlayGame()
        {
            MockUI ui = new MockUI();


            for (int d = 1; d < 10; d++)
            {
                for (int y = 2; y < 8; y += 5)
                {
                    for (int x = 1; x <= 8; x++)
                    {

                        IPlayer whiteStockfish = new MachinePlayer(true, ui, new Stockfish(_stockFishBinPath, d));
                        IPlayer blackStockfish = new MachinePlayer(false, ui, new Stockfish(_stockFishBinPath, d));

                        Board board = BoardFactory.InitNewGame();
                        board.Remove(board.GetPieceOnSquare(new Square(x, y)));
                        Game game = new Game(whiteStockfish, blackStockfish, ui, new MockLog(), new MockValidator(), board);

                        game.Play();

                        whiteStockfish.Kill();
                        blackStockfish.Kill();
                    }

                }
            }

        }

    }
}
