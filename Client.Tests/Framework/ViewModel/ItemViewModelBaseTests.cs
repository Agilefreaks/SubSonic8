namespace Client.Tests.Framework.ViewModel
{
    using Caliburn.Micro;
    using Client.Common.Services;
    using FluentAssertions;
    using global::Common.Mocks;
    using Microsoft.VisualStudio.TestPlatform.UnitTestFramework;
    using Subsonic8.Framework.ViewModel;

    [TestClass]
    public abstract class ItemViewModelBaseTests<TViewModel> : ClientTestBase
        where TViewModel : ItemViewModelBase
    {
        #region Properties

        protected TViewModel Subject { get; set; }

        #endregion

        #region Public Methods and Operators

        [TestMethod]
        public void CoverArtShouldCallSubsonicServiceGetCoverArtForId()
        {
            var subsonicService = new MockSubsonicService();
            IoC.GetInstance = (type, s) => type == typeof(ISubsonicService) ? subsonicService : null;

            var coverArt = Subject.CoverArt;

            coverArt.Should().Be("http://test.mock");
            subsonicService.GetCoverArtForIdCallCount.Should().Be(1);
        }

        #endregion
    }
}