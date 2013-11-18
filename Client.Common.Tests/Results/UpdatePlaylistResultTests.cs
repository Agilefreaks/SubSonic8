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
    public class UpdatePlaylistResultTests
    {
        #region Constants

        private const string Data =
            "<subsonic-response xmlns=\"http://subsonic.org/restapi\" status=\"ok\" version=\"1.8.0\"></subsonic-response>";

        #endregion

        #region Fields

        private List<int> _songIdsToAdd;

        private List<int> _songIndexesToRemove;

        private UpdatePlaylistResultWrapper _subject;

        #endregion

        #region Public Methods and Operators

        [TestMethod]
        public void HandleResponse_ResponseIsEmpty_ReturnsTrue()
        {
            var result = new UpdatePlaylistResultWrapper(
                new SubsonicServiceConfiguration(), 1, _songIdsToAdd, _songIndexesToRemove);

            result.CallHandleResponse(XDocument.Load(new StringReader(Data)));

            result.Result.Should().BeTrue();
        }

        [TestMethod]
        public void RequestUrlShouldBeCorrect()
        {
            _songIdsToAdd.AddRange(Enumerable.Range(0, 2));
            _songIndexesToRemove.AddRange(Enumerable.Range(2, 2));

            _subject.RequestUrl.Should()
                    .EndWith("&playlistId=1&songIdToAdd=0&songIdToAdd=1&songIndexToRemove=2&songIndexToRemove=3");
        }

        [TestInitialize]
        public void Setup()
        {
            _songIdsToAdd = new List<int>();
            _songIndexesToRemove = new List<int>();
            _subject = new UpdatePlaylistResultWrapper(
                new SubsonicServiceConfiguration(), 1, _songIdsToAdd, _songIndexesToRemove);
        }

        [TestMethod]
        public void ViewNameShouldBeUpdatePlaylist()
        {
            _subject.ResourcePath.Should().Be("updatePlaylist.view");
        }

        #endregion

        internal class UpdatePlaylistResultWrapper : UpdatePlaylistResult
        {
            #region Constructors and Destructors

            public UpdatePlaylistResultWrapper(
                ISubsonicServiceConfiguration configuration, 
                int id, 
                IEnumerable<int> songIdsToAdd, 
                IEnumerable<int> songIndexesToRemove)
                : base(configuration, id, songIdsToAdd, songIndexesToRemove)
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