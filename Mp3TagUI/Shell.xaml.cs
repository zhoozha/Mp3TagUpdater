using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using System.ComponentModel.Composition;
using Microsoft.Practices.Unity;
using Microsoft.Practices.Prism.Logging;
using Mp3.Infrastructure.Interfaces;
using Mp3.Infrastructure;

namespace Mp3TagUI
{
    /// <summary>
    /// Interaction logic for Shell.xaml
    /// </summary>
    [Export]
    public partial class Shell : Window
    {
        public Shell(IUnityContainer container, ILoggerFacade logger)
        {
            InitializeComponent();
            var _logger = logger as IMp3Logger;
            if (null == _logger)
                return;
            _logger.ShowLogEvent += _logger_ShowLogEvent;
            SetHeight(_logger.ShowLog);
        }

        void SetHeight(bool showLog)
        {
            this.Height = showLog ? 350 : 200;
        }

        void _logger_ShowLogEvent(object sender, EventArgs e)
        {
            var args = e as Mp3ShowLogEventArgs;
            if (null == args)
                return;
            SetHeight(args.ShowLog);
        }

        ShellViewModel ViewModel
        {
            set
            {
                this.DataContext = value;
            }
        }
    }
}
