using System;
using Microsoft.Practices.Prism.Modularity;
using Microsoft.Practices.Prism.Regions;
using Microsoft.Practices.Unity;
using Mp3.Infrastructure;

namespace LogUI
{
    public class LogUIModule : IModule
    {
        IRegionManager _regionManager;
        IUnityContainer _container;

        public LogUIModule(IUnityContainer container, IRegionManager regionManager)
        {
            _container = container;
            _regionManager = regionManager;
        }

        public void Initialize()
        {
            _container.RegisterType<LogViewModel, LogViewModel>();
            _regionManager.RegisterViewWithRegion(RegionNames.Log, typeof(LogView));
        }
    }
}
