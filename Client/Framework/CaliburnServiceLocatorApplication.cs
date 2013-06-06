namespace Subsonic8.Framework
{
    using System;
    using System.Collections.Generic;
    using Caliburn.Micro;
    using global::Common.MugenExtensions;
    using Microsoft.Practices.ServiceLocation;
    using MugenInjection;
    using MugenInjection.Interface;

    public class CaliburnServiceLocatorApplication : CaliburnApplication
    {
        #region Fields

        protected readonly MugenInjector Kernel = new MugenInjector();

        #endregion

        #region Constructors and Destructors

        public CaliburnServiceLocatorApplication()
        {
            var locator = new MugenServiceLocator(Kernel);
            ServiceLocator.SetLocatorProvider(() => locator);

            Kernel.Bind<IInjector>().ToConstant(Kernel);
            Kernel.Bind<MugenInjector>().ToConstant(Kernel);
            Kernel.Bind<MugenServiceLocator>().ToConstant(locator);
            Kernel.Bind<IServiceLocator>().ToConstant(locator);
        }

        #endregion

        #region Methods

        protected override void BuildUp(object instance)
        {
            Kernel.Inject(instance);
        }

        protected override IEnumerable<object> GetAllInstances(Type service)
        {
            return Kernel.GetAll(service);
        }

        protected override object GetInstance(Type service, string key)
        {
            return string.IsNullOrWhiteSpace(key) ? Kernel.Get(service) : Kernel.Get(service, key);
        }

        #endregion
    }
}