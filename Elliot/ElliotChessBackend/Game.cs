﻿using Blackmitten.Menzel;
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

        public void StartPlay()
        {
            _gameThread = new Thread(Play);
            _gameThread.Start();
        }

        public void Play()
        {
            while (!_gameOver)
            {
                PlaySingleMove();
            }
        }

        public void PlaySingleMove()
        {
            Move move;
            try
            {
                if(_board.HalfMoveClock>50)
                {
                    _gameOver = true;
                }
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
                if (move == null)
                {
                    _gameOver = true;
                }
                if (!_applicationClosing)
                {
                    _userInterface.Redraw();
                }
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
                        _userInterface.InvalidMove(e.Message);
                    }
                    _userInterface.Redraw();
                }
            }
            catch (NoMovesException)
            {
                _gameOver = true;
                _userInterface.MachineThinking = false;
                _userInterface.Redraw();
            }
        }

        public void ApplicationClosing()
        {
            _applicationClosing = true;
            _gameOver = true;
            _userInterface.StopWaiting();
        }

        public bool WhitesTurn => _board.WhitesTurn;
        public bool CurrentPlayerInCheck => _board.CurrentPlayerInCheck;

    }
}
