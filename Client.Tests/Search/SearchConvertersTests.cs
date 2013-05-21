namespace Client.Tests.Search
{
    using Caliburn.Micro;
    using Client.Common.Models;
    using Client.Common.Models.Subsonic;
    using FluentAssertions;
    using Microsoft.VisualStudio.TestPlatform.UnitTestFramework;
    using Subsonic8.Framework.Extensions;
    using Subsonic8.MenuItem;

    [TestClass]
    public class SearchConvertersTests
    {
        #region Fields

        private Album _album;

        private MenuItemViewModel _albumMenuItemViewModel;

        private ISubsonicModel _albumMusicDirectoryChild;

        private MenuItemViewModel _artistMenuItemViewModel;

        private ISubsonicModel _artistMusicDirectoryChild;

        private ExpandedArtist _expandedArtist;

        private Song _song;

        private MenuItemViewModel _songMenuItemViewModel;

        private ISubsonicModel _songMusicDirectoryChild;

        #endregion

        #region Public Methods and Operators

        [TestMethod]
        public void AlbumAsMenuItemViewModelPropertyItemShouldBeOfTypeMusicDirectoryChild()
        {
            _albumMusicDirectoryChild.Should().NotBeNull();
        }

        [TestMethod]
        public void AlbumAsMenuItemViewModelPropertyItemShouldHaveIdSetToAlbumId()
        {
            _albumMusicDirectoryChild.Id.Should().Be(_album.Id);
        }

        [TestMethod]
        public void AlbumAsMenuItemViewModelPropertyItemShouldHaveIsDirectorySetToTrue()
        {
            _albumMusicDirectoryChild.Type.Should().Be(SubsonicModelTypeEnum.Album);
        }

        [TestMethod]
        public void AlbumAsMenuItemViewModelShouldSetSubtitlePropertyOnMenuItemViewModel()
        {
            _albumMenuItemViewModel.Subtitle.Should().NotBeNullOrEmpty();
        }

        [TestMethod]
        public void AlbumAsMenuItemViewModelShouldSetTitlePropertyOnMenuItemViewModel()
        {
            _albumMenuItemViewModel.Title.Should().Be(_album.Name);
        }

        [TestMethod]
        public void AlbumAsMenuItemViewModelShouldSetTypePropertyOnMenuItemViewModel()
        {
            _albumMenuItemViewModel.Type.Should().Be("Album(s)");
        }

        [TestMethod]
        public void ExpandedArtistAsMenuItemViewModelPropertyItemShouldBeOfTypeMusicDirectoryChild()
        {
            _artistMusicDirectoryChild.Should().NotBeNull();
        }

        [TestMethod]
        public void ExpandedArtistAsMenuItemViewModelPropertyItemShouldHaveIdSetToArtistId()
        {
            _artistMusicDirectoryChild.Id.Should().Be(_expandedArtist.Id);
        }

        [TestMethod]
        public void ExpandedArtistAsMenuItemViewModelPropertyItemShouldHaveIsDirectorySetToTrue()
        {
            _artistMusicDirectoryChild.Type.Should().Be(SubsonicModelTypeEnum.Artist);
        }

        [TestMethod]
        public void ExpandedArtistAsMenuItemViewModelShouldSetSubtitlePropertyOnMenuItemViewModel()
        {
            _artistMenuItemViewModel.Subtitle.Should().NotBeNullOrEmpty();
        }

        [TestMethod]
        public void ExpandedArtistAsMenuItemViewModelShouldSetTitlePropertyOnMenuItemViewModel()
        {
            _artistMenuItemViewModel.Title.Should().Be(_expandedArtist.Name);
        }

        [TestMethod]
        public void ExpandedArtistAsMenuItemViewModelShouldSetTypePropertyOnMenuItemViewModel()
        {
            _artistMenuItemViewModel.Type.Should().Be("Artist(s)");
        }

        [TestMethod]
        public void SongAsMenuItemViewModelPropertyItemShouldBeOfTypeMusicDirectoryChild()
        {
            _songMusicDirectoryChild.Should().NotBeNull();
        }

        [TestMethod]
        public void SongAsMenuItemViewModelPropertyItemShouldHaveIdSetToSongId()
        {
            _songMusicDirectoryChild.Id.Should().Be(_song.Id);
        }

        [TestMethod]
        public void SongAsMenuItemViewModelPropertyItemShouldHaveIsDirectorySetToFalse()
        {
            _songMusicDirectoryChild.Type.Should().Be(SubsonicModelTypeEnum.Song);
        }

        [TestMethod]
        public void SongAsMenuItemViewModelShouldSetSubtitlePropertyOnMenuItemViewModel()
        {
            _songMenuItemViewModel.Subtitle.Should().NotBeNullOrEmpty();
        }

        [TestMethod]
        public void SongAsMenuItemViewModelShouldSetTitlePropertyOnMenuItemViewModel()
        {
            _songMenuItemViewModel.Title.Should().Be(_song.Title);
        }

        [TestMethod]
        public void SongAsMenuItemViewModelShouldSetTypePropertyOnMenuItemViewModel()
        {
            _songMenuItemViewModel.Type.Should().Be("Song(s)");
        }

        [TestInitialize]
        public void TestInitialize()
        {
            IoC.GetInstance = (type, s) => null;

            _expandedArtist = new ExpandedArtist { Id = 42, AlbumCount = 12, Name = "artist" };
            _artistMenuItemViewModel = _expandedArtist.AsMenuItemViewModel();
            _artistMusicDirectoryChild = _artistMenuItemViewModel.Item.As<ISubsonicModel>();

            _album = new Album { Id = 24, SongCount = 12, Name = "album" };
            _albumMenuItemViewModel = _album.AsMenuItemViewModel();
            _albumMusicDirectoryChild = _albumMenuItemViewModel.Item.As<ISubsonicModel>();

            _song = new Song { Id = 12, Title = "song", Artist = "artist", Album = "album" };
            _songMenuItemViewModel = _song.AsMenuItemViewModel();
            _songMusicDirectoryChild = _songMenuItemViewModel.Item.As<ISubsonicModel>();
        }

        #endregion
    }
}