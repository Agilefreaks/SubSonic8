namespace Client.Tests.Framework.Services
{
    using FluentAssertions;
    using Microsoft.VisualStudio.TestPlatform.UnitTestFramework;
    using Subsonic8.Framework.Services;

    [TestClass]
    public class NotificationOptionsTests
    {
        #region Public Methods and Operators

        [TestMethod]
        public void SubtitleRerturnsValueSet()
        {
            const string Subtitle = "subtitle";
            var options = new PlaybackNotificationOptions { Subtitle = Subtitle };

            options.Subtitle.Should().Be(Subtitle);
        }

        [TestMethod]
        public void SubtitleReturnsUnknownIfSetToEmpty()
        {
            var options = new PlaybackNotificationOptions { Subtitle = string.Empty };

            options.Subtitle.Should().Be("Unknown");
        }

        [TestMethod]
        public void SubtitleReturnsUnknownIfSetToNull()
        {
            var options = new PlaybackNotificationOptions { Subtitle = null };

            options.Subtitle.Should().Be("Unknown");
        }

        [TestMethod]
        public void TitleRerturnsValueSet()
        {
            const string Title = "title";
            var options = new PlaybackNotificationOptions { Title = Title };

            options.Title.Should().Be(Title);
        }

        [TestMethod]
        public void TitleReturnsUnknownIfItIsSetToEmpty()
        {
            var options = new PlaybackNotificationOptions { Title = string.Empty };

            options.Title.Should().Be("Unknown");
        }

        [TestMethod]
        public void TitleReturnsUnknownIfItIsSetToNull()
        {
            var options = new PlaybackNotificationOptions { Title = null };

            options.Title.Should().Be("Unknown");
        }

        #endregion
    }
}