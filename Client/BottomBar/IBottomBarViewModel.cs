namespace Subsonic8.BottomBar
{
    using System.Collections.ObjectModel;
    using Caliburn.Micro;
    using Client.Common.EventAggregatorMessages;

    public interface IBottomBarViewModel : IHandle<PlaylistStateChangedMessage>
    {
        #region Public Properties

        bool DisplayPlayControls { get; }

        bool IsOpened { get; set; }

        bool IsPlaying { get; }

        ObservableCollection<object> SelectedItems { get; set; }

        #endregion

        #region Public Methods and Operators

        void NavigateToRoot();

        void PlayNext();

        void PlayPause();

        void PlayPrevious();

        void Stop();

        void ToggleShuffle();

        #endregion
    }
}