using Blackmitten.Elliot.Backend;
using Blackmitten.Menzel;
using BlackMitten.Elliot.FaladeEngine;
using BlackMitten.Elliot.StockfishEngine;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Text;

namespace ElliotTests
{
    public class B_SlowTests
    {
        public static void TestPawnTakingOwnKingIsNotValid()
        {
            MockUI ui = new MockUI();
            Falade falade = new Falade(4);

            IPlayer whiteFalade = new MachinePlayer(true, ui, falade);
            IPlayer blackFalade = new MachinePlayer(false, ui, falade);
            Board board = BoardFactory.BoardFromFenString("rnbq1bnr/pp1Q1ppp/8/2p1Pk2/P1P5/7N/1P2PPPP/RNB1KB1R b KQ - 0 6");
            Game game = new Game(whiteFalade, blackFalade, ui, new MockLog(), new MockValidator(), board);

            game.PlaySingleMove(0);
        }

        public static void TestDubiousCheckMate()
        {
            MockUI ui = new MockUI();
            Falade falade = new Falade(4);

            IPlayer whiteFalade = new MachinePlayer(true, ui, falade);
            IPlayer blackFalade = new MachinePlayer(false, ui, falade);
            Board board = BoardFactory.BoardFromFenString("r2k1bnr/ppp1qN1p/6p1/1B1P4/P3p2Q/1nP5/1P1P1PPP/R1B1K2R b KQ - 0 12");
            Game game = new Game(whiteFalade, blackFalade, ui, new MockLog(), new MockValidator(), board);

            game.PlaySingleMove(0);
            Assert.IsTrue(game.GameState != GameState.CheckMate);
        }

        public static void FaladePerformanceMeasure()
        {
            MockUI ui = new MockUI();
            Falade falade = new Falade(4);

            IPlayer whiteFalade = new MachinePlayer(true, ui, falade);
            IPlayer blackFalade = new MachinePlayer(false, ui, falade);
            Board board = BoardFactory.InitNewGame();
            Game game = new Game(whiteFalade, blackFalade, ui, new MockLog(), new MockValidator(), board);

            game.PlaySingleMove(0);
            game.PlaySingleMove(0);
            game.PlaySingleMove(0);
            game.PlaySingleMove(0);
            game.PlaySingleMove(0);
        }



        public static void StockfishTest1()
        {
            MockUI ui = new MockUI();

            IPlayer whiteStockfish = new MachinePlayer(true, ui, new Stockfish(3));
            IPlayer blackStockfish = new MachinePlayer(false, ui, new Stockfish(3));

            Board board = BoardFactory.BoardFromFenString("3nbrk1/1p1P1ppp/4p3/1P2N3/8/r3P3/3R1PPP/4KB1R w K - 0 22");

            Game game = new Game(whiteStockfish, blackStockfish, ui, new MockLog(), new MockValidator(), board);

            Debug.WriteLine(board.ToLongString());
            game.PlaySingleMove(0);

        }

        public static void EnPassant()
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

            IPlayer whiteStockfish = new MachinePlayer(true, ui, new Stockfish(5));
            IPlayer blackStockfish = new MachinePlayer(false, ui, new Stockfish(5));

            Board board = BoardFactory.BuildEnPassantTest();

            Game game = new Game(whiteStockfish, blackStockfish, ui, new MockLog(), new MockValidator(), board);

            game.PlaySingleMove(0);

        }

        public static void PromotePawn()
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

            IPlayer whiteStockfish = new MachinePlayer(true, ui, new Stockfish(5));
            IPlayer blackStockfish = new MachinePlayer(false, ui, new Stockfish(5));

            Board board = new Board();
            board.AddPiece(new King(new Square(4, 1), true));
            board.AddPiece(new Pawn(new Square(2, 7), true));
            board.AddPiece(new King(new Square(4, 8), false));
            board.BlackCanCastleKingside = false;
            board.BlackCanCastleQueenside = false;
            board.WhiteCanCastleKingside = false;
            board.WhiteCanCastleQueenside = false;

            Game game = new Game(whiteStockfish, blackStockfish, ui, new MockLog(), new MockValidator(), board);

            game.PlaySingleMove(0);

            Assert.IsTrue(board.GetPieceOnSquare(new Square(2, 8)).IsQueen);
        }

        public static void Test()
        {

            MockUI ui = new MockUI();

            IPlayer whiteStockfish = new MachinePlayer(true, ui, new Stockfish(2));
            IPlayer blackStockfish = new MachinePlayer(false, ui, new Stockfish(2));

            Board board = BoardFactory.InitNewGame();
            board.RemovePiece(board.GetPieceOnSquare(new Square(4, 2)));

            Game game = new Game(whiteStockfish, blackStockfish, ui, new MockLog(), new MockValidator(), board);

            game.Play(0);

        }

    }

}
