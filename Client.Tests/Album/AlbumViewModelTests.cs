namespace Client.Tests.Album
{
    using Client.Common.Models.Subsonic;
    using Client.Tests.Framework.ViewModel;
    using FluentAssertions;
    using Microsoft.VisualStudio.TestPlatform.UnitTestFramework;
    using Subsonic8.Album;

    [TestClass]
    public class AlbumViewModelTests : DetailViewModelBaseTests<Album, AlbumViewModel>
    {
        #region Properties

        protected override AlbumViewModel Subject { get; set; }

        #endregion

        #region Public Methods and Operators

        [TestMethod]
        public void CtroShouldInstantiateMenuItems()
        {
            Subject.MenuItems.Should().NotBeNull();
        }

        #endregion
    }
}