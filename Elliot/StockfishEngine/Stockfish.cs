using BlackMitten.Elliot.Engine;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace BlackMitten.Elliot.StockfishEngine
{
    internal class Stockfish : IEngine
    {
        Process _process;
        ManualResetEvent _fishReady = new ManualResetEvent(false);

        public Stockfish(string filename)
        {
            _process = new Process();
            _process.StartInfo.FileName = filename;
            _process.StartInfo.RedirectStandardInput = true;
            _process.StartInfo.RedirectStandardOutput = true;
            _process.StartInfo.RedirectStandardError = true;
            _process.StartInfo.CreateNoWindow = true;
            _process.OutputDataReceived += Stockfish_OutputDataReceived;
            _process.ErrorDataReceived += Stockfish_ErrorDataReceived;
            _process.StartInfo.UseShellExecute = false;

            _process.Start();
            _process.BeginOutputReadLine();
            _process.BeginErrorReadLine();

            if (!_fishReady.WaitOne(5000))
            {
                throw new TimeoutException("Stockfish did not respond");
            }

        }

        public void Stop()
        {
            _process.StandardInput.WriteLine("quit");
            _process.WaitForExit();
        }

        private void Stockfish_ErrorDataReceived(object sender, DataReceivedEventArgs e)
        {
            int j = 1;
        }

        private void Stockfish_OutputDataReceived(object sender, DataReceivedEventArgs e)
        {
            if (e.Data == null)
            {
                return;
            }
            if (e.Data.StartsWith("Stockfish "))
            {
                _fishReady.Set();
            }
        }

    }
}
