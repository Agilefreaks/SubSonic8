namespace Client.Tests.Index
{
    using Client.Common.Models.Subsonic;
    using Client.Tests.Framework.ViewModel;
    using FluentAssertions;
    using Microsoft.VisualStudio.TestPlatform.UnitTestFramework;
    using Subsonic8.Index;

    [TestClass]
    public class IndexViewModelTests : DetailViewModelBaseTests<IndexItem, IndexViewModel>
    {
        #region Properties

        protected override IndexViewModel Subject { get; set; }

        #endregion

        #region Public Methods and Operators

        [TestMethod]
        public void CtorShouldSetMenuItems()
        {
            Subject.MenuItems.Should().BeEmpty();
        }

        #endregion
    }
}