using System;
using System.Collections.Generic;
using Microsoft.Practices.ServiceLocation;
using MugenInjection;

namespace Client.Common.MugenExtensions
{
    public class MugenServiceLocator : IServiceLocator
    {
        private readonly MugenInjector _kernel;

        public MugenServiceLocator(MugenInjector kernel)
        {
            _kernel = kernel;
        }

        public object GetService(Type serviceType)
        {
            return _kernel.Get(serviceType);
        }

        public object GetInstance(Type serviceType)
        {
            return _kernel.Get(serviceType);
        }

        public object GetInstance(Type serviceType, string key)
        {
            return _kernel.Get(serviceType, key);
        }

        public IEnumerable<object> GetAllInstances(Type serviceType)
        {
            return _kernel.GetAll(serviceType);
        }

        public TService GetInstance<TService>()
        {
            return _kernel.Get<TService>();
        }

        public TService GetInstance<TService>(string key)
        {
            return _kernel.Get<TService>(key);
        }

        public IEnumerable<TService> GetAllInstances<TService>()
        {
            return _kernel.GetAll<TService>();
        }
    }
}
