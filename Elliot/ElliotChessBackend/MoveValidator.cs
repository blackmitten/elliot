using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Blackmitten.Elliot.Backend
{
    public class MoveValidator : IMoveValidator
    {
        public void Validate(Board board, Move move)
        {
            string moveString = move.ToLongString(board);
            throw new InvalidMoveException(moveString);
        }

    }
}
