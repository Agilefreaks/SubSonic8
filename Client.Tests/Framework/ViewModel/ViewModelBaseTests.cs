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
        where TViewModel : IViewModel
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

        [TestMethod]
        public void SelectedItemWhenBottomBarIsNillShouldNotThrowException()
        {
            Subject.BottomBar = null;

            Subject.SelectedItems.Should().NotBeNull();
        }

        [TestMethod]
        public void OnActivateShouldSetDiplayName()
        {
            Subject.UpdateDisplayName = () => Subject.DisplayName = "42";

            ((IActivate)Subject).Activate();

            Subject.DisplayName.Should().Be("42");
        }
    }
}