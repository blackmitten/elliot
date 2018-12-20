using Blackmitten.Elliot.Backend;
using BlackMitten.Elliot.StockfishEngine;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Collections.Generic;
using System.Text;

namespace ElliotTests
{
    [TestClass]
    public class C_SlowestTests
    {
        [TestMethod]
        public void PlayGamesDepth1()
        {
            MockUI ui = new MockUI();
            PlayGame(ui, 1);

        }

        [TestMethod]
        public void PlayGamesDepth2()
        {
            MockUI ui = new MockUI();
            PlayGame(ui, 2);

        }

        [TestMethod]
        public void PlayGamesDepth3()
        {
            MockUI ui = new MockUI();
            PlayGame(ui, 3);

        }

        [TestMethod]
        public void PlayGamesDepth4()
        {
            MockUI ui = new MockUI();
            PlayGame(ui, 4);

        }

        [TestMethod]
        public void PlayGamesDepth5()
        {
            MockUI ui = new MockUI();
            PlayGame(ui, 5);

        }

        [TestMethod]
        public void PlayGamesDepth6()
        {
            MockUI ui = new MockUI();
            PlayGame(ui, 6);

        }

        [TestMethod]
        public void PlayGamesDepth7()
        {
            MockUI ui = new MockUI();
            PlayGame(ui, 7);

        }

        [TestMethod]
        public void PlayGamesDepth8()
        {
            MockUI ui = new MockUI();
            PlayGame(ui, 8);

        }

        [TestMethod]
        public void PlayGamesDepth9()
        {
            MockUI ui = new MockUI();
            PlayGame(ui, 9);

        }

        [TestMethod]
        public void PlayGamesDepth10()
        {
            MockUI ui = new MockUI();
            PlayGame(ui, 10);

        }

        void PlayGame(MockUI ui, int depth)
        {
            for (int y = 2; y < 8; y += 5)
            {
                for (int x = 1; x <= 8; x++)
                {
                    IPlayer whiteStockfish = new MachinePlayer(true, ui, new Stockfish(A_QuickTests._stockFishBinPath, depth));
                    IPlayer blackStockfish = new MachinePlayer(false, ui, new Stockfish(A_QuickTests._stockFishBinPath, depth));

                    Board board = BoardFactory.InitNewGame();
                    board.RemovePiece(board.GetPieceOnSquare(new Square(x, y)), null);
                    Game game = new Game(whiteStockfish, blackStockfish, ui, new MockLog(), new MockValidator(), board);

                    game.Play(0);

                    whiteStockfish.Kill();
                    blackStockfish.Kill();
                }
            }
        }

    }
}
