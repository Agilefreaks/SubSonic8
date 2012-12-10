using System.Collections.Generic;
using Caliburn.Micro;
using Client.Common.Models.Subsonic;
using Client.Common.Services;
using Client.Tests.Framework.ViewModel;
using Client.Tests.Mocks;
using FluentAssertions;
using Microsoft.VisualStudio.TestPlatform.UnitTestFramework;
using Subsonic8.Album;

namespace Client.Tests.Album
{
    [TestClass]
    public class AlbumViewModelTests : ViewModelBaseTests<IAlbumViewModel>
    {
        protected override IAlbumViewModel Subject { get; set; }

        private INavigationService _navigationService;
        private ISubsonicService _subsonicService;

        [TestInitialize]
        public void TestInitialize()
        {
            _navigationService = new MockNavigationService();
            _subsonicService = new MockSubsonicService();
            Subject = new AlbumViewModel { NavigationService = _navigationService, SubsonicService = _subsonicService };
        }

        [TestMethod]
        public void CtroShouldInstantiateMenuItems()
        {
            Subject.MenuItems.Should().NotBeNull();
        }

        [TestMethod]
        public void AlbumWhenSetPopulatesMenuItemsWithElementsFromSongsProperty()
        {
            var album = new Common.Models.Subsonic.Album { Songs = new List<Song> { new Song(), new Song() } };

            Subject.Item = album;

            Subject.MenuItems.Should().HaveCount(2);
        }
    }
}
