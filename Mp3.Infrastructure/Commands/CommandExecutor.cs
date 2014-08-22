using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Mp3.Infrastructure.Commands
{
    public delegate Task<String> ExecuteDelegate();
    public class CommandExecutor
    {
        public Func<string> Execute { get; set; }
        public Func<bool> CanExecuteMethod;
        public ExecuteDelegate ExecuteAsync { get; set; }
    }
}
