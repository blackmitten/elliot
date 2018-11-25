using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using Blackmitten.Elliot.Backend;

namespace Blackmitten.Elliot.WinForms
{
    public partial class BoardControl : UserControl, IPieceVisitor
    {
        private int m_width = 400;
        private Brush m_whiteBrush = new SolidBrush(Color.White);
        private Brush m_blackBrush = new SolidBrush(Color.Black);
        private Brush m_darkBrush;
        private Brush m_lightBrush;


        public BoardControl()
        {
            InitializeComponent();

            m_darkBrush = new SolidBrush(Color.FromArgb(0x70, 0x70, 0x70));
            m_lightBrush = new SolidBrush(Color.FromArgb(0xa0, 0xa0, 0xa0));
        }

        private void BoardControl_Paint(object sender, PaintEventArgs e)
        {
            e.Graphics.SmoothingMode = System.Drawing.Drawing2D.SmoothingMode.HighQuality;
            bool dark = false;
            for (var x = 0; x < 8; x++)
            {
                dark = !dark;
                for (var y = 0; y < 8; y++)
                {
                    dark = !dark;
                    Brush brush;
                    if (dark)
                    {
                        brush = m_darkBrush;
                    }
                    else
                    {
                        brush = m_lightBrush;
                    }
                    e.Graphics.FillRectangle(brush, x * m_width / 8, y * m_width / 8, m_width / 8, m_width / 8);
                }
            }
            e.Graphics.DrawRectangle(Pens.Black, 0, 0, m_width, m_width);
            foreach (var piece in _board.Pieces)
            {
                piece.Accept(this, e.Graphics);
            }

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

        Board _board;
        public Board Board
        {
            get
            {
                return _board;
            }
            set
            {
                _board = value;
                Invalidate();
            }
        }


    }
}
