using Blackmitten.Elliot.Backend;
using Blackmitten.Menzel;
using BlackMitten.Elliot.FaladeEngine;
using BlackMitten.Elliot.StockfishEngine;
using System;
using System.Collections.Generic;
using System.Text;

namespace ElliotTests
{
    public class B_SlowTests
    {
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

//            Board board = BoardFactory.BoardFromFenString("6kr/3n1ppp/4p3/4P3/1Q3P2/N4b2/7P/qB2K1R1 w Q - 2 28");

            Game game = new Game(whiteStockfish, blackStockfish, ui, new MockLog(), new MockValidator(), board);

            game.Play(0);

        }

        public static void FaladePerformanceMeasure()
        {
            Diags.DoDiags = false;
            MockUI ui = new MockUI();
            Falade falade = new Falade(false);
            IPlayer whiteFalade = new MachinePlayer(true, ui, falade);
            IPlayer blackFalade = new MachinePlayer(false, ui, falade);
            Board board = BoardFactory.InitNewGame();
            Game game = new Game(whiteFalade, blackFalade, ui, new MockLog(), new MockValidator(), board);

            game.PlaySingleMove(0);
            game.PlaySingleMove(0);
            game.PlaySingleMove(0);
            game.PlaySingleMove(0);
            game.PlaySingleMove(0);
            Diags.DoDiags = true;
        }


    }

}
