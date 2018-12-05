using Blackmitten.Menzel;
using System;
using System.Diagnostics;
using System.Threading;

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

        public event EventHandler<EventArgs> GameDone;

        public Game(IPlayer whitePlayer, IPlayer blackPlayer, IUserInterface userInterface, ILogWriter log)
        {
            _log = log;
            Begin(whitePlayer, blackPlayer, userInterface, Board.InitNewGame());
        }

        public Game(IPlayer whitePlayer, IPlayer blackPlayer, IUserInterface userInterface, Board board)
        {
            Begin(whitePlayer, blackPlayer, userInterface, board);
        }

        void Begin(IPlayer whitePlayer, IPlayer blackPlayer, IUserInterface userInterface, Board board)
        {
            Trace.Assert(whitePlayer.White);
            Trace.Assert(!blackPlayer.White);
            _whitePlayer = whitePlayer;
            _blackPlayer = blackPlayer;
            _userInterface = userInterface;
            _board = board;

            userInterface.Board = _board;

        }

        public void Play()
        {
            _gameThread = new Thread(() =>
            {
                while (!_gameOver)
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
                            _log.Write(move.ToLongString(_board));
                            _userInterface.WaitForInstructionToMove();
                            _board.Move(move);
                            _userInterface.Redraw();
                        }
                    }
                    catch(NoMovesException)
                    {
                        _gameOver = true;
                    }
                }
            });
            _gameThread.Start();
        }

        public void Stop()
        {
            _gameOver = true;
            _userInterface.StopWaiting();
        }

        public bool WhitesTurn => _board.WhitesTurn;

    }
}
