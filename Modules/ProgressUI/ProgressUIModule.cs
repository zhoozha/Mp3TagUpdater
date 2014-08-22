using System;
using Microsoft.Practices.Prism.Modularity;
using Microsoft.Practices.Prism.Regions;
using Microsoft.Practices.Unity;
using Mp3.Infrastructure;

namespace ProgressUI
{
    public class ProgressUIModule : IModule
    {
        private readonly IRegionManager _regionManager;
        IUnityContainer _container;

        public ProgressUIModule(IRegionManager regionManager, IUnityContainer container)
        {
            _regionManager = regionManager;
            _container = container;
        }
        public void Initialize()
        {
            _container.RegisterType<ProgressViewModel, ProgressViewModel>();
            _regionManager.RegisterViewWithRegion(RegionNames.Progress, typeof(ProgressView));
        }
    }
}
