using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using Client.Common.Models;
using Client.Common.Models.Subsonic;
using Client.Tests.Framework.ViewModel;
using Client.Tests.Mocks;
using FluentAssertions;
using Microsoft.VisualStudio.TestPlatform.UnitTestFramework;
using Subsonic8.Messages;
using Subsonic8.Playback;
using Subsonic8.PlaylistItem;
using Subsonic8.Shell;
using Windows.UI.Xaml;
using Action = System.Action;

namespace Client.Tests.Playback
{
    [TestClass]
    public class PlaybackViewModelTests : ViewModelBaseTests<IPlaybackViewModel>
    {
        protected override IPlaybackViewModel Subject { get; set; }

        private MockEventAggregator _eventAggregator;
        private MockSubsonicService _subsonicService;
        private MockNavigationService _navigationService;
        private IShellViewModel _shellViewModel;
        private MockPlayerControls _playerControls;
        private MockNotificationManager _notificationManager;

        [TestInitialize]
        public void TestInitialize()
        {
            _eventAggregator = new MockEventAggregator();
            _subsonicService = new MockSubsonicService();
            _navigationService = new MockNavigationService();
            _playerControls = new MockPlayerControls();
            _notificationManager = new MockNotificationManager();
            _shellViewModel = new ShellViewModel(_eventAggregator, _subsonicService, _navigationService)
                                  {
                                      PlayerControls = _playerControls
                                  };

            Subject = new PlaybackViewModel(_eventAggregator, _shellViewModel, _subsonicService, _notificationManager)
                          {
                              NavigationService = _navigationService,
                              SubsonicService = _subsonicService
                          };
        }

        [TestMethod]
        public void CtorShouldInitializePlaylistItems()
        {
            Subject.PlaylistItems.Should().NotBeNull();
        }

        [TestMethod]
        public void OnActivateShouldSetDisplayNameToPlaylist()
        {
            Subject.Activate();

            Subject.DisplayName.Should().Be("Playlist");
        }

        [TestMethod]
        public void ParameterWhenSetToTypeVideoShouldSetSourceOnShellViewModelToNullAndSourceOnPlaybackViewModelToNewUri()
        {
            _shellViewModel.Source = new Uri("http://this-should-become.null");
            Subject.Parameter = new Song { Id = 42, IsVideo = true };

            Subject.Source.Should().NotBeNull();
            _shellViewModel.Source.Should().BeNull();
        }

        [TestMethod]
        public void HandleWithPlayFileShouldSetSourceOnShellViewModel()
        {
            Subject.Handle(new PlayFile { Model = new Song { Id = 42, IsVideo = false } });

            _shellViewModel.Source.OriginalString.Should().Be("http://subsonic.org?id=42");
        }

        [TestMethod]
        public void HandleWithPlayFileOfTypeSongShouldSetSourceOnShellViewModelToNewUriAndSourceOnPlaybackViewModelToNull()
        {
            Subject.Source = new Uri("http://this-should-become.null");
            Subject.Handle(new PlayFile { Model = new Song { Id = 42, IsVideo = false } });

            Subject.Source.Should().BeNull();
            _shellViewModel.Source.Should().NotBeNull();
        }

        [TestMethod]
        public void HandleWithPlaylistShouldAddFilesInQueueToPlaylist()
        {
            Subject.Handle(new PlaylistMessage { Queue = new List<ISubsonicModel> { new Song(), new Song() } });

            Subject.PlaylistItems.Should().HaveCount(2);
        }

        [TestMethod]
        public void HandleWithPlaylistShouldKeepElementsInPlaylist()
        {
            Subject.Handle(new PlaylistMessage { Queue = new List<ISubsonicModel> { new Song(), new Song() } });
            Subject.Handle(new PlaylistMessage { Queue = new List<ISubsonicModel> { new Song() } });

            Subject.PlaylistItems.Should().HaveCount(3);
        }

        [TestMethod]
        public void HandleWithPlaylistMessageWithSubsonicModelsWithTypeSongShouldCallSubsonicServiceGetUriForFileWithId()
        {
            Subject.Handle(new PlaylistMessage { Queue = new List<ISubsonicModel> { new Song { Id = 42 } } });

            _subsonicService.GetUriForFileWithIdCallCount.Should().Be(1);
        }

        [TestMethod]
        public void HandleWithPlaylistMessageWithSunsonicModelsOfTypeVideoShouldCallSubsonicServiceGetUriForVideoWithId()
        {
            Subject.Handle(new PlaylistMessage { Queue = new List<ISubsonicModel> { new Song { IsVideo = true } } });

            _subsonicService.GetUriForVideoWithIdCallCount.Should().Be(1);
        }

        [TestMethod]
        public void HandleWithPlaylistMessageWithSubsonicModelOfAnyTypeVideoShouldAddNewItemsInPlalistItemsCollection()
        {
            Subject.Handle(new PlaylistMessage
                               {
                                   Queue = new List<ISubsonicModel>
                                               {
                                                   new Song {Id = 41},
                                                   new Song {Id = 42, IsVideo = true}
                                               }
                               });

            Subject.PlaylistItems.Should().HaveCount(2);
        }

        [TestMethod]
        public void HandleWithPlaylistMessageShouldSetStatePropertyToNotPlayingOnAllItems()
        {
            Subject.Handle(new PlaylistMessage { Queue = new List<ISubsonicModel> { new Song(), new Song(), new Song() } });

            Subject.PlaylistItems.All(pi => pi.PlayingState == PlaylistItemState.NotPlaying).Should().BeTrue();
        }

        [TestMethod]
        public void HandleWithPlaylistMessageShouldSetItemPropertyToAllPlaylistItemViewModels()
        {
            Subject.Handle(new PlaylistMessage { Queue = new List<ISubsonicModel> { new Song(), new Song(), new Song() } });

            Subject.PlaylistItems.All(pi => pi.Item != null).Should().BeTrue();
        }

        [TestMethod]
        public void NextShouldSetSourceOnShellViewModelToSecondElementInPlaylist()
        {
            var file1 = new PlaylistItemViewModel { Uri = new Uri("http://file1"), Item = new Song { IsVideo = false } };
            var file2 = new PlaylistItemViewModel { Uri = new Uri("http://file2"), Item = new Song { IsVideo = false } };
            Subject.PlaylistItems = new ObservableCollection<PlaylistItemViewModel> { file1, file2 };
            Subject.Handle(new PlayNextMessage());

            Subject.Handle(new PlayNextMessage());

            _shellViewModel.Source.Should().Be(file2.Uri);
        }

        [TestMethod]
        public void HandleWithStopMessageShouldSetSourceOnShellToNull()
        {
            Subject.Handle(new StopMessage());

            _shellViewModel.Source.Should().BeNull();
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
        public void NextIfCurrentTrackIsLastShouldSetShellViewModelSourceToNull()
        {
            var uri = new Uri("http://test");
            Subject.PlaylistItems = new ObservableCollection<PlaylistItemViewModel> { new PlaylistItemViewModel { Uri = uri, Item = new Song { IsVideo = false } } };
            Subject.Handle(new PlayNextMessage());

            Subject.Handle(new PlayNextMessage());

            _shellViewModel.Source.Should().BeNull();
        }

        [TestMethod]
        public void NextCallsNotificationManagerShow()
        {
            Subject.PlaylistItems = new ObservableCollection<PlaylistItemViewModel> { new PlaylistItemViewModel { Item = new Song { IsVideo = false } } };

            Subject.Handle(new PlayNextMessage());

            _notificationManager.ShowCallCount.Should().Be(1);
        }

        [TestMethod]
        public void NextCallsStartIfThereAreElementsToPlay()
        {
            var called = false;
            Subject.Start = item => { called = true; };
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

            _shellViewModel.Source.Should().Be(file1.Uri);
        }

        [TestMethod]
        public void PreviousIfCurrentTrackIsFirstShouldSetShellViewModelSourceToNull()
        {
            Subject.PlaylistItems = new ObservableCollection<PlaylistItemViewModel> { new PlaylistItemViewModel { Uri = new Uri("http://test") } };

            Subject.Previous();

            _shellViewModel.Source.Should().BeNull();
        }

        [TestMethod]
        public void PreviousCallsStartIfThereAreElementsToPlay()
        {
            var called = false;
            Subject.Start = item => { called = true; };
            Subject.PlaylistItems.Add(new PlaylistItemViewModel());
            Subject.PlaylistItems.Add(new PlaylistItemViewModel());
            Subject.Play();
            Subject.Next();

            Subject.Previous();

            called.Should().BeTrue();
        }

        [TestMethod]
        public void PlayIfCurrentTrackIsNulShouldSetSourceOnShellViewModelToFirstItemInThePlaylist()
        {
            var uri = new Uri("http://tests.cs");
            Subject.PlaylistItems.Add(new PlaylistItemViewModel { Uri = uri, Item = new Song { IsVideo = false } });

            Subject.Play();

            _shellViewModel.Source.Should().Be(uri);
        }

        [TestMethod]
        public void PlayIfCurrentTrackIsNotNullShouldSetTheSourceOnShellToSame()
        {
            var uri = new Uri("http://tests.cs");
            Subject.PlaylistItems.Add(new PlaylistItemViewModel { Uri = uri, Item = new Song { IsVideo = false } });
            Subject.Play();

            Subject.Play();

            _shellViewModel.Source.Should().Be(uri);
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
            Subject.Start = item => { called = true; };
            Subject.PlaylistItems.Add(new PlaylistItemViewModel());

            Subject.Play();

            called.Should().BeTrue();
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
        public void StopWillCallShellViewModelStop()
        {
            Subject.ShellViewModel = new MockShellViewModel();

            Subject.Stop();

            ((MockShellViewModel)Subject.ShellViewModel).StopCallCount.Should().Be(1);
        }

        [TestMethod]
        public void StartShouldSetUriToShellViewModelSource()
        {
            var uri = new Uri("http://test.cc");

            Subject.Start(new PlaylistItemViewModel { Uri = uri, Item = new Song() });

            Subject.ShellViewModel.Source.Should().Be(uri);
        }

        [TestMethod]
        public void StartWhenItemIsSongSetsCoverArtProperty()
        {
            Subject.Start(new PlaylistItemViewModel { CoverArtId = "42", Item = new Song { IsVideo = false } });

            Subject.CoverArt.Should().NotBeNull();
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

            Subject.Start(song2);

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

            Subject.Start(song3);

            song3.PlayingState.Should().Be(PlaylistItemState.Playing);
        }

        [TestMethod]
        public void StartWhenItemIsVideoSetsSourceToUri()
        {
            var video = new PlaylistItemViewModel { Item = new Song { IsVideo = true } };

            Subject.Start(video);

            Subject.Source.Should().NotBeNull();
        }

        [TestMethod]
        public void StartWhenItemIsVideoSetsShellViewModelSourceToNull()
        {
            var video = new PlaylistItemViewModel { Item = new Song { IsVideo = true } };

            Subject.Start(video);

            Subject.ShellViewModel.Source.Should().BeNull();
        }

        [TestMethod]
        public void StartWhenItemIsVideoSetsStateToVideo()
        {
            Subject.Start(new PlaylistItemViewModel { Item = new Song { IsVideo = true } });

            Subject.State.Should().Be(PlaybackViewModelStateEnum.Video);
        }

        [TestMethod]
        public void StartWhenItemIsSongSetsStateToAudio()
        {
            Subject.Start(new PlaylistItemViewModel { Item = new Song { IsVideo = false } });

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
            
            Subject.Start(new PlaylistItemViewModel { Item = new Song() });

            (mockSubsonicService.GetCoverArtForIdCallCount > 0).Should().BeTrue();
        }
    }

    internal class MockPlayerControls : IPlayerControls
    {
        public event RoutedEventHandler PlayNextClicked;
        public event RoutedEventHandler PlayPreviousClicked;

        public Action PlayPause { get; private set; }

        public int PlayPauseCallCount { get; set; }

        public MockPlayerControls()
        {
            PlayPause = PlayPauseImpl;
        }

        private void PlayPauseImpl()
        {
            PlayPauseCallCount++;
        }
    }
}
