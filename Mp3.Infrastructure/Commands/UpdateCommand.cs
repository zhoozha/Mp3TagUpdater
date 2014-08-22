using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;
using Microsoft.Practices.Prism.Logging;

namespace Mp3.Infrastructure.Commands
{
    public class UpdateCommand : BaseCommand
    {
        public event EventHandler OnStartExecution;
        public event EventHandler OnEndExecution;
        public UpdateCommand(ILoggerFacade logger, CommandExecutor executor) :
            base(logger, executor)
        { }

        public override void Execute(object parameter)
        {
            if (null != OnStartExecution)
            {
                OnStartExecution(this, new EventArgs());
            }
            RaiseExecuteChanged();
            base.Execute(parameter);
            if (null != OnEndExecution)
            {
                OnEndExecution(this, new EventArgs());
            }
            RaiseExecuteChanged();
        }
    }
}
