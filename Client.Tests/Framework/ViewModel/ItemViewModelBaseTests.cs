using Caliburn.Micro;
using Client.Common.Services;
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
            IoC.GetInstance = (type, s) => type == typeof(ISubsonicService) ? subsonicService : null;

            var coverArt = Subject.CoverArt;

            coverArt.Should().Be("http://test.mock");
            subsonicService.GetCoverArtForIdCallCount.Should().Be(1);
        }
    }
}