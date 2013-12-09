namespace Client.Tests.PlaylistBottomBar
{
    using System.Threading.Tasks;
    using Client.Common.Models.Subsonic;
    using Client.Common.Results;
    using Client.Common.Services.DataStructures.SubsonicService;
    using Client.Tests.BottomBar;
    using Client.Tests.Mocks;
    using FluentAssertions;
    using global::Common.Mocks;
    using Microsoft.VisualStudio.TestPlatform.UnitTestFramework;
    using Subsonic8.BottomBar;
    using Subsonic8.MenuItem;

    [TestClass]
    public class PlaylistBottomBarViewModelTests : BottomBarViewModelTests<PlaylistBottomBarViewModel>
    {
        #region Fields

        private MockSubsonicService _mockSubsonicService;

        #endregion

        #region Public Methods and Operators

        [TestMethod]
        public async Task DeletePlaylist_Always_CallsDeletePlaylistWithTheFirstSelectedItemsId()
        {
            var callCount = 0;
            Subject.SelectedItems.Add(new MenuItemViewModel { Item = new Playlist { Id = 5 } });
            _mockSubsonicService.DeletePlaylist = playlistId =>
                {
                    callCount++;
                    playlistId.Should().Be(5);

                    return new DeletePlaylistResult(new SubsonicServiceConfiguration(), playlistId);
                };

            await Subject.DeletePlaylist();

            callCount.Should().Be(1);
        }

        [TestMethod]
        public async Task RenamePlaylist_Always_CallRenamePlaylistWithTheFirstSelectedItemsId()
        {
            var callCount = 0;
            Subject.SelectedItems.Add(new MenuItemViewModel { Item = new Playlist { Id = 5 } });
            _mockSubsonicService.RenamePlaylist = (playlistId, playlistName) =>
                {
                    callCount++;
                    playlistId.Should().Be(5);
                    playlistName.Should().Be("test");

                    return new MockRenamePlaylistResult();
                };

            await Subject.RenamePlaylist("test");

            callCount.Should().Be(1);
        }

        protected override void TestInitializeExtensions()
        {
            base.TestInitializeExtensions();
            _mockSubsonicService = new MockSubsonicService();
            Subject.SubsonicService = _mockSubsonicService;
        }

        #endregion
    }
}