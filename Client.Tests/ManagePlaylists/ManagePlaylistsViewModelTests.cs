using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Client.Common.EventAggregatorMessages;
using Client.Common.Models;
using Client.Common.Models.Subsonic;
using Client.Common.Results;
using Client.Common.Services.DataStructures.SubsonicService;
using Client.Tests.Framework.ViewModel;
using Client.Tests.Mocks;
using FluentAssertions;
using Microsoft.VisualStudio.TestPlatform.UnitTestFramework;
using Subsonic8.Playlists;

namespace Client.Tests.ManagePlaylists
{
    [TestClass]
    public class ManagePlaylistsViewModelTests : ViewModelBaseTests<ManagePlaylistsViewModel>
    {
        private MockGetAllPlaylistsResult _mockGetAllPlaylistsResult;
        private MockPlyalistManagementService _mockPlyalistManagementService;

        protected override ManagePlaylistsViewModel Subject { get; set; }

        protected override void TestInitializeExtensions()
        {
            base.TestInitializeExtensions();
            _mockGetAllPlaylistsResult = new MockGetAllPlaylistsResult();
            MockSubsonicService.GetAllPlaylists = () => _mockGetAllPlaylistsResult;
            _mockPlyalistManagementService = new MockPlyalistManagementService();
            Subject.PlaylistManagementService = _mockPlyalistManagementService;
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
            var playlistCollection = new PlaylistCollection { Playlists = new List<Playlist> { new Playlist(), new Playlist() } };
            _mockGetAllPlaylistsResult.GetResultFunc = () => playlistCollection;

            await Task.Run(() => Subject.Populate());

            Subject.MenuItems.Count.Should().Be(2);
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
        public void LoadPlaylist_Always_CallsPlaylistManagementServiceLoadPlaylistWithAListOfEquivalentPlaylistItems()
        {
            var playlist = new Playlist { Entries = new List<PlaylistEntry> { new PlaylistEntry { Title = "test", Duration = 123 } } };

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
        public void DeletePlaylist_Always_CallsSubsonicServiceDeletePlaylistWithTheGivenId()
        {
            var callCount = 0;
            MockSubsonicService.DeletePlaylist = i =>
                {
                    callCount++;
                    i.Should().Be(1);

                    return new DeletePlaylistResult(new SubsonicServiceConfiguration(), i);
                };

            Subject.DeletePlaylist(1);

            callCount.Should().Be(1);
        }
    }
}