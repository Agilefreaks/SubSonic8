using System;
using System.Reflection;
using MugenInjection;
using MugenInjection.Interface;

namespace Client.Common.MugenExtensions
{
    public class ServiceConvention : InterfaceToImplementationBaseConvention
    {
        public ServiceConvention(IInjector injector)
            : base(injector)
        {
        }

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
    }
}