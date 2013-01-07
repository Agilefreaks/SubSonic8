using FluentAssertions;
using Microsoft.VisualStudio.TestPlatform.UnitTestFramework;
using Subsonic8.Framework.Services;

namespace Client.Tests.Framework.Services
{
    [TestClass]
    public class DialogNotificationOptionsTests
    {
        private DialogNotificationOptions _subject;

        [TestInitialize]
        public void TestInitialize()
        {
            _subject = new DialogNotificationOptions();    
        }

        [TestMethod]
        public void CtorShouldInitializePossibleActions()
        {
            _subject.PossibleActions.Should().NotBeNull();
        }

        [TestMethod]
        public void PossibleActionsAfterInitShouldReturnADeafaultAction()
        {
            _subject.PossibleActions.Count.Should().Be(1);
        }
    }
}
