using Caliburn.Micro;
using Client.Tests.Mocks;
using FluentAssertions;
using Microsoft.VisualStudio.TestPlatform.UnitTestFramework;
using Subsonic8.BottomBar;
using Subsonic8.Framework.ViewModel;

namespace Client.Tests.Framework.ViewModel
{
    [TestClass]
    public abstract class ViewModelBaseTests<TViewModel> : ClientTestBase
        where TViewModel : ViewModelBase
    {
        protected abstract TViewModel Subject { get; set; }

        [TestMethod]
        public void OnActivateShouldSetDefaultBottomBar()
        {
            var defaultBottomBar = new MockDefaultBottomBarViewModel();
            var mockShellViewMode = new MockShellViewModel();
            IoC.GetInstance = (type, s) => type == typeof (IDefaultBottomBarViewModel) ? (object) defaultBottomBar : mockShellViewMode;

            ((IActivate)Subject).Activate();

            Subject.BottomBar.Should().Be(defaultBottomBar);
        }
    }
}