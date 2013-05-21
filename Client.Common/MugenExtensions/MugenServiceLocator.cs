namespace Client.Common.MugenExtensions
{
    using System;
    using System.Collections.Generic;
    using Microsoft.Practices.ServiceLocation;
    using MugenInjection;

    public class MugenServiceLocator : IServiceLocator
    {
        #region Fields

        private readonly MugenInjector _kernel;

        #endregion

        #region Constructors and Destructors

        public MugenServiceLocator(MugenInjector kernel)
        {
            _kernel = kernel;
        }

        #endregion

        #region Public Methods and Operators

        public IEnumerable<object> GetAllInstances(Type serviceType)
        {
            return _kernel.GetAll(serviceType);
        }

        public IEnumerable<TService> GetAllInstances<TService>()
        {
            return _kernel.GetAll<TService>();
        }

        public object GetInstance(Type serviceType)
        {
            return _kernel.Get(serviceType);
        }

        public object GetInstance(Type serviceType, string key)
        {
            return _kernel.Get(serviceType, key);
        }

        public TService GetInstance<TService>()
        {
            return _kernel.Get<TService>();
        }

        public TService GetInstance<TService>(string key)
        {
            return _kernel.Get<TService>(key);
        }

        public object GetService(Type serviceType)
        {
            return _kernel.Get(serviceType);
        }

        #endregion
    }
}