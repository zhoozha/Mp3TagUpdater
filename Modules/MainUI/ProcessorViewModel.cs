using System;
using System.Collections.Generic;
using System.ComponentModel.Composition;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Windows.Input;
using Microsoft.Practices.Prism.Commands;
using Microsoft.Practices.Prism.Logging;
using Microsoft.Practices.Prism.Mvvm;
using Microsoft.Practices.Unity;
using Mp3.Infrastructure;
using Mp3.Infrastructure.Commands;
using Mp3.Infrastructure.Interfaces;
using Mp3.Infrastructure.Models;

namespace MainUI
{
    public class ProcessorViewModel : BindableBase
    {
        OpenFolderCommand _sourceFolderCommand = null;
        OpenFolderCommand _targetFolderCommand = null;
        UpdateCommand _updateCommand = null;
        IMp3Logger _logger;
        Mp3Folders _model;
        IUnityContainer _container;
        volatile bool _enableCommands = true;

        public bool EnableCommands
        {
            get { return _enableCommands; }
            set
            {
                _enableCommands = value;
                EvaluateCanExecute();
            }
        }

        public ProcessorViewModel(IUnityContainer container, ILoggerFacade logger)
        {
            _container = container;
            _model = new Mp3Folders(_container.Resolve<IMp3Settings>());
            _logger = logger as IMp3Logger;
            var evnt = _container.Resolve<IUILock>();
            evnt.OnUILock += (obj, value) =>
            {
                _enableCommands = !value;
                EvaluateCanExecute();
            };
            _sourceFolderCommand = new OpenFolderCommand(logger, new CommandExecutor()
                {
                    Execute = () =>
                    {
                        evnt.Lock();
                        var ret = this.ExecutedAction("Select source directory where files will be taken for conversion from");
                        evnt.Unlock();
                        return ret;
                    },
                    CanExecuteMethod = () =>
                    {
                        return EnableCommands;
                    }
                }
            );
            _sourceFolderCommand.OnExecuted += (obj, msg) => { if (null != msg && msg.Length > 0)  this.FolderFrom = msg; };
            _sourceFolderCommand.CanExecuteChanged += _sourceFolderCommand_CanExecuteChanged;
            _targetFolderCommand = new OpenFolderCommand(logger, new CommandExecutor()
                {
                    Execute = () =>
                    {
                        evnt.Lock();
                        var ret = this.ExecutedAction("Select source directory where files will be taken for conversion from");
                        evnt.Unlock();
                        return ret;
                    },
                    CanExecuteMethod = () =>
                    {
                        return EnableCommands;
                    }
                }
            );
            _targetFolderCommand.OnExecuted += (obj, msg) => { if (null != msg && msg.Length > 0) this.FolderTo = msg; };
            _targetFolderCommand.CanExecuteChanged += _targetFolderCommand_CanExecuteChanged;
            _updateCommand = new UpdateCommand(logger, new CommandExecutor()
                {
                    ExecuteAsync = async () =>
                    {
                        evnt.Lock();
                        var action = await this.UpdateAction(_model);
                        evnt.Unlock();
                        return action;
                    },
                    CanExecuteMethod = () =>
                    {
                        return ((this.FolderFrom + "").Length > 0 && (this.FolderTo + "").Length > 0) && EnableCommands;
                    }
                }
            );
            _updateCommand.CanExecuteChanged += _updateCommand_CanExecuteChanged;
            _updateCommand.OnStartExecution += _updateCommand_OnStartExecution;
            _updateCommand.OnEndExecution += _updateCommand_OnEndExecution;
        }

        void _updateCommand_OnEndExecution(object sender, EventArgs e)
        {
            //this._enableCommands = true;
        }

        void _updateCommand_OnStartExecution(object sender, EventArgs e)
        {
            //this._enableCommands = false;
        }

        #region Commands
        public ICommand SourceFolder
        {
            get { return _sourceFolderCommand; }
        }
        public ICommand TargetFolder
        {
            get { return _targetFolderCommand; }
        }
        public ICommand Update
        {
            get { return _updateCommand; }
        }

        void _updateCommand_CanExecuteChanged(object sender, EventArgs e)
        {

            canExecuteChanged();
            _logger.Debug("_updateCommand_CanExecuteChanged");
        }

        void _targetFolderCommand_CanExecuteChanged(object sender, EventArgs e)
        {
            canExecuteChanged();
            _logger.Debug("_targetFolderCommand_CanExecuteChanged ");
        }

        void _sourceFolderCommand_CanExecuteChanged(object sender, EventArgs e)
        {
            canExecuteChanged();
            _logger.Debug("_sourceFolderCommand_CanExecuteChanged ");
        }

        void canExecuteChanged()
        {
            FolderFromEnabled = EnableCommands && _sourceFolderCommand.CanExecute(null) & _targetFolderCommand.CanExecute(null);
            FolderToEnabled = FolderFromEnabled;
        }
        #endregion

        #region Model's properties

        bool _folderFromEnabled = true;
        public bool FolderFromEnabled
        {
            get { return _folderFromEnabled; }
            set
            {
                _folderFromEnabled = value;
                OnPropertyChanged("FolderFromEnabled");
            }
        }

        bool _folderToEnabled = true;
        public bool FolderToEnabled
        {
            get { return _folderToEnabled; }
            set
            {
                _folderToEnabled = value;
                OnPropertyChanged("FolderToEnabled");
            }
        }

        public string FolderFrom
        {
            get
            {
                return _model.FolderFrom;
            }
            set
            {
                if (value == null || value.Length == 0 || Directory.Exists(value))
                    _model.FolderFrom = value;

                this.OnPropertyChanged("FolderFrom");
                EvaluateCanExecute();
            }
        }

        public string FolderTo
        {
            get
            {
                return _model.FolderTo;
            }
            set
            {
                if (value == null || value.Length == 0 || Directory.Exists(value))
                    _model.FolderTo = value;

                this.OnPropertyChanged("FolderTo");
                EvaluateCanExecute();
            }
        }

        public bool ProcessTags
        {
            get
            {
                return _model.ProcessTags;
            }
            set
            {
                _model.ProcessTags = value;
                this.OnPropertyChanged("ProcessTags");
            }
        }

        public bool ShowLog
        {
            get
            {
                return _logger.ShowLog;
            }
            set
            {
                _logger.ShowLog = value;
                this.OnPropertyChanged("ShowLog");
            }
        }
        #endregion

        #region Command execution
        void EvaluateCanExecute()
        {
            _sourceFolderCommand.RaiseExecuteChanged();
            _targetFolderCommand.RaiseExecuteChanged();
            _updateCommand.RaiseExecuteChanged();
        }

        public string ExecutedAction(string description)
        {
            FolderBrowserDialog dlg = new FolderBrowserDialog();
            dlg.Description = description;
            if (dlg.ShowDialog() != System.Windows.Forms.DialogResult.OK)
            {
                return string.Empty;
            }
            return dlg.SelectedPath;
        }

        public async Task<string> UpdateAction(Mp3Folders folders)
        {
            _logger.Debug("UpdateAction starts");
            var t = Task.Run(() =>
            {
                try
                {
                    IFolderConverter converter = _container.Resolve<IFolderConverter>();
                    converter.Convert(_model);
                }
                catch (Exception ex)
                {
                    _logger.Error(ex.Message);
                }
            });
            _logger.Debug("UpdateAction ends");
            await t;
            return "";
        }
        #endregion

    }
}
