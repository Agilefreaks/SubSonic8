namespace Client.Tests.VideoPlayback
{
    using Client.Tests.Framework.ViewModel;
    using Client.Tests.Mocks;
    using Microsoft.VisualStudio.TestPlatform.UnitTestFramework;
    using Subsonic8.VideoPlayback;

    [TestClass]
    public class VideoPlaybackViewModelTests : ViewModelBaseTests<VideoPlaybackViewModel>
    {
        #region Fields

        private MockToastNotificationService _mockToastNotificationService;

        #endregion

        #region Methods

        protected override void TestInitializeExtensions()
        {
            base.TestInitializeExtensions();
            _mockToastNotificationService = new MockToastNotificationService();
            Subject.ToastNotificationService = _mockToastNotificationService;
        }

        #endregion
    }
}