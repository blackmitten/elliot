using Blackmitten.Elliot.Backend;
using Blackmitten.Menzel;
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

            IPlayer whiteStockfish = new MachinePlayer(true, ui, new Stockfish(A_QuickTests._stockFishBinPath, 5));
            IPlayer blackStockfish = new MachinePlayer(false, ui, new Stockfish(A_QuickTests._stockFishBinPath, 5));

            Board board = BoardFactory.BuildEnPassantTest();

            Game game = new Game(whiteStockfish, blackStockfish, ui, new MockLog(), new MockValidator(), board);

            game.PlaySingleMove(0, true);

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

            IPlayer whiteStockfish = new MachinePlayer(true, ui, new Stockfish(A_QuickTests._stockFishBinPath, 5));
            IPlayer blackStockfish = new MachinePlayer(false, ui, new Stockfish(A_QuickTests._stockFishBinPath, 5));

            Board board = new Board();
            board.AddPiece(new King(new Square(4, 1), true), null);
            board.AddPiece(new Pawn(new Square(2, 7), true), null);
            board.AddPiece(new King(new Square(4, 8), false), null);
            board.BlackCanCastleKingside = false;
            board.BlackCanCastleQueenside = false;
            board.WhiteCanCastleKingside = false;
            board.WhiteCanCastleQueenside = false;

            Game game = new Game(whiteStockfish, blackStockfish, ui, new MockLog(), new MockValidator(), board);

            game.PlaySingleMove(0, true);

            Assert.IsTrue(board.GetPieceOnSquare(new Square(2, 8)).IsQueen);
        }


    }

}
