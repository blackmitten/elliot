using Blackmitten.Elliot.Backend;
using Blackmitten.Menzel;
using BlackMitten.Elliot.FaladeEngine;
using BlackMitten.Elliot.StockfishEngine;
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
    public partial class Form1 : Form, IUserInterface
    {
        ILog _log;
        Game _game;

        public Board Board { set => boardControl1.Board = value; }
        public bool WaitingForWhiteHuman
        {
            get => boardControl1.WaitingForWhiteHuman;
            set => boardControl1.WaitingForWhiteHuman = value;
        }
        public bool WaitingForBlackHuman
        {
            get => boardControl1.WaitingForBlackHuman;
            set => boardControl1.WaitingForBlackHuman = value;
        }
        public bool MachineThinking
        {
            get => boardControl1.MachineThinking;
            set => boardControl1.MachineThinking = value;
        }

        public Form1()
        {
            InitializeComponent();

            _log = new Log();

            IPlayer whiteHuman = new HumanPlayer(true, this);
            IPlayer blackHuman = new HumanPlayer(false, this);
//            IPlayer whiteMachine = new MachinePlayer(true, this, new Falade());
            IPlayer blackMachine = new MachinePlayer(false, this, StockfishBuilder.Build(
                @"C:\Users\carl\Documents\dev\elliot\Elliot\StockfishBin\stockfish_9_x64.exe"));

            _game = new Game(whiteHuman, blackMachine, this);


            this.boardControl1.Log = _log;
            // this.boardControl1.Board = board;
            this.timer1.Tick += Timer1_Tick;
            this.timer1.Start();
        }

        private void Timer1_Tick(object sender, EventArgs e)
        {
            var strings = _log.Read();
            foreach (var s in strings)
            {
                listBox1.Items.Add(s);
            }
            if (listBox1.Items.Count > 0)
            {
                listBox1.SelectedIndex = listBox1.Items.Count - 1;
            }
            labelWhosTurn.Text = _game.WhitesTurn ? "White's turn" : "Black's turn";
        }

        private void Form1_FormClosing(object sender, FormClosingEventArgs e)
        {
            _game.Stop();
        }

        public Move WaitForHuman() => boardControl1.WaitForHuman();
        public void StopWaiting() => boardControl1.StopWaiting();

        public void Redraw()
        {
            if (this.InvokeRequired)
            {
                this.Invoke(new MethodInvoker(() => Redraw()));
            }
            else
            {
                Invalidate(true);
            }            
        }

    }
}
