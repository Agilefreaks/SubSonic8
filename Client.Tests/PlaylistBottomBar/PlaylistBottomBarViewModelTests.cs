namespace Client.Tests.PlaylistBottomBar
{
    using Client.Common.Models.Subsonic;
    using Client.Common.Results;
    using Client.Common.Services.DataStructures.SubsonicService;
    using Client.Tests.Mocks;
    using global::Common.Mocks;
    using FluentAssertions;
    using Microsoft.VisualStudio.TestPlatform.UnitTestFramework;
    using Subsonic8.BottomBar;
    using Subsonic8.MenuItem;
    using MockSubsonicService = Client.Tests.Mocks.MockSubsonicService;

    [TestClass]
    public class PlaylistBottomBarViewModelTests
    {
        #region Fields

        private MockSubsonicService _mockSubsonicService;

        private PlaylistBottomBarViewModel _subject;

        #endregion

        #region Public Methods and Operators

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

        [TestMethod]
        public void RenamePlaylist_Always_CallRenamePlaylistWithTheFirstSelectedItemsId()
        {
            var callCount = 0;
            _subject.SelectedItems.Add(new MenuItemViewModel { Item = new Playlist { Id = 5 } });
            _mockSubsonicService.RenamePlaylist = (playlistId, playlistName) =>
                {
                    callCount++;
                    playlistId.Should().Be(5);
                    playlistName.Should().Be("test");

                    return new MockRenamePlaylistResult();
                };

            _subject.RenamePlaylist("test");

            callCount.Should().Be(1);
        }

        [TestInitialize]
        public void Setup()
        {
            _mockSubsonicService = new MockSubsonicService();
            _subject = new PlaylistBottomBarViewModel(
                new MockNavigationService(),
                new MockEventAggregator(),
                new MockPlyalistManagementService(),
                new MockErrorDialogViewModel())
                {
                    SubsonicService = _mockSubsonicService
                };
        }

        #endregion
    }
}