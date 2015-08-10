namespace Client.Common.Tests.Helpers
{
    using Client.Common.Helpers;
    using global::Common.Mocks;

    using Microsoft.VisualStudio.TestPlatform.UnitTestFramework;

    [TestClass]
    public class MediaControlHandlerTests
    {
        #region Fields

        private MockEventAggregator _eventAggregator;

        private MediaControlHandler _subject;

        #endregion

        #region Public Methods and Operators

        [TestInitialize]
        public void Setup()
        {
            _eventAggregator = new MockEventAggregator();
            _subject = new MediaControlHandler(_eventAggregator);
        }

        #endregion
    }
}