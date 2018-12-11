using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Blackmitten.Elliot.Backend
{
    public class MachinePlayer : IPlayer
    {
        IUserInterface _userInterface;
        IEngine _engine;

        public MachinePlayer(bool white, IUserInterface userInterface, IEngine engine)
        {
            White = white;
            _userInterface = userInterface;
            _engine = engine;
        }

        public bool Human => false;

        public bool White { get; }

        public void Kill() => _engine.Stop();

        public Move Play(Board board)
        {
            Move move = _engine.GetBestMove(board);
            return move;
        }
    }
}
