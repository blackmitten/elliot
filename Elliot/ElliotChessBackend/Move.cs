using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Blackmitten.Elliot.Backend
{
    public enum PieceType
    {
        None,
        Pawn,
        Rook,
        Knight,
        Bishop,
        Queen,
        King
    }

    public class Move
    {
        public Square Start { get; }
        public Square End { get; }
        public PieceType Promoted { get; private set; }

        public Move(string moveString)
        {
            if (moveString.Length == 4)
            {
                Start = new Square(moveString.Substring(0, 2));
                End = new Square(moveString.Substring(2));
            }
            else if (moveString.Length == 5)
            {
                Start = new Square(moveString.Substring(0, 2));
                End = new Square(moveString.Substring(2));
                switch (moveString[4])
                {
                    case 'q': Promoted = PieceType.Queen; break;
                    case 'k': Promoted = PieceType.King; break;
                    case 'n': Promoted = PieceType.Knight; break;
                    case 'b': Promoted = PieceType.Bishop; break;
                    case 'r': Promoted = PieceType.Rook; break;
                    default:
                        throw new NotImplementedException();
                }
            }
            else if (moveString == "(none)")
            {
                throw new NoMovesException();
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

        string PromotionCode()
        {
            switch(Promoted)
            {
                case PieceType.Bishop: return "b";
                case PieceType.Rook: return "r";
                case PieceType.Knight: return "k";
                case PieceType.Queen: return "q";
                default:
                    throw new InvalidOperationException();
            }
        }

        public override string ToString()
        {
            return Start.ToString() + End.ToString() + PromotionCode();
        }

    }

}
