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

namespace Client.Tests.Playback
{
    [TestClass]
    public class PlaybackViewModelTests
    {
        private PlaybackViewModel _subject;
        private MockEventAggregator _eventAggregator;
        private MockSubsonicService _subsonicService;
        private MockNavigationService _navigationService;
        private ShellViewModel _shellViewModel;

        [TestInitialize]
        public void TestInitialize()
        {
            _eventAggregator = new MockEventAggregator();
            _subsonicService = new MockSubsonicService();
            _navigationService = new MockNavigationService();
            _shellViewModel = new ShellViewModel(_eventAggregator, _subsonicService, _navigationService);
            _subject = new PlaybackViewModel(_eventAggregator, _shellViewModel, _subsonicService)
                           {
                               NavigationService = _navigationService,
                               SubsonicService = _subsonicService
                           };
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
        public void HandleWithMediaEndedMessageShouldSetSourceOnShellViewModelToSecondElementInPlaylist()
        {
            _subject.Playlist = new ObservableCollection<ISubsonicModel> { new Song { Id = 1 }, new Song { Id = 2 } };

            _subject.Handle(new MediaEndedMessage());

            var newUri = _subsonicService.GetUriForFileWithId(2);
            _shellViewModel.Source.AbsoluteUri.Should().Be(newUri.AbsoluteUri);
        }

        [TestMethod]
        public void HandleWithMediaEndedMessageIfCurrentTrackIsLastShouldSetShellViewModelSourceToNull()
        {
            _subject.Playlist = new ObservableCollection<ISubsonicModel> { new Song { Id = 1 } };

            _subject.Handle(new MediaEndedMessage());

            _shellViewModel.Source.Should().BeNull();
        }
    }
}
