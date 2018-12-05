using System;

namespace Blackmitten.Elliot.Backend
{
    public class BoardUpdateEventArgs : EventArgs
    {
        public Board Board { get; }

        public BoardUpdateEventArgs(Board board)
        {
            Board = board;
        }
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
        void DoTheMove();

        /*
        bool MachineThinking { set; }

        event EventHandler<BoardUpdateEventArgs> BoardUpdated;
        */
    }
}
