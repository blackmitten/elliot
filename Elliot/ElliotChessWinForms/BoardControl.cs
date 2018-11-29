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
using System.Threading;

namespace Blackmitten.Elliot.WinForms
{
    public partial class BoardControl : UserControl
    {
        private int m_width = 400;
        private DrawPiecesBadly _drawPiecesBadly;
        Square _moveStartSquare;
        ManualResetEvent _humanMoved = new ManualResetEvent(false);
        ManualResetEvent _quitting = new ManualResetEvent(false);
        Move _humansMove;


        public BoardControl()
        {
            InitializeComponent();

            _drawPiecesBadly = new DrawPiecesBadly(m_width);
        }

        private void BoardControl_Paint(object sender, PaintEventArgs e)
        {
            if (_drawPiecesBadly != null)
            {
                _drawPiecesBadly.Draw(e.Graphics, _board, _moveStartSquare);
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
                if (WaitingForWhiteHuman || WaitingForBlackHuman)
                {
                    if (!_moveStartSquare.InBounds)
                    {
                        IPiece clickedPiece = _board.GetPieceOnSquare(clickedSquare);
                        if (clickedPiece != null)
                        {
                            if ((clickedPiece.White && WaitingForWhiteHuman) || (!clickedPiece.White && WaitingForBlackHuman))
                            {
                                _moveStartSquare = clickedSquare;
                            }
                        }
                    }
                    else
                    {
                        _humansMove = new Move(_moveStartSquare, clickedSquare);
                        _moveStartSquare = new Square();
                        _humanMoved.Set();
                    }
                }
            }
            Invalidate();
        }

        /*
        private void MovePiece(Square startSquare, Square endSquare)
        {
            _board.MovePiece(startSquare, endSquare);
            Invalidate();
        }
        */

        public Move WaitForHuman()
        {
            _humanMoved.Reset();
            _humansMove = null;
            WaitHandle.WaitAny(new[] { _humanMoved, _quitting });
            return _humansMove;
        }

        public void StopWaiting()
        {
            _quitting.Set();
        }

        public ILog Log { private get; set; }
        public bool WaitingForWhiteHuman { get; set; }
        public bool WaitingForBlackHuman { get; set; }
        public bool MachineThinking { get; internal set; }
    }
}
