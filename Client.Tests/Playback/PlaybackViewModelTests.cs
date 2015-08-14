namespace Client.Tests.Playback
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Threading.Tasks;

    using Client.Common.EventAggregatorMessages;
    using Client.Common.Models;
    using Client.Common.Services.DataStructures.PlayerManagementService;
    using Client.Tests.Framework.ViewModel;
    using Client.Tests.Mocks;
    using global::Common.ListCollectionView;
    using global::Common.Mocks;
    using FluentAssertions;
    using Microsoft.VisualStudio.TestPlatform.UnitTestFramework;
    using Microsoft.VisualStudio.TestPlatform.UnitTestFramework.AppContainer;
    using Subsonic8.Playback;
    using Subsonic8.Playlists;

    [TestClass]
    public class PlaybackViewModelTests : ViewModelBaseTests<PlaybackViewModel>
    {
        #region Fields

        private MockEmbeddedVideoPlaybackViewModel _mockEmbeddedVideoPlaybackViewModel;

        private MockPlyalistManagementService _mockPlaylistManagementService;

        private MockToastNotificationService _mockToastNotificationService;

        private MockWinRTWrappersService _mockWinRTWrappersService;

        private MockPlayerManagementService _mockPlayerManagementService;

        private MockSnappedVideoPlaybackViewModel _mockSnappedVideoPlaybackViewModel;

        private MockFullScreenVideoPlaybackViewModel _mockFullScreenVideoPlaybackViewModel;
        private MockArtistInfoViewModel _mockArtistInfoViewModel;

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
        public void Constructor_Always_SetsCoverArtToCoverArtPlaceholderLarge()
        {
            var playbackViewModel = new PlaybackViewModel();

            playbackViewModel.CoverArt.Should().Be(PlaybackViewModel.CoverArtPlaceholderLarge);
        }

        [TestMethod]
        public void DoneFiltering_Should_ClearTheFilterText()
        {
            Subject.FilterText = "random text";

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
        public void HandleStartAudioPlayback_Always_SetsCoverArtToItemConvertArtUrl()
        {
            Subject.Handle(new StartPlaybackMessage(new PlaylistItem { CoverArtUrl = "test" }));

            Subject.CoverArt.Should().Be("test");
        }

        [TestMethod]
        public void HandleStartAudioPlayback_Always_SetsStateToAudio()
        {
            Subject.Handle(new StartPlaybackMessage(new PlaylistItem()));

            Subject.State.Should().Be(PlaybackViewModelStateEnum.Audio);
        }

        [TestMethod]
        public void Handle_PlayFailedMessage_CallsNotificationServiceShowWithANiceMessage()
        {
            Subject.Handle(new PlayFailedMessage("test m", null));

            MockDialogNotificationService.Showed.Count.Should().Be(1);
            MockDialogNotificationService.Showed.First().Message.Should().Be("Could not play item:\r\ntest m");
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
        public void Handle_PlaylistStateChangedMessage_SetsIsPlaylistVisibleToMessageHasElements()
        {
            Subject.Handle(new PlaylistStateChangedMessage(true));

            Subject.IsPlaylistVisible.Should().Be(true);
        }

        [TestMethod]
        public void Handle_PlaylistStateChangedMessage_PlaylistHasNoElementsSetsCoverArtToPlaceHolder()
        {
            Subject.Handle(new PlaylistStateChangedMessage(true));

            Subject.CoverArt.Should().Be(PlaybackViewModel.CoverArtPlaceholderLarge);
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
        public void LoadState_StateContainsPlaylistServiceKey_ShouldCallSetStateFromStringForThePlaylistManagementServiceWithTheObtainedData()
        {
            var statePageState = new Dictionary<string, object>
                                     {
                                         {
                                             PlaybackViewModel.PlaylistServiceStateKey,
                                             "testData"
                                         }
                                     };

            Subject.LoadState(null, statePageState);

            _mockPlaylistManagementService.SetStateFromStringCalls.Count.Should().Be(1);
            _mockPlaylistManagementService.SetStateFromStringCalls[0].Should().Be("testData");
        }

        [TestMethod]
        public void SaveState_Always_ShouldCallGetStateAsStringForThePlaylistManagementServiceWithTheObtainedData()
        {
            var knownTypes = new List<Type>();
            var statePageState = new Dictionary<string, object>();
            var callCount = 0;
            _mockPlaylistManagementService.GetStateAsStringCallback = () =>
                {
                    callCount++;
                    return "testData";
                };

            Subject.SaveState(statePageState, knownTypes);

            callCount.Should().Be(1);
            knownTypes.Should().Contain(typeof(string));
            statePageState[PlaybackViewModel.PlaylistServiceStateKey].Should().Be("testData");
        }

        [TestMethod]
        public void PlaylistItems_Should_BeAListCollectionView()
        {
            Subject.PlaylistItems.Should().BeOfType<ListCollectionView>();
        }

        [TestMethod]
        public async Task SavePlaylist_Always_CallsWinRTWrapperServiceGetNewStorageFile()
        {
            await Subject.SavePlaylist();

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

        [TestMethod]
        public void OnVisualStateChanged_NewStateNameIsSnappedAndCurrentStateIsVideoOrFullScreen_ShouldSentAStopMessage()
        {
            Subject.State = PlaybackViewModelStateEnum.Video;
            _mockPlayerManagementService.CurrentPlayer = new MockVideoPlayer();

            Subject.OnVisualStateChanged(PlaybackViewModel.SnappedStateName);

            MockEventAggregator.Messages.Should().ContainSingle(item => item.GetType() == typeof(StopMessage));
        }

        [TestMethod]
        public void OnVisualStateChanged_NewStateNameIsSnappedAndCurrentStateIsVideoOrFullScreen_ShouldTryToGetThePlaybackTimeInfoFromTheCurrentPlayer()
        {
            var mockVideoPlayer = new MockVideoPlayer();
            var callCount = 0;
            mockVideoPlayer.OnGetPlaybackTimeInfo = () =>
                {
                    callCount++;
                    return new PlaybackStateEventArgs();
                };
            _mockPlayerManagementService.CurrentPlayer = mockVideoPlayer;
            Subject.State = PlaybackViewModelStateEnum.Video;

            Subject.OnVisualStateChanged(PlaybackViewModel.SnappedStateName);

            callCount.Should().Be(1);
        }

        [TestMethod]
        public void OnVisualStateChanged_NewStateNameIsSnappedAndCurrentStateIsVideoOrFullScreen_ShouldSetTheSnappedVideoPlaybackViewModelAsTheDefaultVideoPlayer()
        {
            _mockPlayerManagementService.CurrentPlayer = new MockVideoPlayer();
            Subject.State = PlaybackViewModelStateEnum.Video;

            Subject.OnVisualStateChanged(PlaybackViewModel.SnappedStateName);

            _mockPlayerManagementService.DefaultVideoPlayer.Should().Be(_mockSnappedVideoPlaybackViewModel);
        }

        [TestMethod]
        public void OnVisualStateChanged_NewStateNameIsSnappedAndCurrentStateIsVideoOrFullScreen_ShouldSendAPlayMessageWithTheCurrentPlaybackTimeInfo()
        {
            Subject.State = PlaybackViewModelStateEnum.Video;
            var mockVideoPlayer = new MockVideoPlayer { OnGetPlaybackTimeInfo = () => new PlaybackStateEventArgs() };
            _mockPlayerManagementService.CurrentPlayer = mockVideoPlayer;

            Subject.OnVisualStateChanged(PlaybackViewModel.SnappedStateName);

            MockEventAggregator.Messages.Should().ContainSingle(item => item.GetType() == typeof(PlayMessage));
        }

        [TestMethod]
        public void OnVisualStateChanged_OldStateNameIsSnappedAndCurrentStateIsVideoOrFullScreen_ShouldSentAStopMessage()
        {
            Subject.State = PlaybackViewModelStateEnum.Video;
            _mockPlayerManagementService.CurrentPlayer = new MockVideoPlayer();

            Subject.OnVisualStateChanged(PlaybackViewModel.SnappedStateName);
            Subject.OnVisualStateChanged("test");

            MockEventAggregator.Messages.Count(m => m.GetType() == typeof(StopMessage)).Should().Be(2);
        }

        [TestMethod]
        public void OnVisualStateChanged_OldStateNameIsSnappedAndCurrentStateIsVideoOrFullScreen_ShouldTryToGetThePlaybackTimeInfoFromTheCurrentPlayer()
        {
            var mockVideoPlayer = new MockVideoPlayer();
            var callCount = 0;
            mockVideoPlayer.OnGetPlaybackTimeInfo = () =>
                {
                    callCount++;
                    return new PlaybackStateEventArgs();
                };
            _mockPlayerManagementService.CurrentPlayer = mockVideoPlayer;
            Subject.State = PlaybackViewModelStateEnum.Video;

            Subject.OnVisualStateChanged(PlaybackViewModel.SnappedStateName);
            Subject.OnVisualStateChanged("test");

            callCount.Should().Be(2);
        }

        [TestMethod]
        public void OnVisualStateChanged_OldStateNameIsSnappedAndCurrentStateIsVideo_ShouldSetTheEmbeddedVideoPlaybackViewModelAsTheDefaultVideoPlayer()
        {
            _mockPlayerManagementService.CurrentPlayer = new MockVideoPlayer();
            Subject.State = PlaybackViewModelStateEnum.Video;

            Subject.OnVisualStateChanged(PlaybackViewModel.SnappedStateName);
            Subject.OnVisualStateChanged("test");

            _mockPlayerManagementService.DefaultVideoPlayer.Should().Be(_mockEmbeddedVideoPlaybackViewModel);
        }

        [TestMethod]
        public void OnVisualStateChanged_OldStateNameIsSnappedAndCurrentStateIsFullScreen_ShouldSetTheFullScreenVideoPlaybackViewModelAsTheDefaultVideoPlayer()
        {
            _mockPlayerManagementService.CurrentPlayer = new MockVideoPlayer();
            Subject.State = PlaybackViewModelStateEnum.FullScreen;

            Subject.OnVisualStateChanged(PlaybackViewModel.SnappedStateName);
            Subject.OnVisualStateChanged("test");

            _mockPlayerManagementService.DefaultVideoPlayer.Should().Be(_mockFullScreenVideoPlaybackViewModel);
        }

        [TestMethod]
        public void OnVisualStateChanged_OldStateNameIsSnappedAndCurrentStateIsVideoOrFullScreen_ShouldSentAPlayMessageWithTheCurrentPlaybackTimeInfo()
        {
            Subject.State = PlaybackViewModelStateEnum.Video;
            var mockVideoPlayer = new MockVideoPlayer { OnGetPlaybackTimeInfo = () => new PlaybackStateEventArgs() };
            _mockPlayerManagementService.CurrentPlayer = mockVideoPlayer;

            Subject.OnVisualStateChanged(PlaybackViewModel.SnappedStateName);
            Subject.OnVisualStateChanged("test");

            MockEventAggregator.Messages.Count(m => m.GetType() == typeof(PlayMessage)).Should().Be(2);
        }

        [TestMethod]
        public async Task ShowArtistInfo_PlaylistHasCurrentItem_ShouldChangeTheStateToDetails()
        {
            _mockPlaylistManagementService.CurrentItem = new PlaylistItem { Artist = "test name" };

            await Subject.ShowArtistInfo();

            Subject.State.Should().Be(PlaybackViewModelStateEnum.Details);
        }

        [TestMethod]
        public async Task ShowArtistInfo_PlaylistHasCurrentItem_ShouldSetTheCurrentArtistNameOnTheArtistInfoViewModel()
        {
            _mockPlaylistManagementService.CurrentItem = new PlaylistItem { Artist = "test name" };

            await Subject.ShowArtistInfo();

            Subject.ArtistInfoViewModel.Parameter.Should().Be("test name");
        }

        [TestMethod]
        public async Task ShowArtistInfo_PlaylistHasCurrentItem_ShouldPopulateTheArtistInfoViewModel()
        {
            _mockPlaylistManagementService.CurrentItem = new PlaylistItem { Artist = "test name" };

            await Subject.ShowArtistInfo();

            _mockArtistInfoViewModel.PopulateCallCount.Should().Be(1);
        }

        [TestMethod]
        public async Task ShowArtistInfo_PlaylistDoesNotHaveCurrentItem_ShouldNotChangeTheStateOfTheViewModel()
        {
            _mockPlaylistManagementService.CurrentItem = null;
            Subject.State = PlaybackViewModelStateEnum.Audio;

            await Subject.ShowArtistInfo();

            Subject.State.Should().Be(PlaybackViewModelStateEnum.Audio);
        }

        [TestMethod]
        public void ToggleRepeatMessage_Always_SendsToggleRepeatMessageUsingEventAggregator()
        {
            Subject.ToggleRepeat();

            MockEventAggregator.Messages.Any(message => message.GetType() == typeof (ToggleRepeatMessage))
                .Should()
                .BeTrue();
        }

        #endregion

        #region Methods

        protected override void TestInitializeExtensions()
        {
            _mockToastNotificationService = new MockToastNotificationService();
            _mockWinRTWrappersService = new MockWinRTWrappersService();
            _mockPlaylistManagementService = new MockPlyalistManagementService();
            _mockPlayerManagementService = new MockPlayerManagementService();
            _mockEmbeddedVideoPlaybackViewModel = new MockEmbeddedVideoPlaybackViewModel();
            _mockSnappedVideoPlaybackViewModel = new MockSnappedVideoPlaybackViewModel();
            _mockFullScreenVideoPlaybackViewModel = new MockFullScreenVideoPlaybackViewModel();
            _mockArtistInfoViewModel = new MockArtistInfoViewModel();
            Subject.WinRTWrappersService = _mockWinRTWrappersService;
            Subject.PlaylistManagementService = _mockPlaylistManagementService;
            Subject.PlayerManagementService = _mockPlayerManagementService;
            Subject.EmbeddedVideoPlaybackViewModel = _mockEmbeddedVideoPlaybackViewModel;
            Subject.ToastNotificationService = _mockToastNotificationService;
            Subject.SnappedVideoPlaybackViewModel = _mockSnappedVideoPlaybackViewModel;
            Subject.FullScreenVideoPlaybackViewModel = _mockFullScreenVideoPlaybackViewModel;
            Subject.ArtistInfoViewModel = _mockArtistInfoViewModel;
        }

        #endregion
    }
}