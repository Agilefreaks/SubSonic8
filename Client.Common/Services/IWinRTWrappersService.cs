using Windows.ApplicationModel.Search;
using Windows.Foundation;
using Windows.UI.ApplicationSettings;

namespace Client.Common.Services
{
    public interface IWinRTWrappersService
    {
        void RegisterSearchQueryHandler(TypedEventHandler<SearchPane, SearchPaneQuerySubmittedEventArgs> handler);

        void RegisterSettingsRequestedHandler(
            TypedEventHandler<SettingsPane, SettingsPaneCommandsRequestedEventArgs> handler);
    }
}