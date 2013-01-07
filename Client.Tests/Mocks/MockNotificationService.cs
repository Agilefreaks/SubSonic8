using Subsonic8.Framework.Services;

namespace Client.Tests.Mocks
{
    public class MockNotificationService : IToastNotificationService
    {
        public int ShowCallCount { get; set; }

        public void Show(ToastNotificationOptions options)
        {
            ShowCallCount++;
        }

        public bool UseSound { get; set; }
    }
}
