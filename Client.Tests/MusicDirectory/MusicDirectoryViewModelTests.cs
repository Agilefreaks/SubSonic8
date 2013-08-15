namespace Client.Tests.MusicDirectory
{
    using Client.Common.Models.Subsonic;
    using Client.Tests.Framework.ViewModel;
    using FluentAssertions;
    using Microsoft.VisualStudio.TestPlatform.UnitTestFramework;
    using Subsonic8.MusicDirectory;

    [TestClass]
    public class MusicDirectoryViewModelTests : DetailViewModelBaseTests<MusicDirectory, MusicDirectoryViewModel>
    {
        #region Properties

        protected override MusicDirectoryViewModel Subject { get; set; }

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