using System;

namespace Blackmitten.Elliot.Backend
{
    public class BoardUpdateEventArgs : EventArgs
    {
        public Board Board { get; }

    }

    public interface IUserInterface
    {
        Board Board { set; }
        bool WaitingForWhiteHuman { get; set; }
        bool WaitingForBlackHuman { get; set; }
        bool MachineThinking { get; set; }

        Move WaitForHuman();
        void StopWaiting();
        void Redraw();

        void WaitForInstructionToMove();

    }
}
