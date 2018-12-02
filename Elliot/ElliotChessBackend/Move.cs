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
        public IPiece Promoted { get; }

        public Move(string moveString)
        {
            if (moveString.Length == 4)
            {
                Start = new Square(moveString.Substring(0, 2));
                End = new Square(moveString.Substring(2));
            }
            else if(moveString.Length == 5)
            {
                Start = new Square(moveString.Substring(0, 2));
                End = new Square(moveString.Substring(2));
                throw new NotImplementedException("promotion");
            }
            else if ( moveString == "(none)")
            {
                throw new NotImplementedException();
            }
            else
            {
                throw new NotImplementedException();
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
