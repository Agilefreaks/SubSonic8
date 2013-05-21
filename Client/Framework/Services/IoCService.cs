namespace Subsonic8.Framework.Services
{
    using Caliburn.Micro;

    public class IoCService : IIoCService
    {
        #region Public Methods and Operators

        public T Get<T>()
        {
            return IoC.Get<T>();
        }

        #endregion
    }
}