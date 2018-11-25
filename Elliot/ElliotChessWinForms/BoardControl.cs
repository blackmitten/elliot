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
    public partial class BoardControl : UserControl
    {
        private int m_width = 400;
        private Brush m_darkBrush;
        private Brush m_lightBrush;
        private DrawPiecesBadly _drawPiecesBadly;


        public BoardControl()
        {
            InitializeComponent();

            m_darkBrush = new SolidBrush(Color.FromArgb(0x70, 0x70, 0x70));
            m_lightBrush = new SolidBrush(Color.FromArgb(0xa0, 0xa0, 0xa0));
            _drawPiecesBadly = new DrawPiecesBadly(m_width);
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
                piece.Accept(_drawPiecesBadly, e.Graphics);
            }

        }


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
