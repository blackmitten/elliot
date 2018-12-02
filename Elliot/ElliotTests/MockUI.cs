using Blackmitten.Elliot.Backend;
using System;
using System.Collections.Generic;
using System.Text;

namespace ElliotTests
{
    class MockUI : IUserInterface
    {
        public Board Board
        {
            set
            {
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

        public void Redraw() => throw new NotImplementedException();
        public void StopWaiting() => throw new NotImplementedException();
        public Move WaitForHuman() => throw new NotImplementedException();
    }
}
