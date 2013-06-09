namespace Client.Tests.Playback
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Threading.Tasks;
    using Client.Common.EventAggregatorMessages;
    using Client.Common.Models;
    using Client.Common.Models.Subsonic;
    using Client.Tests.Framework.ViewModel;
    using Client.Tests.Mocks;
    using global::Common.ListCollectionView;
    using FluentAssertions;
    using Microsoft.VisualStudio.TestPlatform.UnitTestFramework;
    using Microsoft.VisualStudio.TestPlatform.UnitTestFramework.AppContainer;
    using Subsonic8.Playback;
    using Subsonic8.Playlists;

    [TestClass]
    public class PlaybackViewModelTests : ViewModelBaseTests<PlaybackViewModel>
    {
        #region Fields

        private MockEmbededVideoPlaybackViewModel _mockEmbededVideoPlaybackViewModel;

        private MockPlyalistManagementService _mockPlaylistManagementService;

        private MockToastNotificationService _mockToastNotificationService;

        private MockWinRTWrappersService _mockWinRTWrappersService;

        #endregion

        #region Properties

        protected override PlaybackViewModel Subject { get; set; }

        #endregion

        #region Public Methods and Operators

        [TestMethod]
        public void ClearPlaylist_Always_CallsPlaylistManagementServiceClearPlaylist()
        {
            Subject.ClearPlaylist();

            _mockPlaylistManagementService.ClearCallCount.Should().Be(1);
        }

        [TestMethod]
        public void ClearPlaylist_Always_CallsPlaylistServiceClearItems()
        {
            Subject.ClearPlaylist();

            _mockPlaylistManagementService.ClearCallCount.Should().Be(1);
        }

        [TestMethod]
        public void Ctor_Always_SetsCoverArtToCoverArtPlaceholderLarge()
        {
            var playbackViewModel = new PlaybackViewModel();

            playbackViewModel.CoverArt.Should().Be(PlaybackViewModel.CoverArtPlaceholderLarge);
        }

        [TestMethod]
        public void DoneFiltering_Should_ClearTheFilterText()
        {
            Subject.FilterText = "asdads";

            Subject.DoneFiltering();

            Subject.FilterText.Should().BeEmpty();
        }

        [TestMethod]
        public void DoneFiltering_Should_SetTheStateToThePreviousStateBeforeStartingFiltering()
        {
            Subject.State = PlaybackViewModelStateEnum.Audio;
            Subject.ShowFilter();

            Subject.DoneFiltering();

            Subject.State.Should().Be(PlaybackViewModelStateEnum.Audio);
        }

        [TestMethod]
        public void HandleStartAudioPlayback_Alawys_SetsCoverArtToItemConverArtUrl()
        {
            Subject.Handle(new StartPlaybackMessage(new PlaylistItem { CoverArtUrl = "test" }));

            Subject.CoverArt.Should().Be("test");
        }

        [TestMethod]
        public void HandleStartAudioPlayback_Alawys_SetsStateToAudio()
        {
            Subject.Handle(new StartPlaybackMessage(new PlaylistItem()));

            Subject.State.Should().Be(PlaybackViewModelStateEnum.Audio);
        }

        [TestMethod]
        public async Task HandleWithPlayFileOfTypeSongShouldAddAnItemToThePlaylist()
        {
            MockLoadModel();

            await Task.Run(() => Subject.AddToPlaylistAndPlay(new Song { IsVideo = false }));

            _mockPlaylistManagementService.Items.Count.Should().Be(1);
        }

        [TestMethod]
        public async Task HandleWithPlayFileOfTypeSongShouldPublishAMessageToStartPlayingTheLastAddedItem()
        {
            MockLoadModel();
            _mockPlaylistManagementService.Items = new PlaylistItemCollection { new PlaylistItem() };

            await Task.Run(() => Subject.AddToPlaylistAndPlay(new Song { IsVideo = false }));

            MockEventAggregator.Messages.Any(
                m =>
                m.GetType() == typeof(PlayItemAtIndexMessage)
                && ((PlayItemAtIndexMessage)m).Index == _mockPlaylistManagementService.Items.Count - 1)
                               .Should()
                               .BeTrue();
        }

        [TestMethod]
        public void Handle_PlayFailedMessage_CallsNotificationServiceShowWithANiceMessage()
        {
            Subject.Handle(new PlayFailedMessage("test m", null));

            MockErrorDialogViewModel.HandleErrorCallCount.Should().Be(1);
            MockErrorDialogViewModel.HandledErrors.First().Should().Be("Could not play item:\r\ntest m");
        }

        [TestMethod]
        public void Handle_PlayFailedMessage_ShouldPublishAStopMessage()
        {
            Subject.Handle(new PlayFailedMessage("test m", null));

            MockEventAggregator.PublishCallCount.Should().Be(1);
            MockEventAggregator.Messages[0].Should().BeOfType<StopMessage>();
        }

        [TestMethod]
        public void Handle_PlaylistStateChangedMessage_SetsPlaybackControlsVisibleToMessageHasElements()
        {
            Subject.Handle(new PlaylistStateChangedMessage(true));

            Subject.PlaybackControlsVisible.Should().Be(true);
        }

        [TestMethod]
        public void IsPlayingReturnsPlaylistManagementServiceIsPlaying()
        {
            _mockPlaylistManagementService.IsPlaying = true;

            Subject.IsPlaying.Should().BeTrue();
        }

        [TestMethod]
        public void LoadRemotePlaylist_Always_ShouldNavigateToManagePlaylistsViewModel()
        {
            Subject.LoadRemotePlaylist();

            MockNavigationService.NavigateToViewModelCalls.Count.Should().Be(1);
            MockNavigationService.NavigateToViewModelCalls.First().Key.Should().Be(typeof(ManagePlaylistsViewModel));
            MockNavigationService.NavigateToViewModelCalls.First().Value.Should().BeNull();
        }

        [TestMethod]
        public void LoadState_StateContainsPlaylistKey_IsAbleToRestorePlaylistFromPreviouslySavePlaylist()
        {
            var knownTypes = new List<Type>();
            var statePageState = new Dictionary<string, object>();
            _mockPlaylistManagementService.Items.Add(
                new PlaylistItem { Artist = "test_a", UriAsString = "http://google.com/" });
            Subject.SaveState(statePageState, knownTypes);
            _mockPlaylistManagementService.Items.Clear();
            _mockPlaylistManagementService.Items.Count.Should().Be(0);

            Subject.LoadState(null, statePageState);

            _mockPlaylistManagementService.Items.Count.Should().Be(1);
            _mockPlaylistManagementService.Items[0].Artist.Should().Be("test_a");
            _mockPlaylistManagementService.Items[0].UriAsString.Should().Be("http://google.com/");
        }

        [TestMethod]
        public async Task ParameterWhenSetShouldAddAnItemToThePlaylist()
        {
            MockLoadModel();
            var itemsCount = _mockPlaylistManagementService.Items.Count;
            await Task.Run(() => { Subject.Parameter = 1; });

            _mockPlaylistManagementService.Items.Count.Should().Be(itemsCount + 1);
        }

        [TestMethod]
        public async Task ParameterWhenSetShouldStartPlayingTheLastAddedItem()
        {
            MockLoadModel();
            _mockPlaylistManagementService.Items = new PlaylistItemCollection { new PlaylistItem() };
            await Task.Run(() => { Subject.Parameter = 2; });

            MockEventAggregator.Messages.Any(
                m =>
                m.GetType() == typeof(PlayItemAtIndexMessage)
                && ((PlayItemAtIndexMessage)m).Index == _mockPlaylistManagementService.Items.Count - 1)
                               .Should()
                               .BeTrue();
        }

        [TestMethod]
        public void PlaylistItems_Should_BeAListCollectionView()
        {
            Subject.PlaylistItems.Should().BeOfType<ListCollectionView>();
        }

        [TestMethod]
        public void SavePlaylist_Always_CallsWinRTWrapperServiceGetNewStorageFile()
        {
            Subject.SavePlaylist();

            _mockWinRTWrappersService.GetNewStorageFileCallCount.Should().Be(1);
        }

        [UITestMethod]
        public void SettingAEmptyFilter_Should_ClearTheFilter()
        {
            var playlistItem1 = new PlaylistItem { Title = "test song" };
            var playlistItem2 = new PlaylistItem { Title = "song1 super" };
            _mockPlaylistManagementService.Items.Add(playlistItem1);
            _mockPlaylistManagementService.Items.Add(playlistItem2);

            Subject.FilterText = string.Empty;

            Subject.PlaylistItems.Count.Should().Be(2);
            Subject.PlaylistItems[0].Should().Be(playlistItem1);
            Subject.PlaylistItems[1].Should().Be(playlistItem2);
        }

        [UITestMethod]
        public void SettingAFilter_Should_FilterThePlaylistItemsByArtist()
        {
            var playlistItem1 = new PlaylistItem { Artist = "test artist" };
            var playlistItem2 = new PlaylistItem { Artist = "artist super" };
            _mockPlaylistManagementService.Items.Add(playlistItem1);
            _mockPlaylistManagementService.Items.Add(playlistItem2);

            Subject.FilterText = "SuP";

            Subject.PlaylistItems.Count.Should().Be(1);
            Subject.PlaylistItems[0].Should().Be(playlistItem2);
        }

        [UITestMethod]
        public void SettingAFilter_Should_FilterThePlaylistItemsByTitle()
        {
            var playlistItem1 = new PlaylistItem { Title = "test song" };
            var playlistItem2 = new PlaylistItem { Title = "song1 super" };
            _mockPlaylistManagementService.Items.Add(playlistItem1);
            _mockPlaylistManagementService.Items.Add(playlistItem2);

            Subject.FilterText = "SuP";

            Subject.PlaylistItems.Count.Should().Be(1);
            Subject.PlaylistItems[0].Should().Be(playlistItem2);
        }

        [UITestMethod]
        public void SettingANullFilter_Should_ClearTheFilter()
        {
            var playlistItem1 = new PlaylistItem { Title = "test song" };
            var playlistItem2 = new PlaylistItem { Title = "song1 super" };
            _mockPlaylistManagementService.Items.Add(playlistItem1);
            _mockPlaylistManagementService.Items.Add(playlistItem2);

            Subject.FilterText = null;

            Subject.PlaylistItems.Count.Should().Be(2);
            Subject.PlaylistItems[0].Should().Be(playlistItem1);
            Subject.PlaylistItems[1].Should().Be(playlistItem2);
        }

        [TestMethod]
        public void ShowFilter_Should_SetTheStateToFilter()
        {
            Subject.ShowFilter();

            Subject.State.Should().Be(PlaybackViewModelStateEnum.Filter);
        }

        #endregion

        #region Methods

        protected override void TestInitializeExtensions()
        {
            _mockToastNotificationService = new MockToastNotificationService();
            _mockWinRTWrappersService = new MockWinRTWrappersService();
            _mockPlaylistManagementService = new MockPlyalistManagementService();
            _mockEmbededVideoPlaybackViewModel = new MockEmbededVideoPlaybackViewModel();
            Subject.WinRTWrappersService = _mockWinRTWrappersService;
            Subject.PlaylistManagementService = _mockPlaylistManagementService;
            Subject.EmbededVideoPlaybackViewModel = _mockEmbededVideoPlaybackViewModel;
            Subject.ToastNotificationService = _mockToastNotificationService;
            Subject.LoadModel = model =>
                {
                    var tcr = new TaskCompletionSource<PlaylistItem>();
                    tcr.SetResult(new PlaylistItem());
                    return tcr.Task;
                };
        }

        private void MockLoadModel()
        {
            Subject.LoadModel = model =>
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