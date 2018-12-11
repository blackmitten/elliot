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

        public Board Board
        {
            set
            {
                _board = value;
                Trace.WriteLine(_board.GetFenString());
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

        public void DoTheMove() => throw new NotImplementedException();
        public void InvalidMove(string message)
        {
            Trace.WriteLine("Invalid move: " + message);
        }

        public void Redraw()
        {
//            Trace.WriteLine(_board.GetFenString());
        }

        public void StopWaiting() => throw new NotImplementedException();
        public Move WaitForHuman() => throw new NotImplementedException();
        public void WaitForInstructionToMove()
        {

        }
    }
}
