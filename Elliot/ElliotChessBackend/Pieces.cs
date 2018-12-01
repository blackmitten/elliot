﻿using System;
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

        public override string ToString() => (White ? "White " : "Black ") + Name + " on " + Pos;
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
    }

}