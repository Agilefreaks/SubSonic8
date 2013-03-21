using System.Collections.Generic;
using Client.Common.Models.Subsonic;
using Client.Tests.Framework.ViewModel;
using FluentAssertions;
using Microsoft.VisualStudio.TestPlatform.UnitTestFramework;
using Subsonic8.Index;

namespace Client.Tests.Index
{
    [TestClass]
    public class IndexViewModelTests : ViewModelBaseTests<IndexViewModel>
    {
        protected override IndexViewModel Subject { get; set; }

        [TestMethod]
        public void CtorShouldSetMenuItems()
        {
            Subject.MenuItems.Should().BeEmpty();
        }

        [TestMethod]
        public void ParameterSetShouldPopulateMenuItems()
        {
            Subject.Parameter = new IndexItem { Artists = new List<Common.Models.Subsonic.Artist> { new Common.Models.Subsonic.Artist(), new Common.Models.Subsonic.Artist() } };

            Subject.MenuItems.Should().HaveCount(2);
        }
    }
}