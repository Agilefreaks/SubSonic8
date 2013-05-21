namespace Client.Common.Tests.Results
{
    using System.Collections.Generic;
    using System.IO;
    using System.Xml.Linq;
    using Client.Common.Results;
    using Client.Common.Services.DataStructures.SubsonicService;
    using FluentAssertions;
    using Microsoft.VisualStudio.TestPlatform.UnitTestFramework;

    [TestClass]
    public class RenamePlaylistResultTests
    {
        #region Constants

        private const string Data =
            "<subsonic-response xmlns=\"http://subsonic.org/restapi\" status=\"ok\" version=\"1.8.0\"></subsonic-response>";

        #endregion

        #region Fields

        private List<int> _songIdsToAdd;

        private List<int> _songIndexesToRemove;

        private RenamePlaylistResultWrapper _subject;

        #endregion

        #region Public Methods and Operators

        [TestMethod]
        public void HandleResponse_ResponseIsEmpty_ReturnsTrue()
        {
            var result = new UpdatePlaylistResultTests.UpdatePlaylistResultWrapper(
                new SubsonicServiceConfiguration(), 1, _songIdsToAdd, _songIndexesToRemove);

            result.CallHandleResponse(XDocument.Load(new StringReader(Data)));

            result.Result.Should().BeTrue();
        }

        [TestMethod]
        public void RequestUrlShouldBeCorrect()
        {
            _subject.RequestUrl.Should().EndWith("&playlistId=1&name=test+a");
        }

        [TestInitialize]
        public void Setup()
        {
            _songIdsToAdd = new List<int>();
            _songIndexesToRemove = new List<int>();
            _subject = new RenamePlaylistResultWrapper(new SubsonicServiceConfiguration(), 1, "test a");
        }

        [TestMethod]
        public void ViewNameShouldBeupdatePlaylist()
        {
            _subject.ViewName.Should().Be("updatePlaylist.view");
        }

        #endregion

        internal class RenamePlaylistResultWrapper : RenamePlaylistResult
        {
            #region Constructors and Destructors

            public RenamePlaylistResultWrapper(ISubsonicServiceConfiguration configuration, int id, string name)
                : base(configuration, id, name)
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