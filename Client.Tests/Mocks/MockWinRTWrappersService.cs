using System;
using System.Threading.Tasks;
using Client.Common.Helpers;
using Client.Common.Services;
using Windows.ApplicationModel.Search;
using Windows.Foundation;
using Windows.Storage;
using Windows.UI.ApplicationSettings;

namespace Client.Tests.Mocks
{
    public class MockWinRTWrappersService : IWinRTWrappersService
    {
        public int RegisterSearchQueryHandlerCallCount { get; set; }

        public int RegisterSettingsRequestedHandlerCallCount { get; set; }

        public int GetNewStorageFileCallCount { get; set; }

        protected int SaveToFileCount { get; set; }

        public Action<IStorageFile, object> SaveToFileAction { get; set; }

        public Func<IStorageFile> GetNewStorageFileFunc { get; set; }

        public MockWinRTWrappersService()
        {
            SaveToFileAction = (file, o) => { };
            GetNewStorageFileFunc = () => null;
        }

        public void RegisterSearchQueryHandler(TypedEventHandler<SearchPane, SearchPaneQuerySubmittedEventArgs> handler)
        {
            RegisterSearchQueryHandlerCallCount++;
        }

        public void RegisterSettingsRequestedHandler(TypedEventHandler<SettingsPane, SettingsPaneCommandsRequestedEventArgs> handler)
        {
            RegisterSettingsRequestedHandlerCallCount++;
        }

        public Task<IStorageFile> GetNewStorageFile()
        {
            GetNewStorageFileCallCount++;
            var taskCompletionSource = new TaskCompletionSource<IStorageFile>();
            taskCompletionSource.SetResult(GetNewStorageFileFunc());

            return taskCompletionSource.Task;
        }

        public Task SaveToFile<T>(IStorageFile storageFile, T @object)
        {
            SaveToFileCount++;
            SaveToFileAction(storageFile, @object);
            var taskCompletionSource = new TaskCompletionSource<int>();
            taskCompletionSource.SetResult(0);
            return taskCompletionSource.Task;
        }

        public Task<IStorageFile> OpenStorageFile()
        {
            throw new NotImplementedException();
        }

        public Task<T> LoadFromFile<T>(IStorageFile storageFile) where T : new()
        {
            throw new NotImplementedException();
        }

        public void RegisterMediaControlHandler(IMediaControlHandler mediaControlHandler)
        {
        }
    }
}