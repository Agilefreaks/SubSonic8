using Client.Tests.Framework.ViewModel;
using Microsoft.VisualStudio.TestPlatform.UnitTestFramework;
using Subsonic8.MenuItem;

namespace Client.Tests.MenuItem
{
    [TestClass]
    public class MenuItemViewModelBaseTests : ItemViewModelBaseTests<MenuItemViewModel>
    {
        [TestInitialize]
        public void TestInitialize()
        {
            Subject = new MenuItemViewModel();
        }
    }
}
