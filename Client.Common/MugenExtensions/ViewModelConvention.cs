namespace Client.Common.MugenExtensions
{
    using System;
    using System.Reflection;
    using MugenInjection;
    using MugenInjection.Interface;

    public class ViewModelConvention : InterfaceToImplementationBaseConvention
    {
        #region Constructors and Destructors

        public ViewModelConvention(IInjector injector)
            : base(injector)
        {
        }

        #endregion

        #region Public Methods and Operators

        public override bool ConditionMet(Type type)
        {
            var typeInfo = type.GetTypeInfo();

            return typeInfo.IsInterface && typeInfo.Name.EndsWith("ViewModel") && GetTargetType(type) != null;
        }

        public override void CreateBinding(Type type)
        {
            var targetType = GetTargetType(type);

            Injector.Bind(type).To(targetType).InSingletonScope();
        }

        #endregion
    }
}