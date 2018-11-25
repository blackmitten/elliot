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
using Blackmitten.Menzel;

namespace Blackmitten.Elliot.WinForms
{
    public partial class BoardControl : UserControl
    {
        private int m_width = 400;
        private DrawPiecesBadly _drawPiecesBadly;
        Square _moveStartSquare;


        public BoardControl()
        {
            InitializeComponent();

            _drawPiecesBadly = new DrawPiecesBadly(m_width);
        }

        private void BoardControl_Paint(object sender, PaintEventArgs e)
        {
            if (_drawPiecesBadly != null)
            {
                _drawPiecesBadly.Draw(e.Graphics, _board);
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

        private void BoardControl_MouseClick(object sender, MouseEventArgs e)
        {
            double squareWidth = m_width / 8;
            int x = 1 + (int)Math.Floor(e.X / squareWidth);
            int y = 8 - (int)Math.Floor(e.Y / squareWidth);
            SquareClicked(new Square(x, y));
        }

        private void SquareClicked(Square clickedSquare)
        {
            Log.Write("Clicked " + clickedSquare);
            if (clickedSquare.InBounds)
            {
                if (!_moveStartSquare.InBounds)
                {
                    _moveStartSquare = clickedSquare;
                }
                else
                {
                    MovePiece(_moveStartSquare, clickedSquare);
                    _moveStartSquare = new Square();
                }
            }
        }

        private void MovePiece(Square startSquare, Square endSquare)
        {
            _board.MovePiece(startSquare, endSquare);
            Invalidate();
        }

        public ILog Log { private get; set; }

    }
}
