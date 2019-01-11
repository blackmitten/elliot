using Blackmitten.Menzel;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Threading;

namespace Blackmitten.Elliot.Backend
{
    public enum GameState
    {
        InPlay,
        StaleMate,
        WhiteWins,
        BlackWins,
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

            Assert.IsTrue(whitePlayer.White);
            Assert.IsTrue(!blackPlayer.White);
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
            _gameThread = new Thread(() => Play(moveDelay));
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
            Move move = null;
            if (_board.HalfMoveClock > 50)
            {
                GameState = GameState.StaleMate;
            }
            if (_board.WhitesTurn)
            {
                if (_board.WhiteCanMove)
                {
                    if (!_whitePlayer.Human)
                    {
                        _userInterface.MachineThinking = true;
                    }
                    move = _whitePlayer.Play(_board);
                    Thread.Sleep(delay);
                }
                else if (_board.WhiteInCheck)
                {
                    GameState = GameState.BlackWins;
                }
                else
                {
                    GameState = GameState.StaleMate;
                }
                _userInterface.MachineThinking = false;
            }
            else
            {
                if (_board.BlackCanMove)
                {
                    if (!_blackPlayer.Human)
                    {
                        _userInterface.MachineThinking = true;
                    }
                    move = _blackPlayer.Play(_board);
                    Thread.Sleep(delay);
                }
                else if (_board.BlackInCheck)
                {
                    GameState = GameState.WhiteWins;
                }
                else
                {
                    GameState = GameState.StaleMate;
                }
                _userInterface.MachineThinking = false;
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
                    var undo = new Undo();
#if DIAGNOSTIC
                    string fenBefore = _board.GetFenString();

                    _board.Move(move, true, undo);
                    string fenAfter = _board.GetFenString();
                    _board.CheckIntegrity();
                    Assert.IsTrue(fenBefore != fenAfter);

                    _board.UndoLastmove( undo );
                    string fenAfterUndo = _board.GetFenString();
                    _board.CheckIntegrity();
                    Assert.IsTrue(fenBefore == fenAfterUndo);

                    _board.Move(move, true, undo);
                    string fenAfterAgain = _board.GetFenString();
                    _board.CheckIntegrity();
                    Assert.IsTrue(fenAfter == fenAfterAgain);
#else
                    _board.Move(move, true, undo);
                    _board.CheckIntegrity();
#endif
                }
                catch (InvalidMoveException e)
                {
                    _userInterface.InvalidMove(e.Message);
                }

                _userInterface.Redraw();
            }
        }

        public void EndGame()
        {
            _gameThread.Join();
        }

        public void ApplicationClosing()
        {
            _applicationClosing = true;
            _whitePlayer.Kill();
            _blackPlayer.Kill();
            GameState = GameState.Abandoned;
            _userInterface.StopWaiting();
        }

        public bool WhitesTurn => _board.WhitesTurn;
        public bool CurrentPlayerInCheck => _board.CurrentPlayerInCheck;

        public int MoveNumber => _board.FullMoveClock;

        public string FenString => _board.GetFenString();
    }
}
