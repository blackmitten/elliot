using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace BlackMitten.Elliot.Winforms
{
    public partial class AboutForm : Form
    {
        public AboutForm(string contents)
        {
            InitializeComponent();
            this.richTextBox1.Text = contents;
        }
    }
}
