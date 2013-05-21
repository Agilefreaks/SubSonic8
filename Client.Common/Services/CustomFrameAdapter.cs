namespace Client.Common.Services
{
    using Caliburn.Micro;
    using Windows.UI.Xaml.Controls;
    using Windows.UI.Xaml.Navigation;

    public class CustomFrameAdapter : FrameAdapter, ICustomFrameAdapter
    {
        #region Constructors and Destructors

        public CustomFrameAdapter(Frame frame, bool treatViewAsLoaded = false)
            : base(frame, treatViewAsLoaded)
        {
        }

        #endregion

        #region Public Methods and Operators

        public void DoNavigated(object sender, NavigationEventArgs eventArgs)
        {
            OnNavigated(sender, eventArgs);
        }

        #endregion

        #region Explicit Interface Methods

        void ICustomFrameAdapter.NavigateToViewModel<T>(object parameter)
        {
            this.NavigateToViewModel<T>(parameter);
        }

        #endregion
    }
}