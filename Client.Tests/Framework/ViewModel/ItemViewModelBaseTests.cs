using Client.Tests.Mocks;
using FluentAssertions;
using Microsoft.VisualStudio.TestPlatform.UnitTestFramework;
using Subsonic8.Framework.ViewModel;

namespace Client.Tests.Framework.ViewModel
{
    [TestClass]
    public class ItemViewModelBaseTests<TViewModel> : ClientTestBase
        where TViewModel : ItemViewModelBase
    {
        protected TViewModel Subject;

        [TestMethod]
        public void CoverArtShouldCallSubsonicServiceGetCoverArtForId()
        {
            var subsonicService = new MockSubsonicService();
            Subject.SubsonicService = subsonicService;

            var coverArt = Subject.CoverArt;

            subsonicService.GetCoverArtForIdCallCount.Should().Be(1);
        }

        [TestMethod]
        public void CoverArtWhenSetToNullShouldReturnCoverArtPlaceholder()
        {
            Subject.CoverArt = null;

            Subject.CoverArt.Should().Be(ItemViewModelBase.CoverArtPlaceholder);
        }

        [TestMethod]
        public void CoverArtWhenSetToEmptyStringShouldReturnCoverArtPlaceholder()
        {
            Subject.CoverArt = string.Empty;

            Subject.CoverArt.Should().Be(ItemViewModelBase.CoverArtPlaceholder);
        }
    }
}