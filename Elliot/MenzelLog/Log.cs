using System.Collections.Generic;

namespace Blackmitten.Menzel
{

    internal class Log : ILog
    {
        List<string> _log = new List<string>();
        object _lock = new object();

        public Log()
        {

        }

        public IEnumerable<string> Read()
        {
            List<string> copy = new List<string>();
            lock (_lock)
            {
                copy.AddRange(_log);
                _log.Clear();
            }
            return copy;
        }

        public void Write(string s)
        {
            lock (_lock)
            {
                _log.Add(s);
            }
        }



    }
}
