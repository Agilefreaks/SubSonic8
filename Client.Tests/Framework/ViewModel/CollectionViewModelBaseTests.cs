namespace Client.Tests.Framework.ViewModel
{
    using Client.Tests.Mocks;
    using FluentAssertions;
    using Microsoft.VisualStudio.TestPlatform.UnitTestFramework;
    using Subsonic8.Framework.ViewModel;

    [TestClass]
    public abstract class CollectionViewModelBaseTests<TViewModel, TParameter> : ViewModelBaseTests<TViewModel>
        where TViewModel : ICollectionViewModel<TParameter>, new()
    {
        #region Properties

        protected MockDefaultBottomBarViewModel MockDefaultBottomBar { get; set; }

        #endregion

        #region Public Methods and Operators

        [TestMethod]
        public void SelectedItemWhenBottomBarIsNillShouldNotThrowException()
        {
            Subject.BottomBar = null;

            Subject.SelectedItems.Should().NotBeNull();
        }

        #endregion

        #region Methods

        protected override void TestInitializeExtensions()
        {
            MockDefaultBottomBar = new MockDefaultBottomBarViewModel();
            Subject.BottomBar = MockDefaultBottomBar;
        }

        #endregion
    }
}