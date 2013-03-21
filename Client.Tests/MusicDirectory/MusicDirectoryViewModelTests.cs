﻿using System.Collections.Generic;
using Client.Common.Models.Subsonic;
using Client.Tests.Framework.ViewModel;
using FluentAssertions;
using Microsoft.VisualStudio.TestPlatform.UnitTestFramework;
using Subsonic8.MusicDirectory;

namespace Client.Tests.MusicDirectory
{
    [TestClass]
    public class MusicDirectoryViewModelTests : DetailViewModelBaseTests<Common.Models.Subsonic.MusicDirectory, MusicDirectoryViewModel>
    {
        protected override MusicDirectoryViewModel Subject { get; set; }

        [TestMethod]
        public void CtorShouldSetMenuItems()
        {
            Subject.MenuItems.Should().BeEmpty();
        }

        [TestMethod]
        public void ParameterSetShouldPopulateMenuItems()
        {
            Subject.Item = new Common.Models.Subsonic.MusicDirectory { Children = new List<MusicDirectoryChild> { new MusicDirectoryChild(), new MusicDirectoryChild() } };

            Subject.MenuItems.Should().HaveCount(2);
        }
    }
}