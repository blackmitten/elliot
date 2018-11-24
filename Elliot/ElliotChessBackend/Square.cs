﻿using System;
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

        public int x;
        public int y;

        public Square(string s)
        {
            int c = Char.ConvertToUtf32(s, 0);
            int r = Char.ConvertToUtf32(s, 1);
            x = c-96;
            y = r-48;
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
            return x + ", " + y;
        }

    }
}