using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Blackmitten.Elliot.Backend
{
    public class HumanPlayer : IPlayer
    {
        IUserInterface _userInterface;

        public HumanPlayer(bool white, IUserInterface userInterface)
        {
            White = white;
            _userInterface = userInterface;
        }

        public bool Human => true;

        public bool White { get; }
        public string Name { get; set; } = "Human";

        public void Kill()
        {
        }

        public Move Play(Board board)
        {
            Move move;
            if (White)
            {
                _userInterface.WaitingForBlackHuman = false;
                _userInterface.WaitingForWhiteHuman = true;
                move = _userInterface.WaitForHuman();
            }
            else
            {
                _userInterface.WaitingForBlackHuman = true;
                _userInterface.WaitingForWhiteHuman = false;
                move = _userInterface.WaitForHuman();
            }
            return move;
        }

    }
}
