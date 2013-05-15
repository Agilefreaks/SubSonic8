using Caliburn.Micro;

namespace Subsonic8.Framework.Services
{
    public class IoCService : IIoCService
    {
        public T Get<T>()
        {
            return IoC.Get<T>();
        }
    }
}