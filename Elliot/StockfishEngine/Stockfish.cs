﻿using Blackmitten.Elliot.Backend;
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
        AutoResetEvent _bestMoveReady = new AutoResetEvent(false);
        AutoResetEvent _readyOk = new AutoResetEvent(false);
        private bool _quitting = false;

        private string _bestMove;

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

        void SendCommand(string command)
        {
            Console.WriteLine(command);
            _process.StandardInput.WriteLine(command);
        }

        public void Stop()
        {
            _quitting = true;
            SendCommand("quit");
            _process.WaitForExit();
        }

        public Move GetBestMove(Board board)
        {
            throw new NotImplementedException("need to use board parameter");
            SendCommand("go depth 20");
            _bestMoveReady.WaitOne();
            return new Move(_bestMove);
        }

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
            Console.WriteLine(e.Data);
            if (!_quitting)
            {
                throw new Exception();
            }
        }

        private void Stockfish_OutputDataReceived(object sender, DataReceivedEventArgs e)
        {
            if (e.Data == null)
            {
                return;
            }
            Console.WriteLine(e.Data);
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
                throw new Exception(e.Data);
            }
        }

    }
}
