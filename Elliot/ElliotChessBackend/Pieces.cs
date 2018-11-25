using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Blackmitten.Elliot.Backend
{
    public interface IPiece
    {
        Square Pos { get; set; }
        bool White { get; }
        void Accept(IPieceVisitor visitor, object data);
    }

    public interface IPieceVisitor
    {
        void Visit(Pawn pawn, object data);
        void Visit(Rook rook, object data);
        void Visit(Knight knight, object data);
        void Visit(Bishop bishop, object data);
        void Visit(Queen queen, object data);
        void Visit(King king, object data);
    }

    public abstract class Piece : IPiece
    {
        public Piece(Square pos, bool white)
        {
            Pos = pos;
            White = white;
        }

        public Square Pos { get; set; }

        public bool White { get; set; }

        abstract public void Accept(IPieceVisitor visitor, object data);

    }

    public class Pawn : Piece
    {
        public Pawn(Square pos, bool white)
            : base(pos, white)
        {
        }

        public override void Accept(IPieceVisitor visitor, object data) => visitor.Visit(this, data);
    }

    public class Rook : Piece
    {
        public Rook(Square pos, bool white)
            : base(pos, white)
        {
        }

        public override void Accept(IPieceVisitor visitor, object data) => visitor.Visit(this, data);
    }

    public class Knight : Piece
    {
        public Knight(Square pos, bool white)
            : base(pos, white)
        {
        }

        public override void Accept(IPieceVisitor visitor, object data) => visitor.Visit(this, data);
    }

    public class Bishop : Piece
    {
        public Bishop(Square pos, bool white)
            : base(pos, white)
        {
        }

        public override void Accept(IPieceVisitor visitor, object data) => visitor.Visit(this, data);
    }

    public class Queen : Piece
    {
        public Queen(Square pos, bool white)
            : base(pos, white)
        {
        }

        public override void Accept(IPieceVisitor visitor, object data) => visitor.Visit(this, data);
    }

    public class King : Piece
    {
        public King(Square pos, bool white)
            : base(pos, white)
        {
        }

        public override void Accept(IPieceVisitor visitor, object data) => visitor.Visit(this, data);
    }
}
