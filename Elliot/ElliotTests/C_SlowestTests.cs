using Blackmitten.Elliot.Backend;
using BlackMitten.Elliot.FaladeEngine;
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
        public void PlayStockfishVsStockfish1()
        {
            MockUI ui = new MockUI();
            PlayStockfishVsStockfish(ui, 1);

        }

        [TestMethod]
        public void PlayStockfishVsStockfish2()
        {
            MockUI ui = new MockUI();
            PlayStockfishVsStockfish(ui, 2);

        }

        [TestMethod]
        public void PlayStockfishVsStockfish3()
        {
            MockUI ui = new MockUI();
            PlayStockfishVsStockfish(ui, 3);

        }

        [TestMethod]
        public void PlayStockfishVsStockfish4()
        {
            MockUI ui = new MockUI();
            PlayStockfishVsStockfish(ui, 4);

        }

        [TestMethod]
        public void PlayStockfishVsStockfish5()
        {
            MockUI ui = new MockUI();
            PlayStockfishVsStockfish(ui, 5);

        }

        [TestMethod]
        public void PlayStockfishVsStockfish6()
        {
            MockUI ui = new MockUI();
            PlayStockfishVsStockfish(ui, 6);

        }

        [TestMethod]
        public void PlayStockfishVsStockfish7()
        {
            MockUI ui = new MockUI();
            PlayStockfishVsStockfish(ui, 7);

        }

        [TestMethod]
        public void PlayStockfishVsStockfish8()
        {
            MockUI ui = new MockUI();
            PlayStockfishVsStockfish(ui, 8);

        }

        [TestMethod]
        public void PlayStockfishVsStockfish9()
        {
            MockUI ui = new MockUI();
            PlayStockfishVsStockfish(ui, 9);

        }

        [TestMethod]
        public void PlayStockfishVsStockfishDepth10()
        {
            MockUI ui = new MockUI();
            PlayStockfishVsStockfish(ui, 10);

        }

        void PlayStockfishVsStockfish(MockUI ui, int depth)
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

                    game.Play(0, true);

                    whiteStockfish.Kill();
                    blackStockfish.Kill();
                }
            }
        }

        [TestMethod]
        public void PlayFaladeVsFalade()
        {
            MockUI ui = new MockUI();
            Falade falade = new Falade();
            IPlayer whiteFalade = new MachinePlayer(true, ui, falade);
            IPlayer blackFalade = new MachinePlayer(false, ui, falade);
            Board board = BoardFactory.InitNewGame();
            Game game = new Game(whiteFalade, blackFalade, ui, new MockLog(), new MockValidator(), board);

            game.Play(0, true);
            int i = 1;
        }



    }
}
