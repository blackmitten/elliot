using Blackmitten.Elliot.Backend;
using Blackmitten.Menzel;
using BlackMitten.Elliot.FaladeEngine;
using BlackMitten.Elliot.StockfishEngine;
using System;
using System.Configuration;
using System.Threading;
using System.Windows.Forms;

namespace BlackMitten.Elliot.Winforms
{
    public partial class Form1 : Form, IUserInterface
    {
        ILog _log;
        Game _game;
        AutoResetEvent _instructToMove = new AutoResetEvent(false);

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

            _log = LogBuilder.Build();

            var path = ConfigurationManager.AppSettings["StockfishBinPath"];

            IPlayer whiteHuman = new HumanPlayer(true, this);
            IPlayer blackHuman = new HumanPlayer(false, this);
            IPlayer whiteFalade = new MachinePlayer(true, this, new Falade());
            IPlayer whiteStockfish = new MachinePlayer(true, this, new Stockfish(path, 10));
            IPlayer blackStockfish = new MachinePlayer(false, this, new Stockfish(path, 10));



            this.boardControl1.Log = _log;
            // this.boardControl1.Board = board;
            this.timer1.Tick += Timer1_Tick;
            this.timer1.Start();

            _game = new Game(whiteStockfish, blackHuman, this, _log, new MoveValidator());
            _game.Play();
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

        public void WaitForInstructionToMove()
        {
            //            _instructToMove.WaitOne();
        }

        private void buttonInstructToMove_Click(object sender, EventArgs e)
        {
            _instructToMove.Set();
        }
    }
}
