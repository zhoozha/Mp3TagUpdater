using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Practices.Unity;

namespace Mp3.Infrastructure
{
    internal static class IoC
    {
        static readonly IUnityContainer _container;

        static IoC()
        {
            _container = new UnityContainer();
        }

        public static T Resolve<T>(params ResolverOverride[] overrides)
        {
            return _container.Resolve<T>(overrides);
        }

        public static IUnityContainer RegisterType<TFrom, TTo>(params InjectionMember[] injectionMembers) where TTo : TFrom
        {
            return _container.RegisterType<TFrom, TTo>(injectionMembers);
        }
    }
}
