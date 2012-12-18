using Client.Tests.Framework.ViewModel;
using Microsoft.VisualStudio.TestPlatform.UnitTestFramework;

namespace Client.Tests.PlaylistItem
{
    [TestClass]
    public class PlaylistItemViewModelTests : ItemViewModelBaseTests<Subsonic8.PlaylistItem.PlaylistItemViewModel>
    {
        [TestInitialize]
        public void TestInitialize()
        {
            Subject = new Subsonic8.PlaylistItem.PlaylistItemViewModel();
        }
    }
}
