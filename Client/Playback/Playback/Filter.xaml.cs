namespace Subsonic8.Playback.Playback
{
    using global::Common.ExtensionsMethods;
    using Windows.System;
    using Windows.UI.Xaml;
    using Windows.UI.Xaml.Automation.Peers;
    using Windows.UI.Xaml.Input;

    public sealed partial class Filter
    {
        #region Constructors and Destructors

        public Filter()
        {
            InitializeComponent();
            this.RegisterDependencyPropertyChanged(() => Visibility, VisibilityChangedCallback);
        }

        #endregion

        #region Methods

        private void FilterTextBox_OnKeyUp(object sender, KeyRoutedEventArgs eventArgs)
        {
            if (eventArgs.Key == VirtualKey.Escape)
            {
                var buttonAutomationPeer = new ButtonAutomationPeer(DoneButton);
                buttonAutomationPeer.Invoke();
            }
        }

        private void VisibilityChangedCallback(DependencyPropertyChangedEventArgs dependencyPropertyChangedEventArgs)
        {
            if ((Visibility)dependencyPropertyChangedEventArgs.NewValue == Visibility.Visible)
            {
                FilterTextBox.Focus(FocusState.Programmatic);
            }
        }

        #endregion
    }
}