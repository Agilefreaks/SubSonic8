using Subsonic8.Framework;

namespace Client.Tests.Mocks
{
    public class MockNotificationManager : INotificationManager
    {
        public int ShowCallCount { get; set; }

        public void Show(NotificationOptions options)
        {
            ShowCallCount++;
        }
    }
}
