using Windows.UI.ApplicationSettings;
using Windows.UI.Xaml.Navigation;
using Caliburn.Micro;

namespace Subsonic8.Menu
{
    public sealed partial class MenuView
    {
        private static bool _initialized;

        public MenuView()
        {
            InitializeComponent();

            if (_initialized) return;

            SettingsPane.GetForCurrentView().CommandsRequested += OnCommandsRequested;
            _initialized = true;
        }

        private static void OnCommandsRequested(SettingsPane sender, SettingsPaneCommandsRequestedEventArgs args)
        {
            args.AddSetting<SettingsViewModel>();
        }

        protected override void OnNavigatedTo(NavigationEventArgs e)
        {
        }
    }
}
