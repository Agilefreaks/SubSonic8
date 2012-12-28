using System.Threading.Tasks;
using WinRtUtility;

namespace Client.Common.Services
{
    public class StorageService : IStorageService
    {
        public async Task Save<T>(T data)
            where T : class
        {
            var objectStorageHelper = new ObjectStorageHelper<T>(StorageType.Roaming);
            await objectStorageHelper.SaveAsync(data);
        }

        public async Task<T> Load<T>()
            where T : class
        {
            var objectStorageHelper = new ObjectStorageHelper<T>(StorageType.Roaming);
            return await objectStorageHelper.LoadAsync();
        }

        public async Task Delete<T>()
            where T : class
        {
            var objectStorageHelper = new ObjectStorageHelper<T>(StorageType.Roaming);
            await objectStorageHelper.DeleteAsync();
        }
    }
}