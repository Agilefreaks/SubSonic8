using System.Threading.Tasks;
using Subsonic8.Framework.Services;

namespace Client.Tests.Mocks
{
    public class MockNotificationService : IToastNotificationService
    {
        public int ShowCallCount { get; set; }

        public bool UseSound { get; set; }

        public Task Show(ToastNotificationOptions options)
        {
            ShowCallCount++;

            return new Task(() => { });
        }
    }
}
