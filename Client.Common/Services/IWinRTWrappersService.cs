using System.Threading.Tasks;
using Windows.ApplicationModel.Search;
using Windows.Foundation;
using Windows.Storage;
using Windows.UI.ApplicationSettings;

namespace Client.Common.Services
{
    public interface IWinRTWrappersService
    {
        void RegisterSearchQueryHandler(TypedEventHandler<SearchPane, SearchPaneQuerySubmittedEventArgs> handler);

        void RegisterSettingsRequestedHandler(TypedEventHandler<SettingsPane, SettingsPaneCommandsRequestedEventArgs> handler);

        Task<IStorageFile> GetNewStorageFile();

        Task SaveToFile<T>(IStorageFile storageFile, T @object);
    }
}