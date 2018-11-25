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
        private DrawPiecesBadly _drawPiecesBadly;


        public BoardControl()
        {
            InitializeComponent();

            _drawPiecesBadly = new DrawPiecesBadly(m_width);
        }

        private void BoardControl_Paint(object sender, PaintEventArgs e)
        {
            _drawPiecesBadly.Draw(e.Graphics,_board);
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
