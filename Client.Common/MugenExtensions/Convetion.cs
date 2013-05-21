namespace Client.Common.MugenExtensions
{
    using System;
    using MugenInjection.Interface;

    public abstract class MugenConvetion
    {
        #region Fields

        protected readonly IInjector Injector;

        #endregion

        #region Constructors and Destructors

        protected MugenConvetion(IInjector injector)
        {
            Injector = injector;
        }

        #endregion

        #region Public Methods and Operators

        public abstract bool ConditionMet(Type type);

        public abstract void CreateBinding(Type type);

        public abstract Type GetTargetType(Type type);

        #endregion
    }
}