using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

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

        public Game(IPlayer whitePlayer, IPlayer blackPlayer, IUserInterface userInterface)
        {
            Trace.Assert(whitePlayer.White);
            Trace.Assert(!blackPlayer.White);
            _whitePlayer = whitePlayer;
            _blackPlayer = blackPlayer;
            _userInterface = userInterface;
            _board = Board.InitNewGame();

            userInterface.Board = _board;
            //            userInterface.BoardUpdated += BoardUpdated;

            Play();
        }

        private void Play()
        {
            Thread thread = new Thread(() =>
            {
                while (!_gameOver)
                {
                    Move move;
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
                    if (!_gameOver)
                    {
                        _board.Move(move);
                        _userInterface.Redraw();
                    }
                }
            });
            thread.Start();
        }

        private void BoardUpdated(object sender, BoardUpdateEventArgs e)
        {
            _board = e.Board;
            Play();
        }

        public void Stop()
        {
            _gameOver = true;
            _userInterface.StopWaiting();
        }

        public bool WhitesTurn => _board.WhitesTurn;

    }
}
