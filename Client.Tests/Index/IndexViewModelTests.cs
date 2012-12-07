using System.Collections.Generic;
using Client.Common.Models.Subsonic;
using FluentAssertions;
using Microsoft.VisualStudio.TestPlatform.UnitTestFramework;
using Subsonic8.Index;

namespace Client.Tests.Index
{
    [TestClass]
    public class IndexViewModelTests : ClientTestBase
    {
        private IIndexViewModel _subject;

        [TestInitialize]
        public void TestInitialize()
        {
            _subject = new IndexViewModel();
        }

        [TestMethod]
        public void CtorShouldSetMenuItems()
        {
            _subject.MenuItems.Should().BeEmpty();
        }

        [TestMethod]
        public void ParameterSetShouldPopulateMenuItems()
        {
            _subject.Parameter = new IndexItem { Artists = new List<Artist> { new Artist(), new Artist() } };

            _subject.MenuItems.Should().HaveCount(2);
        }
    }
}