using Client.Tests.Mocks;
using FluentAssertions;
using Microsoft.VisualStudio.TestPlatform.UnitTestFramework;
using Subsonic8.Framework.ViewModel;

namespace Client.Tests.Framework.ViewModel
{
    [TestClass]
    public abstract class CollectionViewModelBaseTests<TViewModel, TParameter> : ViewModelBaseTests<TViewModel>
        where TViewModel : ICollectionViewModel<TParameter>, new()
    {
        protected MockDefaultBottomBarViewModel MockDefaultBottomBar;

        protected override void TestInitializeExtensions()
        {
            MockDefaultBottomBar = new MockDefaultBottomBarViewModel();
            Subject.BottomBar = MockDefaultBottomBar;
        }

        [TestMethod]
        public void SelectedItemWhenBottomBarIsNillShouldNotThrowException()
        {
            Subject.BottomBar = null;

            Subject.SelectedItems.Should().NotBeNull();
        }
    }
}