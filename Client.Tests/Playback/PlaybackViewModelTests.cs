using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Threading.Tasks;
using Client.Common.Models;
using Client.Common.Models.Subsonic;
using Client.Tests.Framework.ViewModel;
using Client.Tests.Mocks;
using FluentAssertions;
using Microsoft.VisualStudio.TestPlatform.UnitTestFramework;
using Subsonic8.Messages;
using Subsonic8.Playback;
using Subsonic8.PlaylistItem;

namespace Client.Tests.Playback
{
    [TestClass]
    public class PlaybackViewModelTests : ViewModelBaseTests<IPlaybackViewModel>
    {
        protected override IPlaybackViewModel Subject { get; set; }

        private MockEventAggregator _mockEventAggregator;
        private MockNotificationService _mockNotificationService;

        protected override void TestInitializeExtensions()
        {
            _mockEventAggregator = new MockEventAggregator();
            _mockNotificationService = new MockNotificationService();
            MockShellViewModel.SubsonicService = MockSubsonicService;

            Subject = new PlaybackViewModel(_mockEventAggregator, MockShellViewModel, MockSubsonicService, _mockNotificationService)
                          {
                              NavigationService = MockNavigationService,
                              SubsonicService = MockSubsonicService,
                              LoadModel = model =>
                                              {
                                                  var tcr = new TaskCompletionSource<PlaylistItemViewModel>();
                                                  tcr.SetResult(new PlaylistItemViewModel { Item = (ISubsonicModel)model });
                                                  return tcr.Task;
                                              }
                          };
        }

        [TestMethod]
        public void CtorShouldInitializePlaylistItems()
        {
            Subject.PlaylistItems.Should().NotBeNull();
        }

        [TestMethod]
        public void Ctor_Always_SetsNextActionToNext()
        {
            var playbackViewModel = new PlaybackViewModel(_mockEventAggregator, MockShellViewModel,
                                                          MockSubsonicService, _mockNotificationService);

            Assert.AreEqual(playbackViewModel.NextAction, playbackViewModel.Next);
        }

        [TestMethod]
        public void OnActivateShouldSetDisplayNameToPlaylist()
        {
            Subject.Activate();

            Subject.DisplayName.Should().Be("Playlist");
        }

        [TestMethod]
        public void NextShouldSetSourceOnShellViewModelToSecondElementInPlaylistIfShuffleOff()
        {
            var file1 = new PlaylistItemViewModel { Uri = new Uri("http://file1"), Item = new Song { IsVideo = false } };
            var file2 = new PlaylistItemViewModel { Uri = new Uri("http://file2"), Item = new Song { IsVideo = false } };
            Subject.PlaylistItems = new ObservableCollection<PlaylistItemViewModel> { file1, file2 };
            Subject.Next();

            Subject.Next();

            MockShellViewModel.Source.Should().Be(file2.Uri);
        }

        [TestMethod]
        public void Next_ShuffleOnTrue_SetsARandomSongAsTheNewSource()
        {
            var playlist = GeneratePlaylist(100);
            Subject.PlaylistItems = new ObservableCollection<PlaylistItemViewModel>(playlist);
            Subject.Handle(new ToggleShuffleMessage());

            Subject.Next();

            MockShellViewModel.Source.Should().NotBe(playlist[0].Uri);
        }

        [TestMethod]
        public void Next_WasPlayingASong_PushesTheCurrentTrackIndexToTheTrackHistory()
        {
            var playlist = GeneratePlaylist(15);
            Subject.PlaylistItems = new ObservableCollection<PlaylistItemViewModel>(playlist);

            Subject.PlayPause();
            Subject.Next();
            Subject.Next();

            Subject.PlaylistHistory.Count.Should().Be(2);
            Subject.PlaylistHistory.Pop().Should().Be(1);
            Subject.PlaylistHistory.Pop().Should().Be(0);
        }

        [TestMethod]
        public void Next_WasNotPlayingASong_DoesNotPushTheCurrentTrackIndexToTheTrackHistory()
        {
            var playlist = GeneratePlaylist(15);
            Subject.PlaylistItems = new ObservableCollection<PlaylistItemViewModel>(playlist);

            Subject.Next();

            Subject.PlaylistHistory.Count.Should().Be(0);
        }

        [TestMethod]
        public void NextCallsNotificationManagerShow()
        {
            Subject.PlaylistItems = new ObservableCollection<PlaylistItemViewModel> { new PlaylistItemViewModel { Item = new Song { IsVideo = false } } };

            Subject.Next();

            _mockNotificationService.ShowCallCount.Should().Be(1);
        }

        [TestMethod]
        public void NextCallsStartIfThereAreElementsToPlay()
        {
            var called = false;
            Subject.StartAction = item => { called = true; };
            Subject.PlaylistItems.Add(new PlaylistItemViewModel());

            Subject.Next();

            called.Should().BeTrue();
        }

        [TestMethod]
        public void PreviousShouldSetSourceOnShellViewModelToPreviousElementInPlaylist()
        {
            var file1 = new PlaylistItemViewModel { Uri = new Uri("http://file1"), Item = new Song { IsVideo = false } };
            var file2 = new PlaylistItemViewModel { Uri = new Uri("http://file2"), Item = new Song { IsVideo = false } };
            Subject.PlaylistItems = new ObservableCollection<PlaylistItemViewModel> { file1, file2 };
            Subject.Play();
            Subject.Next();

            Subject.Previous();

            MockShellViewModel.Source.Should().Be(file1.Uri);
        }

        [TestMethod]
        public void PreviousCallsStartIfThereAreElementsToPlay()
        {
            var called = false;
            Subject.StartAction = item => { called = true; };
            Subject.PlaylistItems.Add(new PlaylistItemViewModel());
            Subject.PlaylistItems.Add(new PlaylistItemViewModel());
            Subject.Play();
            Subject.Next();

            Subject.Previous();

            called.Should().BeTrue();
        }

        [TestMethod]
        public void Previous_ShuffleOnAndHistoryIsEmpty_DoesNotChangeTheCurrentTrack()
        {
            var playlist = GeneratePlaylist(15);
            Subject.PlaylistItems = new ObservableCollection<PlaylistItemViewModel>(playlist);
            Subject.Handle(new ToggleShuffleMessage());
            Subject.Next();
            var currentSong = MockShellViewModel.Source;

            Subject.Previous();

            MockShellViewModel.Source.Should().Be(currentSong);
        }

        [TestMethod]
        public void Previous_ShuffleOnAndHistoryIsNotEmpty_SetsTheSourceToThePreviousItem()
        {
            var playlist = GeneratePlaylist(15);
            Subject.PlaylistItems = new ObservableCollection<PlaylistItemViewModel>(playlist);
            Subject.Handle(new ToggleShuffleMessage());
            Subject.Next();
            var previousSource = MockShellViewModel.Source;
            Subject.Next();

            Subject.Previous();

            MockShellViewModel.Source.Should().Be(previousSource);
        }

        [TestMethod]
        public void Previous_ShuffleOff_SetsTheSourceToThePreviousItemInThePlaylist()
        {
            var playlist = GeneratePlaylist(15);
            Subject.PlaylistItems = new ObservableCollection<PlaylistItemViewModel>(playlist);
            Subject.Next();
            Subject.Next();

            Subject.Previous();

            MockShellViewModel.Source.Should().Be(playlist[0].Uri);
        }

        [TestMethod]
        public void PlayIfCurrentTrackIsNulShouldSetSourceOnShellViewModelToFirstItemInThePlaylist()
        {
            var uri = new Uri("http://tests.cs");
            Subject.PlaylistItems.Add(new PlaylistItemViewModel { Uri = uri, Item = new Song { IsVideo = false } });

            Subject.Play();

            MockShellViewModel.Source.Should().Be(uri);
        }

        [TestMethod]
        public void PlayIfCurrentTrackIsNotNullShouldSetTheSourceOnShellToSame()
        {
            var uri = new Uri("http://tests.cs");
            Subject.PlaylistItems.Add(new PlaylistItemViewModel { Uri = uri, Item = new Song { IsVideo = false } });
            Subject.Play();

            Subject.Play();

            MockShellViewModel.Source.Should().Be(uri);
        }

        [TestMethod]
        public void IsPlayingReturnsTrueIfAnyOfThePlaylistElementsHasStatePlaying()
        {
            Subject.PlaylistItems.Add(new PlaylistItemViewModel { PlayingState = PlaylistItemState.Playing });

            Subject.IsPlaying.Should().BeTrue();
        }

        [TestMethod]
        public void IsPlayingShouldBeFalseIfAllOfThePlaylistElementsHasStateNotPlaying()
        {
            Subject.PlaylistItems.Add(new PlaylistItemViewModel { PlayingState = PlaylistItemState.NotPlaying });

            Subject.IsPlaying.Should().BeFalse();
        }

        [TestMethod]
        public void PlayWhenPlaylistHasElementsSetsIsPlayingToTrue()
        {
            Subject.PlaylistItems.Add(new PlaylistItemViewModel { Item = new Song { IsVideo = false } });

            Subject.Play();

            Subject.IsPlaying.Should().BeTrue();
        }

        [TestMethod]
        public void PlayWhenPlaylistDoesNotHaveElementsSetsIsPlayingToFalse()
        {
            Subject.PlaylistItems.Clear();

            Subject.Play();

            Subject.IsPlaying.Should().BeFalse();
        }

        [TestMethod]
        public void PlaySetsStateOnFirstPlaylistItemToPlaying()
        {
            var song1 = new PlaylistItemViewModel { Item = new Song { IsVideo = false } };
            var song2 = new PlaylistItemViewModel();
            var song3 = new PlaylistItemViewModel();
            Subject.PlaylistItems.Add(song1);
            Subject.PlaylistItems.Add(song2);
            Subject.PlaylistItems.Add(song3);

            Subject.Play();

            song1.PlayingState.Should().Be(PlaylistItemState.Playing);
        }

        [TestMethod]
        public void PlayCallsStartIfPlaylistContainsElements()
        {
            var called = false;
            Subject.StartAction = item => { called = true; };
            Subject.PlaylistItems.Add(new PlaylistItemViewModel());

            Subject.Play();

            called.Should().BeTrue();
        }

        [TestMethod]
        public void PlaySetsIsOpenOnBottomBarToTrue()
        {
            Subject.PlaylistItems.Add(new PlaylistItemViewModel { Item = new Song() });

            Subject.Play();

            Subject.BottomBar.IsPlaying.Should().BeTrue();
        }

        [TestMethod]
        public void PauseIfPlayerIsPlayingCallsShellViewModelPlayPause()
        {
            Subject.PlaylistItems.Add(new PlaylistItemViewModel { PlayingState = PlaylistItemState.Playing });
            Subject.ShellViewModel = new MockShellViewModel();

            Subject.Pause();

            ((MockShellViewModel)Subject.ShellViewModel).PlayPauseCallCount.Should().Be(1);
        }

        [TestMethod]
        public void PauseIfPlayerIsNotPlayingDoesNotCallShellViewModelPlayPause()
        {
            Subject.PlaylistItems.Add(new PlaylistItemViewModel { PlayingState = PlaylistItemState.NotPlaying });
            Subject.ShellViewModel = new MockShellViewModel();

            Subject.Pause();

            ((MockShellViewModel)Subject.ShellViewModel).PlayPauseCallCount.Should().Be(0);
        }

        [TestMethod]
        public void StopSetsIsOpenOnBottomBarToFalse()
        {
            Subject.BottomBar.IsPlaying = true;

            Subject.Stop();

            Subject.BottomBar.IsPlaying.Should().BeFalse();
        }

        [TestMethod]
        public void StopWillCallShellViewModelStop()
        {
            Subject.ShellViewModel = new MockShellViewModel();

            Subject.Stop();

            ((MockShellViewModel)Subject.ShellViewModel).StopCallCount.Should().Be(1);
        }

        [TestMethod]
        public void StopWillSetSourceToNull()
        {
            Subject.Source = new Uri("http://this-will-be.null");

            Subject.Stop();

            Subject.Source.Should().BeNull();
        }

        [TestMethod]
        public void StopWillSetIsPlayingToFalse()
        {
            Subject.PlaylistItems.Add(new PlaylistItemViewModel { Item = new Song { IsVideo = false } });
            Subject.Play();

            Subject.Stop();

            Subject.IsPlaying.Should().BeFalse();
        }

        [TestMethod]
        public void StartShouldSetUriToShellViewModelSource()
        {
            var uri = new Uri("http://test.cc");

            Subject.StartAction(new PlaylistItemViewModel { Uri = uri, Item = new Song() });

            Subject.ShellViewModel.Source.Should().Be(uri);
        }

        [TestMethod]
        public void StartWhenItemIsSongSetsCoverArtProperty()
        {
            Subject.StartAction(new PlaylistItemViewModel { CoverArtId = "42", Item = new Song { IsVideo = false } });

            Subject.CoverArt.Should().NotBeNull();
        }

        [TestMethod]
        public void StartSetsIsPlayingToTrue()
        {
            Subject.PlaylistItems.Add(new PlaylistItemViewModel { Item = new Song() });

            Subject.Play();

            Subject.IsPlaying.Should().BeTrue();
        }

        [TestMethod]
        public void StartSetsPlayingStateToOnlyOneObjectFromPlaylist()
        {
            var song1 = new PlaylistItemViewModel { PlayingState = PlaylistItemState.Playing, Item = new Song() };
            var song2 = new PlaylistItemViewModel { PlayingState = PlaylistItemState.Playing, Item = new Song() };
            var song3 = new PlaylistItemViewModel { PlayingState = PlaylistItemState.Playing, Item = new Song() };
            Subject.PlaylistItems.Add(song1);
            Subject.PlaylistItems.Add(song2);
            Subject.PlaylistItems.Add(song3);

            Subject.StartAction(song2);

            Subject.PlaylistItems.Count(pi => pi.PlayingState == PlaylistItemState.Playing).Should().Be(1);
        }

        [TestMethod]
        public void StartSetsPlayingStateToObjectFromParameter()
        {
            var song1 = new PlaylistItemViewModel { PlayingState = PlaylistItemState.Playing, Item = new Song() };
            var song2 = new PlaylistItemViewModel { PlayingState = PlaylistItemState.Playing, Item = new Song() };
            var song3 = new PlaylistItemViewModel { PlayingState = PlaylistItemState.Playing, Item = new Song() };
            Subject.PlaylistItems.Add(song1);
            Subject.PlaylistItems.Add(song2);
            Subject.PlaylistItems.Add(song3);

            Subject.StartAction(song3);

            song3.PlayingState.Should().Be(PlaylistItemState.Playing);
        }

        [TestMethod]
        public void StartWhenItemIsVideoSetsSourceToUri()
        {
            var video = new PlaylistItemViewModel { Item = new Song { IsVideo = true } };

            Subject.StartAction(video);

            Subject.Source.Should().NotBeNull();
        }

        [TestMethod]
        public void StartWhenItemIsVideoSetsShellViewModelSourceToNull()
        {
            var video = new PlaylistItemViewModel { Item = new Song { IsVideo = true } };

            Subject.StartAction(video);

            Subject.ShellViewModel.Source.Should().BeNull();
        }

        [TestMethod]
        public void StartWhenItemIsVideoSetsStateToVideo()
        {
            Subject.StartAction(new PlaylistItemViewModel { Item = new Song { IsVideo = true } });

            Subject.State.Should().Be(PlaybackViewModelStateEnum.Video);
        }

        [TestMethod]
        public void StartWhenItemIsSongSetsStateToAudio()
        {
            Subject.StartAction(new PlaylistItemViewModel { Item = new Song { IsVideo = false } });

            Subject.State.Should().Be(PlaybackViewModelStateEnum.Audio);
        }

        [TestMethod]
        public void NextWhenCurrentItemIsVideoAndNextIsSongSetsStateToAudio()
        {
            Subject.PlaylistItems.Add(new PlaylistItemViewModel { Item = new Song { IsVideo = true } });
            Subject.PlaylistItems.Add(new PlaylistItemViewModel { Item = new Song { IsVideo = false } });
            Subject.Play();

            Subject.Next();

            Subject.State.Should().Be(PlaybackViewModelStateEnum.Audio);
        }

        [TestMethod]
        public void StartAlwaysCallSubsonicServiceGetCoverArtForId()
        {
            var mockSubsonicService = new MockSubsonicService();
            Subject.SubsonicService = mockSubsonicService;

            Subject.StartAction(new PlaylistItemViewModel { Item = new Song() });

            (mockSubsonicService.GetCoverArtForIdCallCount > 0).Should().BeTrue();
        }

        [TestMethod]
        public void PauseWillSetIsPlayingToFalse()
        {
            Subject.PlaylistItems.Add(new PlaylistItemViewModel { Item = new Song() });
            Subject.Play();

            Subject.Pause();

            Subject.IsPlaying.Should().BeFalse();
        }

        [TestMethod]
        public async Task ParameterWhenSetToTypeVideoShouldSetSourceOnShellViewModelToNullAndSourceOnPlaybackViewModelToNewUri()
        {
            MockLoadModel(true);
            await Task.Run(() =>
            {
                MockShellViewModel.Source = new Uri("http://this-should-become.null");
                Subject.Parameter = new Song { IsVideo = true };
            });

            Subject.Source.Should().NotBeNull();
            MockShellViewModel.Source.Should().BeNull();
        }

        [TestMethod]
        public void HandleWithStopMessageShouldSetSourceOnShellToNull()
        {
            Subject.Handle(new StopMessage());

            MockShellViewModel.Source.Should().BeNull();
        }

        [TestMethod]
        public void HandleWithRemoveFromPlaylistMessageShouldItemsInQueueFromCurrentPlaylist()
        {
            var playlistItemViewModel = new PlaylistItemViewModel();
            Subject.PlaylistItems.Add(playlistItemViewModel);

            Subject.Handle(new RemoveFromPlaylistMessage { Queue = new List<PlaylistItemViewModel> { playlistItemViewModel } });

            Subject.PlaylistItems.Should().HaveCount(0);
        }

        [TestMethod]
        public async Task HandleWithPlaylistShouldAddFilesInQueueToPlaylist()
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

            Subject.PlaylistItems.Should().HaveCount(2);
        }

        [TestMethod]
        public async Task HandleWithPlaylistShouldKeepElementsInPlaylist()
        {
            await Task.Run(() =>
                               {
                                   Subject.Handle(new PlaylistMessage
                                                      {
                                                          Queue =
                                                              new List<ISubsonicModel>
                                                                  {
                                                                      new Song(),
                                                                      new Song()
                                                                  }
                                                      });
                                   Subject.Handle(new PlaylistMessage { Queue = new List<ISubsonicModel> { new Song() } });
                               });

            Subject.PlaylistItems.Should().HaveCount(3);
        }

        [TestMethod]
        public async Task HandleWithPlaylistMessageWithSubsonicModelOfAnyTypeVideoShouldAddNewItemsInPlalistItemsCollection()
        {
            await Task.Run(() => Subject.Handle(new PlaylistMessage
                                {
                                    Queue = new List<ISubsonicModel>
                                                {
                                                    new Song {Id = 41},
                                                    new Song {Id = 42, IsVideo = true}
                                                }
                                }));

            Subject.PlaylistItems.Should().HaveCount(2);
        }

        [TestMethod]
        public async Task HandleWithPlaylistMessage_ClearCurrentTrye_ShouldSetStatePropertyToPlayingOnlyOnOneItem()
        {
            await Task.Run(() => Subject.Handle(new PlaylistMessage
                                   {
                                       Queue = new List<ISubsonicModel> { new Song(), new Song(), new Song() },
                                       ClearCurrent = true
                                   }));

            Subject.PlaylistItems.Count(pi => pi.PlayingState == PlaylistItemState.Playing).Should().Be(1);
        }

        [TestMethod]
        public async Task HandleWithPlaylistMessageWhenClearCurrentIsTrueClearsTheCurrentPlaylist()
        {
            Subject.PlaylistItems.Add(new PlaylistItemViewModel());
            Subject.PlaylistItems.Add(new PlaylistItemViewModel());
            Subject.PlaylistItems.Add(new PlaylistItemViewModel());

            await Task.Run(() => Subject.Handle(new PlaylistMessage { Queue = new List<ISubsonicModel>(), ClearCurrent = true }));

            Subject.PlaylistItems.Count.Should().Be(0);
        }

        [TestMethod]
        public async Task HandleWithPlaylistMessage_ClearCurrentTrueAndSourceOnPlaybackViewModelAndOnShellViewModelAreNull_CallsNext()
        {
            var called = false;
            Subject.NextAction = () => { called = true; };
            MockLoadModel();

            await Task.Run(() => Subject.Handle(new PlaylistMessage
                {
                    Queue = new List<ISubsonicModel> { new Song { IsVideo = false } },
                    ClearCurrent = true
                }));

            called.Should().BeTrue();
        }

        [TestMethod]
        public async Task HandleWithPlaylistMessageWhenSourceOnPlaybackViewModelIsNotNullDoesNotCallsStart()
        {
            var called = false;
            Subject.Source = new Uri("http://test");
            Subject.StartAction = item => { called = true; };
            MockLoadModel();

            await Task.Run(() => Subject.Handle(new PlaylistMessage { Queue = new List<ISubsonicModel> { new Song { IsVideo = false } } }));

            called.Should().BeFalse();
        }

        [TestMethod]
        public async Task HandleWithPlaylistMessageWhenSourceOnShellViewModelIsNotNullDoesNotCallsStart()
        {
            var called = false;
            Subject.ShellViewModel.Source = new Uri("http://test");
            Subject.StartAction = item => { called = true; };
            MockLoadModel();

            await Task.Run(() => Subject.Handle(new PlaylistMessage { Queue = new List<ISubsonicModel> { new Song { IsVideo = false } } }));

            called.Should().BeFalse();
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
            Subject.PlaylistItems.Count.Should().Be(2);
        }

        [TestMethod]
        public async Task HandleWithPlaylistMessage_QueHasItemOfTypeArtist_CallsSubsonicServiceGetArtistAndAddsAllSongsFromAllAlbumsToThePlaylist()
        {
            MockLoadModel();
            var addToPlaylistQue = new List<ISubsonicModel> { new ExpandedArtist { Id = 5 } };
            var albums = new List<Common.Models.Subsonic.Album> { new Common.Models.Subsonic.Album(), new Common.Models.Subsonic.Album() };
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
            Subject.PlaylistItems.Count.Should().Be(2);
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
            Subject.PlaylistItems.Count.Should().Be(2);
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
            Subject.PlaylistItems.Count.Should().Be(2);
        }

        [TestMethod]
        public async Task HandleWithPLaylistWhenClearCurrentIsSetToTrueSetsSourceOnPlaybacViewModelToNull()
        {
            Subject.Source = new Uri("http://this-will-be.null");

            await Task.Run(() => Subject.Handle(new PlaylistMessage { ClearCurrent = true }));

            Subject.Source.Should().BeNull();
        }

        [TestMethod]
        public async Task HandleWithPlayFileShouldSetSourceOnShellViewModel()
        {
            MockLoadModel();

            await Task.Run(() => Subject.Handle(new PlayFile { Model = new Song { IsVideo = false } }));

            MockShellViewModel.Source.OriginalString.Should().Be("http://something");
        }

        [TestMethod]
        public async Task HandleWithPlayFileOfTypeSongShouldSetSourceOnShellViewModelToNewUriAndSourceOnPlaybackViewModelToNull()
        {
            MockLoadModel();
            Subject.Source = new Uri("http://this-should-become.null");

            await Task.Run(() => Subject.Handle(new PlayFile { Model = new Song { IsVideo = false } }));

            Subject.Source.Should().BeNull();
            MockShellViewModel.Source.Should().NotBeNull();
        }

        [TestMethod]
        public async Task HandleWithPlayfileShouldAddItemToPlaylist()
        {
            var item = MockLoadModel();

            await Task.Run(() => Subject.Handle(new PlayFile { Model = new Song() }));

            Subject.PlaylistItems.Select(pi => pi.Item).Should().Contain(item);
        }

        [TestMethod]
        public async Task HandleWithPlayfileShouldCallStart()
        {
            var called = false;
            Subject.StartAction = item => { called = true; };
            var model = new Song();

            await Task.Run(() => Subject.Handle(new PlayFile { Model = model }));

            called.Should().BeTrue();
        }

        [TestMethod]
        public void PlaylistItemsWhenChangedFromEmptyToNotEmptyCallsEventAggregatorPublishWithShowControlsMessage()
        {
            Subject.PlaylistItems.Add(new PlaylistItemViewModel());

            _mockEventAggregator.PublishCallCount.Should().Be(1);
            _mockEventAggregator.Messages.First().GetType().Should().Be(typeof(ShowControlsMessage));
        }

        [TestMethod]
        public void PLaylistItemsWhenChangedFromNotEmptyToNotEmptyCallsEventAggregatorPublishWithShowControlsMessage()
        {
            Subject.PlaylistItems.Add(new PlaylistItemViewModel());
            Subject.PlaylistItems.Add(new PlaylistItemViewModel());

            Subject.PlaylistItems.Clear();

            _mockEventAggregator.PublishCallCount.Should().Be(2);
            _mockEventAggregator.Messages.ElementAt(1).GetType().Should().Be(typeof(ShowControlsMessage));
        }

        [TestMethod]
        public void Ctor_Always_SetsShuffleOnFalse()
        {
            Subject.ShuffleOn.Should().BeFalse();
        }

        [TestMethod]
        public void HandleToggleShuffle_ShuffleOnFalse_SetsShuffleOnTrue()
        {
            Subject.Handle(new ToggleShuffleMessage());

            Subject.ShuffleOn.Should().BeTrue();
        }

        [TestMethod]
        public void HandleToggleShuffle_ShuffleOnTrue_SetsShuffleOnFalse()
        {
            //Set it to true
            Subject.Handle(new ToggleShuffleMessage());

            Subject.Handle(new ToggleShuffleMessage());

            Subject.ShuffleOn.Should().BeFalse();
        }

        [TestMethod]
        public void ClearPlaylist_Always_ClearsAllItemsFromThePlaylist()
        {
            Subject.PlaylistItems.Add(new PlaylistItemViewModel());
            Subject.PlaylistItems.Add(new PlaylistItemViewModel());

            Subject.ClearPlaylist();

            Subject.PlaylistItems.Count.Should().Be(0);
        }

        [TestMethod]
        public void OnActivate_Always_SetsBottomBarIsOpenToFalse()
        {
            Subject.BottomBar.IsOpened = true;

            Subject.Activate();

            Subject.BottomBar.IsOpened.Should().BeFalse();
        }

        public override void OnActivateShouldSetBottomBarIsOnPlaylistToFalse()
        {
            Subject.BottomBar.IsOnPlaylist = false;

            Subject.Activate();

            Subject.BottomBar.IsOnPlaylist.Should().BeTrue();
        }

        private ISubsonicModel MockLoadModel(bool isVideo = false)
        {
            var item = new Song
                {
                    IsVideo = isVideo
                };
            Subject.LoadModel = model =>
                {
                    var tcr = new TaskCompletionSource<PlaylistItemViewModel>();
                    tcr.SetResult(new PlaylistItemViewModel
                        {
                            Item = item,
                            PlayingState = PlaylistItemState.NotPlaying,
                            Uri = new Uri("http://something")
                        });
                    return tcr.Task;
                };

            return item;
        }

        private List<PlaylistItemViewModel> GeneratePlaylist(int itemsCount = 2)
        {
            var playlistItemViewModels = new List<PlaylistItemViewModel>();
            for (var i = 0; i < itemsCount; i++)
            {
                playlistItemViewModels.Add(new PlaylistItemViewModel { Uri = new Uri(string.Format("http://file{0}", i)), Item = new Song { IsVideo = false } });
            }

            return playlistItemViewModels;
        }
    }
}
