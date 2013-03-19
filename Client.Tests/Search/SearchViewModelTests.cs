using System.Collections.Generic;
using System.Linq;
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
    public class SearchViewModelTests : ViewModelBaseTests<ISearchViewModel>
    {
        private MockEventAggregator _eventAggregator;

        protected override ISearchViewModel Subject { get; set; }

        protected override void TestInitializeExtensions()
        {
            _eventAggregator = new MockEventAggregator();

            var bottomBarViewModel = new DefaultBottomBarViewModel(MockNavigationService, _eventAggregator);
            Subject = new SearchViewModel
                {
                    BottomBar = bottomBarViewModel,
                    SubsonicService = MockSubsonicService,
                    Parameter = new SearchResultCollection {Query = "I search high and low"}
                };
        }

        [TestMethod]
        public void CtorSetsMenuItesmViewModel()
        {
            Subject.MenuItemViewModels.Should().NotBeNull();
        }

        [TestMethod]
        public void ParameterAlbumsContainsOneAlbumWillAddToMenuItemsOneEntry()
        {
            Subject.Parameter = new SearchResultCollection
                                     {
                                         Artists = new List<ExpandedArtist>
                                                       {
                                                           new ExpandedArtist()
                                                       }
                                     };

            Subject.MenuItems.Should().HaveCount(1);
        }

        [TestMethod]
        public void ParameterArtistsContainsOneArtistWillAddToMenuItemsOneEntry()
        {
            Subject.Parameter = new SearchResultCollection
                                     {
                                         Albums = new List<Common.Models.Subsonic.Album>
                                                      {
                                                          new Common.Models.Subsonic.Album()
                                                      },
                                     };

            Subject.MenuItems.Should().HaveCount(1);
        }

        [TestMethod]
        public void ParameterSongsContainsOneSongWillAddToMenuItemsOneEntry()
        {
            Subject.Parameter = new SearchResultCollection
                                     {
                                         Songs = new List<Song>
                                                     {
                                                         new Song()
                                                     },
                                     };

            Subject.MenuItems.Should().HaveCount(1);
        }

        [TestMethod]
        public void ParameterWhenNullWillResultInMenuItemsWillContainNoEntry()
        {
            Subject.Parameter = null;

            Subject.MenuItems.Should().HaveCount(0);
        }

        [TestMethod]
        public void Parameter_WhenSetWithNotEmptySearchCollectionResult_SetsStateToResultsFound()
        {
            Subject.Parameter = new SearchResultCollection { Songs = new List<Song> { new Song() } };

            Subject.State.Should().Be(SearchResultState.ResultsFound);
        }

        [TestMethod]
        public void Parameter_WhenSetWithEmptySearchCollectionResult_SetsStateToNoResultsFound()
        {
            Subject.Parameter = new SearchResultCollection();

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
            Subject.Parameter = new SearchResultCollection { Albums = new List<Common.Models.Subsonic.Album> { new Common.Models.Subsonic.Album() } };

            Subject.PopulateMenuItems();

            (MockSubsonicService.GetCoverArtForIdCallCount > 0).Should().BeTrue();
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