using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Practices.Prism.Logging;
using Microsoft.Practices.Prism.Modularity;
using Microsoft.Practices.Unity;
using Mp3.Infrastructure;

namespace Modules.FolderConverter
{
    [Module(ModuleName = "FolderModule")]
    [ModuleDependency("CharModule")]
    public class FolderModule : IModule
    {
        IUnityContainer _container;
        public FolderModule(IUnityContainer container)
        {
            _container = container;
        }
        public void Initialize()
        {
            _container.RegisterType<IFolderConverter, FolderConverter>();
        }
    }
}
