using Blackmitten.Elliot.Backend;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading;
using Blackmitten.Menzel;

namespace BlackMitten.Elliot.StockfishEngine
{
    public class Stockfish : IEngine
    {
        Process _process;
        ManualResetEvent _fishReady = new ManualResetEvent(false);
        AutoResetEvent _bestMoveReady = new AutoResetEvent(false);
        AutoResetEvent _readyOk = new AutoResetEvent(false);
        AutoResetEvent _errorEvent = new AutoResetEvent(false);
        private bool _quitting = false;
        int _depth;
        private string _bestMove;
        string _stockfishLinBinPath = @"/home/carl/Downloads/stockfish-10-linux/Linux/stockfish_10_x64";
        string _stockfishWinBinPath = @"C:\bin\stockfish\stockfish_9_x64.exe";

        public Stockfish(int depth)
        {
            _depth = depth;

            _process = new Process();
            _process.StartInfo.FileName = Diags.Platform == Diags.PlatformType.Windows ? _stockfishWinBinPath : _stockfishLinBinPath;
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
                throw new EngineTimeoutException("Stockfish did not respond");
            }

        }

        void SendCommand(string command)
        {
//            Console.WriteLine(command);
            _process.StandardInput.WriteLine(command);
        }

        public void Stop()
        {
            _quitting = true;
            SendCommand("quit");
            _process.WaitForExit();
        }

        public Move GetBestMove(Board board, bool doDiags)
        {
            SendCommand("position fen " + board.GetFenString());
            SendCommand("go depth " + _depth.ToString(CultureInfo.InvariantCulture));
            int eventIndex = WaitHandle.WaitAny(new WaitHandle[] { _bestMoveReady, _errorEvent });
            if (eventIndex == 1)
            {
                throw new EngineErrorException("Engine error");
            }
            return new Move(board, _bestMove);
        }

        public string Name { get; } = "Stockfish";

        public void Move(string move)
        {
            SendCommand("position startpos moves " + move);
            WaitForReady();
        }

        private void WaitForReady()
        {
            SendCommand("isready");
            _readyOk.WaitOne();
        }

        private void Stockfish_ErrorDataReceived(object sender, DataReceivedEventArgs e)
        {
            if (e.Data != null)
            {
                throw new Exception(e.Data);
            }
//            Console.WriteLine(e.Data);
            if (!_quitting)
            {
                _errorEvent.Set();
            }
        }

        private void Stockfish_OutputDataReceived(object sender, DataReceivedEventArgs e)
        {
            if (e.Data == null)
            {
                return;
            }
//            Console.WriteLine(e.Data);
            if (e.Data.StartsWith("Stockfish "))
            {
                _fishReady.Set();
            }
            else if (e.Data.StartsWith("bestmove "))
            {
                string[] bits = e.Data.Split(' ');
                _bestMove = bits[1];
                _bestMoveReady.Set();
            }
            else if (e.Data.StartsWith("info "))
            {
                // ignore
            }
            else if (e.Data == "readyok")
            {
                _readyOk.Set();
            }
            else
            {
                throw new EngineErrorException("Unknown engine return: " + e.Data);
            }
        }

    }
}
