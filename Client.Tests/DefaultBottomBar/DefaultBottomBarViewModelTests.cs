namespace Client.Tests.DefaultBottomBar
{
    using System;
    using System.Collections.Generic;
    using System.Diagnostics;
    using System.Linq;
    using System.Threading.Tasks;
    using Caliburn.Micro;
    using Client.Common.EventAggregatorMessages;
    using Client.Common.Models;
    using Client.Common.Models.Subsonic;
    using Client.Tests.BottomBar;
    using Client.Tests.Mocks;
    using FluentAssertions;
    using global::Common.Mocks;
    using Microsoft.VisualStudio.TestPlatform.UnitTestFramework;
    using Subsonic8.BottomBar;
    using Subsonic8.MenuItem;
    using Subsonic8.Playback;

    [TestClass]
    public class DefaultBottomBarViewModelTests : BottomBarViewModelTests<DefaultBottomBarViewModel>
    {
        #region Fields

        private MockSubsonicService _mockSubsonicService;

        #endregion

        #region Public Methods and Operators

        [TestMethod]
        public async Task AddToPlaylistCallShouldClearSelectedItemsCollection()
        {
            Subject.SelectedItems.Add(new MenuItemViewModel { Item = new MockSubsonicModel() });
            Subject.SelectedItems.Add(new MenuItemViewModel { Item = new MockSubsonicModel() });

            await Subject.AddToPlaylist();

            Subject.SelectedItems.Should().HaveCount(0);
        }

        [TestMethod]
        public async Task AddToPlaylistShouldFireAnAddItemsMessageForEachMediaItem()
        {
            Subject.SelectedItems.Add(new MenuItemViewModel { Item = new Song { IsVideo = true } });
            Subject.SelectedItems.Add(new MenuItemViewModel { Item = new Song { IsVideo = false } });

            await Subject.AddToPlaylist();

            MockEventAggregator.Messages.Count.Should().Be(2);
            MockEventAggregator.Messages.All(m => m.GetType() == typeof(AddItemsMessage)).Should().BeTrue();
        }

        [TestMethod]
        public async Task
            AddToPlaylist_QueHasItemOfTypeArtist_CallsSubsonicServiceGetArtistAndAddsAllSongsFromAllAlbumsToThePlaylist()
        {
            MockLoadModel();
            Subject.SelectedItems.Add(new MenuItemViewModel { Item = new ExpandedArtist { Id = 5 } });
            var albums = new List<Album> { new Album(), new Album() };
            var artist = new ExpandedArtist { Albums = albums };
            var mockGetAlbumResult = new MockGetArtistResult { GetResultFunc = () => artist };
            var callCount = 0;
            _mockSubsonicService.GetArtist = albumId =>
                {
                    callCount++;
                    albumId.Should().Be(5);
                    return mockGetAlbumResult;
                };
            var getAlbumCallCount = 0;
            _mockSubsonicService.GetAlbum = artistId =>
                {
                    getAlbumCallCount++;
                    return new MockGetAlbumResult
                               {
                                   GetResultFunc =
                                       () => new Album { Songs = new List<Song> { new Song() } }
                               };
                };

            await Subject.AddToPlaylist();

            callCount.Should().Be(1);
            getAlbumCallCount.Should().Be(2);
            MockEventAggregator.Messages.All(m => m.GetType() == typeof(AddItemsMessage)).Should().BeTrue();
            MockEventAggregator.Messages.Count.Should().Be(2);
        }

        [TestMethod]
        public async Task AddToPlaylist_QueHasItemOfTypeIndexItem_CallsSubsonicServiceGetMusicDirectoriesForEachChildArtist()
        {
            MockLoadModel();
            Subject.SelectedItems.Add(
                new MenuItemViewModel
                    {
                        Item =
                            new IndexItem
                                {
                                    Id = 5,
                                    Artists = new List<Artist> { new Artist { Id = 3 } }
                                }
                    });
            var children = new List<MusicDirectoryChild> { new MusicDirectoryChild(), new MusicDirectoryChild() };
            var musicDirectory = new MusicDirectory { Children = children };
            var mockGetMusicDirectoryResult = new MockGetMusicDirectoryResult { GetResultFunc = () => musicDirectory };
            var callCount = 0;
            _mockSubsonicService.GetMusicDirectory = directoryId =>
                {
                    callCount++;
                    directoryId.Should().Be(3);
                    return mockGetMusicDirectoryResult;
                };

            await Subject.AddToPlaylist();

            callCount.Should().Be(1);
            MockEventAggregator.Messages.All(m => m.GetType() == typeof(AddItemsMessage)).Should().BeTrue();
            MockEventAggregator.Messages.Count.Should().Be(2);
        }

        [TestMethod]
        public async Task
            AddToPlaylist_QueHasItemOfTypeMusicDirectory_CallsSubsonicServiceGetMusicDirectoriesAndAddsAllSongsToThePlaylist()
        {
            MockLoadModel();
            Subject.SelectedItems.Add(new MenuItemViewModel { Item = new MusicDirectory { Id = 5 } });
            var children = new List<MusicDirectoryChild> { new MusicDirectoryChild(), new MusicDirectoryChild() };
            var musicDirectory = new MusicDirectory { Children = children };
            var mockGetAlbumResult = new MockGetMusicDirectoryResult { GetResultFunc = () => musicDirectory };
            var callCount = 0;
            _mockSubsonicService.GetMusicDirectory = directoryId =>
                {
                    callCount++;
                    directoryId.Should().Be(5);
                    return mockGetAlbumResult;
                };

            await Subject.AddToPlaylist();

            callCount.Should().Be(1);
            MockEventAggregator.Messages.All(m => m.GetType() == typeof(AddItemsMessage)).Should().BeTrue();
            MockEventAggregator.Messages.Count.Should().Be(2);
        }

        [TestMethod]
        public async Task
            AddToPlaylist_SelectedItemsHasItemOfTypeAlbum_CallsSubsonicServiceGetAlbumAndAdsAllItsSongsToThePlaylist()
        {
            MockLoadModel();
            Subject.SelectedItems.Add(new MenuItemViewModel { Item = new Album { Id = 5 } });
            var songs = new List<Song> { new Song(), new Song() };
            var album = new Album { Songs = songs };
            var mockGetAlbumResult = new MockGetAlbumResult { GetResultFunc = () => album };
            var callCount = 0;
            _mockSubsonicService.GetAlbum = albumId =>
                {
                    callCount++;
                    albumId.Should().Be(5);
                    return mockGetAlbumResult;
                };

            await Subject.AddToPlaylist();

            callCount.Should().Be(1);
            MockEventAggregator.Messages.All(m => m.GetType() == typeof(AddItemsMessage)).Should().BeTrue();
            MockEventAggregator.Messages.Count.Should().Be(2);
        }

        [TestMethod]
        public void CanAddToPlaylist_SelectedItemsAreNotAllOfTypeMenuItemViewModel_ReturnsFalse()
        {
            Subject.SelectedItems.Add(new MenuItemViewModel());
            Subject.SelectedItems.Add(42);
            Subject.SelectedItems.Add(new PlaylistItem());

            Subject.CanAddToPlaylist.Should().BeFalse();
        }

        [TestMethod]
        public void CanAddToPlaylist_SelectedItemsAreNotAllOfTypePlaylistItemViewModel_ReturnsFalse()
        {
            Subject.SelectedItems.Add(42);
            Subject.SelectedItems.Add(new PlaylistItem());
            Subject.SelectedItems.Add(new MenuItemViewModel());

            Subject.CanAddToPlaylist.Should().BeFalse();
        }

        [TestMethod]
        public void CanAddToPlaylist_SelectedItemsAreOfTypeMenuItemViewModel_ReturnsTrue()
        {
            Subject.SelectedItems.Add(new MenuItemViewModel());
            Subject.SelectedItems.Add(new MenuItemViewModel());
            Subject.SelectedItems.Add(new MenuItemViewModel());

            Subject.CanAddToPlaylist.Should().BeTrue();
        }

        [TestMethod]
        public void ConstructorShouldInitializeSelectedItemsCollection()
        {
            Subject.SelectedItems.Should().NotBeNull();
        }

        [TestMethod]
        public void HandleWithShowControlsMessage_WhenPlaylistDoesNotHaveElements_SetsDisplayPlayControlsToFalse()
        {
            var showControlsMessage = new PlaylistStateChangedMessage(false);
            MockPlyalistManagementService.HasElements = false;

            Subject.Handle(showControlsMessage);

            Subject.DisplayPlayControls.Should().BeFalse();
        }

        [TestMethod]
        public void HandleWithShowControlsMessage_WhenPlaylistHasElements_SetsDisplayPlayControlsToTrue()
        {
            var showControlsMessage = new PlaylistStateChangedMessage(true);
            MockPlyalistManagementService.HasElements = true;

            Subject.Handle(showControlsMessage);

            Subject.DisplayPlayControls.Should().BeTrue();
        }

        [TestMethod]
        public async Task PlayAllCallShouldClearSelectedItemsCollection()
        {
            Subject.SelectedItems.Add(new MenuItemViewModel { Item = new MockSubsonicModel() });
            Subject.SelectedItems.Add(new MenuItemViewModel { Item = new MockSubsonicModel() });

            await Subject.PlayAll();

            Subject.SelectedItems.Should().HaveCount(0);
        }

        [TestMethod]
        public async Task PlayAllCallsEventAggregatorPublish()
        {
            await Subject.PlayAll();

            MockEventAggregator.PublishCallCount.Should().Be(1);
        }

        [TestMethod]
        public async Task PlayAllCallsNavigationServiceNavigateToViewModel()
        {
            await Subject.PlayAll();

            MockNavigationService.NavigateToViewModelCalls.Count.Should().Be(1);
        }

        [TestMethod]
        public async Task PlayAll_ClearsTheCurrentPlaylist()
        {
            Subject.SelectedItems.Add(new MenuItemViewModel { Item = new Song() });

            await Subject.PlayAll();

            MockPlyalistManagementService.ClearCallCount.Should().Be(1);
        }

        [TestMethod]
        public async Task PlayAll_ShouldPublishAnAddItemToPlaylistMessageWithStartPlayingTrue()
        {
            Subject.SelectedItems.Add(new MenuItemViewModel { Item = new Song() });
            Subject.SelectedItems.Add(new MenuItemViewModel { Item = new Song() });
            Subject.SelectedItems.Add(new MenuItemViewModel { Item = new Song() });

            await Subject.PlayAll();

            var message = (AddItemsMessage)MockEventAggregator.Messages.First(m => m.GetType() == typeof(AddItemsMessage));
            message.StartPlaying.Should().BeTrue();
        }

        [TestMethod]
        public void NavigateToPlaylist_Always_SetsCanDismissTrue()
        {
            Subject.NavigateToPlaylist();

            Subject.CanDismiss.Should().BeTrue();
        }

        [TestMethod]
        public void NavigateToPlaylist_Always_SetsIsOpenedFalse()
        {
            Subject.NavigateToPlaylist();

            Subject.IsOpened.Should().BeFalse();
        }

        [TestMethod]
        public void NavigateToPlaylist_Always_NavigatesToPlaybackViewModel()
        {
            Subject.NavigateToPlaylist();

            MockNavigationService.NavigateToViewModelCalls.Count.Should().Be(1);
            MockNavigationService.NavigateToViewModelCalls[0].Key.Should().Be<PlaybackViewModel>();
        }

        #endregion

        #region Methods

        protected override void TestInitializeExtensions()
        {
            base.TestInitializeExtensions();
            IoC.GetInstance = (type, s) => null;
            _mockSubsonicService = new MockSubsonicService();
            Subject.NavigateOnPlay = MockNavigationService.DoNavigate;
            Subject.SubsonicService = _mockSubsonicService;
        }

        private void MockLoadModel()
        {
            Subject.LoadPlaylistItem = model =>
                {
                    var taskCompletionSource = new TaskCompletionSource<PlaylistItem>();
                    taskCompletionSource.SetResult(
                        new PlaylistItem
                            {
                                PlayingState = PlaylistItemState.NotPlaying,
                                Uri = new Uri("http://test-uri"),
                                Artist = "test-artist"
                            });
                    return taskCompletionSource.Task;
                };
        }

        #endregion
    }
}