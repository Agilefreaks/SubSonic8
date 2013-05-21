namespace Client.Common.Services
{
    using System.Threading.Tasks;
    using Client.Common.Helpers;
    using Windows.ApplicationModel.Search;
    using Windows.Foundation;
    using Windows.Storage;
    using Windows.UI.ApplicationSettings;

    public interface IWinRTWrappersService
    {
        #region Public Methods and Operators

        Task<IStorageFile> GetNewStorageFile();

        Task<T> LoadFromFile<T>(IStorageFile storageFile) where T : new();

        Task<IStorageFile> OpenStorageFile();

        void RegisterMediaControlHandler(IMediaControlHandler mediaControlHandler);

        void RegisterSearchQueryHandler(TypedEventHandler<SearchPane, SearchPaneQuerySubmittedEventArgs> handler);

        void RegisterSettingsRequestedHandler(
            TypedEventHandler<SettingsPane, SettingsPaneCommandsRequestedEventArgs> handler);

        Task SaveToFile<T>(IStorageFile storageFile, T @object);

        #endregion
    }
}