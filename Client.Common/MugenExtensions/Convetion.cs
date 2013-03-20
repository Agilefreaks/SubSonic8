using System;
using MugenInjection.Interface;

namespace Client.Common.MugenExtensions
{
    public abstract class MugenConvetion
    {
        protected readonly IInjector Injector;

        protected MugenConvetion(IInjector injector)
        {
            Injector = injector;
        }

        public abstract bool ConditionMet(Type type);

        public abstract Type GetTargetType(Type type);

        public abstract void CreateBinding(Type type);
    }
}