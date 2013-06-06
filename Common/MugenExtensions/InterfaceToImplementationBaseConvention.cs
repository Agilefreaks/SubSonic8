namespace Common.MugenExtensions
{
    using System;
    using System.Reflection;
    using MugenInjection.Interface;

    public abstract class InterfaceToImplementationBaseConvention : MugenConvetion
    {
        #region Constructors and Destructors

        protected InterfaceToImplementationBaseConvention(IInjector injector)
            : base(injector)
        {
        }

        #endregion

        #region Public Methods and Operators

        public override Type GetTargetType(Type type)
        {
            var typeInfo = type.GetTypeInfo();
            var implementationTypeName = typeInfo.Name.Remove(0, 1);
            var implementationTypeFullName = typeInfo.FullName.Replace(typeInfo.Name, implementationTypeName);

            return typeInfo.Assembly.GetType(implementationTypeFullName);
        }

        #endregion
    }
}