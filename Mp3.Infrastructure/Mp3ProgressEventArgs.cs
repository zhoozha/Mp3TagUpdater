using System;

namespace Mp3.Infrastructure
{
    public class Mp3ProgressEventArgs : EventArgs
    {
        public double Current { get; private set; }
        public double Total { get; private set; }
        
        public Mp3ProgressEventArgs(double current, double total)
        {
            Current = current;
            Total = total;
        }
    }
}
