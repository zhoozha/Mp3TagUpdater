using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows;
using Microsoft.Practices.Prism.Logging;
using Microsoft.Practices.Prism.Modularity;
using Microsoft.Practices.Prism.Regions;
using Microsoft.Practices.Prism.UnityExtensions;
using Microsoft.Practices.ServiceLocation;
using Microsoft.Practices.Unity;
using Mp3.Infrastructure;
using Mp3.Infrastructure.Behavior;
using Mp3.Infrastructure.Interfaces;
using Mp3.Infrastructure.Models;

namespace Mp3TagUI
{
    public class UILock : IUILock
    {
        private class DisposeLock : IDisposable
        {
            UILock _lock;
            public DisposeLock(UILock uiLock)
            {
                _lock = uiLock;
            }
            public void Dispose()
            {
                _lock.Unlock();
            }
        }
        public event EventHandler<bool> OnUILock;

        public void Lock()
        {
            if (null == OnUILock)
                return;
            OnUILock(this, true);
        }

        public void Unlock()
        {
            if (null == OnUILock)
                return;
            OnUILock(this, false);
        }

        public IDisposable Disposer()
        {
            return new DisposeLock(this);
        }

    }

    public class Bootstrapper : UnityBootstrapper
    {

        protected override System.Windows.DependencyObject CreateShell()
        {
            return Container.Resolve<Shell>();
        }

        protected override void InitializeShell()
        {
            Application.Current.MainWindow = (Window)Shell;
            Application.Current.MainWindow.Show();
        }

        protected override IModuleCatalog CreateModuleCatalog()
        {
            var path = AppDomain.CurrentDomain.BaseDirectory;
            var catalog = new DirectoryModuleCatalog() { ModulePath = path + @"Modules" };
            return catalog;
        }

        protected override void ConfigureContainer()
        {
            base.ConfigureContainer();
            this.Container.RegisterInstance<ILoggerFacade>(Logger);
            this.Container.RegisterInstance<IProgressFacade>(new Mp3Progress());
            this.Container.RegisterType<IMp3Settings, Mp3Settings>();
            this.Container.RegisterType<IRegionManager, RegionManager>();
            this.Container.RegisterType<IUILock, UILock>();
        }

        protected override void ConfigureModuleCatalog()
        {
            base.ConfigureModuleCatalog();
        }

        protected override ILoggerFacade CreateLogger()
        {
            return new Mp3Logger();
        }

        protected override Microsoft.Practices.Prism.Regions.IRegionBehaviorFactory ConfigureDefaultRegionBehaviors()
        {
            var factory = base.ConfigureDefaultRegionBehaviors();
            factory.AddIfMissing("AutoPopulateExportedViewsBehavior", typeof(AutoPopulateExportedViewsBehavior));

            return factory;
        }

    }
}
