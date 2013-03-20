using System;
using System.Reflection;
using MugenInjection;
using MugenInjection.Interface;
using MugenInjection.Interface.Behaviors;

namespace Subsonic8.Framework
{
    public class MugenUnregesiteredTypeResolver : IResolveUnregisteredTypeBehavior
    {
        public bool Resolve(IBindingContext bindingContext, out object result)
        {
            var success = false;
            result = null;
            var typeInfo = bindingContext.Service.GetTypeInfo();
            if (!typeInfo.IsInterface)
            {
                result = Activator.CreateInstance(bindingContext.Service);
                success = true;
            }
            else
            {
                if (typeInfo.Name.EndsWith("ViewModel"))
                {
                    var viewModelTypeName = typeInfo.Name.Remove(0, 1);
                    var viewModelTypeFullName = typeInfo.FullName.Replace(typeInfo.Name, viewModelTypeName);
                    var type = typeInfo.Assembly.GetType(viewModelTypeFullName);
                    result = bindingContext.Injector.Get(type);
                    success = true;
                }
            }

            return success;
        }
    }
}
