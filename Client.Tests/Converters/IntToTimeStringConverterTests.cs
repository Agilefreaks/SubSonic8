namespace Client.Tests.Converters
{
    using FluentAssertions;
    using Microsoft.VisualStudio.TestPlatform.UnitTestFramework;
    using Subsonic8.Converters;

    [TestClass]
    public class IntToTimeStringConverterTests
    {
        #region Fields

        private IntToTimeStringConverter _subject;

        #endregion

        #region Public Methods and Operators

        [TestMethod]
        public void ConvertWhenTimeSpanContainsHoursFormattsTimeWithHours()
        {
            var result = (string)_subject.Convert(3600, null, null, null);

            result.Should().Be("1:00:00");
        }

        [TestMethod]
        public void ConvertWhenTimeSpanContainsMoreThan10HoursFormattsTimeWithHours()
        {
            var result = (string)_subject.Convert(36000, null, null, null);

            result.Should().Be("10:00:00");
        }

        [TestMethod]
        public void ConvertWhenTimeSpanDoesNotContainHoursFormattsTimeWithoutHours()
        {
            var result = (string)_subject.Convert(600, null, null, null);

            result.Should().Be("10:00");
        }

        [TestInitialize]
        public void TestInitialize()
        {
            _subject = new IntToTimeStringConverter();
        }

        #endregion
    }
}