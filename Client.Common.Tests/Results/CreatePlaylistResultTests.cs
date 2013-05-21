namespace Client.Common.Tests.Results
{
    using System.Collections.Generic;
    using System.IO;
    using System.Linq;
    using System.Xml.Linq;
    using Client.Common.Results;
    using Client.Common.Services.DataStructures.SubsonicService;
    using FluentAssertions;
    using Microsoft.VisualStudio.TestPlatform.UnitTestFramework;

    [TestClass]
    public class CreatePlaylistResultTests
    {
        #region Constants

        private const string Data =
            "<subsonic-response xmlns=\"http://subsonic.org/restapi\" status=\"ok\" version=\"1.8.0\"></subsonic-response>";

        #endregion

        #region Fields

        private List<int> _songIds;

        private CreatePlaylistResultWrapper _subject;

        #endregion

        #region Public Methods and Operators

        [TestMethod]
        public void HandleResponse_ResponseIsEmpty_ReturnsTrue()
        {
            var result = new CreatePlaylistResultWrapper(new SubsonicServiceConfiguration(), string.Empty, new int[0]);

            result.CallHandleResponse(XDocument.Load(new StringReader(Data)));

            result.Result.Should().BeTrue();
        }

        [TestMethod]
        public void RequestUrlShouldBeCorrect()
        {
            _songIds.AddRange(Enumerable.Range(0, 5));

            _subject.RequestUrl.Should().EndWith("&name=test+playlist&songId=0&songId=1&songId=2&songId=3&songId=4");
        }

        [TestInitialize]
        public void Setup()
        {
            _songIds = new List<int>();
            _subject = new CreatePlaylistResultWrapper(new SubsonicServiceConfiguration(), "test playlist", _songIds);
        }

        [TestMethod]
        public void ViewNameShouldBecreatePlaylist()
        {
            _subject.ViewName.Should().Be("createPlaylist.view");
        }

        #endregion

        internal class CreatePlaylistResultWrapper : CreatePlaylistResult
        {
            #region Constructors and Destructors

            public CreatePlaylistResultWrapper(
                ISubsonicServiceConfiguration configuration, string name, IEnumerable<int> songIds)
                : base(configuration, name, songIds)
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