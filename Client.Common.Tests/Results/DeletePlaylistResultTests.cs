namespace Client.Common.Tests.Results
{
    using System.IO;
    using System.Xml.Linq;
    using Client.Common.Results;
    using Client.Common.Services.DataStructures.SubsonicService;
    using FluentAssertions;
    using Microsoft.VisualStudio.TestPlatform.UnitTestFramework;

    [TestClass]
    public class DeletePlaylistResultTests
    {
        #region Constants

        private const string Data =
            "<subsonic-response xmlns=\"http://subsonic.org/restapi\" status=\"ok\" version=\"1.8.0\"></subsonic-response>";

        #endregion

        #region Fields

        private DeletePlaylistResultWrapper _subject;

        #endregion

        #region Public Methods and Operators

        [TestInitialize]
        public void Setup()
        {
            var subsonicServiceConfiguration = new SubsonicServiceConfiguration();
            _subject = new DeletePlaylistResultWrapper(subsonicServiceConfiguration, 1);
        }

        [TestMethod]
        public void HandleResponse_ResponseIsEmpty_ReturnsTrue()
        {
            var result = new DeletePlaylistResultWrapper(new SubsonicServiceConfiguration(), 1);

            result.CallHandleResponse(XDocument.Load(new StringReader(Data)));

            result.Result.Should().BeTrue();
        }

        [TestMethod]
        public void RequestUrlShouldBeCorrect()
        {
            _subject.RequestUrl.Should().EndWith("&id=1");
        }

        [TestMethod]
        public void ViewNameShouldBegetMusicDirectory()
        {
            _subject.ResourcePath.Should().Be("deletePlaylist.view");
        }

        #endregion

        internal class DeletePlaylistResultWrapper : DeletePlaylistResult
        {
            #region Constructors and Destructors

            public DeletePlaylistResultWrapper(ISubsonicServiceConfiguration configuration, int id)
                : base(configuration, id)
            {
            }

            #endregion

            #region Public Methods and Operators

            public void CallHandleResponse(XDocument xDocument)
            {
                HandleResponse(xDocument);
            }

            #endregion
        }
    }
}