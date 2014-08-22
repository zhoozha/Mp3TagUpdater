using System;
using Microsoft.Practices.Prism.Logging;
using Microsoft.Practices.Prism.Mvvm;
using Microsoft.Practices.Unity;
using Mp3.Infrastructure;
using Mp3.Infrastructure.Interfaces;

namespace ProgressUI
{
    public class ProgressViewModel : BindableBase
    {
        IMp3Logger _logger;
        IUnityContainer _container;
        IProgressFacade _progressEvent;
        double _progress = 0;
        double _max = 100;

        public ProgressViewModel(IUnityContainer container, ILoggerFacade logger, IProgressFacade progress)
        {
            _container = container;
            _logger = logger as IMp3Logger;
            _progressEvent = progress;
            _progressEvent.OnProgress += _progressEvent_OnProgress;
        }

        void _progressEvent_OnProgress(object sender, EventArgs e)
        {
            var evnt = e as Mp3ProgressEventArgs;
            if (null == evnt)
                return;
            if (_max != evnt.Total)
            {
                MaxProgress = evnt.Total;
            }
            CurrentProgress = evnt.Total == 0 ? 0 : evnt.Current; ;
        }

        public double MaxProgress
        {
            get
            {
                return _max;
            }
            set
            {
                _max = value;
                OnPropertyChanged("MaxProgress");
            }
        }

        public double CurrentProgress
        {
            get
            {
                return _progress;
            }
            set
            {
                _progress = value;
                OnPropertyChanged("CurrentProgress");
            }
        }
    }
}
