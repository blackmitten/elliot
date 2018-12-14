using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Blackmitten.Elliot.Backend
{

    public struct Square
    {
        //   a b c d e f g h
        // 8 r n b q k b n r 
        // 7 p p p p p p p p
        // 6   ■   ■   ■   ■
        // 5 ■   ■   ■   ■
        // 4   ■   ■   ■   ■
        // 3 ■   ■   ■   ■
        // 2 P P P P P P P P 
        // 1 R N B Q K B N R
        //   1 2 3 4 5 6 7 8

        public int x;
        public int y;

        public static Square WhiteKingStart { get; } = new Square(5, 1);
        public static Square WhiteKingCastledQueenside { get; } = new Square(3, 1);
        public static Square WhiteKingCastledKingside { get; } = new Square(7, 1);
        public static Square WhiteQueensRookStart { get; } = new Square(1, 1);
        public static Square WhiteKingsRookStart { get; } = new Square(8, 1);
        public static Square WhiteQueensRookCastled { get; } = new Square(4, 1);
        public static Square WhiteKingsRookCastled { get; } = new Square(6, 1);

        public static Square BlackKingStart { get; } = new Square(5, 8);
        public static Square BlackKingCastledQueenside { get; } = new Square(3, 8);
        public static Square BlackKingCastledKingside { get; } = new Square(7, 8);
        public static Square BlackQueensRookStart { get; } = new Square(1, 8);
        public static Square BlackKingsRookStart { get; } = new Square(8, 8);
        public static Square BlackQueensRookCastled { get; } = new Square(4, 8);
        public static Square BlackKingsRookCastled { get; } = new Square(6, 8);

        public bool InBounds => x >= 1 && x <= 8 && y >= 1 && y <= 8;

        public static Square Random =>
            new Square(1 + StaticRandom.Instance.Next % 8, 1 + StaticRandom.Instance.Next % 8);

        public Square(string s)
        {
            int c = char.ConvertToUtf32(s, 0);
            int r = char.ConvertToUtf32(s, 1);
            x = c - 96;
            y = r - 48;
            if (!InBounds)
            {
                throw new InvalidOperationException("Bad square");
            }
        }

        public Square(int x, int y)
        {
            this.x = x;
            this.y = y;
        }

        public override bool Equals(Object obj)
        {
            return obj is Square && this == (Square)obj;
        }

        public override int GetHashCode()
        {
            return x.GetHashCode() ^ y.GetHashCode();
        }

        public static bool operator ==(Square s1, Square s2)
        {
            return s1.x == s2.x && s1.y == s2.y;
        }

        public static bool operator !=(Square s1, Square s2)
        {
            return s1.x != s2.x || s1.y != s2.y;
        }

        public static Vector operator -(Square s1, Square s2)
        {
            return new Vector(s1.x - s2.x, s1.y - s2.y);
        }

        public Square Offset(int dx, int dy)
        {
            return new Square(x + dx, y + dy);
        }

        public override string ToString()
        {
            if (x < 1 || x > 8 || y < 1 || y > 8)
            {
                return "Invalid";
            }
            string c = char.ConvertFromUtf32(x + 96);
            string r = char.ConvertFromUtf32(y + 48);
            return c + r;
        }

    }
}
