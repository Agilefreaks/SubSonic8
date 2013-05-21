namespace Client.Common.Tests.Results
{
    using Client.Common.Results;
    using Client.Common.Services.DataStructures.SubsonicService;
    using FluentAssertions;
    using Microsoft.VisualStudio.TestPlatform.UnitTestFramework;

    [TestClass]
    public class PingResultTests
    {
        #region Fields

        private PingResult _subject;

        #endregion

        #region Public Methods and Operators

        [TestInitialize]
        public void Setup()
        {
            _subject = new PingResult(new SubsonicServiceConfiguration());
        }

        [TestMethod]
        public void ViewName_Always_SouldReturnCorrectViewName()
        {
            _subject.ViewName.Should().Be("ping.view");
        }

        #endregion
    }
}