using Client.Common.Models.Subsonic;
using Client.Tests.Framework.ViewModel;
using Client.Tests.Mocks;
using Microsoft.VisualStudio.TestPlatform.UnitTestFramework;
using Subsonic8.Artist;

namespace Client.Tests.Artist
{
    [TestClass]
    public class ArtistViewModelTests : DetailViewModelBaseTests<ExpandedArtist, IArtistViewModel>
    {
        protected override IArtistViewModel Subject { get; set; }

        [TestInitialize]
        public void TestInitialize()
        {
            Subject = new ArtistViewModel();
            Subject.SubsonicService = new MockSubsonicService();
        }
    }
}
