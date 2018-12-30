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
        public Board Board { get; private set; }
        public bool Capturing { get; internal set; }

        public Move(Board board, string moveString)
        {
            Board = board;
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

        public Move(Board board, Square start, Square end)
        {
            Board = board;
            Start = start;
            End = end;
        }

        string PromotionCode()
        {
            switch (Promoted)
            {
                case PieceType.Bishop: return "b";
                case PieceType.Rook: return "r";
                case PieceType.Knight: return "k";
                case PieceType.Queen: return "q";
                case PieceType.None: return "";
                default:
                    throw new InvalidOperationException();
            }
        }

        public override string ToString()
        {
            return Start.ToString() + End.ToString() + PromotionCode();
        }

        internal string ToLongString()
        {
            IPiece piece = Board.GetPieceOnSquare(Start);
            string promotion = "";
            switch (Promoted)
            {
                case PieceType.Bishop: promotion = " promoted to Bishop"; break;
                case PieceType.Rook: promotion = " promoted to Rook"; break;
                case PieceType.Knight: promotion = " promoted to Knight"; break;
                case PieceType.Queen: promotion = " promoted to Queen"; break;
                case PieceType.None: break;
                default:
                    throw new InvalidOperationException();
            }
            IPiece capturedPiece = Board.GetPieceOnSquare(End);
            if (capturedPiece != null)
            {
                return piece.ToString() + " captures " + capturedPiece.ToString() + " " + Start + "->" + End + promotion;
            }
            return piece.ToString() + " " + Start + "->" + End + promotion;
        }
    }

}
