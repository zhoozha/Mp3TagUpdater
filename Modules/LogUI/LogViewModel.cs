using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel.Composition;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Data;
using System.Windows.Forms;
using System.Windows.Input;
using Microsoft.Practices.Prism.Commands;
using Microsoft.Practices.Prism.Logging;
using Microsoft.Practices.Prism.Mvvm;
using Microsoft.Practices.Unity;
using Mp3.Infrastructure;
using Mp3.Infrastructure.Interfaces;

namespace LogUI
{
    [Export]
    public class LogViewModel : BindableBase
    {
        IMp3Logger _logger;
        bool _allwoLogUI = true;
        List<Mp3LoggerEventArgs> _message = new List<Mp3LoggerEventArgs>();
        DelegateCommand _logClearCommand;
        DelegateCommand _logSaveCommand;

        public LogViewModel(ILoggerFacade logger)
        {
            _logger = logger as IMp3Logger;
            _logger.LogEvent += _logger_LogEvent;
            _logger.ShowLogEvent += _logger_ShowLogEvent;
            _logClearCommand = new DelegateCommand(this.ClearLog, this.CanClearLog);
            _logSaveCommand = new DelegateCommand(SaveLog, CanSaveLog);
            LogMessage = new ObservableCollection<Mp3LoggerEventArgs>();
        }

        public ICommand ClearCommand { get { return _logClearCommand; } }
        public ICommand SaveCommand { get { return _logSaveCommand; } }
        private bool CanSaveLog()
        {
            return CanClearLog();
        }

        private void SaveLog()
        {
            _allwoLogUI = false;
            SaveFileDialog dlg = new SaveFileDialog();
            dlg.AddExtension = true;
            dlg.DefaultExt = "*.txt";
            dlg.Filter = "Text File(*.txt)|*.txt|Log File(*.log)|*.log|All Files(*.*)|*.*";
            dlg.FileName = "Mp3Log.txt";
            if (dlg.ShowDialog() == System.Windows.Forms.DialogResult.OK)
            {
                try
                {
                    using (var stream = new StreamWriter(dlg.FileName))
                    {
                        foreach (var item in _message)
                        {
                            stream.WriteLine(item.Message);
                        }
                    }
                }
                catch (Exception ex)
                {
                    MessageBox.Show(ex.Message, "Error on Log Saving", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    _logger.Error(ex.Message);
                }

            }
            _allwoLogUI = true;
        }

        bool CanClearLog()
        {
            return _message.Count > 0 && _allwoLogUI;
        }
        void ClearLog()
        {
            LogMessage.Clear();
        }

        void _logger_LogEvent(object sender, EventArgs e)
        {
            var args = e as Mp3LoggerEventArgs;
            if (null == args)
                return;
            _message.Add(args);
            System.Windows.Application.Current.Dispatcher.BeginInvoke((Action)(() =>
            {
                LogMessage.Clear();
                for (int i = 0; i < _message.Count(); i++)
                {
                    LogMessage.Add(_message[i]);
                }
            }));
        }


        void _logger_ShowLogEvent(object sender, EventArgs e)
        {
            var args = e as Mp3ShowLogEventArgs;
            if (null == e)
                return;
            this.ShowUI = args.ShowLog;
        }

        public bool ShowUI
        {
            get { return _logger.ShowLog; }
            set { OnPropertyChanged("ShowUI"); OnPropertyChanged("Height"); }
        }

        public ObservableCollection<Mp3LoggerEventArgs> LogMessage { get; set; }

    }
}
