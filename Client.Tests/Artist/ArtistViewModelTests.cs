namespace Client.Tests.Artist
{
    using Client.Common.Models.Subsonic;
    using Client.Tests.Framework.ViewModel;
    using Microsoft.VisualStudio.TestPlatform.UnitTestFramework;
    using Subsonic8.Artist;

    [TestClass]
    public class ArtistViewModelTests : DetailViewModelBaseTests<ExpandedArtist, ArtistViewModel>
    {
        #region Properties

        protected override ArtistViewModel Subject { get; set; }

        #endregion
    }
}