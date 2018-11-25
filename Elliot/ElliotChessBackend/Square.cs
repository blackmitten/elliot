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
        // 8   ■   ■   ■   ■
        // 7 ■   ■   ■   ■
        // 6   ■   ■   ■   ■
        // 5 ■   ■   ■   ■
        // 4   ■   ■   ■   ■
        // 3 ■   ■   ■   ■
        // 2   ■   ■   ■   ■
        // 1 ■   ■   ■   ■
        //   1 2 3 4 5 6 7 8

        public int x;
        public int y;

        public bool InBounds => x >= 1 && x <= 8 && y >= 1 && y <= 8;

        public Square(string s)
        {
            int c = char.ConvertToUtf32(s, 0);
            int r = char.ConvertToUtf32(s, 1);
            x = c - 96;
            y = r - 48;
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

        public Square Offset(int dx, int dy)
        {
            return new Square(x + dx, y + dy);
        }

        public override string ToString()
        {
            string c = char.ConvertFromUtf32(x + 96);
            string r = char.ConvertFromUtf32(y + 48);
            return c + r;
        }

    }
}
