using Blackmitten.Menzel;
using System;
using System.Diagnostics;
using System.Threading;
using System.Windows.Forms;

namespace Blackmitten.Elliot.Backend
{
    public class Game
    {
        IUserInterface _userInterface;
        Board _board;
        IPlayer _whitePlayer;
        IPlayer _blackPlayer;
        bool _gameOver = false;
        //        TranspositionTable m_transpositionTable = new TranspositionTable();
        Thread _gameThread;
        ILogWriter _log;
        IMoveValidator _moveValidator;

        public event EventHandler<EventArgs> GameDone;

        public Game(IPlayer whitePlayer, IPlayer blackPlayer, IUserInterface userInterface, ILogWriter log,
            IMoveValidator moveValidator, Board board = null)
        {
            if (board == null)
            {
                board = Board.InitNewGame();
            }

            Trace.Assert(whitePlayer.White);
            Trace.Assert(!blackPlayer.White);
            _whitePlayer = whitePlayer;
            _blackPlayer = blackPlayer;
            _userInterface = userInterface;
            _log = log;
            _moveValidator = moveValidator;
            _board = board;

            userInterface.Board = _board;
        }

        public void Play()
        {
            _gameThread = new Thread(() =>
            {
                while (!_gameOver)
                {
                    PlaySingleMove();
                }
            });
            _gameThread.Start();
        }

        public void PlaySingleMove()
        {
            Move move;
            try
            {
                if (_board.WhitesTurn)
                {
                    if (!_whitePlayer.Human)
                    {
                        _userInterface.MachineThinking = true;
                    }
                    move = _whitePlayer.Play(_board);
                    _userInterface.MachineThinking = false;
                }
                else
                {
                    if (!_blackPlayer.Human)
                    {
                        _userInterface.MachineThinking = true;
                    }
                    move = _blackPlayer.Play(_board);
                    _userInterface.MachineThinking = false;
                }
                _userInterface.Redraw();
                if (!_gameOver)
                {
                    _log.Write(move.ToLongString());
                    _userInterface.WaitForInstructionToMove();
                    try
                    {
                        _moveValidator.Validate(move);
                        _board.Move(move);
                    }
                    catch (InvalidMoveException e)
                    {
                        MessageBox.Show(e.Message, "Invalid move");
                    }
                    _userInterface.Redraw();
                }
            }
            catch (NoMovesException)
            {
                _gameOver = true;
            }
        }

        public void Stop()
        {
            _gameOver = true;
            _userInterface.StopWaiting();
        }

        public bool WhitesTurn => _board.WhitesTurn;

    }
}
