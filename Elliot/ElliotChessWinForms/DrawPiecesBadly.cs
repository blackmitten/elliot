using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Blackmitten.Elliot.Backend;

namespace Blackmitten.Elliot.WinForms
{
    class DrawPiecesBadly : IPieceVisitor
    {
        private Brush m_whiteBrush = new SolidBrush(Color.White);
        private Brush m_blackBrush = new SolidBrush(Color.Black);
        private int m_width;

        public DrawPiecesBadly(int width)
        {
            m_width = width;
        }

        #region piece drawing

        public void Visit(Pawn piece, object data)
        {
            int squareWidth = m_width / 8;
            Graphics g = data as Graphics;
            drawPiecePreamble(piece, out Brush brush, out PointF pt);
            g.FillEllipse(brush, pt.X - squareWidth / 5, pt.Y - squareWidth / 5, 2 * squareWidth / 5, 2 * squareWidth / 5);
        }

        public void Visit(Rook piece, object data)
        {
            int squareWidth = m_width / 8;
            Graphics g = data as Graphics;
            drawPiecePreamble(piece, out Brush brush, out PointF pt);
            g.FillRectangle(brush, pt.X - squareWidth / 7, pt.Y - squareWidth / 4, 2 * squareWidth / 7, 2 * squareWidth / 4);
            g.FillRectangle(brush, pt.X - squareWidth / 5, pt.Y - squareWidth / 4, 2 * squareWidth / 5, squareWidth / 8);
            g.FillRectangle(brush, pt.X - squareWidth / 5, pt.Y + squareWidth / 4 - squareWidth / 8, 2 * squareWidth / 5, squareWidth / 8);

        }

        public void Visit(Knight piece, object data)
        {
            int squareWidth = m_width / 8;
            Graphics g = data as Graphics;
            drawPiecePreamble(piece, out Brush brush, out PointF pt);
            g.FillRectangle(brush, pt.X - squareWidth / 5, pt.Y - squareWidth / 4, 2 * squareWidth / 7, 2 * squareWidth / 4);
            g.FillRectangle(brush, pt.X - squareWidth / 5, pt.Y - squareWidth / 4, 2 * squareWidth / 5, 2 * squareWidth / 8);
        }

        public void Visit(Bishop piece, object data)
        {
            int squareWidth = m_width / 8;
            Graphics g = data as Graphics;
            drawPiecePreamble(piece, out Brush brush, out PointF pt);
            PointF[] pts = new PointF[] {
                new PointF( pt.X - squareWidth / 5, pt.Y + squareWidth / 4 ),
                new PointF(pt.X+squareWidth/5,pt.Y+squareWidth/4),
                new PointF(pt.X,pt.Y-squareWidth/4)
            };
            g.FillPolygon(brush, pts);
        }

        public void Visit(Queen piece, object data)
        {
            int squareWidth = m_width / 8;
            Graphics g = data as Graphics;
            drawPiecePreamble(piece, out Brush brush, out PointF pt);
            PointF[] pts = new PointF[] {
                new PointF( pt.X-squareWidth/3.5f,pt.Y+squareWidth/7),
                new PointF(pt.X+squareWidth/3.5f,pt.Y+squareWidth/7),
                new PointF(pt.X,pt.Y-squareWidth/3)
            };
            g.FillPolygon(brush, pts);
            pts = new PointF[] {
                new PointF( pt.X-squareWidth/3.5f,pt.Y-squareWidth/7),
                new PointF(pt.X+squareWidth/3.5f,pt.Y-squareWidth/7),
                new PointF(pt.X,pt.Y+squareWidth/3)
            };
            g.FillPolygon(brush, pts);
        }

        public void Visit(King piece, object data)
        {
            int squareWidth = m_width / 8;
            Graphics g = data as Graphics;
            drawPiecePreamble(piece, out Brush brush, out PointF pt);
            g.FillRectangle(brush, pt.X - squareWidth / 7, pt.Y - squareWidth / 3.5f, 2 * squareWidth / 7, 2 * squareWidth / 3.5f);
            g.FillRectangle(brush, pt.X - squareWidth / 3.5f, pt.Y - squareWidth / 7, 2 * squareWidth / 3.5f, 2 * squareWidth / 7);
        }

        void drawPiecePreamble(IPiece piece, out Brush brush, out PointF pt)
        {
            int squareWidth = m_width / 8;
            brush = piece.White ? m_whiteBrush : m_blackBrush;
            pt = new Point(piece.Pos.x * squareWidth - squareWidth / 2, piece.Pos.y * squareWidth - squareWidth / 2);

        }

        #endregion
    }
}
