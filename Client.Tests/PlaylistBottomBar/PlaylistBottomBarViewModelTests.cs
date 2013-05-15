using Client.Common.Models.Subsonic;
using Client.Common.Results;
using Client.Common.Services.DataStructures.SubsonicService;
using Client.Tests.Mocks;
using FluentAssertions;
using Microsoft.VisualStudio.TestPlatform.UnitTestFramework;
using Subsonic8.BottomBar;
using Subsonic8.MenuItem;

namespace Client.Tests.PlaylistBottomBar
{
    [TestClass]
    public class PlaylistBottomBarViewModelTests
    {
        PlaylistBottomBarViewModel _subject;
        private MockSubsonicService _mockSubsonicService;

        [TestInitialize]
        public void Setup()
        {
            _mockSubsonicService = new MockSubsonicService();
            _subject = new PlaylistBottomBarViewModel(new MockNavigationService(), new MockEventAggregator(),
                                                      new MockPlyalistManagementService())
                {
                    SubsonicService = _mockSubsonicService
                };
        }

        [TestMethod]
        public void DeletePlaylist_Always_CallsDeletePlaylistWithTheFirstSelectedItemsId()
        {
            var callCount = 0;
            _subject.SelectedItems.Add(new MenuItemViewModel { Item = new Playlist { Id = 5 } });
            _mockSubsonicService.DeletePlaylist = playlistId =>
                {
                    callCount++;
                    playlistId.Should().Be(5);

                    return new DeletePlaylistResult(new SubsonicServiceConfiguration(), playlistId);
                };

            _subject.DeletePlaylist();

            callCount.Should().Be(1);
        }
    }
}