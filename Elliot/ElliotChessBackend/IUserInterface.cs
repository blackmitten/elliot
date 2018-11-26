using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

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
        /*
        bool MachineThinking { set; }

        event EventHandler<BoardUpdateEventArgs> BoardUpdated;
        */
    }
}
