using Blackmitten.Elliot.Backend;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Text;

namespace ElliotTests
{
    class MockUI : IUserInterface
    {
        Board _board;
        string _fenString = "";

        void WriteLine(string s)
        {
//            Debug.WriteLine(s);
        }

        public Board Board
        {
            set
            {
                _board = value;
                WriteLine(_board.GetFenString());
            }
        }
        public bool WaitingForWhiteHuman { get => throw new NotImplementedException(); set => throw new NotImplementedException(); }
        public bool WaitingForBlackHuman { get => throw new NotImplementedException(); set => throw new NotImplementedException(); }
        public bool MachineThinking
        {
            get => false;
            set
            {
            }
        }


        public void InvalidMove(string message)
        {
            WriteLine("Invalid move: " + message);
        }

        public void Redraw()
        {
            string fen = _board.GetFenString();
            if (fen != _fenString)
            {
                _fenString = fen;
                WriteLine(_board.ToLongString());
            }
        }

        public void StopWaiting() => throw new NotImplementedException();
        public Move WaitForHuman() => throw new NotImplementedException();
        public void WaitForInstructionToMove()
        {

        }
    }
}
