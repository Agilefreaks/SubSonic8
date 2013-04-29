using System.Threading.Tasks;
using Subsonic8.Framework.Services;

namespace Client.Tests.Mocks
{
    public class MockToastNotificationService : IToastNotificationService
    {
        public int ShowCallCount { get; set; }

        public bool UseSound { get; set; }

        public Task Show(PlaybackNotificationOptions options)
        {
            ShowCallCount++;

            return new Task(() => { });
        }
    }
}
