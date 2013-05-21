namespace Client.Common.Services
{
    using System.Threading.Tasks;

    public interface IStorageService
    {
        #region Public Methods and Operators

        Task Delete<T>();

        Task<string> GetData<T>();

        Task<T> Load<T>();

        Task Save<T>(T data);

        #endregion
    }
}