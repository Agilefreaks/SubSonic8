using System.Collections.Generic;
using System.Linq;
using Client.Common.Models.Subsonic;
using Client.Tests.Mocks;
using FluentAssertions;
using Microsoft.VisualStudio.TestPlatform.UnitTestFramework;
using Subsonic8.MenuItem;
using Subsonic8.Search;
using Subsonic8.Shell;

namespace Client.Tests.Search
{
    [TestClass]
    public class SearchViewModelTests
    {
        private ISearchViewModel _subject;
        private MockEventAggregator _eventAggregator;
        private MockSubsonicService _subsonicService;
        private ShellViewModel _shellViewModel;
        private MockNavigationService _navigationService;

        [TestInitialize]
        public void TestInitialize()
        {
            _eventAggregator = new MockEventAggregator();
            _subsonicService = new MockSubsonicService();
            _navigationService = new MockNavigationService();
            _shellViewModel = new ShellViewModel(_eventAggregator, _subsonicService, _navigationService);
            _subject = new SearchViewModel(_shellViewModel, _eventAggregator);
        }

        [TestMethod]
        public void CtorSetsMenuItesmViewModel()
        {
            _subject.MenuItemViewModels.Should().NotBeNull();
        }

        [TestMethod]
        public void ParameterAlbumsContainsOneAlbumWillAddToMenuItemsOneEntry()
        {
            _subject.Parameter = new SearchResultCollection
                                     {
                                         Artists = new List<ExpandedArtist>
                                                       {
                                                           new ExpandedArtist()
                                                       }
                                     };

            _subject.MenuItems.Should().HaveCount(1);
        }

        [TestMethod]
        public void ParameterArtistsContainsOneArtistWillAddToMenuItemsOneEntry()
        {
            _subject.Parameter = new SearchResultCollection
                                     {
                                         Albums = new List<Client.Common.Models.Subsonic.Album>
                                                      {
                                                          new Client.Common.Models.Subsonic.Album()
                                                      },
                                     };

            _subject.MenuItems.Should().HaveCount(1);
        }

        [TestMethod]
        public void ParameterSongsContainsOneSongWillAddToMenuItemsOneEntry()
        {
            _subject.Parameter = new SearchResultCollection
                                     {
                                         Songs = new List<Song>
                                                     {
                                                         new Song()
                                                     },
                                     };

            _subject.MenuItems.Should().HaveCount(1);
        }

        [TestMethod]
        public void ParameterWhenNullWillResultInMenuItemsWillContainNoEntry()
        {
            _subject.Parameter = null;

            _subject.MenuItems.Should().HaveCount(0);
        }

        [TestMethod]
        public void PopulateArtistsShouldAddMenuItems()
        {
            _subject.PopulateArtists(new List<ExpandedArtist> { new ExpandedArtist() });

            _subject.MenuItemViewModels.Should().HaveCount(1);
        }

        [TestMethod]
        public void PopulateAlbumsShouldAddMenuItems()
        {
            _subject.PopulateAlbums(new List<Client.Common.Models.Subsonic.Album> { new Client.Common.Models.Subsonic.Album() });

            _subject.MenuItemViewModels.Should().HaveCount(1);
        }

        [TestMethod]
        public void PopulateSongsShouldAddMenuItems()
        {
            _subject.PopulateSongs(new List<Song> { new Song() });

            _subject.MenuItemViewModels.Should().HaveCount(1);
        }

        [TestMethod]
        public void MenuItemsShouldReturnMenuItemsViewModelsGroupedByType()
        {
            const string type = "testType";
            _subject.MenuItemViewModels.Add(new MenuItemViewModel
                                                {
                                                    Type = type
                                                });

            _subject.MenuItems.Should().Contain(i => i.Any(x => x.Type == type));
        }
    }
}