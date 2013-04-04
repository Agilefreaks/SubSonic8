using System.Threading.Tasks;

namespace Client.Common.Services
{
    public interface IStorageService
    {
        Task Save<T>(T data);

        Task<T> Load<T>();

        Task Delete<T>();

        Task Save<T>(T data, string handle);

        Task<T> Load<T>(string handle);
    }
}