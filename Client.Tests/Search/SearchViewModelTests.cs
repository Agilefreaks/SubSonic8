namespace Client.Tests.Search
{
    using System.Collections.Generic;
    using System.Linq;
    using System.Threading.Tasks;
    using Client.Common.Models.Subsonic;
    using Client.Tests.Framework.ViewModel;
    using Client.Tests.Mocks;
    using global::Common.Mocks;
    using FluentAssertions;
    using global::Common.Mocks.Results;
    using Microsoft.VisualStudio.TestPlatform.UnitTestFramework;
    using Subsonic8.BottomBar;
    using Subsonic8.MenuItem;
    using Subsonic8.Search;

    [TestClass]
    public class SearchViewModelTests : CollectionViewModelBaseTests<SearchViewModel, string>
    {
        #region Fields

        private MockEventAggregator _eventAggregator;

        #endregion

        #region Properties

        protected override SearchViewModel Subject { get; set; }

        #endregion

        #region Public Methods and Operators

        [TestMethod]
        public void CtorSetsMenuItesm()
        {
            Subject.MenuItems.Should().NotBeNull();
        }

        [TestMethod]
        public void MenuItemsShouldReturnMenuItemsViewModelsGroupedByType()
        {
            const string ItemType = "testType";
            Subject.MenuItems.Add(new MenuItemViewModel { Type = ItemType });

            Subject.GroupedMenuItems.Should().Contain(i => i.Any(x => x.Type == ItemType));
        }

        [TestMethod]
        public async Task PopulateMenuItems_AlbumsContainsOneAlbumWillAddToMenuItemsOneEntry()
        {
            var searchResult = new SearchResultCollection
                                   {
                                       Artists = new List<ExpandedArtist> { new ExpandedArtist() }
                                   };
            MockSubsonicService.Search = s => new MockSearchResult { GetResultFunc = () => searchResult };

            await Subject.Populate();

            Subject.MenuItems.Should().HaveCount(1);
        }

        [TestMethod]
        public async Task PopulateMenuItems_ArtistsContainsOneArtistWillAddToMenuItemsOneEntry()
        {
            var searchResult = new SearchResultCollection { Albums = new List<Album> { new Album() }, };

            MockSubsonicService.Search = s => new MockSearchResult { GetResultFunc = () => searchResult };

            await Subject.Populate();

            Subject.MenuItems.Should().HaveCount(1);
        }

        [TestMethod]
        public async Task PopulateMenuItems_SongsContainsOneSongWillAddToMenuItemsOneEntry()
        {
            var searchResult = new SearchResultCollection { Songs = new List<Song> { new Song() }, };
            MockSubsonicService.Search = s => new MockSearchResult { GetResultFunc = () => searchResult };

            await Subject.Populate();

            Subject.MenuItems.Should().HaveCount(1);
        }

        [TestMethod]
        public void PopulateMenuItems_WhenNullWillResultInMenuItemsWillContainNoEntry()
        {
            Subject.Parameter = null;

            Subject.MenuItems.Should().HaveCount(0);
        }

        [TestMethod]
        public void Populate_Always_PerformsASubsonicSearch()
        {
            var callCount = 0;
            var searchResult = new MockSearchResult();
            MockSubsonicService.Search = s =>
                {
                    Assert.AreEqual("test", s);
                    callCount++;
                    return searchResult;
                };

            Subject.Parameter = "test";

            Assert.AreEqual(1, callCount);
        }

        [TestMethod]
        public async Task Populate_WhenSearchHasNoResults_SetsStateToNoResultsFound()
        {
            var searchResultCollection = new SearchResultCollection();
            var searchResult = new MockSearchResult { GetResultFunc = () => searchResultCollection };
            MockSubsonicService.Search = s => searchResult;

            await Subject.Populate();

            Subject.State.Should().Be(SearchResultState.NoResultsFound);
        }

        [TestMethod]
        public async Task Populate_WhenSearchHasResults_SetsStateToResultsFound()
        {
            var searchResultCollection = new SearchResultCollection { Songs = new List<Song> { new Song() } };
            var searchResult = new MockSearchResult { GetResultFunc = () => searchResultCollection };
            MockSubsonicService.Search = s => searchResult;

            await Subject.Populate();

            Subject.State.Should().Be(SearchResultState.ResultsFound);
        }

        #endregion

        #region Methods

        protected override void TestInitializeExtensions()
        {
            _eventAggregator = new MockEventAggregator();

            var bottomBarViewModel = new DefaultBottomBarViewModel
                                         {
                                             NavigationService = MockNavigationService,
                                             EventAggregator = _eventAggregator,
                                             PlaylistManagementService = new MockPlyalistManagementService(),
                                             ErrorDialogViewModel = new MockErrorDialogViewModel()
                                         };
            Subject.BottomBar = bottomBarViewModel;
        }

        #endregion
    }
}