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
        IUserInterface m_userInterface;
        Board m_currentBoard;
        IPlayer m_white;
        IPlayer m_black;
//        TranspositionTable m_transpositionTable = new TranspositionTable();

        public Game(IPlayer white, IPlayer black, IUserInterface userInterface)
        {
            Trace.Assert(white.White);
            Trace.Assert(!black.White);
            m_white = white;
            m_black = black;
            m_userInterface = userInterface;
            m_currentBoard = Board.InitNewGame();

            userInterface.Board = m_currentBoard;
            userInterface.BoardUpdated += BoardUpdated;

            Play();
        }

        private void Play()
        {
            Thread thread = new Thread(() =>
            {
                bool gameOver = false;
                while (!gameOver)
                {
                    m_white.Play();
                    m_black.Play();
                }

                /*
                if (m_currentBoard.WhitesTurn && !m_whiteHuman)
                {
                    m_userInterface.MachineThinking = true;
                    Board newBoard = m_currentBoard.ThinkAndMove(m_transpositionTable);
                    m_userInterface.Update(newBoard);
                    m_userInterface.MachineThinking = false;
                }
                else if (!m_currentBoard.WhitesTurn && !m_blackHuman)
                {
                    m_userInterface.MachineThinking = true;
                    Board newBoard = m_currentBoard.ThinkAndMove(m_transpositionTable);
                    m_userInterface.Update(newBoard);
                    m_userInterface.MachineThinking = false;
                }
                */
            });
            thread.Start();
        }

        private void BoardUpdated(object sender, BoardUpdateEventArgs e)
        {
            m_currentBoard = e.Board;
            Play();
        }


    }
}
