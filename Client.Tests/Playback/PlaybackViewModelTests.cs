using System.Collections.Generic;
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
            _subject = new PlaybackViewModel(_eventAggregator, _shellViewModel)
                           {
                               NavigationService = _navigationService,
                               SubsonicService = _subsonicService
                           };
        }

        [TestMethod]
        public void HandleWithPlayFileShouldSetSourceOnShellViewModel()
        {
            _subject.Handle(new PlayFile { Id = 42 });
            _shellViewModel.Source.OriginalString.Should().Be("http://subsonic.org?id=42");
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
    }
}
