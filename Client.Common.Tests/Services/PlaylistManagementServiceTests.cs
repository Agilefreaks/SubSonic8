namespace Client.Common.Tests.Services
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using Client.Common.EventAggregatorMessages;
    using Client.Common.Models;
    using Client.Common.Services;
    using global::Common.Mocks;
    using FluentAssertions;
    using Microsoft.VisualStudio.TestPlatform.UnitTestFramework;

    [TestClass]
    public class PlaylistManagementServiceTests
    {
        #region Fields

        private MockEventAggregator _mockEventAggregator;

        private PlaylistManagementService _subject;

        #endregion

        #region Public Methods and Operators

        [TestMethod]
        public void Ctor_Always_SetsGetNextTrackNumberFuncToGetNextTrackNumber()
        {
            var playlistManagementService = new PlaylistManagementService(_mockEventAggregator);

            playlistManagementService.GetNextTrackNumberFunc.Should()
                                     .Be((Func<int>)playlistManagementService.GetNextTrackNumber);
        }

        [TestMethod]
        public void Ctor_Always_SetsGetPreviousTrackNumberFuncToGetPreviousTrackNumber()
        {
            var playlistManagementService = new PlaylistManagementService(_mockEventAggregator);

            playlistManagementService.GetPreviousTrackNumberFunc.Should()
                                     .Be((Func<int>)playlistManagementService.GetPreviousTrackNumber);
        }

        [TestMethod]
        public void Ctor_Always_SetsShuffleOnFalse()
        {
            _subject.ShuffleOn.Should().BeFalse();
        }

        [TestMethod]
        public void Ctor_Always_SetsStartPlaybackActionToStartPlyaback()
        {
            var playlistManagementService = new PlaylistManagementService(_mockEventAggregator);

            playlistManagementService.StartPlaybackAction.Should()
                                     .Be((Action<int>)playlistManagementService.StartPlayback);
        }

        [TestMethod]
        public void Ctor_Always_SetsStopPlybackActionToStopPlyback()
        {
            var playlistManagementService = new PlaylistManagementService(_mockEventAggregator);

            playlistManagementService.StopPlaybackAction.Should().Be((Action)playlistManagementService.StopPlayback);
        }

        [TestMethod]
        public void GetNextTrackNumber_ShuffleOffAndCurrentItemLastItem_Return0()
        {
            _subject.Items.AddRange(GeneratePlaylistItems());
            _subject.Handle(new PlayNextMessage());
            _subject.Handle(new PlayNextMessage());

            _subject.GetNextTrackNumber().Should().Be(0);
        }

        [TestMethod]
        public void GetNextTrackNumber_ShuffleOffAndCurrentItemNull_ShouldReturn0()
        {
            _subject.Items.AddRange(GeneratePlaylistItems());
            _subject.CurrentItem.Should().BeNull();

            _subject.GetNextTrackNumber().Should().Be(0);
        }

        [TestMethod]
        public void GetNextTrackNumber_ShuffleOff_ShouldReturnCurrentTrackNumberIncrementedBy1()
        {
            _subject.Items.AddRange(GeneratePlaylistItems());
            _subject.Handle(new PlayNextMessage());

            _subject.GetNextTrackNumber().Should().Be(1);
        }

        [TestMethod]
        public void GetNextTrackNumber_ShuffleOn_ReturnARandomNumberSmallerThanTheTotalNumberOfItems()
        {
            _subject.Items.AddRange(GeneratePlaylistItems(100));
            _subject.Handle(new ToggleShuffleMessage());

            _subject.GetNextTrackNumber().Should().NotBe(0);
        }

        [TestMethod]
        public void GetPreviousTrackNumber_ShuffleOff_SetsTheSourceToThePreviousItemInThePlaylist()
        {
            _subject.Items.AddRange(GeneratePlaylistItems(15));
            _subject.Handle(new PlayNextMessage());
            _subject.Handle(new PlayNextMessage());

            var previousTrackNumber = _subject.GetPreviousTrackNumber();

            previousTrackNumber.Should().Be(0);
        }

        [TestMethod]
        public void GetPreviousTrackNumber_ShuffleOnAndHistoryIsEmpty_ReturnsMinus1()
        {
            _subject.Items.AddRange(GeneratePlaylistItems(15));
            _subject.Handle(new ToggleShuffleMessage());

            var previousTrackNumber = _subject.GetPreviousTrackNumber();

            previousTrackNumber.Should().Be(-1);
        }

        [TestMethod]
        public void GetPreviousTrackNumber_ShuffleOnAndHistoryIsNotEmpty_ReturnsThePreviousItem()
        {
            _subject.Items.AddRange(GeneratePlaylistItems(15));
            _subject.Handle(new ToggleShuffleMessage());
            _subject.Handle(new PlayNextMessage());
            var currentIndex = _subject.Items.IndexOf(_subject.CurrentItem);
            _subject.Handle(new PlayNextMessage());

            var previousTrackNumber = _subject.GetPreviousTrackNumber();

            previousTrackNumber.Should().Be(currentIndex);
        }

        [TestMethod]
        public void HandlePlayItemAtIndexMessage_EqualToItemCount_DoesNotCallsStartPlaybackAction()
        {
            var callCount = 0;
            _subject.StartPlaybackAction = i => { callCount++; };

            _subject.Handle(new PlayItemAtIndexMessage(0));

            callCount.Should().Be(0);
        }

        [TestMethod]
        public void HandlePlayItemAtIndexMessage_IndexBetweenZeroAndItemsCount_CallsStartPlaybackActionWithGivenIndex()
        {
            _subject.Items.AddRange(GeneratePlaylistItems());
            var callCount = 0;
            _subject.StartPlaybackAction = i =>
                {
                    i.Should().Be(1);
                    callCount++;
                };

            _subject.Handle(new PlayItemAtIndexMessage(1));

            callCount.Should().Be(1);
        }

        [TestMethod]
        public void HandlePlayItemAtIndexMessage_IndexLessThanZero_DoesNotCallsStartPlaybackAction()
        {
            var callCount = 0;
            _subject.StartPlaybackAction = i => { callCount++; };

            _subject.Handle(new PlayItemAtIndexMessage(-1));

            callCount.Should().Be(0);
        }

        [TestMethod]
        public void HandlePlayMessage_TheCurrentItemIsNotSet_SendsAStartPlaybackMessage()
        {
            _subject.Items.Add(new PlaylistItem());

            _subject.Handle(new PlayMessage());

            _mockEventAggregator.Messages.Any(m => m.GetType() == typeof(StartPlaybackMessage)).Should().BeTrue();
        }

        [TestMethod]
        public void HandlePlayMessage_TheCurrentItemIsPaused_SendsAResumePlaybackMessage()
        {
            _subject.Items.Add(new PlaylistItem());
            _subject.StartPlayback(0);
            _subject.Pause();

            _subject.Handle(new PlayMessage());

            _mockEventAggregator.Messages.Any(m => m.GetType() == typeof(ResumePlaybackMessage)).Should().BeTrue();
        }

        [TestMethod]
        public void HandlePlayNext_Always_CallsGetNextTrackNumber()
        {
            var callCount = 0;
            _subject.Items.AddRange(GeneratePlaylistItems(5));
            _subject.GetNextTrackNumberFunc = () =>
                {
                    callCount++;
                    return 2;
                };

            _subject.Handle(new PlayNextMessage());

            callCount.Should().Be(1);
        }

        [TestMethod]
        public void HandlePlayNext_Always_CallsStartPlaybackActionWithTheCurrentItem()
        {
            _subject.Items.AddRange(GeneratePlaylistItems(15));
            _subject.GetNextTrackNumberFunc = () => 2;
            var callCount = 0;
            _subject.StartPlaybackAction = index =>
                {
                    index.Should().Be(2);
                    callCount++;
                };

            _subject.Handle(new PlayNextMessage());

            callCount.Should().Be(1);
        }

        [TestMethod]
        public void HandlePlayNext_CurrentTrackIsInitialized_PushesTheCurrentTrackIndexToTheTrackHistory()
        {
            _subject.Items.AddRange(GeneratePlaylistItems(15));

            _subject.Handle(new PlayNextMessage());
            _subject.Handle(new PlayNextMessage());

            _subject.PlaylistHistory.Count.Should().Be(1);
            _subject.PlaylistHistory.Pop().Should().Be(0);
        }

        [TestMethod]
        public void HandlePlayNext_CurrentTrackIsNotInitialized_DoesNotAlterTrackHistory()
        {
            _subject.Items.AddRange(GeneratePlaylistItems(15));

            _subject.Handle(new PlayNextMessage());

            _subject.PlaylistHistory.Count.Should().Be(0);
        }

        [TestMethod]
        public void HandlePlayPrevious_Always_CallsGetPreviousTrackNumberFunc()
        {
            var callCount = 0;
            _subject.Items.AddRange(GeneratePlaylistItems(5));
            _subject.GetPreviousTrackNumberFunc = () =>
                {
                    callCount++;
                    return 2;
                };

            _subject.Handle(new PlayPreviousMessage());

            callCount.Should().Be(1);
            _subject.Items.IndexOf(_subject.CurrentItem).Should().Be(2);
        }

        [TestMethod]
        public void HandlePlayPrevious_ReturnedIndexIsGreatertThanMinusOne_CallsStartPlaybackActionWithTheObtainedIndex()
        {
            _subject.Items.AddRange(GeneratePlaylistItems(15));
            _subject.GetPreviousTrackNumberFunc = () => 3;
            var callCount = 0;
            _subject.StartPlaybackAction = index =>
                {
                    index.Should().Be(3);
                    callCount++;
                };

            _subject.Handle(new PlayPreviousMessage());

            callCount.Should().Be(1);
        }

        [TestMethod]
        public void HandlePlayPrevious_ReturnedIndexIsMinusOne_DoesNotCallsStartPlaybackAction()
        {
            _subject.Items.AddRange(GeneratePlaylistItems(15));
            _subject.GetPreviousTrackNumberFunc = () => -1;
            var callCount = 0;
            _subject.StartPlaybackAction = index => callCount++;

            _subject.Handle(new PlayPreviousMessage());

            callCount.Should().Be(0);
        }

        [TestMethod]
        public void HandleToggleShuffle_ShuffleOnFalse_SetsShuffleOnTrue()
        {
            _subject.Handle(new ToggleShuffleMessage());

            _subject.ShuffleOn.Should().BeTrue();
        }

        [TestMethod]
        public void HandleToggleShuffle_ShuffleOnTrue_SetsShuffleOnFalse()
        {
            // Set it to true
            _subject.Handle(new ToggleShuffleMessage());

            _subject.Handle(new ToggleShuffleMessage());

            _subject.ShuffleOn.Should().BeFalse();
        }

        [TestMethod]
        public void HandleWithRemoveFromPlaylistMessageShouldRemoveItemsInQueueFromCurrentPlaylist()
        {
            var playlistItem = new PlaylistItem();
            _subject.Items.Add(playlistItem);

            _subject.Handle(new RemoveItemsMessage { Queue = new List<PlaylistItem> { playlistItem } });

            _subject.Items.Should().HaveCount(0);
        }

        [TestMethod]
        public void Handle_StopPlaybackMessage_AlwaysCallsStopPlybackAction()
        {
            var callCount = 0;
            _subject.StopPlaybackAction = () => { callCount++; };

            _subject.Handle(new StopMessage());

            callCount.Should().Be(1);
        }

        [TestMethod]
        public void Handle_WithAddItemsMessage_AddsAllItemsFromQueueItems()
        {
            var initialList = GeneratePlaylistItems(3).ToList();
            _subject.Items.AddRange(initialList);

            var playlistItems = new List<PlaylistItem> { new PlaylistItem() };

            _subject.Handle(new AddItemsMessage { Queue = playlistItems });

            playlistItems.All(i => _subject.Items.Contains(i)).Should().BeTrue();
        }

        [TestMethod]
        public void Handle_WithAddItemsMessage_AddsItemsAfterOldItems()
        {
            var initialList = GeneratePlaylistItems(3).ToList();
            _subject.Items.AddRange(initialList);

            _subject.Handle(new AddItemsMessage { Queue = new List<PlaylistItem> { new PlaylistItem() } });

            initialList.All(i => _subject.Items.Contains(i)).Should().BeTrue();
        }

        [TestMethod]
        public void IsPlaying_ByDefault_IsFalse()
        {
            _subject.IsPlaying.Should().BeFalse();
        }

        [TestMethod]
        public void PLaylistItemsWhenChangedFromNotEmptyToNotEmptyCallsEventAggregatorPublishWithShowControlsMessage()
        {
            _subject.Items.AddRange(GeneratePlaylistItems());

            _subject.Items.Clear();

            _mockEventAggregator.PublishCallCount.Should().Be(2);
            _mockEventAggregator.Messages.ElementAt(1).GetType().Should().Be(typeof(PlaylistStateChangedMessage));
        }

        [TestMethod]
        public void Pause_Always_SetsIsPlayingToFalse()
        {
            _subject.Items.Add(new PlaylistItem());
            _subject.StartPlayback(0);

            _subject.Pause();

            _subject.IsPlaying.Should().BeFalse();
        }

        [TestMethod]
        public void Pause_IsNotPlaying_DoesNotSetIsPausedTrue()
        {
            _subject.Items.Add(new PlaylistItem());

            _subject.Pause();

            _subject.IsPaused.Should().BeFalse();
        }

        [TestMethod]
        public void Pause_IsPlaying_SendsPausePlaybackMessage()
        {
            _subject.Items.Add(new PlaylistItem());
            _subject.Play();

            _subject.Pause();

            _mockEventAggregator.Messages.Any(m => m.GetType() == typeof(PausePlaybackMessage)).Should().BeTrue();
        }

        [TestMethod]
        public void Pause_IsPlaying_SetsIsPlayingFalseAndIsPausedTrue()
        {
            _subject.Items.Add(new PlaylistItem());
            _subject.Play();

            _subject.Pause();

            _subject.IsPlaying.Should().BeFalse();
            _subject.IsPaused.Should().BeTrue();
        }

        [TestMethod]
        public void PlayPause_IsNotPlayingAndCurrentItemSetAndIsNotPaused_SendsAStartPlaybackMessage()
        {
            _subject.Items.Add(new PlaylistItem());
            _subject.StartPlayback(0);
            _subject.StopPlayback();

            _subject.PlayPause();

            _mockEventAggregator.Messages.Any(m => m.GetType() == typeof(StartPlaybackMessage)).Should().BeTrue();
        }

        [TestMethod]
        public void PlayPause_IsNotPlayingAndCurrentItemSetAndIsPaused_SendsAResumePlaybackMessage()
        {
            _subject.Items.Add(new PlaylistItem());
            _subject.StartPlayback(0);
            _subject.Pause();

            _subject.PlayPause();

            _mockEventAggregator.Messages.Any(m => m.GetType() == typeof(ResumePlaybackMessage)).Should().BeTrue();
        }

        [TestMethod]
        public void PlayPause_IsNotPlayingAndNoCurrentItemSet_SendsAStartPlaybackMessage()
        {
            _subject.Items.Add(new PlaylistItem());

            _subject.PlayPause();

            _mockEventAggregator.Messages.Any(m => m.GetType() == typeof(StartPlaybackMessage)).Should().BeTrue();
        }

        [TestMethod]
        public void PlayPause_IsPlaying_SendsAPausePlaybackMessage()
        {
            _subject.Items.Add(new PlaylistItem());
            _subject.StartPlayback(0);

            _subject.PlayPause();

            _mockEventAggregator.Messages.Any(m => m.GetType() == typeof(PausePlaybackMessage)).Should().BeTrue();
        }

        [TestMethod]
        public void Play_Always_SetsIsPlayingTrueAndIsPausedFalse()
        {
            _subject.Items.Add(new PlaylistItem());

            _subject.Play();

            _subject.IsPlaying.Should().BeTrue();
            _subject.IsPaused.Should().BeFalse();
        }

        [TestMethod]
        public void Play_CurrentItemSetAndIsNotPaused_SendsAStartPlaybackMessage()
        {
            _subject.Items.Add(new PlaylistItem());
            _subject.StartPlayback(0);
            _subject.StopPlayback();

            _subject.Play();

            _mockEventAggregator.Messages.Any(m => m.GetType() == typeof(StartPlaybackMessage)).Should().BeTrue();
        }

        [TestMethod]
        public void Play_CurrentItemSetAndIsPaused_SendsAResumePlaybackMessage()
        {
            _subject.Items.Add(new PlaylistItem());
            _subject.StartPlayback(0);
            _subject.Pause();

            _subject.Play();

            _mockEventAggregator.Messages.Any(m => m.GetType() == typeof(ResumePlaybackMessage)).Should().BeTrue();
        }

        [TestMethod]
        public void Play_NoCurrentItemSet_SendsAStartPlaybackMessage()
        {
            _subject.Items.Add(new PlaylistItem());

            _subject.Play();

            _mockEventAggregator.Messages.Any(m => m.GetType() == typeof(StartPlaybackMessage)).Should().BeTrue();
        }

        [TestMethod]
        public void PlaylistItemsWhenChangedFromEmptyToNotEmptyCallsEventAggregatorPublishWithShowControlsMessage()
        {
            _subject.Items.Add(new PlaylistItem());

            _mockEventAggregator.PublishCallCount.Should().Be(1);
            _mockEventAggregator.Messages.First().GetType().Should().Be(typeof(PlaylistStateChangedMessage));
        }

        [TestMethod]
        public void Resume_Always_SendsResumePlaybackMessage()
        {
            _subject.Resume();

            _mockEventAggregator.Messages.Any(m => m.GetType() == typeof(ResumePlaybackMessage)).Should().BeTrue();
        }

        [TestMethod]
        public void Resume_Always_SetsIsPlayingToTrue()
        {
            _subject.Resume();

            _subject.IsPlaying.Should().BeTrue();
        }

        [TestMethod]
        public void StartPlayback_Always_CallsStopPlaybackAction()
        {
            _subject.Items.Add(new PlaylistItem());
            var callCount = 0;
            _subject.StopPlaybackAction = () => { callCount++; };

            _subject.StartPlayback(0);

            callCount.Should().Be(1);
        }

        [TestMethod]
        public void StartPlayback_Always_SetsIsPlayingTrue()
        {
            _subject.Items.Add(new PlaylistItem());

            _subject.StartPlayback(0);

            _subject.IsPlaying.Should().BeTrue();
        }

        [TestMethod]
        public void StartPlayback_Always_SetsPlayingStateOnCurrentItemToPlaying()
        {
            _subject.Items.Add(new PlaylistItem { Type = PlaylistItemTypeEnum.Video });

            _subject.StartPlayback(0);

            _subject.CurrentItem.PlayingState.Should().Be(PlaylistItemState.Playing);
        }

        [TestMethod]
        public void StartPlayback_ItemAtGivenIndexIsAudio_SendsStartAudioPlaybackMessageWithGivenItem()
        {
            _subject.Items.AddRange(GeneratePlaylistItems());

            _subject.StartPlayback(1);

            _mockEventAggregator.Messages.Any(m => m.GetType() == typeof(StartPlaybackMessage)).Should().BeTrue();
        }

        [TestMethod]
        public void StartPlayback_ItemAtGivenIndexIsVideo_SendsStartPlaybackMessageWithGivenItem()
        {
            _subject.Items.Add(new PlaylistItem { Type = PlaylistItemTypeEnum.Video });

            _subject.StartPlayback(0);

            _mockEventAggregator.Messages.Any(m => m.GetType() == typeof(StartPlaybackMessage)).Should().BeTrue();
        }

        [TestMethod]
        public void StopPlayback_Always_SetsCurrentItemPlayingStateNotPlyaing()
        {
            _subject.Items.Add(new PlaylistItem());
            _subject.Handle(new PlayNextMessage());

            _subject.StopPlayback();

            _subject.CurrentItem.PlayingState.Should().Be(PlaylistItemState.NotPlaying);
        }

        [TestMethod]
        public void StopPlayback_Always_SetsIsPlayingFalse()
        {
            _subject.Items.Add(new PlaylistItem());

            _subject.StopPlayback();

            _subject.IsPlaying.Should().BeFalse();
        }

        [TestMethod]
        public void StopPlayback_CurrentItemIsOfTypeAudio_SendsStopAudioPlaybackMessage()
        {
            _subject.Items.Add(new PlaylistItem());
            _subject.Handle(new PlayNextMessage());

            _subject.StopPlayback();

            _mockEventAggregator.Messages.Any(m => m.GetType() == typeof(StopPlaybackMessage)).Should().BeTrue();
        }

        [TestMethod]
        public void StopPlayback_CurrentItemIsOfTypeVideo_SendsStopPlaybackMessage()
        {
            _subject.Items.Add(new PlaylistItem { Type = PlaylistItemTypeEnum.Video });
            _subject.Handle(new PlayNextMessage());

            _subject.StopPlayback();

            _mockEventAggregator.Messages.Any(m => m.GetType() == typeof(StopPlaybackMessage)).Should().BeTrue();
        }

        [TestMethod]
        public void StopPlayback_IsPaused_SetsIsPausedFalse()
        {
            _subject.Items.Add(new PlaylistItem());
            _subject.Play();
            _subject.Pause();

            _subject.StopPlayback();

            _subject.IsPaused.Should().BeFalse();
        }

        [TestMethod]
        public void StopPlayback_IsPlaying_SetsIsPlayingFalse()
        {
            _subject.Items.Add(new PlaylistItem());
            _subject.Play();

            _subject.StopPlayback();

            _subject.IsPlaying.Should().BeFalse();
        }

        [TestInitialize]
        public void TestInitialize()
        {
            _mockEventAggregator = new MockEventAggregator();
            _subject = new PlaylistManagementService(_mockEventAggregator);
        }

        #endregion

        #region Methods

        private static IEnumerable<PlaylistItem> GeneratePlaylistItems(int itemsCount = 2)
        {
            var playlistItemViewModels = new List<PlaylistItem>();
            for (var i = 0; i < itemsCount; i++)
            {
                playlistItemViewModels.Add(new PlaylistItem { Uri = new Uri(string.Format("http://file{0}", i)) });
            }

            return playlistItemViewModels;
        }

        #endregion
    }
}