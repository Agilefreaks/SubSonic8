using Subsonic8.Framework.Services;

namespace Client.Tests.Mocks
{
    public class MockNotificationService : INotificationService
    {
        public int ShowCallCount { get; set; }

        public void Show(NotificationOptions options)
        {
            ShowCallCount++;
        }

        public bool UseSound { get; set; }
    }
}
