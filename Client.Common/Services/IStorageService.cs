using System.Threading.Tasks;

namespace Client.Common.Services
{
    public interface IStorageService
    {
        Task Save<T>(T data)
            where T : class;

        Task<T> Load<T>()
            where T : class;
    }
}