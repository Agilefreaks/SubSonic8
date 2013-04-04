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
    public class SearchViewModelTests : ViewModelBaseTests<SearchViewModel>
    {
        private MockEventAggregator _eventAggregator;

        protected override SearchViewModel Subject { get; set; }

        protected override void TestInitializeExtensions()
        {
            _eventAggregator = new MockEventAggregator();

            var bottomBarViewModel = new DefaultBottomBarViewModel(MockNavigationService, _eventAggregator);
            Subject.BottomBar = bottomBarViewModel;
        }

        [TestMethod]
        public void CtorSetsMenuItesmViewModel()
        {
            Subject.MenuItemViewModels.Should().NotBeNull();
        }

        [TestMethod]
        public async Task PerformSearch_Always_PerformsASubsonicSearch()
        {
            var callCount = 0;
            var searchResult = new MockSearchResult();
            MockSubsonicService.Search = s =>
                {
                    Assert.AreEqual("test", s);
                    callCount++;
                    return searchResult;
                };

            await Task.Run(() => Subject.PerformSearch("test"));

            Assert.AreEqual(1, callCount);
        }

        [TestMethod]
        public async Task PerformSearch_WhenSearchHasResults_SetsStateToResultsFound()
        {
            var searchResultCollection = new SearchResultCollection { Songs = new List<Song> { new Song() } };
            var searchResult = new MockSearchResult { GetResultFunc = () => searchResultCollection };
            MockSubsonicService.Search = s => searchResult;

            await Task.Run(() => Subject.PerformSearch("test"));

            Subject.State.Should().Be(SearchResultState.ResultsFound);
        }

        [TestMethod]
        public async Task PerformSearch_WhenSearchHasNoResults_SetsStateToNoResultsFound()
        {
            var searchResultCollection = new SearchResultCollection();
            var searchResult = new MockSearchResult { GetResultFunc = () => searchResultCollection };
            MockSubsonicService.Search = s => searchResult;

            await Task.Run(() => Subject.PerformSearch("test"));

            Subject.State.Should().Be(SearchResultState.NoResultsFound);
        }

        [TestMethod]
        public void PopulateArtistsShouldAddMenuItems()
        {
            Subject.PopulateArtists(new List<ExpandedArtist> { new ExpandedArtist() });

            Subject.MenuItemViewModels.Should().HaveCount(1);
        }

        [TestMethod]
        public void PopulateAlbumsShouldAddMenuItems()
        {
            Subject.PopulateAlbums(new List<Common.Models.Subsonic.Album> { new Common.Models.Subsonic.Album() });

            Subject.MenuItemViewModels.Should().HaveCount(1);
        }

        [TestMethod]
        public void PopulateSongsShouldAddMenuItems()
        {
            Subject.PopulateSongs(new List<Song> { new Song() });

            Subject.MenuItemViewModels.Should().HaveCount(1);
        }

        [TestMethod]
        public void PopulateMenuItemsShouldCallSubsonicServiceGetCoverArtForId()
        {
            var searchResult = new SearchResultCollection { Albums = new List<Common.Models.Subsonic.Album> { new Common.Models.Subsonic.Album() } };

            Subject.PopulateMenuItems(searchResult);

            (MockSubsonicService.GetCoverArtForIdCallCount > 0).Should().BeTrue();
        }

        [TestMethod]
        public void PopulateMenuItems_AlbumsContainsOneAlbumWillAddToMenuItemsOneEntry()
        {
            var searchResult = new SearchResultCollection
                {
                    Artists = new List<ExpandedArtist>
                        {
                            new ExpandedArtist()
                        }
                };

            Subject.PopulateMenuItems(searchResult);

            Subject.MenuItems.Should().HaveCount(1);
        }

        [TestMethod]
        public void PopulateMenuItems_ArtistsContainsOneArtistWillAddToMenuItemsOneEntry()
        {
            var searchResult = new SearchResultCollection
                {
                    Albums = new List<Common.Models.Subsonic.Album>
                        {
                            new Common.Models.Subsonic.Album()
                        },
                };

            Subject.PopulateMenuItems(searchResult);

            Subject.MenuItems.Should().HaveCount(1);
        }

        [TestMethod]
        public void PopulateMenuItems_SongsContainsOneSongWillAddToMenuItemsOneEntry()
        {
            var searchResult = new SearchResultCollection
                                     {
                                         Songs = new List<Song>
                                                     {
                                                         new Song()
                                                     },
                                     };

            Subject.PopulateMenuItems(searchResult);

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
            Subject.MenuItemViewModels.Add(new MenuItemViewModel
                                                {
                                                    Type = type
                                                });

            Subject.MenuItems.Should().Contain(i => i.Any(x => x.Type == type));
        }
    }
}