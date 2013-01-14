using System.Collections.Generic;
using Client.Common.Models.Subsonic;
using Client.Tests.Framework.ViewModel;
using FluentAssertions;
using Microsoft.VisualStudio.TestPlatform.UnitTestFramework;
using Subsonic8.Album;

namespace Client.Tests.Album
{
    [TestClass]
    public class AlbumViewModelTests : DetailViewModelBaseTests<Common.Models.Subsonic.Album, IAlbumViewModel>
    {
        protected override IAlbumViewModel Subject { get; set; }

        protected override void TestInitializeExtensions()
        {
            Subject = new AlbumViewModel
                {
                    UpdateDisplayName = () => Subject.DisplayName = string.Empty
                };
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
