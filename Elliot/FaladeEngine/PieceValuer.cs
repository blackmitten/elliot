using Blackmitten.Elliot.Backend;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BlackMitten.Elliot.FaladeEngine
{
    class PieceValuer : IPieceVisitor
    {
        public double Value { get; private set; }

        public void Visit(Pawn pawn, object data)
        {
            Board board = (Board) data;
            Value = 1;
            Value *= NegativeForBlack(pawn);
        }

        public void Visit(Rook rook, object data)
        {
            Value = 5;
            Value *= NegativeForBlack(rook);
        }

        public void Visit(Knight knight, object data)
        {
            Value = 3;
            Value *= NegativeForBlack(knight);
        }

        public void Visit(Bishop bishop, object data)
        {
            Value = 3;
            Value *= NegativeForBlack(bishop);
        }

        public void Visit(Queen queen, object data)
        {
            Value = 9;
            Value *= NegativeForBlack(queen);
        }

        public void Visit(King king, object data)
        {
            Value = 100;
            Value *= NegativeForBlack(king);
        }

        private double NegativeForBlack(IPiece piece)
        {
            return piece.White ? 1 : -1;
        }


    }
}
