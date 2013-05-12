namespace Subsonic8.Framework.Services
{
    public interface IIoCService
    {
        T Get<T>();
    }
}