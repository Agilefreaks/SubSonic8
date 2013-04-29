using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Client.Common.Models.Subsonic;
using Client.Tests.Framework.ViewModel;
using Client.Tests.Mocks;
using FluentAssertions;
using Microsoft.VisualStudio.TestPlatform.UnitTestFramework;
using Subsonic8.BottomBar;
using Subsonic8.MenuItem;
using Subsonic8.Search;

namespace Client.Tests.Search
{
    [TestClass]
    public class SearchViewModelTests : CollectionViewModelBaseTests<SearchViewModel, string>
    {
        private MockEventAggregator _eventAggregator;

        protected override SearchViewModel Subject { get; set; }

        protected override void TestInitializeExtensions()
        {
            _eventAggregator = new MockEventAggregator();

            var bottomBarViewModel = new DefaultBottomBarViewModel(MockNavigationService, _eventAggregator, new MockPlyalistManagementService());
            Subject.BottomBar = bottomBarViewModel;
        }

        [TestMethod]
        public void CtorSetsMenuItesm()
        {
            Subject.MenuItems.Should().NotBeNull();
        }

        [TestMethod]
        public async Task Populate_Always_PerformsASubsonicSearch()
        {
            var callCount = 0;
            var searchResult = new MockSearchResult();
            MockSubsonicService.Search = s =>
                {
                    Assert.AreEqual("test", s);
                    callCount++;
                    return searchResult;
                };

            await Task.Run(() => Subject.Parameter = "test");

            Assert.AreEqual(1, callCount);
        }

        [TestMethod]
        public async Task Populate_WhenSearchHasResults_SetsStateToResultsFound()
        {
            var searchResultCollection = new SearchResultCollection { Songs = new List<Song> { new Song() } };
            var searchResult = new MockSearchResult { GetResultFunc = () => searchResultCollection };
            MockSubsonicService.Search = s => searchResult;

            await Task.Run(() => Subject.Parameter = "test");

            Subject.State.Should().Be(SearchResultState.ResultsFound);
        }

        [TestMethod]
        public async Task Populate_WhenSearchHasNoResults_SetsStateToNoResultsFound()
        {
            var searchResultCollection = new SearchResultCollection();
            var searchResult = new MockSearchResult { GetResultFunc = () => searchResultCollection };
            MockSubsonicService.Search = s => searchResult;

            await Task.Run(() => Subject.Parameter = "test");

            Subject.State.Should().Be(SearchResultState.NoResultsFound);
        }

        [TestMethod]
        public async Task PopulateMenuItems_AlbumsContainsOneAlbumWillAddToMenuItemsOneEntry()
        {
            var searchResult = new SearchResultCollection
                {
                    Artists = new List<ExpandedArtist>
                        {
                            new ExpandedArtist()
                        }
                };
            MockSubsonicService.Search = s => new MockSearchResult { GetResultFunc = () => searchResult };

            await Task.Run(() => Subject.Parameter = "test");

            Subject.MenuItems.Should().HaveCount(1);
        }

        [TestMethod]
        public async Task PopulateMenuItems_ArtistsContainsOneArtistWillAddToMenuItemsOneEntry()
        {
            var searchResult = new SearchResultCollection
                {
                    Albums = new List<Common.Models.Subsonic.Album>
                        {
                            new Common.Models.Subsonic.Album()
                        },
                };

            MockSubsonicService.Search = s => new MockSearchResult { GetResultFunc = () => searchResult };

            await Task.Run(() => Subject.Parameter = "test");

            Subject.MenuItems.Should().HaveCount(1);
        }

        [TestMethod]
        public async Task PopulateMenuItems_SongsContainsOneSongWillAddToMenuItemsOneEntry()
        {
            var searchResult = new SearchResultCollection
                                     {
                                         Songs = new List<Song>
                                                     {
                                                         new Song()
                                                     },
                                     };
            MockSubsonicService.Search = s => new MockSearchResult { GetResultFunc = () => searchResult };

            await Task.Run(() => Subject.Parameter = "test");

            Subject.MenuItems.Should().HaveCount(1);
        }

        [TestMethod]
        public void PopulateMenuItems_WhenNullWillResultInMenuItemsWillContainNoEntry()
        {
            Subject.Parameter = null;

            Subject.MenuItems.Should().HaveCount(0);
        }

        [TestMethod]
        public void MenuItemsShouldReturnMenuItemsViewModelsGroupedByType()
        {
            const string type = "testType";
            Subject.MenuItems.Add(new MenuItemViewModel
                                                {
                                                    Type = type
                                                });

            Subject.GroupedMenuItems.Should().Contain(i => i.Any(x => x.Type == type));
        }
    }
}