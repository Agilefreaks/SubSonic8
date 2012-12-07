using System.Collections.ObjectModel;
using Caliburn.Micro;
using FluentAssertions;
using Microsoft.VisualStudio.TestPlatform.UnitTestFramework;
using Subsonic8.BottomBar;
using Subsonic8.Framework.ViewModel;
using Subsonic8.MenuItem;
using Subsonic8.Shell;

namespace Client.Tests.Framework.ViewModel
{
    [TestClass]
    public class ViewModelBaseTests : ClientTestBase
    {
        private ViewModelBaseImpl _subject;

        [TestInitialize]
        public void TestInitialize()
        {
            _subject = new ViewModelBaseImpl();
        }

        [TestMethod]
        public void CtorAlwaysSetsBottomBarToCurrentlyRegisteredDefaultBottomBar()
        {
            var defaultBottomBar = new MockDefaultBottomBarViewModel();
            var oldGetInstance = IoC.GetInstance;
            IoC.GetInstance = (type, s) =>
                                  {
                                      var obj = oldGetInstance(type, s);
                                      if (type == typeof(IDefaultBottomBarViewModel))
                                          obj = defaultBottomBar;

                                      return obj;
                                  };

            _subject = new ViewModelBaseImpl();

            _subject.BottomBar.Should().Be(defaultBottomBar);
        }

        [TestMethod]
        public void BottomBarWhenSetShouldSetBottomBarOnShellViewModel()
        {
            var defaultBottomBar = new MockDefaultBottomBarViewModel();
            var oldGetInstance = IoC.GetInstance;
            IoC.GetInstance = (type, s) =>
            {
                var obj = oldGetInstance(type, s);
                if (type == typeof(IDefaultBottomBarViewModel))
                    obj = defaultBottomBar;

                return obj;
            };

            _subject = new ViewModelBaseImpl();
            var bottomBarViewModel = new MockDefaultBottomBarViewModel();
            _subject.BottomBar = bottomBarViewModel;

            IoC.Get<IShellViewModel>().BottomBar.Should().Be(bottomBarViewModel);
        }
    }

    #region Mocks
    internal class ViewModelBaseImpl : ViewModelBase
    {
    }

    internal class MockDefaultBottomBarViewModel : IDefaultBottomBarViewModel
    {
        public bool IsOpened { get; set; }

        public void NavigateToPlaylist()
        {

        }

        public void PlayPrevious()
        {
            throw new System.NotImplementedException();
        }

        public void PlayNext()
        {
            throw new System.NotImplementedException();
        }

        public void PlayPause()
        {
            throw new System.NotImplementedException();
        }

        public void Play()
        {
            throw new System.NotImplementedException();
        }

        public void Stop()
        {
            throw new System.NotImplementedException();
        }

        public void AddToPlaylist()
        {

        }

        public ObservableCollection<IMenuItemViewModel> SelectedItems { get; set; }
    }
#endregion
}
