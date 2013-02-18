using System.Threading.Tasks;
using Client.Common.Models;
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

        Task<IStorageFile> OpenStorageFile();

        Task<T> LoadFromFile<T>(IStorageFile storageFile, PlaylistItemCollection playlistItems)
            where T : new();
    }
}