using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Blackmitten.Elliot.Backend
{
    public class MoveValidator : IMoveValidator, IPieceVisitor
    {

        public void Validate(Move move)
        {
            string moveString = move.ToLongString();

            IPiece piece = move.Board.GetPieceOnSquare(move.Start);
            piece.Accept(this, move);
            //throw new InvalidMoveException(moveString);
        }

        public void Visit(Pawn pawn, object data) => throw new NotImplementedException();
        public void Visit(Rook rook, object data) => throw new NotImplementedException();
        public void Visit(Knight knight, object data) => throw new NotImplementedException();
        public void Visit(Bishop bishop, object data) => throw new NotImplementedException();
        public void Visit(Queen queen, object data) => throw new NotImplementedException();
        public void Visit(King king, object data) => throw new NotImplementedException();
    }
}
