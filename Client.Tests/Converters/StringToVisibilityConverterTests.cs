namespace Client.Tests.Converters
{
    using System;
    using Windows.UI.Xaml;
    using FluentAssertions;
    using Microsoft.VisualStudio.TestPlatform.UnitTestFramework;
    using Subsonic8.Converters;

    [TestClass]
    public class StringToVisibilityConverterTests
    {
        #region Fields

        private StringToVisibilityConverter _subject;

        #endregion

        #region Public Methods and Operators

        [TestInitialize]
        public void TestInitialize()
        {
            _subject = new StringToVisibilityConverter();
        }

        [TestMethod]
        public void ConvertWhenArgumentIsNullReturnsCollapsed()
        {
            var result = (Visibility)_subject.Convert(null, null, null, null);

            result.Should().Be(Visibility.Collapsed);
        }

        [TestMethod]
        public void ConvertWhenArgumentIsEmptyStringReturnsCollapsed()
        {
            var result = (Visibility)_subject.Convert(string.Empty, null, null, null);

            result.Should().Be(Visibility.Collapsed);
        }

        [TestMethod]
        public void ConvertWhenArgumentIsNonEmptyStringReturnsVisible()
        {
            // Tip: You will have to write this test to verify that when 
            // you pass a non empty string as value argument
            // you will receive as output the Visibility.Visible value (See examples above)
            throw new NotImplementedException();
        }
        
        #endregion
    }
}
