using Client.Common.Services;
using Windows.ApplicationModel.Search;
using Windows.Foundation;
using Windows.UI.ApplicationSettings;

namespace Client.Tests.Mocks
{
    public class MockWinRTWrappersService : IWinRTWrappersService
    {
        public int RegisterSearchQueryHandlerCallCount { get; set; }

        public int RegisterSettingsRequestedHandlerCallCount { get; set; }

        public void RegisterSearchQueryHandler(TypedEventHandler<SearchPane, SearchPaneQuerySubmittedEventArgs> handler)
        {
            RegisterSearchQueryHandlerCallCount++;
        }

        public void RegisterSettingsRequestedHandler(TypedEventHandler<SettingsPane, SettingsPaneCommandsRequestedEventArgs> handler)
        {
            RegisterSettingsRequestedHandlerCallCount++;
        }
    }
}