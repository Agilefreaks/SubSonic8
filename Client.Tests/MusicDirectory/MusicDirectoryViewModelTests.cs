using System.Collections.Generic;
using Caliburn.Micro;
using Client.Common.Models.Subsonic;
using FluentAssertions;
using Microsoft.VisualStudio.TestPlatform.UnitTestFramework;
using Subsonic8.MusicDirectory;

namespace Client.Tests.MusicDirectory
{
    [TestClass]
    public class MusicDirectoryViewModelTests
    {
        private IMusicDirectoryViewModel _subject;

        [TestInitialize]
        public void TestInitialize()
        {
            IoC.GetInstance = (type, s) => null;

            _subject = new MusicDirectoryViewModel();
        }

        [TestMethod]
        public void CtorShouldSetMenuItems()
        {
            _subject.MenuItems.Should().BeEmpty();
        }

        [TestMethod]
        public void ParameterSetShouldPopulateMenuItems()
        {
            _subject.Item = new Common.Models.Subsonic.MusicDirectory { Children = new List<MusicDirectoryChild> { new MusicDirectoryChild(), new MusicDirectoryChild() } };

            _subject.MenuItems.Should().HaveCount(2);
        }
    }
}