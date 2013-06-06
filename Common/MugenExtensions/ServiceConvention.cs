namespace Common.MugenExtensions
{
    using System;
    using System.Reflection;
    using MugenInjection;
    using MugenInjection.Interface;

    public class ServiceConvention : InterfaceToImplementationBaseConvention
    {
        #region Constructors and Destructors

        public ServiceConvention(IInjector injector)
            : base(injector)
        {
        }

        #endregion

        #region Public Methods and Operators

        public override bool ConditionMet(Type type)
        {
            var typeInfo = type.GetTypeInfo();

            return typeInfo.IsInterface && typeInfo.Name.EndsWith("Service") && GetTargetType(type) != null;
        }

        public override void CreateBinding(Type type)
        {
            var targetType = GetTargetType(type);

            Injector.Bind(type).To(targetType).InSingletonScope();
        }

        #endregion
    }
}