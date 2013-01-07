using Windows.ApplicationModel.Search;
using Windows.Foundation;
using Windows.UI.ApplicationSettings;

namespace Client.Common.Services
{
    public class WinRTWrappersService : IWinRTWrappersService
    {
        public void RegisterSearchQueryHandler(TypedEventHandler<SearchPane, SearchPaneQuerySubmittedEventArgs> handler)
        {
            SearchPane.GetForCurrentView().QuerySubmitted += handler;
        }

        public void RegisterSettingsRequestedHandler(TypedEventHandler<SettingsPane, SettingsPaneCommandsRequestedEventArgs> handler)
        {
            SettingsPane.GetForCurrentView().CommandsRequested += handler;
        }
    }
}