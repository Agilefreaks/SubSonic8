using System;
using Client.Common;
using FluentAssertions;
using Microsoft.VisualStudio.TestPlatform.UnitTestFramework;
using Subsonic8.Messages;
using Subsonic8.Shell;

namespace Client.Tests.Shell
{
    [TestClass]
    public class ShellViewModelTests
    {
        private IShellViewModel _shellViewModel;

        [TestInitialize]
        public void TestInitialize()
        {
            _shellViewModel = new ShellViewModel { SubsonicService = new MockSubsonicService() };
        }

        [TestMethod]
        public void HandleWithPlayFileShouldSetSource()
        {
            _shellViewModel.Handle(new PlayFile { Id = 42 });
            _shellViewModel.Source.OriginalString.Should().Be("http://subsonic.org?id=42");
        }

        #region MockSubsonicService

        internal class MockSubsonicService : SubsonicService
        {
            public override Uri GetUriForFileWithId(int id)
            {
                return new Uri(string.Format("http://subsonic.org?id={0}", id));
            }
        }

        #endregion
    }
}