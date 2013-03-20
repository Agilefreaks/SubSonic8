using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using MugenInjection.Core;

namespace Client.Common.MugenExtensions
{
    public abstract class MugenModuleWithAutoDiscoveryBase : InjectorModule
    {
        protected readonly List<MugenConvetion> Convetions = new List<MugenConvetion>();
        protected readonly List<Type> Singletons = new List<Type>();

        public override void Load()
        {
            PrepareForLoad();
            var types = GetType().GetTypeInfo().Assembly.GetTypes().Except(Singletons);
            ApplyConventions(types);

            var serviceConvention = new ServiceConvention(Injector);
            foreach (var singleton in Singletons)
            {
                serviceConvention.CreateBinding(singleton);
            }
        }

        protected abstract void PrepareForLoad();

        private void ApplyConventions(IEnumerable<Type> types)
        {
            foreach (var result in types.SelectMany(type => Convetions.Select(c => new { type, convention = c, isMatch = c.ConditionMet(type) }))
                .Where(result => result.isMatch))
            {
                result.convention.CreateBinding(result.type);
            }
        }
    }
}