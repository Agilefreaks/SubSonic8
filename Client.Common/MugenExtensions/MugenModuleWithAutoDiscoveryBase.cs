using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using MugenInjection;
using MugenInjection.Core;

namespace Client.Common.MugenExtensions
{
    public abstract class MugenModuleWithAutoDiscoveryBase : InjectorModule
    {
        protected readonly List<MugenConvetion> Convetions = new List<MugenConvetion>();
        protected readonly List<Tuple<Type[], Type>> Singletons = new List<Tuple<Type[], Type>>();

        public override void Load()
        {
            PrepareForLoad();
            var singletonTypes = Singletons.SelectMany(singleton => singleton.Item1);
            var types = GetType().GetTypeInfo().Assembly.GetTypes().Except(singletonTypes);
            ApplyConventions(types);

            foreach (var singleton in Singletons)
            {
                Injector.Bind(singleton.Item1).To(singleton.Item2).InSingletonScope();
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