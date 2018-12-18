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
            checkBoxWaitToProceed.Checked = bool.Parse( ConfigurationManager.AppSettings["WaitToProceed"] );

            IPlayer whiteHuman = new HumanPlayer(true, this);
            IPlayer blackHuman = new HumanPlayer(false, this);
            IPlayer blackFalade = new MachinePlayer(false, this, new Falade());
            IPlayer whiteFalade = new MachinePlayer(true, this, new Falade());
            IPlayer whiteStockfish = new MachinePlayer(true, this, new Stockfish(path, 1));
            IPlayer blackStockfish = new MachinePlayer(false, this, new Stockfish(path, 1));

            IPlayer blackPlayer = blackStockfish;
            IPlayer whitePlayer = whiteStockfish;

            boardControl1.Log = _log;

            Board board = BoardFactory.InitNewGame();
            board.Remove(board.GetPieceOnSquare(new Square(2, 2)));
            
            _game = new Game(whitePlayer, blackPlayer, this, _log, new MoveValidator(), board);
            _game.StartPlay( 200 );
            labelPlayers.Text = "White " + whitePlayer.Name + " vs. black " + blackPlayer.Name;

            timer1.Tick += Timer1_Tick;
            timer1.Start();
        }

        private void Timer1_Tick(object sender, EventArgs e)
        {
            var strings = _log.Read();
            if (strings.Count > 0)
            {
                foreach (var s in strings)
                {
                    listBox1.Items.Add(s);
                }
                if (listBox1.Items.Count > 0)
                {
                    listBox1.SelectedIndex = listBox1.Items.Count - 1;
                }
            }
            if(_game.WhitesTurn)
            {
                if(_game.GameState== GameState.CheckMate)
                {
                    labelWhosTurn.Text = "Black wins";
                }
                else if(_game.CurrentPlayerInCheck)
                {
                    labelWhosTurn.Text = "White in check";
                }
                else
                {
                    labelWhosTurn.Text = "White's turn";
                }
            }
            else
            {
                if (_game.GameState == GameState.CheckMate)
                {
                    labelWhosTurn.Text = "White wins";
                }
                else if(_game.CurrentPlayerInCheck)
                {
                    labelWhosTurn.Text = "Black in check";
                }
                else
                {
                    labelWhosTurn.Text = "Black's turn";
                }
            }
            labelMoveNumber.Text = "Move " + _game.MoveNumber.ToString();
        }

        private void Form1_FormClosing(object sender, FormClosingEventArgs e)
        {
            _game.ApplicationClosing();
        }

        public Move WaitForHuman() => boardControl1.WaitForHuman();
        public void StopWaiting() => boardControl1.StopWaiting();

        public void Redraw()
        {
            if (InvokeRequired)
            {
                Invoke(new MethodInvoker(Redraw));
            }
            else
            {
                Invalidate(true);
            }
        }

        public void WaitForInstructionToMove()
        {
            if (checkBoxWaitToProceed.Checked)
            {
                _instructToMove.WaitOne();
            }
        }

        private void buttonInstructToMove_Click(object sender, EventArgs e)
        {
            _instructToMove.Set();
        }

        public void InvalidMove(string message)
        {
            MessageBox.Show(message, "Invalid move");
        }
    }
}
