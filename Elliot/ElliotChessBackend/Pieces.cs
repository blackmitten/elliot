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
        void Accept(IPieceVisitor visitor, object data = null);
        IPiece Copy();
        string Name { get; }
        bool IsKing { get; }
        bool IsRook { get; }
        bool IsPawn { get; }
        bool IsQueen { get; }
        bool IsBishop{ get; }
        bool IsDiagonalMover { get; }
        bool IsStraightMover { get; }
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

        abstract public IPiece Copy();

        public abstract string Name { get; }

        public override string ToString() => (White ? "White " : "Black ") + Name;

        public abstract bool IsKing { get; }
        public abstract bool IsRook { get; }
        public abstract bool IsPawn { get; }
        public abstract bool IsQueen { get; }

        public abstract bool IsBishop { get; }

        public bool IsDiagonalMover => IsBishop || IsQueen;
        public bool IsStraightMover => IsRook || IsQueen;
    }

    public class Pawn : Piece
    {
        public Pawn(Square pos, bool white)
            : base(pos, white)
        {
        }

        public override void Accept(IPieceVisitor visitor, object data) => visitor.Visit(this, data);

        public override IPiece Copy() => new Pawn(Pos, White);

        public override string Name => "Pawn";

        public override bool IsKing => false;
        public override bool IsRook => false;
        public override bool IsPawn => true;
        public override bool IsQueen => false;
        public override bool IsBishop => false;
    }

    public class Rook : Piece
    {
        public Rook(Square pos, bool white)
            : base(pos, white)
        {
        }

        public override void Accept(IPieceVisitor visitor, object data) => visitor.Visit(this, data);

        public override IPiece Copy() => new Rook(Pos, White);

        public override string Name => "Rook";

        public override bool IsKing => false;
        public override bool IsRook => true;
        public override bool IsPawn => false;
        public override bool IsQueen => false;
        public override bool IsBishop => false;
    }

    public class Knight : Piece
    {
        public Knight(Square pos, bool white)
            : base(pos, white)
        {
        }

        public override void Accept(IPieceVisitor visitor, object data) => visitor.Visit(this, data);

        public override IPiece Copy() => new Knight(Pos, White);

        public override string Name => "Knight";

        public override bool IsKing => false;
        public override bool IsRook => false;
        public override bool IsPawn => false;
        public override bool IsQueen => false;
        public override bool IsBishop => false;
    }

    public class Bishop : Piece
    {
        public Bishop(Square pos, bool white)
            : base(pos, white)
        {
        }

        public override void Accept(IPieceVisitor visitor, object data) => visitor.Visit(this, data);

        public override IPiece Copy() => new Bishop(Pos, White);

        public override string Name => "Bishop";

        public override bool IsKing => false;
        public override bool IsRook => false;
        public override bool IsPawn => false;
        public override bool IsQueen => false;
        public override bool IsBishop => true;
    }

    public class Queen : Piece
    {
        public Queen(Square pos, bool white)
            : base(pos, white)
        {
        }

        public override void Accept(IPieceVisitor visitor, object data) => visitor.Visit(this, data);

        public override IPiece Copy() => new Queen(Pos, White);

        public override string Name => "Queen";

        public override bool IsKing => false;
        public override bool IsRook => false;
        public override bool IsPawn => false;
        public override bool IsQueen => true;
        public override bool IsBishop => false;
    }

    public class King : Piece
    {
        public King(Square pos, bool white)
            : base(pos, white)
        {
        }

        public override void Accept(IPieceVisitor visitor, object data) => visitor.Visit(this, data);

        public override IPiece Copy() => new King(Pos, White);

        public override string Name => "King";

        public override bool IsKing => true;
        public override bool IsRook => false;
        public override bool IsPawn => false;
        public override bool IsQueen => false;
        public override bool IsBishop => false;
    }

}
