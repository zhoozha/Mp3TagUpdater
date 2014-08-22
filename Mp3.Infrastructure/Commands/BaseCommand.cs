using System;
using System.Collections.Generic;
using System.ComponentModel.Composition;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;
using Microsoft.Practices.Prism.Logging;
using Microsoft.Practices.Unity;

namespace Mp3.Infrastructure.Commands
{
    public class CommandEventArgs : EventArgs
    {
        public bool CanExecute { get; set; }
    }
    public class BaseCommand : ICommand
    {
        public event EventHandler CanExecuteChanged;
        protected CommandExecutor _executor;
        protected ILoggerFacade _logger;

        public BaseCommand(ILoggerFacade logger, CommandExecutor executor)
        {
            _logger = logger;
            _executor = executor;
        }

        public virtual void Execute(object parameter)
        {
            this.ExecuteAsync(parameter);
        }

        public virtual async void ExecuteAsync(object parameter)
        {
            await _executor.ExecuteAsync.Invoke();
        }

        public bool CanExecute(object parameter)
        {
            return _executor.CanExecuteMethod.Invoke();
        }

        public void RaiseExecuteChanged()
        {
            if (null == CanExecuteChanged)
            {
                return;
            }
            CanExecuteChanged(this, new CommandEventArgs() { CanExecute = _executor.CanExecuteMethod() });
        }
    }
}
