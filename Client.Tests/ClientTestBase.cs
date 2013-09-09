namespace Client.Tests
{
    using Caliburn.Micro;
    using Client.Common.Services;
    using Client.Tests.Mocks;
    using global::Common.Mocks;
    using Subsonic8.Shell;
    using MockSubsonicService = Client.Tests.Mocks.MockSubsonicService;

    public abstract class ClientTestBase
    {
        #region Constructors and Destructors

        protected ClientTestBase()
        {
            Configure();
        }

        #endregion

        #region Methods

        protected void Configure()
        {
            var mockEventAggregator = new MockEventAggregator();
            var mockSubsonicService = new MockSubsonicService();
            var mockNavigationService = new MockNavigationService();
            var mockNotificationService = new MockToastNotificationService();
            var mockDialogNotificationService = new MockDialogNotificationService();
            var mockStorageService = new MockStorageService();
            var mockWinRTWrappersService = new MockWinRTWrappersService();
            var mockErrorDialogViewModel = new MockErrorDialogViewModel();
            var shellViewModel = new ShellViewModel
                                     {
                                         EventAggregator = mockEventAggregator,
                                         SubsonicService = mockSubsonicService,
                                         NavigationService = mockNavigationService,
                                         NotificationService = mockNotificationService,
                                         DialogNotificationService = mockDialogNotificationService,
                                         StorageService = mockStorageService,
                                         WinRTWrappersService = mockWinRTWrappersService,
                                         ErrorDialogViewModel = mockErrorDialogViewModel
                                     };

            IoC.GetInstance = (type, s) =>
                {
                    object instance = null;
                    if (type == typeof(IShellViewModel))
                    {
                        instance = shellViewModel;
                    }
                    else if (type == typeof(ISubsonicService))
                    {
                        instance = mockSubsonicService;
                    }

                    return instance;
                };
        }

        #endregion
    }
}