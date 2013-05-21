namespace Client.Tests.Playlists
{
    using System.Collections.Generic;
    using System.Linq;
    using System.Threading.Tasks;
    using Client.Common.EventAggregatorMessages;
    using Client.Common.Models;
    using Client.Common.Models.Subsonic;
    using Client.Tests.Framework.ViewModel;
    using Client.Tests.Mocks;
    using FluentAssertions;
    using Microsoft.VisualStudio.TestPlatform.UnitTestFramework;
    using Subsonic8.Playlists;

    [TestClass]
    public class ManagePlaylistsViewModelTests : ViewModelBaseTests<ManagePlaylistsViewModel>
    {
        #region Fields

        private MockGetAllPlaylistsResult _mockGetAllPlaylistsResult;

        private MockPlyalistManagementService _mockPlyalistManagementService;

        #endregion

        #region Properties

        protected override ManagePlaylistsViewModel Subject { get; set; }

        #endregion

        #region Public Methods and Operators

        [TestMethod]
        public void LoadPlaylist_Always_CallsPlaylistManagementServiceLoadPlaylistWithAListOfEquivalentPlaylistItems()
        {
            var playlist = new Playlist
                               {
                                   Entries =
                                       new List<PlaylistEntry>
                                           {
                                               new PlaylistEntry
                                                   {
                                                       Title = "test", 
                                                       Duration = 123
                                                   }
                                           }
                               };

            Subject.LoadPlaylist(playlist);

            var methodCall = _mockPlyalistManagementService.MethodCalls.First();
            methodCall.Key.Should().Be("LoadPlaylist");
            var playlistItemCollection = methodCall.Value as PlaylistItemCollection;
            Assert.IsNotNull(playlistItemCollection);
            playlistItemCollection.Count.Should().Be(1);
            playlistItemCollection[0].Title.Should().Be("test");
            playlistItemCollection[0].Duration.Should().Be(123);
        }

        [TestMethod]
        public void LoadPlaylist_Always_SendsAStopPlaybackMessage()
        {
            Subject.LoadPlaylist(new Playlist());

            MockEventAggregator.PublishCallCount.Should().Be(1);
            MockEventAggregator.Messages[0].Should().BeOfType<StopMessage>();
        }

        [TestMethod]
        public void LoadPlaylist_Always_ShouldClearThePlaylist()
        {
            Subject.LoadPlaylist(new Playlist());

            _mockPlyalistManagementService.ClearCallCount.Should().Be(1);
        }

        [TestMethod]
        public void LoadPlaylist_Always_ShouldGoBack()
        {
            Subject.LoadPlaylist(new Playlist());

            MockNavigationService.GoBackCallCount.Should().Be(1);
        }

        [TestMethod]
        public async Task Populate_Always_ShouldExecuteAGetAllPlaylistsResult()
        {
            await Task.Run(() => Subject.Populate());

            _mockGetAllPlaylistsResult.ExecuteCallCount.Should().Be(1);
        }

        [TestMethod]
        public async Task Populate_ShouldSetMenuItemsForEachPlaylist()
        {
            var playlistCollection = new PlaylistCollection
                                         {
                                             Playlists =
                                                 new List<Playlist>
                                                     {
                                                         new Playlist(), 
                                                         new Playlist()
                                                     }
                                         };
            _mockGetAllPlaylistsResult.GetResultFunc = () => playlistCollection;

            await Task.Run(() => Subject.Populate());

            Subject.MenuItems.Count.Should().Be(2);
        }

        #endregion

        #region Methods

        protected override void TestInitializeExtensions()
        {
            base.TestInitializeExtensions();
            _mockGetAllPlaylistsResult = new MockGetAllPlaylistsResult();
            MockSubsonicService.GetAllPlaylists = () => _mockGetAllPlaylistsResult;
            _mockPlyalistManagementService = new MockPlyalistManagementService();
            Subject.PlaylistManagementService = _mockPlyalistManagementService;
        }

        #endregion
    }
}