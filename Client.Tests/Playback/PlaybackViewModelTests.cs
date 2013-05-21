using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Client.Common.EventAggregatorMessages;
using Client.Common.Models;
using Client.Common.Models.Subsonic;
using Client.Tests.Framework.ViewModel;
using Client.Tests.Mocks;
using FluentAssertions;
using Microsoft.VisualStudio.TestPlatform.UnitTestFramework;
using Subsonic8.Playback;
using Subsonic8.Playlists;

namespace Client.Tests.Playback
{
    [TestClass]
    public class PlaybackViewModelTests : ViewModelBaseTests<PlaybackViewModel>
    {
        protected override PlaybackViewModel Subject { get; set; }

        private MockWinRTWrappersService _mockWinRTWrappersService;
        private MockPlyalistManagementService _mockPlaylistManagementService;
        private MockEmbededVideoPlaybackViewModel _mockEmbededVideoPlaybackViewModel;
        private MockToastNotificationService _mockToastNotificationService;

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

        [TestMethod]
        public void HandleStartAudioPlayback_Alawys_SetsStateToAudio()
        {
            Subject.Handle(new StartPlaybackMessage(new PlaylistItem()));

            Subject.State.Should().Be(PlaybackViewModelStateEnum.Audio);
        }

        [TestMethod]
        public void HandleStartAudioPlayback_Alawys_SetsCoverArtToItemConverArtUrl()
        {
            Subject.Handle(new StartPlaybackMessage(new PlaylistItem { CoverArtUrl = "test" }));

            Subject.CoverArt.Should().Be("test");
        }

        [TestMethod]
        public void ClearPlaylist_Always_CallsPlaylistServiceClearItems()
        {
            Subject.ClearPlaylist();

            _mockPlaylistManagementService.ClearCallCount.Should().Be(1);
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
        public async Task ParameterWhenSetShouldAddAnItemToThePlaylist()
        {
            MockLoadModel();
            var itemsCount = _mockPlaylistManagementService.Items.Count;
            await Task.Run(() =>
                {
                    Subject.Parameter = 1;
                });

            _mockPlaylistManagementService.Items.Count.Should().Be(itemsCount + 1);
        }

        [TestMethod]
        public async Task ParameterWhenSetShouldStartPlayingTheLastAddedItem()
        {
            MockLoadModel();
            _mockPlaylistManagementService.Items = new PlaylistItemCollection { new PlaylistItem() };
            await Task.Run(() =>
                {
                    Subject.Parameter = 2;
                });

            MockEventAggregator.Messages.Any(
                m =>
                m.GetType() == typeof(PlayItemAtIndexMessage) &&
                ((PlayItemAtIndexMessage)m).Index == _mockPlaylistManagementService.Items.Count - 1).Should().BeTrue();
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
                m.GetType() == typeof(PlayItemAtIndexMessage) &&
                ((PlayItemAtIndexMessage)m).Index == _mockPlaylistManagementService.Items.Count - 1).Should().BeTrue();
        }

        [TestMethod]
        public void ClearPlaylist_Always_CallsPlaylistManagementServiceClearPlaylist()
        {
            Subject.ClearPlaylist();

            _mockPlaylistManagementService.ClearCallCount.Should().Be(1);
        }

        [TestMethod]
        public void SavePlaylist_Always_CallsWinRTWrapperServiceGetNewStorageFile()
        {
            Subject.SavePlaylist();

            _mockWinRTWrappersService.GetNewStorageFileCallCount.Should().Be(1);
        }

        [TestMethod]
        public void Ctor_Always_SetsCoverArtToCoverArtPlaceholderLarge()
        {
            var playbackViewModel = new PlaybackViewModel();

            playbackViewModel.CoverArt.Should().Be(PlaybackViewModel.CoverArtPlaceholderLarge);
        }

        [TestMethod]
        public void LoadState_StateContainsPlaylistKey_IsAbleToRestorePlaylistFromPreviouslySavePlaylist()
        {
            var knownTypes = new List<Type>();
            var statePageState = new Dictionary<string, object>();
            Subject.PlaylistItems.Add(new PlaylistItem { Artist = "test_a", UriAsString = "http://google.com/" });
            Subject.SaveState(statePageState, knownTypes);
            Subject.PlaylistItems.Clear();
            Subject.PlaylistItems.Count.Should().Be(0);

            Subject.LoadState(null, statePageState);

            Subject.PlaylistItems.Count.Should().Be(1);
            Subject.PlaylistItems[0].Artist.Should().Be("test_a");
            Subject.PlaylistItems[0].UriAsString.Should().Be("http://google.com/");
        }

        [TestMethod]
        public void LoadRemotePlaylist_Always_ShouldNavigateToManagePlaylistsViewModel()
        {
            Subject.LoadRemotePlaylist();

            MockNavigationService.NavigateToViewModelCalls.Count.Should().Be(1);
            MockNavigationService.NavigateToViewModelCalls.First().Key.Should().Be(typeof(ManagePlaylistsViewModel));
            MockNavigationService.NavigateToViewModelCalls.First().Value.Should().BeNull();
        }

        private void MockLoadModel()
        {
            Subject.LoadModel = model =>
                {
                    var tcr = new TaskCompletionSource<PlaylistItem>();
                    tcr.SetResult(new PlaylistItem
                        {
                            PlayingState = PlaylistItemState.NotPlaying,
                            Uri = new Uri("http://test-uri"),
                            Artist = "test-artist"
                        });
                    return tcr.Task;
                };
        }
    }
}
