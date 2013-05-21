namespace Client.Common.Services
{
    using System;
    using System.IO;
    using System.Threading.Tasks;
    using Windows.Security.Cryptography;
    using Windows.Storage;
    using Windows.Storage.Streams;
    using WinRtUtility;

    public class StorageService : IStorageService
    {
        #region Constants

        private const StorageType UsedStorageType = StorageType.Roaming;

        #endregion

        #region Public Methods and Operators

        public async Task Delete<T>()
        {
            var objectStorageHelper = new ObjectStorageHelper<T>(UsedStorageType);
            await objectStorageHelper.DeleteAsync();
        }

        public async Task<string> GetData<T>()
        {
            string data = null;
            var file = await GetStorageFile<T>();
            if (file != null)
            {
                using (var readStream = await file.OpenAsync(FileAccessMode.Read))
                {
                    var dataReader = new DataReader(readStream.GetInputStreamAt(0));
                    await dataReader.LoadAsync((uint)readStream.Size);
                    var buffer = dataReader.ReadBuffer((uint)readStream.Size);
                    data = CryptographicBuffer.ConvertBinaryToString(BinaryStringEncoding.Utf8, buffer);
                }
            }

            return data;
        }

        public async Task<T> Load<T>()
        {
            var objectStorageHelper = new ObjectStorageHelper<T>(UsedStorageType);
            return await objectStorageHelper.LoadAsync();
        }

        public async Task Save<T>(T data)
        {
            var objectStorageHelper = new ObjectStorageHelper<T>(UsedStorageType);
            await objectStorageHelper.SaveAsync(data);
        }

        #endregion

        #region Methods

        private static string GetFileName<T>(string handle = "")
        {
            var typeName = typeof(T).FullName;
            handle = string.IsNullOrWhiteSpace(handle) ? typeName : handle;
            return handle + typeName;
        }

        private async Task<StorageFile> GetStorageFile<T>()
        {
            var fileName = GetFileName<T>();
            var folder = ApplicationData.Current.RoamingFolder;
            StorageFile file = null;
            try
            {
                file = await folder.GetFileAsync(fileName);
            }
            catch (FileNotFoundException exception)
            {
                this.Log(exception);
            }

            return file;
        }

        #endregion
    }
}