using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using Client.Common.Models;
using Client.Common.Models.Subsonic;
using Client.Tests.Mocks;
using FluentAssertions;
using Microsoft.VisualStudio.TestPlatform.UnitTestFramework;
using Subsonic8.Messages;
using Subsonic8.Playback;
using Subsonic8.Shell;
using Windows.UI.Xaml;

namespace Client.Tests.Playback
{
    [TestClass]
    public class PlaybackViewModelTests : ClientTestBase
    {
        private PlaybackViewModel _subject;
        private MockEventAggregator _eventAggregator;
        private MockSubsonicService _subsonicService;
        private MockNavigationService _navigationService;
        private ShellViewModel _shellViewModel;
        private MockPlayerControls _playerControls;

        [TestInitialize]
        public void TestInitialize()
        {
            _eventAggregator = new MockEventAggregator();
            _subsonicService = new MockSubsonicService();
            _navigationService = new MockNavigationService();
            _playerControls = new MockPlayerControls();
            _shellViewModel = new ShellViewModel(_eventAggregator, _subsonicService, _navigationService)
                                  {
                                      PlayerControls = _playerControls
                                  };

            _subject = new PlaybackViewModel(_eventAggregator, _shellViewModel, _subsonicService)
                           {
                               NavigationService = _navigationService,
                               SubsonicService = _subsonicService
                           };
        }

        [TestMethod]
        public void CtorShouldInitializePlaylistItems()
        {
            _subject.PlaylistItems.Should().NotBeNull();
        }

        [TestMethod]
        public void ParameterWhenSetToTypeVideoShouldSetSourceOnShellViewModelToNullAndSourceOnPlaybackViewModelToNewUri()
        {
            _shellViewModel.Source = new Uri("http://this-should-become.null");
            _subject.Parameter = new Common.Models.Subsonic.Album { Id = 42 };

            _subject.Source.Should().NotBeNull();
            _shellViewModel.Source.Should().BeNull();
        }

        [TestMethod]
        public void HandleWithPlayFileShouldSetSourceOnShellViewModel()
        {
            _subject.Handle(new PlayFile { Id = 42 });
            _shellViewModel.Source.OriginalString.Should().Be("http://subsonic.org?id=42");
        }

        [TestMethod]
        public void HandleWithPlayFileOfTypeSongShouldSetSourceOnShellViewModelToNewUriAndSourceOnPlaybackViewModelToNull()
        {
            _subject.Source = new Uri("http://this-should-become.null");
            _subject.Handle(new PlayFile { Id = 42 });

            _subject.Source.Should().BeNull();
            _shellViewModel.Source.Should().NotBeNull();
        }

        [TestMethod]
        public void HandleWithPlaylistShouldAddFilesInQueueToPlaylist()
        {
            _subject.Handle(new PlaylistMessage { Queue = new List<ISubsonicModel> { new Song(), new Song() } });

            _subject.Playlist.Should().HaveCount(2);
        }

        [TestMethod]
        public void HandleWithPlaylistShouldKeepElementsInPlaylist()
        {
            _subject.Handle(new PlaylistMessage { Queue = new List<ISubsonicModel> { new Song(), new Song() } });
            _subject.Handle(new PlaylistMessage { Queue = new List<ISubsonicModel> { new Song() } });

            _subject.Playlist.Should().HaveCount(3);
        }

        [TestMethod]
        public void HandleWithPlaylistMessageWithSubsonicModelsWithTypeSongShouldCallSubsonicServiceGetUriForFileWithId()
        {
            _subject.Handle(new PlaylistMessage { Queue = new List<ISubsonicModel> { new Song { Id = 42 } } });

            _subsonicService.GetUriForFileWithIdCallCount.Should().Be(1);
        }

        [TestMethod]
        public void HandleWithPlaylistMessageWithSunsonicModelsOfTypeVideoShouldCallSubsonicServiceGetUriForVideoWithId()
        {
            _subject.Handle(new PlaylistMessage { Queue = new List<ISubsonicModel> { new Song { IsVideo = true } } });

            _subsonicService.GetUriForVideoWithIdCallCount.Should().Be(1);
        }

        [TestMethod]
        public void HandleWithPlaylistMessageWithSubsonicModelOfAnyTypeVideoShouldAddNewItemsInPlalistItemsCollection()
        {
            _subject.Handle(new PlaylistMessage
                                {
                                    Queue = new List<ISubsonicModel>
                                                {
                                                    new Song { Id=  41 },
                                                    new Song { Id = 42, IsVideo = true }
                                                }
                                });

            _subject.PlaylistItems.Should().HaveCount(2);
        }

        [TestMethod]
        public void HandleWithPlayNextMessageShouldSetSourceOnShellViewModelToSecondElementInPlaylist()
        {
            _subject.Playlist = new ObservableCollection<ISubsonicModel> { new Song { Id = 1 }, new Song { Id = 2 } };

            _subject.Handle(new PlayNextMessage());

            var newUri = _subsonicService.GetUriForFileWithId(2);
            _shellViewModel.Source.AbsoluteUri.Should().Be(newUri.AbsoluteUri);
        }

        [TestMethod]
        public void HandleWithPlayNextMessageIfCurrentTrackIsLastShouldSetShellViewModelSourceToNull()
        {
            _subject.Playlist = new ObservableCollection<ISubsonicModel> { new Song { Id = 1 } };

            _subject.Handle(new PlayNextMessage());

            _shellViewModel.Source.Should().BeNull();
        }

        [TestMethod]
        public void HandleWithPlayPreviousMessageShouldSetSourceOnShellViewModelToPreviousElementInPlaylist()
        {
            _subject.Playlist = new ObservableCollection<ISubsonicModel> { new Song { Id = 1 }, new Song { Id = 2 } };
            _subject.Handle(new PlayNextMessage());

            _subject.Handle(new PlayPreviousMessage());

            var newUri = _subsonicService.GetUriForFileWithId(1);
            _shellViewModel.Source.AbsoluteUri.Should().Be(newUri.AbsoluteUri);
        }

        [TestMethod]
        public void HandleWithPlayPreviousMessageIfCurrentTrackIsFirstShouldSetShellViewModelSourceToNull()
        {
            _subject.Playlist = new ObservableCollection<ISubsonicModel> { new Song { Id = 1 } };

            _subject.Handle(new PlayPreviousMessage());

            _shellViewModel.Source.Should().BeNull();
        }

        [TestMethod]
        public void HandleWithStopMessageShouldSetSourceOnShellToNull()
        {
            _subject.Handle(new StopMessage());

            _shellViewModel.Source.Should().BeNull();
        }

        [TestMethod]
        public void HandleWithPlayPauseMessageIfShellViewModelSourceIsNotNullCallsPlayPauseOnIPlayerControls()
        {
            _shellViewModel.Source = new Uri("http://test.t");

            _subject.Handle(new PlayPauseMessage());

            _playerControls.PlayPauseCallCount.Should().Be(1);
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
