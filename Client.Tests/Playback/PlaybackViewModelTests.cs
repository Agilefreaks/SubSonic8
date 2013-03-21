using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Caliburn.Micro;
using Client.Common.EventAggregatorMessages;
using Client.Common.Models;
using Client.Common.Models.Subsonic;
using Client.Tests.Framework.ViewModel;
using Client.Tests.Mocks;
using FluentAssertions;
using Microsoft.VisualStudio.TestPlatform.UnitTestFramework;
using Subsonic8.Messages;
using Subsonic8.Playback;

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
        public void OnActivate_Always_SetsBottomBarIsOpenToFalse()
        {
            Subject.BottomBar.IsOpened = true;

            ((IActivate)Subject).Activate();

            Subject.BottomBar.IsOpened.Should().BeFalse();
        }

        [TestMethod]
        public override void OnActivateShouldSetBottomBarIsOnPlaylistToFalse()
        {
            Subject.BottomBar.IsOnPlaylist = false;

            ((IActivate)Subject).Activate();

            Subject.BottomBar.IsOnPlaylist.Should().BeTrue();
        }

        [TestMethod]
        public void HandleStartVideoPlayback_Alawys_CallsNotificationManagerShow()
        {
            Subject.Handle(new StartVideoPlaybackMessage(new PlaylistItem()) { FullScreen = true });

            _mockToastNotificationService.ShowCallCount.Should().Be(1);
        }

        [TestMethod]
        public void HandleStartVideoPlayback_MessageIsNotForFullScreen_SetsStateToVideo()
        {
            ((IActivate)Subject).Activate();
            Subject.Handle(new StartVideoPlaybackMessage(new PlaylistItem()) { FullScreen = false });

            Subject.State.Should().Be(PlaybackViewModelStateEnum.Video);
        }

        [TestMethod]
        public void HandleStartAudioPlayback_Alawys_CallsNotificationManagerShow()
        {
            Subject.Handle(new StartAudioPlaybackMessage(new PlaylistItem()));

            _mockToastNotificationService.ShowCallCount.Should().Be(1);
        }

        [TestMethod]
        public void HandleStartAudioPlayback_Alawys_SetsStateToAudio()
        {
            Subject.Handle(new StartAudioPlaybackMessage(new PlaylistItem()));

            Subject.State.Should().Be(PlaybackViewModelStateEnum.Audio);
        }

        [TestMethod]
        public void HandleStartAudioPlayback_Alawys_SetsCoverArtToItemConverArtUrl()
        {
            Subject.Handle(new StartAudioPlaybackMessage(new PlaylistItem { CoverArtUrl = "test" }));

            Subject.CoverArt.Should().Be("test");
        }

        [TestMethod]
        public void ClearPlaylist_Always_CallsPlaylistServiceClearItems()
        {
            Subject.ClearPlaylist();

            _mockPlaylistManagementService.ClearCallCount.Should().Be(1);
        }

        [TestMethod]
        public void Handle_PlaylistStateChangedMessage_PublishesAShowControlsMessageWithShowSetToHasElements()
        {
            Subject.Handle(new PlaylistStateChangedMessage(true));

            MockEventAggregator.Messages.Any(
                m => m.GetType() == typeof(ShowControlsMessage) && ((ShowControlsMessage)m).Show).Should().BeTrue();
        }

        [TestMethod]
        public void IsPlayingReturnsPlaylistManagementServiceIsPlaying()
        {
            _mockPlaylistManagementService.IsPlaying = true;

            Subject.IsPlaying.Should().BeTrue();
        }

        [TestMethod]
        public void ShuffleOnReturnsPlaylistManagementServiceShuffleOn()
        {
            _mockPlaylistManagementService.ShuffleOn = true;

            Subject.ShuffleOn.Should().BeTrue();
        }

        [TestMethod]
        public async Task ParameterWhenSetShouldAddAnItemToThePlaylistWithItsInfo()
        {
            MockLoadModel();
            await Task.Run(() =>
                {
                    Subject.Parameter = new Song { IsVideo = true };
                });

            MockEventAggregator.Messages.Any(
                m => m.GetType() == typeof(AddItemsMessage) && ItemMatches(((AddItemsMessage)m).Queue[0])).Should().BeTrue();
        }

        [TestMethod]
        public async Task ParameterWhenSetShouldStartPlayingTheLastAddedItem()
        {
            MockLoadModel();
            _mockPlaylistManagementService.Items = new PlaylistItemCollection { new PlaylistItem() };
            await Task.Run(() =>
                {
                    Subject.Parameter = new Song { IsVideo = true };
                });

            MockEventAggregator.Messages.Any(
                m => m.GetType() == typeof(PlayItemAtIndexMessage) && ((PlayItemAtIndexMessage)m).Index == 0).Should().BeTrue();
        }

        [TestMethod]
        public async Task HandleWithPlaylistShouldFireAnAddItemsMessageForEachMediaItem()
        {
            await Task.Run(() => Subject.Handle(new PlaylistMessage
                                       {
                                           Queue =
                                               new List<ISubsonicModel>
                                                           {
                                                               new Song {IsVideo = true},
                                                               new Song {IsVideo = false}
                                                           }
                                       }));

            MockEventAggregator.Messages.Count.Should().Be(2);
            MockEventAggregator.Messages.All(m => m.GetType() == typeof(AddItemsMessage)).Should().BeTrue();
        }

        [TestMethod]
        public async Task HandleWithPlaylistMessage_ClearCurrentTrue_ShouldPublishAPlayNextMessage()
        {
            await Task.Run(() => Subject.Handle(new PlaylistMessage
                                   {
                                       Queue = new List<ISubsonicModel> { new Song(), new Song(), new Song() },
                                       ClearCurrent = true
                                   }));

            MockEventAggregator.Messages.Single(m => m.GetType() == typeof(PlayNextMessage)).Should().NotBeNull();
        }

        [TestMethod]
        public async Task HandleWithPlaylistMessageWhenClearCurrentIsTrueClearsTheCurrentPlaylist()
        {
            await Task.Run(() => Subject.Handle(new PlaylistMessage { Queue = new List<ISubsonicModel>(), ClearCurrent = true }));

            _mockPlaylistManagementService.ClearCallCount.Should().Be(1);
        }

        [TestMethod]
        public async Task HandleWithPlaylistMessage_QueHasItemOfTypeAlbum_CallsSubsonicServiceGetAlbumAndAdsAllItsSongsToThePlaylist()
        {
            MockLoadModel();
            var subsonicModels = new List<ISubsonicModel> { new Common.Models.Subsonic.Album { Id = 5 } };
            var songs = new List<Song> { new Song(), new Song() };
            var album = new Common.Models.Subsonic.Album { Songs = songs };
            var mockGetAlbumResult = new MockGetAlbumResult { GetResultFunc = () => album };
            var callCount = 0;
            MockSubsonicService.GetAlbum = albumId =>
                {
                    callCount++;
                    albumId.Should().Be(5);
                    return mockGetAlbumResult;
                };

            await Task.Run(() => Subject.Handle(new PlaylistMessage { Queue = subsonicModels }));

            callCount.Should().Be(1);
            MockEventAggregator.Messages.All(m => m.GetType() == typeof(AddItemsMessage)).Should().BeTrue();
            MockEventAggregator.Messages.Count.Should().Be(2);
        }

        [TestMethod]
        public async Task HandleWithPlaylistMessage_QueHasItemOfTypeArtist_CallsSubsonicServiceGetArtistAndAddsAllSongsFromAllAlbumsToThePlaylist()
        {
            MockLoadModel();
            var addToPlaylistQue = new List<ISubsonicModel> { new ExpandedArtist { Id = 5 } };
            var albums = new List<Common.Models.Subsonic.Album>
                {
                    new Common.Models.Subsonic.Album(),
                    new Common.Models.Subsonic.Album()
                };
            var artist = new ExpandedArtist { Albums = albums };
            var mockGetAlbumResult = new MockGetArtistResult { GetResultFunc = () => artist };
            var callCount = 0;
            MockSubsonicService.GetArtist = albumId =>
                {
                    callCount++;
                    albumId.Should().Be(5);
                    return mockGetAlbumResult;
                };
            var getAlbumCallCount = 0;
            MockSubsonicService.GetAlbum = artistId =>
                {
                    getAlbumCallCount++;
                    return new MockGetAlbumResult
                        {
                            GetResultFunc = () => new Common.Models.Subsonic.Album { Songs = new List<Song> { new Song() } }
                        };
                };

            await Task.Run(() => Subject.Handle(new PlaylistMessage { Queue = addToPlaylistQue }));

            callCount.Should().Be(1);
            getAlbumCallCount.Should().Be(2);
            MockEventAggregator.Messages.All(m => m.GetType() == typeof(AddItemsMessage)).Should().BeTrue();
            MockEventAggregator.Messages.Count.Should().Be(2);
        }

        [TestMethod]
        public async Task HandleWithPlaylistMessage_QueHasItemOfTypeMusicDirectory_CallsSubsonicServiceGetMusicDirectorysAndAddsAllSongsToThePlaylist()
        {
            MockLoadModel();
            var addToPlaylistQue = new List<ISubsonicModel> { new Common.Models.Subsonic.MusicDirectory { Id = 5 } };
            var children = new List<MusicDirectoryChild> { new MusicDirectoryChild(), new MusicDirectoryChild() };
            var musicDirectory = new Common.Models.Subsonic.MusicDirectory { Children = children };
            var mockGetAlbumResult = new MockGetMusicDirectoryResult { GetResultFunc = () => musicDirectory };
            var callCount = 0;
            MockSubsonicService.GetMusicDirectory = directoryId =>
                {
                    callCount++;
                    directoryId.Should().Be(5);
                    return mockGetAlbumResult;
                };

            await Task.Run(() => Subject.Handle(new PlaylistMessage { Queue = addToPlaylistQue }));

            callCount.Should().Be(1);
            MockEventAggregator.Messages.All(m => m.GetType() == typeof(AddItemsMessage)).Should().BeTrue();
            MockEventAggregator.Messages.Count.Should().Be(2);
        }

        [TestMethod]
        public async Task HandleWithPlaylistMessage_QueHasItemOfTypeIndexItem_CallsSubsonicServiceGetMusicDirectorysForEachChildArtist()
        {
            MockLoadModel();
            var addToPlaylistQue = new List<ISubsonicModel> { new IndexItem { Id = 5, Artists = new List<Common.Models.Subsonic.Artist> { new Common.Models.Subsonic.Artist { Id = 3 } } } };
            var children = new List<MusicDirectoryChild> { new MusicDirectoryChild(), new MusicDirectoryChild() };
            var musicDirectory = new Common.Models.Subsonic.MusicDirectory { Children = children };
            var mockGetMusicDirectoryResult = new MockGetMusicDirectoryResult { GetResultFunc = () => musicDirectory };
            var callCount = 0;
            MockSubsonicService.GetMusicDirectory = directoryId =>
                {
                    callCount++;
                    directoryId.Should().Be(3);
                    return mockGetMusicDirectoryResult;
                };

            await Task.Run(() => Subject.Handle(new PlaylistMessage { Queue = addToPlaylistQue }));

            callCount.Should().Be(1);
            MockEventAggregator.Messages.All(m => m.GetType() == typeof(AddItemsMessage)).Should().BeTrue();
            MockEventAggregator.Messages.Count.Should().Be(2);
        }

        [TestMethod]
        public async Task HandleWithPlayFileOfTypeSongShouldPublishAAddItemMessage()
        {
            MockLoadModel();

            await Task.Run(() => Subject.Handle(new PlayFile { Model = new Song { IsVideo = false } }));

            MockEventAggregator.Messages.Any(
                m =>
                m.GetType() == typeof(AddItemsMessage) && ItemMatches(((AddItemsMessage)m).Queue[0]))
                                .Should().BeTrue();
        }

        [TestMethod]
        public async Task HandleWithPlayFileOfTypeSongShouldPublishAMessageToStartPlayingTheLastAddedItem()
        {
            MockLoadModel();
            _mockPlaylistManagementService.Items = new PlaylistItemCollection { new PlaylistItem() };

            await Task.Run(() => Subject.Handle(new PlayFile { Model = new Song { IsVideo = false } }));

            MockEventAggregator.Messages.Any(
                m => m.GetType() == typeof(PlayItemAtIndexMessage) && ((PlayItemAtIndexMessage)m).Index == 0).Should().BeTrue();
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

        private static bool ItemMatches(PlaylistItem itemToCheck)
        {
            return itemToCheck.Uri.ToString() == "http://test-uri/";
        }
    }
}
