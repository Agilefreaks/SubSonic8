using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
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

namespace Client.Tests.Playback
{
    [TestClass]
    public class PlaybackViewModelTests : ViewModelBaseTests<IPlaybackViewModel>
    {
        protected override IPlaybackViewModel Subject { get; set; }

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

            Subject = new PlaybackViewModel(_eventAggregator, _shellViewModel, _subsonicService)
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
        public void ParameterWhenSetToTypeVideoShouldSetSourceOnShellViewModelToNullAndSourceOnPlaybackViewModelToNewUri
            ()
        {
            _shellViewModel.Source = new Uri("http://this-should-become.null");
            Subject.Parameter = new Common.Models.Subsonic.Album { Id = 42 };

            Subject.Source.Should().NotBeNull();
            _shellViewModel.Source.Should().BeNull();
        }

        [TestMethod]
        public void HandleWithPlayFileShouldSetSourceOnShellViewModel()
        {
            Subject.Handle(new PlayFile { Id = 42 });
            _shellViewModel.Source.OriginalString.Should().Be("http://subsonic.org?id=42");
        }

        [TestMethod]
        public void
            HandleWithPlayFileOfTypeSongShouldSetSourceOnShellViewModelToNewUriAndSourceOnPlaybackViewModelToNull()
        {
            Subject.Source = new Uri("http://this-should-become.null");
            Subject.Handle(new PlayFile { Id = 42 });

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
        public void HandleWithPlayNextMessageShouldSetSourceOnShellViewModelToSecondElementInPlaylist()
        {
            var file1 = new PlaylistItemViewModel { Uri = new Uri("http://file1") };
            var file2 = new PlaylistItemViewModel { Uri = new Uri("http://file2") };
            Subject.PlaylistItems = new ObservableCollection<PlaylistItemViewModel> { file1, file2 };

            Subject.Handle(new PlayNextMessage());

            _shellViewModel.Source.Should().Be(file2.Uri);
        }

        [TestMethod]
        public void HandleWithPlayNextMessageIfCurrentTrackIsLastShouldSetShellViewModelSourceToNull()
        {
            var uri = new Uri("http://test");
            Subject.PlaylistItems = new ObservableCollection<PlaylistItemViewModel> { new PlaylistItemViewModel { Uri = uri } };

            Subject.Handle(new PlayNextMessage());

            _shellViewModel.Source.Should().BeNull();
        }

        [TestMethod]
        public void HandleWithPlayPreviousMessageShouldSetSourceOnShellViewModelToPreviousElementInPlaylist()
        {
            var file1 = new PlaylistItemViewModel { Uri = new Uri("http://file1") };
            var file2 = new PlaylistItemViewModel { Uri = new Uri("http://file2") };
            Subject.PlaylistItems = new ObservableCollection<PlaylistItemViewModel> { file1, file2 };

            Subject.Handle(new PlayNextMessage());
            Subject.Handle(new PlayPreviousMessage());

            _shellViewModel.Source.Should().Be(file1.Uri);
        }

        [TestMethod]
        public void HandleWithPlayPreviousMessageIfCurrentTrackIsFirstShouldSetShellViewModelSourceToNull()
        {
            Subject.PlaylistItems = new ObservableCollection<PlaylistItemViewModel> { new PlaylistItemViewModel{ Uri = new Uri("http://test")} };

            Subject.Handle(new PlayPreviousMessage());

            _shellViewModel.Source.Should().BeNull();
        }

        [TestMethod]
        public void HandleWithStopMessageShouldSetSourceOnShellToNull()
        {
            Subject.Handle(new StopMessage());

            _shellViewModel.Source.Should().BeNull();
        }

        [TestMethod]
        public void HandleWithPlayPauseMessageIfShellViewModelSourceIsNotNullCallsPlayPauseOnIPlayerControls()
        {
            _shellViewModel.Source = new Uri("http://test.t");

            Subject.Handle(new PlayPauseMessage());

            _playerControls.PlayPauseCallCount.Should().Be(1);
        }

        [TestMethod]
        public void HandleWithRemoveFromPlaylistMessageShouldItemsInQueueFromCurrentPlaylist()
        {
            var playlistItemViewModel = new PlaylistItemViewModel();
            Subject.PlaylistItems.Add(playlistItemViewModel);

            Subject.Handle(new RemoveFromPlaylistMessage { Queue = new List<PlaylistItemViewModel> { playlistItemViewModel } });

            Subject.PlaylistItems.Should().HaveCount(0);
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
