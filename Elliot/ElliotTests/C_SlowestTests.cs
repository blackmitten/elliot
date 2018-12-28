using Blackmitten.Elliot.Backend;
using BlackMitten.Elliot.FaladeEngine;
using BlackMitten.Elliot.StockfishEngine;
using System;
using System.Collections.Generic;
using System.Text;

namespace ElliotTests
{
    public class C_SlowestTests
    {
        public static void PlayStockfishVsStockfish1()
        {
            MockUI ui = new MockUI();
            PlayStockfishVsStockfish(ui, 1);

        }

        public static void PlayStockfishVsStockfish2()
        {
            MockUI ui = new MockUI();
            PlayStockfishVsStockfish(ui, 2);

        }

        public static void PlayStockfishVsStockfish3()
        {
            MockUI ui = new MockUI();
            PlayStockfishVsStockfish(ui, 3);

        }

        public static void PlayStockfishVsStockfish4()
        {
            MockUI ui = new MockUI();
            PlayStockfishVsStockfish(ui, 4);

        }

        public static void PlayStockfishVsStockfish5()
        {
            MockUI ui = new MockUI();
            PlayStockfishVsStockfish(ui, 5);

        }

        public static void PlayStockfishVsStockfish6()
        {
            MockUI ui = new MockUI();
            PlayStockfishVsStockfish(ui, 6);

        }

        public static void PlayStockfishVsStockfish7()
        {
            MockUI ui = new MockUI();
            PlayStockfishVsStockfish(ui, 7);

        }

        public static void PlayStockfishVsStockfish8()
        {
            MockUI ui = new MockUI();
            PlayStockfishVsStockfish(ui, 8);

        }

        public static void PlayStockfishVsStockfish9()
        {
            MockUI ui = new MockUI();
            PlayStockfishVsStockfish(ui, 9);

        }

        public static void PlayStockfishVsStockfishDepth10()
        {
            MockUI ui = new MockUI();
            PlayStockfishVsStockfish(ui, 10);

        }

        static void PlayStockfishVsStockfish(MockUI ui, int depth)
        {
            for (int y = 2; y < 8; y += 5)
            {
                for (int x = 1; x <= 8; x++)
                {
                    IPlayer whiteStockfish = new MachinePlayer(true, ui, new Stockfish(depth));
                    IPlayer blackStockfish = new MachinePlayer(false, ui, new Stockfish(depth));

                    Board board = BoardFactory.InitNewGame();
                    board.RemovePiece(board.GetPieceOnSquare(new Square(x, y)));
                    Game game = new Game(whiteStockfish, blackStockfish, ui, new MockLog(), new MockValidator(), board);

                    game.Play(0);

                    whiteStockfish.Kill();
                    blackStockfish.Kill();
                }
            }
        }

        public static void PlayFaladeVsFalade()
        {
            MockUI ui = new MockUI();
            Falade falade = new Falade();
            IPlayer whiteFalade = new MachinePlayer(true, ui, falade);
            IPlayer blackFalade = new MachinePlayer(false, ui, falade);
            Board board = BoardFactory.InitNewGame();
            Game game = new Game(whiteFalade, blackFalade, ui, new MockLog(), new MockValidator(), board);

            game.Play(0);
        }



    }
}
