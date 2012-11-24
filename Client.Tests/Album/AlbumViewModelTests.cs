using System.Collections.Generic;
using Caliburn.Micro;
using Client.Common;
using Client.Common.Models.Subsonic;
using Client.Common.Results;
using Client.Tests.Mocks;
using FluentAssertions;
using Microsoft.VisualStudio.TestPlatform.UnitTestFramework;
using Subsonic8.Album;

namespace Client.Tests.Album
{
    [TestClass]
    public class AlbumViewModelTests
    {
        private IAlbumViewModel _subject;
        private INavigationService _navigationService;
        private ISubsonicService _subsonicService;

        [TestInitialize]
        public void TestInitialize()
        {
            _navigationService = new MockNavigationService();
            _subsonicService = new SubsonicService();
            _subject = new AlbumViewModel { NavigationService = _navigationService, SubsonicService = _subsonicService };
        }

        [TestMethod]
        public void CtroShouldInstantiateMenuItems()
        {
            _subject.MenuItems.Should().NotBeNull();
        }

        [TestMethod]
        public void AlbumWhenSetPopulatesMenuItemsWithElementsFromSongsProperty()
        {
            var album = new Common.Models.Subsonic.Album { Songs = new List<Song> { new Song(), new Song() } };

            _subject.Album = album;

            _subject.MenuItems.Should().HaveCount(2);
        }
    }
}
