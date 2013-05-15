using System.Collections.Generic;
using System.Threading.Tasks;
using Client.Common.Models;
using Client.Common.Models.Subsonic;
using Client.Tests.Framework.ViewModel;
using Client.Tests.Mocks;
using FluentAssertions;
using Microsoft.VisualStudio.TestPlatform.UnitTestFramework;
using Subsonic8.MenuItem;
using Subsonic8.Playlists;

namespace Client.Tests.Playlists
{
    [TestClass]
    public class SavePlaylistViewModelTests : ViewModelBaseTests<SavePlaylistViewModel>
    {
        private MockDialogNotificationService _mockDialogNotificationService;
        private MockPlyalistManagementService _mockPlyalistManagementService;
        protected override SavePlaylistViewModel Subject { get; set; }

        protected override void TestInitializeExtensions()
        {
            base.TestInitializeExtensions();
            _mockDialogNotificationService = new MockDialogNotificationService();
            Subject.DialogNotificationService = _mockDialogNotificationService;
            _mockPlyalistManagementService = new MockPlyalistManagementService();
            Subject.PlaylistManagementService = _mockPlyalistManagementService;
        }

        [TestMethod]
        public void Cancel_Always_CallsNavigationServiceGoBack()
        {
            Subject.Cancel();

            MockNavigationService.GoBackCallCount.Should().Be(1);
        }

        [TestMethod]
        public void CanSave_PlaylistNameIsEmpty_ReturnsFalse()
        {
            Subject.PlaylistName = string.Empty;

            Subject.CanSave.Should().BeFalse();
        }

        [TestMethod]
        public void CanSave_PlaylistNameIsNotEmpty_ReturnsTrue()
        {
            Subject.PlaylistName = "asda";

            Subject.CanSave.Should().BeTrue();
        }

        [TestMethod]
        public async Task Save_MenuItemsContainsPlaylistWithSameNameAsPlaylistName_WillCallSubsonicServiceGetPlaylistWithThePlaylistId()
        {
            Subject.MenuItems.Add(new MenuItemViewModel { Item = new Playlist { Name = "test", Id = 2 } });
            Subject.PlaylistName = "test";

            var callCount = 0;
            MockSubsonicService.GetPlaylist = id =>
                {
                    callCount++;
                    id.Should().Be(2);
                    return new MockGetPlaylistResult();
                };

            await Task.Factory.StartNew(() => Subject.Save());

            callCount.Should().Be(1);
        }

        [TestMethod]
        public async Task Save_MenuItemsContainsPlaylistWithSameNameAsPlaylistName_CallsUpdatePlaylistWithTheExistingPlaylistIdAndSongDifferences()
        {
            _mockPlyalistManagementService.Items.Add(new PlaylistItem { UriAsString = "http://view.view?id=1" });
            _mockPlyalistManagementService.Items.Add(new PlaylistItem { UriAsString = "http://view.view?id=2" });
            Subject.MenuItems.Add(new MenuItemViewModel { Item = new Playlist { Name = "test", Id = 2 } });
            Subject.PlaylistName = "test";

            var existingPlaylist = new Playlist { Id = 3, Entries = new List<PlaylistEntry> { new PlaylistEntry { Id = 1 }, new PlaylistEntry { Id = 3 } } };

            var callCount = 0;
            MockSubsonicService.GetPlaylist = id => new MockGetPlaylistResult { GetResultFunc = () => existingPlaylist };
            MockSubsonicService.UpdatePlaylist = (id, songIdsToAdd, songIndexesToRemove) =>
                {
                    callCount++;
                    id.Should().Be(3);
                    songIdsToAdd.Should().BeEquivalentTo(new List<int> { 2 });
                    songIndexesToRemove.Should().BeEquivalentTo(new List<int> { 1 });
                    return new MockUpdatePlaylistResult();
                };

            await Task.Factory.StartNew(() => Subject.Save());

            callCount.Should().Be(1);
        }

        [TestMethod]
        public async Task Save_MenuItemsDoesNotContainPlaylistWithSameNameAsPlaylistNamse_CallsCreatePlaylistWithThePlaylistNameAndCurrentPlaylistItemIds()
        {
            _mockPlyalistManagementService.Items.Add(new PlaylistItem { UriAsString = "http://view.view?id=1" });
            _mockPlyalistManagementService.Items.Add(new PlaylistItem { UriAsString = "http://view.view?id=2" });
            Subject.MenuItems.Add(new MenuItemViewModel { Item = new Playlist { Name = "test", Id = 2 } });
            Subject.PlaylistName = "test2";

            var callCount = 0;
            MockSubsonicService.CreatePlaylist = (playlistName, songIdsToAdd) =>
            {
                callCount++;
                playlistName.Should().Be("test2");
                songIdsToAdd.Should().BeEquivalentTo(new List<int> { 2 });
                return new MockCreatePlaylistResult();
            };

            await Task.Factory.StartNew(() => Subject.Save());

            callCount.Should().Be(1);
        }
    }
}