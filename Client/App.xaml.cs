namespace Subsonic8
{
    using Caliburn.Micro;
    using Client.Common;
    using Client.Common.Services;
    using global::Common;
    using MugenInjection;
    using SubEchoNest;
    using SubLastFm;
    using Subsonic8.Framework;
    using Subsonic8.Shell;
    using Windows.ApplicationModel;
    using Windows.ApplicationModel.Activation;
    using Windows.UI.Xaml;
    using Windows.UI.Xaml.Controls;

    public sealed partial class App
    {
        #region Fields

        private CustomFrameAdapter _navigationService;

        private IShellViewModel _shellViewModel;

        #endregion

        #region Constructors and Destructors

        public App()
        {
            InitializeComponent();
        }

        #endregion

        #region Methods

        protected override void Configure()
        {
            Kernel.Load<CommonModule>();
            Kernel.Load<SubsonicCommonModule>();
            Kernel.Load<SubLastFmModule>();
            Kernel.Load<SubEchoNestModule>();
            Kernel.Load<ClientModule>();
        }

        protected override void OnLaunched(LaunchActivatedEventArgs args)
        {
            StartApplication();
        }

        protected override void OnSearchActivated(SearchActivatedEventArgs args)
        {
            var frame = Window.Current.Content as Frame;
            if (frame == null)
            {
                StartApplication();
            }

            _shellViewModel.SendSearchQueryMessage(args.QueryText);
        }

        protected override async void OnSuspending(object sender, SuspendingEventArgs e)
        {
            var deferral = e.SuspendingOperation.GetDeferral();
            await SuspensionManager.SaveAsync();
            deferral.Complete();
        }

        private void BindShellViewModelToView(ShellView shellView)
        {
            _shellViewModel = Kernel.Get<IShellViewModel>();

            ViewModelBinder.Bind(_shellViewModel, shellView, null);
        }

        private ShellView GetShellView()
        {
            return (ShellView)RootFrame.Content;
        }

        private void RegisterNavigationService(Frame shellFrame, bool treatViewAsLoaded = false)
        {
            _navigationService = new CustomFrameAdapter(shellFrame, treatViewAsLoaded);
            Kernel.Bind<INavigationService>().ToConstant(_navigationService);
            Kernel.Bind<ICustomFrameAdapter>().ToConstant(_navigationService);
        }

        private void StartApplication()
        {
            DisplayRootView<ShellView>();

            var shellView = GetShellView();

            RegisterNavigationService(shellView.ShellFrame);

            BindShellViewModelToView(shellView);
        }

        #endregion
    }
}