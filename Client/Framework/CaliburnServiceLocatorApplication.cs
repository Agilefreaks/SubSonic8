using System;
using System.Collections.Generic;
using Caliburn.Micro;
using Microsoft.Practices.ServiceLocation;
using MugenInjection;
using MugenInjection.Interface;
using MugenInjection.Interface.Behaviors;
using MugenInjection.Interface.Components;

namespace Subsonic8.Framework
{
    public class CaliburnServiceLocatorApplication : CaliburnApplication
    {
        protected readonly MugenInjector Kernel = new MugenInjector();

        public CaliburnServiceLocatorApplication()
        {
            Kernel.Components.Get<IBehaviorManagerComponent>()
                .Add<IResolveUnregisteredTypeBehavior>(new MugenUnregesiteredTypeResolver());

            var locator = new MugenServiceLocator(Kernel);
            ServiceLocator.SetLocatorProvider(() => locator);

            Kernel.Bind<IInjector>().ToConstant(Kernel);
            Kernel.Bind<MugenInjector>().ToConstant(Kernel);
            Kernel.Bind<MugenServiceLocator>().ToConstant(locator);
            Kernel.Bind<IServiceLocator>().ToConstant(locator);
        }

        protected override object GetInstance(Type service, string key)
        {
            return string.IsNullOrWhiteSpace(key) ? Kernel.Get(service) : Kernel.Get(service, key);
        }

        protected override IEnumerable<object> GetAllInstances(Type service)
        {
            return Kernel.GetAll(service);
        }

        protected override void BuildUp(object instance)
        {
            Kernel.Inject(instance);
        }
    }
}
