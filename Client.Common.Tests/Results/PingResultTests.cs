using Client.Common.Results;
using Client.Common.Services.DataStructures.SubsonicService;
using FluentAssertions;
using Microsoft.VisualStudio.TestPlatform.UnitTestFramework;

namespace Client.Common.Tests.Results
{
    [TestClass]
    public class PingResultTests
    {
        PingResult _subject;

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
    }
}