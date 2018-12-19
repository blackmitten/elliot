using Blackmitten.Menzel;
using System;
using System.Diagnostics;
using System.Threading;

namespace Blackmitten.Elliot.Backend
{
    public enum GameState
    {
        InPlay,
        StaleMate,
        CheckMate,
        Abandoned
    };

    public class Game
    {
        IUserInterface _userInterface;
        Board _board;
        IPlayer _whitePlayer;
        IPlayer _blackPlayer;
        public GameState GameState { get; private set; } = GameState.InPlay;
        bool _applicationClosing = false;
        Thread _gameThread;
        ILogWriter _log;
        IMoveValidator _moveValidator;



        public Game(IPlayer whitePlayer, IPlayer blackPlayer, IUserInterface userInterface, ILogWriter log,
            IMoveValidator moveValidator, Board board = null)
        {
            if (board == null)
            {
                board = BoardFactory.InitNewGame();
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

        public void StartPlay(int moveDelay)
        {
            _gameThread = new Thread(()=>Play(moveDelay));
            _gameThread.Start();
        }

        public void Play(int moveDelay)
        {
            while (GameState == GameState.InPlay)
            {
                PlaySingleMove(moveDelay);
            }
        }

        public void PlaySingleMove(int delay)
        {
            Move move;
            try
            {
                if (_board.HalfMoveClock > 50)
                {
                    GameState = GameState.StaleMate;
                }
                if (_board.WhitesTurn)
                {
                    if (!_whitePlayer.Human)
                    {
                        _userInterface.MachineThinking = true;
                    }
                    move = _whitePlayer.Play(_board);
                    Thread.Sleep(delay);
                    _userInterface.MachineThinking = false;
                }
                else
                {
                    if (!_blackPlayer.Human)
                    {
                        _userInterface.MachineThinking = true;
                    }
                    move = _blackPlayer.Play(_board);
                    Thread.Sleep(delay);
                    _userInterface.MachineThinking = false;
                }
                if (move == null)
                {
                    GameState = GameState.CheckMate;
                }
                if (!_applicationClosing)
                {
                    _userInterface.Redraw();
                }
                if (GameState == GameState.InPlay)
                {
                    _log.Write(move.ToLongString());
                    _userInterface.WaitForInstructionToMove();
                    try
                    {
                        _moveValidator.Validate(move);
                        _board.Move(move, true);
                    }
                    catch (InvalidMoveException e)
                    {
                        _userInterface.InvalidMove(e.Message);
                    }
                    _userInterface.Redraw();
                }
            }
            catch (NoMovesException)
            {
                GameState = GameState.CheckMate;
                _userInterface.MachineThinking = false;
                _userInterface.Redraw();
            }
        }

        public void ApplicationClosing()
        {
            _applicationClosing = true;
            GameState = GameState.Abandoned;
            _userInterface.StopWaiting();
        }

        public bool WhitesTurn => _board.WhitesTurn;
        public bool CurrentPlayerInCheck => _board.CurrentPlayerInCheck;

        public int MoveNumber => _board.FullMoveClock;

    }
}
