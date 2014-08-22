using System;
using System.ComponentModel.Composition;
using Microsoft.Practices.Prism.Modularity;
using Microsoft.Practices.Prism.Regions;
using Microsoft.Practices.Unity;
using Mp3.Infrastructure;

namespace MainUI
{
    [Module(ModuleName = "MainUIModule")]
    public class MainUIModule : IModule
    {
        private readonly IRegionManager _regionManager;
        IUnityContainer _container;

        public MainUIModule(IRegionManager regionManager, IUnityContainer container)
        {
            _regionManager = regionManager;
            _container = container;
        }
        public void Initialize()
        {
           _container.RegisterType<ProcessorViewModel, ProcessorViewModel>();
           _regionManager.RegisterViewWithRegion(RegionNames.Main, typeof(ProcessorView));
        }
    }
}
