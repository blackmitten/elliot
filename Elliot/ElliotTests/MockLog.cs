using Blackmitten.Menzel;
using System;
using System.Diagnostics;

namespace ElliotTests
{
    internal class MockLog : ILogWriter
    {
        public void Write(string s)
        {
            Debug.WriteLine(s);
        }

    }
}