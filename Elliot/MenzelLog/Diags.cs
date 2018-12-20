using System;
using System.Collections.Generic;
using System.Text;

namespace Blackmitten.Menzel
{
    public class Diags
    {
        public static void Assert(bool assertion)
        {
            if (!assertion)
            {
                throw new AssertionFailedException();
            }
        }
    }
}
