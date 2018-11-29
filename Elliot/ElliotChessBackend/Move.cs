using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Blackmitten.Elliot.Backend
{
    public class Move
    {
        public Square Start { get; }
        public Square End { get; }

        public Move(string moveString)
        {
            if (moveString.Length == 4)
            {
                Start = new Square(moveString.Substring(0, 2));
                End = new Square(moveString.Substring(2));
            }
        }

        public Move(Square start, Square end)
        {
            Start = start;
            End = end;
        }

        public override string ToString()
        {
            return Start.ToString() + End.ToString();
        }

    }

}
