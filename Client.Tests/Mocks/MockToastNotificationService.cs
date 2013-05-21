namespace Client.Tests.Mocks
{
    using System.Threading.Tasks;
    using Subsonic8.Framework.Services;

    public class MockToastNotificationService : IToastNotificationService
    {
        #region Public Properties

        public int ShowCallCount { get; set; }

        public bool UseSound { get; set; }

        #endregion

        #region Public Methods and Operators

        public Task Show(PlaybackNotificationOptions options)
        {
            ShowCallCount++;

            return Task.Factory.StartNew(() => { });
        }

        #endregion
    }
}