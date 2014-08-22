using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;
using Microsoft.Practices.Prism.Logging;

namespace Mp3.Infrastructure.Commands
{
    public class OpenFolderCommand : BaseCommand
    {
        public delegate void ExecuteEvent(object obj, string msg);
        public event ExecuteEvent OnExecuted;

        public OpenFolderCommand(ILoggerFacade logger, CommandExecutor executor)
            : base(logger, executor) { }
        
        public override void Execute(object parameter)
        {
            var result = this._executor.Execute();
            if (null == OnExecuted)
            {
                return;
            }
            OnExecuted(this, result);
        }
    }
}
