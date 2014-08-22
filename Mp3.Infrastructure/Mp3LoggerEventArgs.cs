using System;
using Microsoft.Practices.Prism.Logging;

namespace Mp3.Infrastructure
{
    public class Mp3LoggerEventArgs : EventArgs
    {
        public Mp3LoggerEventArgs(string message, Category category, Priority priority)
        {
            this.Message = message;
            this.Category = category;
            this.Priority = priority;
        }
        public string Message { get; private set; }
        public Category Category { get; private set; }
        public Priority Priority { get; private set; }
    }

}
