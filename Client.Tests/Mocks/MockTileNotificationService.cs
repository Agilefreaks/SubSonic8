using System.Threading.Tasks;
using Subsonic8.Framework.Services;

namespace Client.Tests.Mocks
{
    public class MockTileNotificationService : ITileNotificationService
    {
        public int ShowCallCount { get; set; }

        public Task Show(PlaybackNotificationOptions options)
        {
            ShowCallCount++;

            return Task.Factory.StartNew(() => { });
        }
    }
}