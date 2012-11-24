using Client.Common.Models;
using Client.Common.Models.Subsonic;
using FluentAssertions;
using Subsonic8.Framework.Extensions;
using Subsonic8.MenuItem;
using Microsoft.VisualStudio.TestPlatform.UnitTestFramework;

namespace Client.Tests.Search
{
    [TestClass]
    public class SearchConvertersTests
    {
        private ExpandedArtist _expandedArtist;
        private ISubsonicModel _artistMusicDirectoryChild;
        private MenuItemViewModel _artistMenuItemViewModel;

        private Common.Models.Subsonic.Album _album;
        private ISubsonicModel _albumMusicDirectoryChild;
        private MenuItemViewModel _albumMenuItemViewModel;

        private Song _song;
        private ISubsonicModel _songMusicDirectoryChild;
        private MenuItemViewModel _songMenuItemViewModel;

        [TestInitialize]
        public void TestInitialize()
        {
            _expandedArtist = new ExpandedArtist
                                  {
                                      Id = 42,
                                      AlbumCount = 12,
                                      CovertArt = "cover",
                                      Name = "artist"
                                  };
            _artistMenuItemViewModel = _expandedArtist.AsMenuItemViewModel();
            _artistMusicDirectoryChild = _artistMenuItemViewModel.Item.As<ISubsonicModel>();

            _album = new Common.Models.Subsonic.Album
                         {
                             Id = 24,
                             SongCount = 12,
                             Name = "album"
                         };
            _albumMenuItemViewModel = _album.AsMenuItemViewModel();
            _albumMusicDirectoryChild = _albumMenuItemViewModel.Item.As<ISubsonicModel>();

            _song = new Song
                        {
                            Id = 12,
                            Title = "song",
                            Artist = "artist",
                            Album = "album"
                        };
            _songMenuItemViewModel = _song.AsMenuItemViewModel();
            _songMusicDirectoryChild = _songMenuItemViewModel.Item.As<ISubsonicModel>();
        }

        [TestMethod]
        public void ExpandedArtistAsMenuItemViewModelPropertyItemShouldBeOfTypeMusicDirectoryChild()
        {
            _artistMusicDirectoryChild.Should().NotBeNull();
        }

        [TestMethod]
        public void ExpandedArtistAsMenuItemViewModelPropertyItemShouldHaveIsDirectorySetToTrue()
        {
            _artistMusicDirectoryChild.Type.Should().Be(SubsonicModelTypeEnum.Artist);
        }

        [TestMethod]
        public void ExpandedArtistAsMenuItemViewModelPropertyItemShouldHaveIdSetToArtistId()
        {
            _artistMusicDirectoryChild.Id.Should().Be(_expandedArtist.Id);
        }

        [TestMethod]
        public void ExpandedArtistAsMenuItemViewModelShouldSetTitlePropertyOnMenuItemViewModel()
        {
            _artistMenuItemViewModel.Title.Should().Be(_expandedArtist.Name);
        }

        [TestMethod]
        public void ExpandedArtistAsMenuItemViewModelShouldSetSubtitlePropertyOnMenuItemViewModel()
        {
            _artistMenuItemViewModel.Subtitle.Should().NotBeNullOrEmpty();
        }

        [TestMethod]
        public void ExpandedArtistAsMenuItemViewModelShouldSetTypePropertyOnMenuItemViewModel()
        {
            _artistMenuItemViewModel.Type.Should().Be("Artist(s)");
        }

        [TestMethod]
        public void AlbumAsMenuItemViewModelPropertyItemShouldBeOfTypeMusicDirectoryChild()
        {
            _albumMusicDirectoryChild.Should().NotBeNull();
        }

        [TestMethod]
        public void AlbumAsMenuItemViewModelPropertyItemShouldHaveIsDirectorySetToTrue()
        {
            _albumMusicDirectoryChild.Type.Should().Be(SubsonicModelTypeEnum.Album);
        }

        [TestMethod]
        public void AlbumAsMenuItemViewModelPropertyItemShouldHaveIdSetToAlbumId()
        {
            _albumMusicDirectoryChild.Id.Should().Be(_album.Id);
        }

        [TestMethod]
        public void AlbumAsMenuItemViewModelShouldSetTitlePropertyOnMenuItemViewModel()
        {
            _albumMenuItemViewModel.Title.Should().Be(_album.Name);
        }

        [TestMethod]
        public void AlbumAsMenuItemViewModelShouldSetSubtitlePropertyOnMenuItemViewModel()
        {
            _albumMenuItemViewModel.Subtitle.Should().NotBeNullOrEmpty();
        }

        [TestMethod]
        public void AlbumAsMenuItemViewModelShouldSetTypePropertyOnMenuItemViewModel()
        {
            _albumMenuItemViewModel.Type.Should().Be("Album(s)");
        }

        [TestMethod]
        public void SongAsMenuItemViewModelPropertyItemShouldBeOfTypeMusicDirectoryChild()
        {
            _songMusicDirectoryChild.Should().NotBeNull();
        }

        [TestMethod]
        public void SongAsMenuItemViewModelPropertyItemShouldHaveIsDirectorySetToFalse()
        {
            _songMusicDirectoryChild.Type.Should().Be(SubsonicModelTypeEnum.Song);
        }

        [TestMethod]
        public void SongAsMenuItemViewModelPropertyItemShouldHaveIdSetToSongId()
        {
            _songMusicDirectoryChild.Id.Should().Be(_song.Id);
        }

        [TestMethod]
        public void SongAsMenuItemViewModelShouldSetTitlePropertyOnMenuItemViewModel()
        {
            _songMenuItemViewModel.Title.Should().Be(_song.Title);
        }

        [TestMethod]
        public void SongAsMenuItemViewModelShouldSetSubtitlePropertyOnMenuItemViewModel()
        {
            _songMenuItemViewModel.Subtitle.Should().NotBeNullOrEmpty();
        }

        [TestMethod]
        public void SongAsMenuItemViewModelShouldSetTypePropertyOnMenuItemViewModel()
        {
            _songMenuItemViewModel.Type.Should().Be("Song(s)");
        }
    }
}