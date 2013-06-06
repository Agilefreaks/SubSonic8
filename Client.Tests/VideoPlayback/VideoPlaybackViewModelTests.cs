namespace Client.Tests.VideoPlayback
{
    using Client.Tests.Framework.ViewModel;
    using Client.Tests.Mocks;
    using Microsoft.VisualStudio.TestPlatform.UnitTestFramework;
    using Subsonic8.VideoPlayback;

    [TestClass]
    public class VideoPlaybackViewModelTests : ViewModelBaseTests<VideoPlaybackViewModel>
    {
        private MockToastNotificationService _mockToastNotificationService;

        protected override void TestInitializeExtensions()
        {
            base.TestInitializeExtensions();
            _mockToastNotificationService = new MockToastNotificationService();
            Subject.ToastNotificationService = _mockToastNotificationService;
        }
    }
}