﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Blackmitten.Elliot.Backend
{
    public class InvalidMoveException : ElliotChessException
    {
        string _message;

        public InvalidMoveException(string message)
        {
            _message = message;
        }

        public override string Message => _message;
    }
}
