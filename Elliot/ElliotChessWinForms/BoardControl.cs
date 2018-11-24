using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Blackmitten.Elliot.WinForms
{
    public partial class BoardControl : UserControl
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

        }
    }
}
