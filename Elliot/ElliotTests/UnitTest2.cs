using Blackmitten.Elliot.Backend;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Collections.Generic;
using System.Text;

namespace ElliotTests
{
    [TestClass]
    public class UnitTest2
    {
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
            board.Move(new Move(board, new Square(4, 2), new Square(4, 4)));
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

    }
}
