using System;
using System.Reflection;
using MugenInjection.Interface;

namespace Client.Common.MugenExtensions
{
    public abstract class InterfaceToImplementationBaseConvention : MugenConvetion
    {
        protected InterfaceToImplementationBaseConvention(IInjector injector) 
            : base(injector)
        {
        }

        public override Type GetTargetType(Type type)
        {
            var typeInfo = type.GetTypeInfo();
            var implementationTypeName = typeInfo.Name.Remove(0, 1);
            var implementationTypeFullName = typeInfo.FullName.Replace(typeInfo.Name, implementationTypeName);

            return typeInfo.Assembly.GetType(implementationTypeFullName);
        }
    }
}