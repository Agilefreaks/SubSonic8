namespace Client.Tests.DefaultBottomBar
{
    using System;
    using System.Collections.Generic;
    using System.Collections.ObjectModel;
    using System.Linq;
    using System.Threading.Tasks;
    using Caliburn.Micro;
    using Client.Common.EventAggregatorMessages;
    using Client.Common.Models;
    using Client.Common.Models.Subsonic;
    using Client.Tests.Mocks;
    using FluentAssertions;
    using Microsoft.VisualStudio.TestPlatform.UnitTestFramework;
    using Subsonic8.BottomBar;
    using Subsonic8.MenuItem;

    [TestClass]
    public class DefaultBottomBarViewModelTests
    {
        #region Fields

        private MockEventAggregator _eventAggregator;

        private MockPlyalistManagementService _mockPlyalistManagementService;

        private MockSubsonicService _mockSubsonicService;

        private MockNavigationService _navigationService;

        private DefaultBottomBarViewModel _subject;

        #endregion

        #region Public Methods and Operators

        [TestInitialize]
        public void TestInitialize()
        {
            IoC.GetInstance = (type, s) => null;
            _eventAggregator = new MockEventAggregator();
            _navigationService = new MockNavigationService();
            _mockPlyalistManagementService = new MockPlyalistManagementService();
            _mockSubsonicService = new MockSubsonicService();
            _subject = new DefaultBottomBarViewModel(
                _navigationService, _eventAggregator, _mockPlyalistManagementService, new MockErrorDialogViewModel())
            {
                NavigateOnPlay = _navigationService.DoNavigate,
                SubsonicService = _mockSubsonicService
            };
        }

        [TestMethod]
        public void AddToPlaylistCallShouldClearSelectedItemsCollection()
        {
            _subject.SelectedItems.Add(new MenuItemViewModel());
            _subject.SelectedItems.Add(new MenuItemViewModel());

            _subject.AddToPlaylist();

            _subject.SelectedItems.Should().HaveCount(0);
        }

        [TestMethod]
        public void AddToPlaylistShouldFireAnAddItemsMessageForEachMediaItem()
        {
            _subject.SelectedItems.Add(new MenuItemViewModel { Item = new Song { IsVideo = true } });
            _subject.SelectedItems.Add(new MenuItemViewModel { Item = new Song { IsVideo = false } });

            _subject.AddToPlaylist();

            _eventAggregator.Messages.Count.Should().Be(2);
            _eventAggregator.Messages.All(m => m.GetType() == typeof(AddItemsMessage)).Should().BeTrue();
        }

        [TestMethod]
        public void
            AddToPlaylist_QueHasItemOfTypeArtist_CallsSubsonicServiceGetArtistAndAddsAllSongsFromAllAlbumsToThePlaylist()
        {
            MockLoadModel();
            _subject.SelectedItems.Add(new MenuItemViewModel { Item = new ExpandedArtist { Id = 5 } });
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

            _subject.AddToPlaylist();

            callCount.Should().Be(1);
            getAlbumCallCount.Should().Be(2);
            _eventAggregator.Messages.All(m => m.GetType() == typeof(AddItemsMessage)).Should().BeTrue();
            _eventAggregator.Messages.Count.Should().Be(2);
        }

        [TestMethod]
        public void AddToPlaylist_QueHasItemOfTypeIndexItem_CallsSubsonicServiceGetMusicDirectorysForEachChildArtist()
        {
            MockLoadModel();
            _subject.SelectedItems.Add(
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

            _subject.AddToPlaylist();

            callCount.Should().Be(1);
            _eventAggregator.Messages.All(m => m.GetType() == typeof(AddItemsMessage)).Should().BeTrue();
            _eventAggregator.Messages.Count.Should().Be(2);
        }

        [TestMethod]
        public void
            AddToPlaylist_QueHasItemOfTypeMusicDirectory_CallsSubsonicServiceGetMusicDirectorysAndAddsAllSongsToThePlaylist()
        {
            MockLoadModel();
            _subject.SelectedItems.Add(new MenuItemViewModel { Item = new MusicDirectory { Id = 5 } });
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

            _subject.AddToPlaylist();

            callCount.Should().Be(1);
            _eventAggregator.Messages.All(m => m.GetType() == typeof(AddItemsMessage)).Should().BeTrue();
            _eventAggregator.Messages.Count.Should().Be(2);
        }

        [TestMethod]
        public void
            AddToPlaylist_SelectedItemsHasItemOfTypeAlbum_CallsSubsonicServiceGetAlbumAndAdsAllItsSongsToThePlaylist()
        {
            MockLoadModel();
            _subject.SelectedItems.Add(new MenuItemViewModel { Item = new Album { Id = 5 } });
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

            _subject.AddToPlaylist();

            callCount.Should().Be(1);
            _eventAggregator.Messages.All(m => m.GetType() == typeof(AddItemsMessage)).Should().BeTrue();
            _eventAggregator.Messages.Count.Should().Be(2);
        }

        [TestMethod]
        public void CanAddToPlaylist_SelectedItemsAreNotAllOfTypeMenuItemViewModel_ReturnsFalse()
        {
            _subject.SelectedItems.Add(new MenuItemViewModel());
            _subject.SelectedItems.Add(42);
            _subject.SelectedItems.Add(new PlaylistItem());

            _subject.CanAddToPlaylist.Should().BeFalse();
        }

        [TestMethod]
        public void CanAddToPlaylist_SelectedItemsAreNotAllOfTypePlaylisttemViewModel_ReturnsFalse()
        {
            _subject.SelectedItems.Add(42);
            _subject.SelectedItems.Add(new PlaylistItem());
            _subject.SelectedItems.Add(new MenuItemViewModel());

            _subject.CanAddToPlaylist.Should().BeFalse();
        }

        [TestMethod]
        public void CanAddToPlaylist_SelectedItemsAreOfTypeMenuItemViewModel_ReturnsTrue()
        {
            _subject.SelectedItems.Add(new MenuItemViewModel());
            _subject.SelectedItems.Add(new MenuItemViewModel());
            _subject.SelectedItems.Add(new MenuItemViewModel());

            _subject.CanAddToPlaylist.Should().BeTrue();
        }

        [TestMethod]
        public void CtorShouldInitializeSelectedItemsCollection()
        {
            _subject.SelectedItems.Should().NotBeNull();
        }

        [TestMethod]
        public void HandleWithShowControlsMessage_WhenPlaylistDoesNotHaveElements_SetsDisplayPlayControlsToFalse()
        {
            var showControlsMessage = new PlaylistStateChangedMessage(false);
            _mockPlyalistManagementService.HasElements = false;

            _subject.Handle(showControlsMessage);

            _subject.DisplayPlayControls.Should().BeFalse();
        }

        [TestMethod]
        public void HandleWithShowControlsMessage_WhenPlaylistHasElements_SetsDisplayPlayControlsToTrue()
        {
            var showControlsMessage = new PlaylistStateChangedMessage(true);
            _mockPlyalistManagementService.HasElements = true;

            _subject.Handle(showControlsMessage);

            _subject.DisplayPlayControls.Should().BeTrue();
        }

        [TestMethod]
        public void IsOpened_SelectedItemsIsEmpty_ReturnsFalse()
        {
            _subject.SelectedItems = new ObservableCollection<object>();

            var isOpened = _subject.IsOpened;

            isOpened.Should().BeFalse();
        }

        [TestMethod]
        public void IsOpened_SelectedItemsIsNotEmpty_ReturnsTrue()
        {
            _subject.SelectedItems = new ObservableCollection<object> { new MenuItemViewModel() };

            var isOpened = _subject.IsOpened;

            isOpened.Should().BeTrue();
        }

        [TestMethod]
        public void PlayAllCallShouldClearSelectedItemsCollection()
        {
            _subject.SelectedItems.Add(new MenuItemViewModel());
            _subject.SelectedItems.Add(new MenuItemViewModel());

            _subject.PlayAll();

            _subject.SelectedItems.Should().HaveCount(0);
        }

        [TestMethod]
        public void PlayAllCallsEventAggregatorPublish()
        {
            _subject.PlayAll();

            _eventAggregator.PublishCallCount.Should().Be(1);
        }

        [TestMethod]
        public void PlayAllCallsNavigationServiceNavigateToViewModel()
        {
            _subject.PlayAll();

            _navigationService.NavigateToViewModelCalls.Count.Should().Be(1);
        }

        [TestMethod]
        public void PlayAll_ClearsTheCurrentPlaylist()
        {
            _subject.SelectedItems.Add(new MenuItemViewModel { Item = new Song() });

            _subject.PlayAll();

            _mockPlyalistManagementService.ClearCallCount.Should().Be(1);
        }

        [TestMethod]
        public void PlayAll_ShouldPublishAPlayNextMessage()
        {
            _subject.SelectedItems.Add(new MenuItemViewModel { Item = new Song() });
            _subject.SelectedItems.Add(new MenuItemViewModel { Item = new Song() });
            _subject.SelectedItems.Add(new MenuItemViewModel { Item = new Song() });

            _subject.PlayAll();

            _eventAggregator.Messages.Single(m => m.GetType() == typeof(PlayNextMessage)).Should().NotBeNull();
        }

        [TestMethod]
        public void Stop_Always_SendsAStopPlaybackMessage()
        {
            _subject.Stop();

            _eventAggregator.Messages.Count.Should().Be(1);
            _eventAggregator.Messages[0].Should().BeOfType<StopMessage>();
        }

        [TestMethod]
        public void ToggleShuffle_Always_PublishesANewToggleShuffleMessage()
        {
            _subject.ToggleShuffle();

            _eventAggregator.PublishCallCount.Should().Be(1);
            _eventAggregator.Messages[0].Should().BeOfType<ToggleShuffleMessage>();
        }

        #endregion

        #region Methods

        private void MockLoadModel()
        {
            _subject.LoadModel = model =>
                {
                    var tcr = new TaskCompletionSource<PlaylistItem>();
                    tcr.SetResult(
                        new PlaylistItem
                            {
                                PlayingState = PlaylistItemState.NotPlaying,
                                Uri = new Uri("http://test-uri"),
                                Artist = "test-artist"
                            });
                    return tcr.Task;
                };
        }

        #endregion
    }
}