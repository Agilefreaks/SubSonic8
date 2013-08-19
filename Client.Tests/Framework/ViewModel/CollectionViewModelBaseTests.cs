namespace Client.Tests.Framework.ViewModel
{
    using System;
    using System.Linq;
    using System.Threading.Tasks;
    using Client.Common.EventAggregatorMessages;
    using Client.Common.Models;
    using Client.Tests.Mocks;
    using FluentAssertions;
    using Microsoft.VisualStudio.TestPlatform.UnitTestFramework;
    using Subsonic8.Framework.ViewModel;
    using Subsonic8.Playback;

    [TestClass]
    public abstract class CollectionViewModelBaseTests<TViewModel, TParameter> : ViewModelBaseTests<TViewModel>
        where TViewModel : ICollectionViewModel<TParameter>, new()
    {
        #region Properties

        protected MockDefaultBottomBarViewModel MockDefaultBottomBar { get; set; }

        #endregion

        #region Public Methods and Operators

        [TestMethod]
        public void SelectedItemWhenBottomBarIsNillShouldNotThrowException()
        {
            Subject.BottomBar = null;

            Subject.SelectedItems.Should().NotBeNull();
        }

        [DataTestMethod]
        [DataRow(SubsonicModelTypeEnum.Song)]
        [DataRow(SubsonicModelTypeEnum.Video)]
        public async Task HandleItemSelection_ModelTypeIsSongOrVideo_ShouldTryToLoadThePlaylistItemForTheGivenItem(SubsonicModelTypeEnum type)
        {
            var callCount = 0;
            MockLoadModel(() =>
                {
                    callCount++;
                    return new PlaylistItem();
                });

            await Task.Run(() => Subject.HandleItemSelection(new MockSubsonicModel { Type = type }));

            callCount.Should().Be(1);
        }

        [DataTestMethod]
        [DataRow(SubsonicModelTypeEnum.Song)]
        [DataRow(SubsonicModelTypeEnum.Video)]
        public async Task HandleItemSelection_ModelTypeIsSong_ShouldTryToPublishAnAddItemMessageWithTheObtainedPlaylistItemAndStartPlayingTrue(SubsonicModelTypeEnum type)
        {
            MockEventAggregator.Messages.Clear();
            var playlistItem = new PlaylistItem();
            MockLoadModel(() => playlistItem);

            await Task.Run(() => Subject.HandleItemSelection(new MockSubsonicModel { Type = type }));

            MockEventAggregator.PublishCallCount.Should().Be(1);
            MockEventAggregator.Messages[0].Should().BeOfType<AddItemsMessage>();
            ((AddItemsMessage)MockEventAggregator.Messages[0]).Queue.Count.Should().Be(1);
            ((AddItemsMessage)MockEventAggregator.Messages[0]).Queue[0].Should().Be(playlistItem);
            ((AddItemsMessage)MockEventAggregator.Messages[0]).StartPlaying.Should().BeTrue();
        }

        [DataTestMethod]
        [DataRow(SubsonicModelTypeEnum.Song)]
        [DataRow(SubsonicModelTypeEnum.Video)]
        public async Task HandleItemSelection_ItemIsASong_ShouldTryToNavigateToThePlaybackViewModel(SubsonicModelTypeEnum type)
        {
            MockNavigationService.NavigateToViewModelCalls.Clear();
            MockLoadModel();

            await Task.Run(() => Subject.HandleItemSelection(new MockSubsonicModel { Type = type }));

            MockNavigationService.NavigateToViewModelCalls.Count.Should().Be(1);
            MockNavigationService.NavigateToViewModelCalls.First().Key.Should().Be<PlaybackViewModel>();
        }

        #endregion

        #region Methods

        protected override void TestInitializeExtensions()
        {
            MockDefaultBottomBar = new MockDefaultBottomBarViewModel();
            Subject.BottomBar = MockDefaultBottomBar;
            Subject.LoadPlaylistItem = model =>
                {
                    var tcr = new TaskCompletionSource<PlaylistItem>();
                    tcr.SetResult(new PlaylistItem());
                    return tcr.Task;
                };
        }

        private void MockLoadModel(Func<PlaylistItem> callBack = null)
        {
            Subject.LoadPlaylistItem = model =>
                {
                    var playlistItem = callBack != null
                                           ? callBack()
                                           : new PlaylistItem
                                                 {
                                                     PlayingState = PlaylistItemState.NotPlaying,
                                                     Uri = new Uri("http://test-uri"),
                                                     Artist = "test-artist"
                                                 };
                    var tcr = new TaskCompletionSource<PlaylistItem>();
                    tcr.SetResult(playlistItem);
                    return tcr.Task;
                };
        }

        #endregion
    }
}