using Client.Common.Models.Subsonic;
using Client.Tests.Framework.ViewModel;
using Microsoft.VisualStudio.TestPlatform.UnitTestFramework;
using Subsonic8.Artist;

namespace Client.Tests.Artist
{
    [TestClass]
    public class ArtistViewModelTests : DetailViewModelBaseTests<ExpandedArtist, IArtistViewModel>
    {
        protected override IArtistViewModel Subject { get; set; }

        protected override void TestInitializeExtensions()
        {
            Subject = new ArtistViewModel
                          {
                              UpdateDisplayName = () => Subject.DisplayName = ""
                          };
        }
    }
}
