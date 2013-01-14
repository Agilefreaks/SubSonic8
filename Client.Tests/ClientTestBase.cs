using Caliburn.Micro;
using Client.Common.Services;
using Client.Tests.Mocks;
using Subsonic8.Shell;

namespace Client.Tests
{
    public abstract class ClientTestBase
    {
        protected ClientTestBase()
        {
            Configure();
        }

        protected void Configure()
        {
            var mockEventAggregator = new MockEventAggregator();
            var mockSubsonicService = new MockSubsonicService();
            var mockNavigationService = new MockNavigationService();
            var mockNotificationService = new MockNotificationService();
            var mockDialogNotificationService = new MockDialogNotificationService();
            var mockStorageService = new MockStorageService();
            var mockWinRTWrappersService = new MockWinRTWrappersService();
            var shellViewModel = new ShellViewModel(mockEventAggregator, mockSubsonicService, mockNavigationService,
                mockNotificationService, mockDialogNotificationService, mockStorageService, mockWinRTWrappersService);

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
    }
}
