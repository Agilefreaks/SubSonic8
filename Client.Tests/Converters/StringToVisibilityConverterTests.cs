namespace Client.Tests.Converters
{
    using Windows.UI.Xaml;

    using FluentAssertions;

    using Microsoft.VisualStudio.TestPlatform.UnitTestFramework;

    using Subsonic8.Converters;

    [TestClass]
    public class StringToVisibilityConverterTests
    {
        private StringToVisibilityConverter _subject;

        [TestInitialize]
        public void Setup()
        {
            _subject = new StringToVisibilityConverter();
        }

        [TestMethod]
        public void Convert_ValueIsAnEmptyString_ReturnsVisibilityCollapsed()
        {
            var result = _subject.Convert(string.Empty, null, null, null);

            result.Should().Be(Visibility.Collapsed);
        }
    }
}