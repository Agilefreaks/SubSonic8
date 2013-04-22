using System.Linq;
using Client.Common.EventAggregatorMessages;
using Client.Common.Models;
using Client.Common.Services;
using Client.Common.Tests.Mocks;
using FluentAssertions;
using Microsoft.VisualStudio.TestPlatform.UnitTestFramework;

namespace Client.Common.Tests.Services
{
    [TestClass]
    public class PlayerManagementServiceTests
    {
        PlayerManagementService _subject;

        [TestInitialize]
        public void Setup()
        {
            _subject = new PlayerManagementService(new MockEventAggregator());
        }

        [TestCleanup]
        public void Cleanup()
        {
            _subject.ClearPlayers();
        }

        [TestMethod]
        public void RegisterVideoPlayer_Should_AddTheGivenPlayerToTheRegisteredVideoPlayersList()
        {
            var mockPlayer = new MockPlayer();

            _subject.RegisterVideoPlayer(mockPlayer);

            _subject.RegisteredVideoPlayers.Should().Contain(mockPlayer);
        }

        [TestMethod]
        public void RegisterAudioPlayer_Should_AddTheGivenPlayerToTheRegisteredAudioPlayersList()
        {
            var mockPlayer = new MockPlayer();

            _subject.RegisterAudioPlayer(mockPlayer);

            _subject.RegisteredAudioPlayers.Should().Contain(mockPlayer);
        }

        [TestMethod]
        public void RegisteredPlayers_Always_ReutrnsBothAudioAndVideoRegisteredPlayers()
        {
            var mockPlayer1 = new MockPlayer();
            var mockPlayer2 = new MockPlayer();

            _subject.RegisterAudioPlayer(mockPlayer1);
            _subject.RegisterVideoPlayer(mockPlayer2);

            _subject.RegisteredPlayers.Count().Should().Be(2);
            _subject.RegisteredPlayers.Should().Contain(mockPlayer1);
            _subject.RegisteredPlayers.Should().Contain(mockPlayer2);
        }

        [TestMethod]
        public void VideoPlayer_RegisteredVideoPlayersEmpty_ReturnsNull()
        {
            _subject.VideoPlayer.Should().BeNull();
        }

        [TestMethod]
        public void VideoPlayer_RegisteredVideoPlayersHasOnePlayer_ReturnsThePlayer()
        {
            var mockPlayer = new MockPlayer();
            _subject.RegisterVideoPlayer(mockPlayer);

            _subject.VideoPlayer.Should().Be(mockPlayer);
        }

        [TestMethod]
        public void VideoPlayer_RegisteredVideoPlayersHasTwoPlayersAndNoDefaultVideoPlayer_ReturnsTheFirstPlayer()
        {
            var mockPlayer1 = new MockPlayer();
            var mockPlayer2 = new MockPlayer();
            _subject.RegisterVideoPlayer(mockPlayer1);
            _subject.RegisterVideoPlayer(mockPlayer2);
            _subject.DefaultVideoPlayer = null;

            _subject.VideoPlayer.Should().Be(mockPlayer1);
        }

        [TestMethod]
        public void VideoPlayer_RegisteredVideoPlayersHasTwoPlayersAndADefaultVideoPlayerIsSet_ReturnsTheDefaultPlayer()
        {
            var mockPlayer1 = new MockPlayer();
            var mockPlayer2 = new MockPlayer();
            _subject.RegisterVideoPlayer(mockPlayer1);
            _subject.RegisterVideoPlayer(mockPlayer2);
            var mockPlayer3 = new MockPlayer();
            _subject.DefaultVideoPlayer = mockPlayer3;

            _subject.VideoPlayer.Should().Be(mockPlayer3);
        }

        [TestMethod]
        public void GetPlayerFor_ModelHasTypeAudio_ReturnTheAudioPlayer()
        {
            var mockPlayer1 = new MockPlayer();
            _subject.DefaultAudioPlayer = mockPlayer1;
            var mockPlayer2 = new MockPlayer();
            _subject.DefaultVideoPlayer = mockPlayer2;

            var player = _subject.GetPlayerFor(new PlaylistItem { Type = PlaylistItemTypeEnum.Audio });

            player.Should().Be(mockPlayer1);
        }

        [TestMethod]
        public void GetPlayerFor_ModelHasTypeVideo_ReturnTheVideoPlayer()
        {
            var mockPlayer1 = new MockPlayer();
            _subject.DefaultAudioPlayer = mockPlayer1;
            var mockPlayer2 = new MockPlayer();
            _subject.DefaultVideoPlayer = mockPlayer2;

            var player = _subject.GetPlayerFor(new PlaylistItem { Type = PlaylistItemTypeEnum.Video });

            player.Should().Be(mockPlayer2);
        }

        [TestMethod]
        public void HandleStartPlayback_Always_CallsPlayOnThePlayerForTheModelInTheMessage()
        {
            var mockPlayer = new MockPlayer();
            _subject.DefaultAudioPlayer = mockPlayer;
            var playlistItem = new PlaylistItem();

            _subject.Handle(new StartPlaybackMessage(playlistItem));

            mockPlayer.PlayCount.Should().Be(1);
            mockPlayer.PlayCallArguments.First().Should().Be(playlistItem);
        }

        [TestMethod]
        public void HandleStartPlayback_Always_SetsTheCurrentPlayerToThePlayerForTheItem()
        {
            var mockPlayer = new MockPlayer();
            _subject.DefaultAudioPlayer = mockPlayer;
            var playlistItem = new PlaylistItem();

            _subject.Handle(new StartPlaybackMessage(playlistItem));

            _subject.CurrentPlayer.Should().Be(mockPlayer);
        }
    }
}