namespace Subsonic8.AudioPlayback
{
    using System;
    using System.Threading.Tasks;

    using Windows.UI.Core;

    using Subsonic8.Framework.Interfaces;

    public sealed partial class AudioPlayerView : IPlayerControls
    {
        #region Constructors and Destructors

        public AudioPlayerView()
        {
            InitializeComponent();
        }

        #endregion

        #region Public Methods and Operators

        public async void Pause()
        {
            await RunOnDispatcher(() => MediaElement.Pause());
        }

        public async void Play()
        {
            await RunOnDispatcher(() => MediaElement.Play());
        }

        public async void Stop()
        {
            await RunOnDispatcher(() => MediaElement.Stop());
        }

        #endregion

        #region Methods

        private async Task RunOnDispatcher(Action action)
        {
            await Dispatcher.RunAsync(CoreDispatcherPriority.Normal, () => action());
        }

        #endregion
    }
}