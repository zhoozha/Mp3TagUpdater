using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Practices.Prism.Logging;

namespace Mp3.Infrastructure.Interfaces
{
    public interface IMp3Logger : ILoggerFacade
    {
        event EventHandler LogEvent;
        bool ShowLog { get; set; }
        void Log(string message);
        void Debug(string message);
        void Error(string message);
    }
}
