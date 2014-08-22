using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Practices.Prism.Logging;
using Mp3.Infrastructure;
using Mp3.Infrastructure.Interfaces;

namespace Mp3TagUI
{
    public class Mp3Progress : IProgressFacade
    {
        public event EventHandler OnProgress;
        public void Update(double current, double total)
        {
            if (OnProgress == null)
            {
                return;
            }
            OnProgress(this, new Mp3ProgressEventArgs(current, total));
        }
    }

    public class Mp3Logger : IMp3Logger
    {
        public event EventHandler LogEvent;
        bool _showLog = true;

        public void Log(string message, Category category, Priority priority)
        {
            if (LogEvent == null || !ShowLog)
            {
                return;
            }
            LogEvent(this, new Mp3LoggerEventArgs(message, category, priority));
        }


        public void Log(string message)
        {
            this.Log(message, Category.Info, Priority.None);
        }


        public bool ShowLog
        {
            get
            {
                return _showLog;
            }
            set
            {
                _showLog = value;
            }
        }

        public void Debug(string message)
        {
#if DEBUG
            Log(message, Category.Debug, Priority.None);
#endif
        }

        public void Error(string message)
        {
            Log(message, Category.Exception, Priority.High);
        }
    }
}
