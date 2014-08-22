using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Practices.Prism.Modularity;
using Microsoft.Practices.ServiceLocation;
using Microsoft.Practices.Unity;
using Mp3.Infrastructure;

namespace Modules.CharConverter
{
    [Module(ModuleName = "CharModule")]
    public class CharModule : IModule
    {
        IUnityContainer _container;
        public CharModule(IUnityContainer container)
        {
            _container = container;
        }
        public void Initialize()
        {
            _container.RegisterType<IStringConverter, CharConverter>();
        }
    }
}
