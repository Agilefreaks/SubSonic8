using Client.Common.Results;
using FluentAssertions;
using Microsoft.VisualStudio.TestPlatform.UnitTestFramework;

namespace Client.Common.Tests.Results
{
    [TestClass]
    public class GetRootResultTests
    {
        private IGetRootResult _subject;
        private ISubsonicServiceConfiguration _subsonicServiceConfiguration;

        [TestInitialize]
        public void Setup()
        {
            _subsonicServiceConfiguration = new SubsonicServiceConfiguration();
            _subject = new GetRootResult(_subsonicServiceConfiguration);
        }

        [TestMethod]
        public void CtorShouldSetConfiguration()
        {
            _subject.Configuration.Should().BeSameAs(_subsonicServiceConfiguration);
        }

        [TestMethod]
        public void ExecuteShould()
        {
            
        }
    }
}