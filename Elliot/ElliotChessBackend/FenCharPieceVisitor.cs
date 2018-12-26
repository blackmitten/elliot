using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Blackmitten.Elliot.Backend
{
    public class FenCharPieceVisitor : IPieceVisitor
    {
        public char GetFenChar(IPiece piece)
        {
            piece.Accept(this);
            return Char;
        }

        public char Char { get; private set; }

        public void Visit(Pawn piece, object data)
        {
            Char = piece.White ? 'P' : 'p';
        }

        public void Visit(Rook piece, object data)
        {
            Char = piece.White ? 'R' : 'r';
        }

        public void Visit(Knight piece, object data)
        {
            Char = piece.White ? 'N' : 'n';
        }

        public void Visit(Bishop piece, object data)
        {
            Char = piece.White ? 'B' : 'b';
        }

        public void Visit(Queen piece, object data)
        {
            Char = piece.White ? 'Q' : 'q';
        }

        public void Visit(King piece, object data)
        {
            Char = piece.White ? 'K' : 'k';
        }

    }
}
