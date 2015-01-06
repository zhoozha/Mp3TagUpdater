using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Mp3.Infrastructure
{
    public class Mp3ShowLogEventArgs : EventArgs
    {
        public Mp3ShowLogEventArgs(bool showLog)
        {
            this.ShowLog = showLog;
        }
        public bool ShowLog { get; private set; }
    }
}
