using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Diagnostics;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace sfishtest
{
    public partial class Form1 : Form
    {
        private Process Stockfish;

        public Form1()
        {
            InitializeComponent();
        }

        private void Form1_Load( object sender, EventArgs e )
        {
            Stockfish = new Process();
            Stockfish.StartInfo.FileName = @"C:\Users\cpage\Downloads\stockfish-9-win\Windows\stockfish_9_x64_bmi2.exe";
            Stockfish.StartInfo.RedirectStandardInput = true;
            Stockfish.StartInfo.RedirectStandardOutput = true;
            Stockfish.StartInfo.RedirectStandardError = true;
            Stockfish.StartInfo.CreateNoWindow = true;
            Stockfish.OutputDataReceived += Stockfish_OutputDataReceived;
            Stockfish.ErrorDataReceived += Stockfish_ErrorDataReceived;
            Stockfish.StartInfo.UseShellExecute = false;

            Stockfish.Start();
            Stockfish.BeginOutputReadLine();
            Stockfish.BeginErrorReadLine();

            
        }

        private void Stockfish_ErrorDataReceived(object sender, DataReceivedEventArgs e)
        {
            int j = 1;
        }

        private void Stockfish_OutputDataReceived(object sender, DataReceivedEventArgs e)
        {
            int i = 1;
        }

        private void button1_Click( object sender, EventArgs e )
        {
            Stockfish.StandardInput.WriteLine("uci");
        }
    }
}
