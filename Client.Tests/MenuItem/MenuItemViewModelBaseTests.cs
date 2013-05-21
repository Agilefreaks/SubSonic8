namespace Client.Tests.MenuItem
{
    using Client.Tests.Framework.ViewModel;
    using Microsoft.VisualStudio.TestPlatform.UnitTestFramework;
    using Subsonic8.MenuItem;

    [TestClass]
    public class MenuItemViewModelBaseTests : ItemViewModelBaseTests<MenuItemViewModel>
    {
        #region Public Methods and Operators

        [TestInitialize]
        public void TestInitialize()
        {
            Subject = new MenuItemViewModel();
        }

        #endregion
    }
}