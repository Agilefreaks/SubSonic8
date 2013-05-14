using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Xml.Linq;
using Client.Common.Results;
using Client.Common.Services.DataStructures.SubsonicService;
using FluentAssertions;
using Microsoft.VisualStudio.TestPlatform.UnitTestFramework;

namespace Client.Common.Tests.Results
{
    [TestClass]
    public class UpdatePlaylistResultTests
    {
        internal class UpdatePlaylistResultWrapper : UpdatePlaylistResult
        {
            public UpdatePlaylistResultWrapper(ISubsonicServiceConfiguration configuration, int id, IEnumerable<int> songIdsToAdd, IEnumerable<int> songIndexesToRemove)
                : base(configuration, id, songIdsToAdd, songIndexesToRemove)
            {
            }

            public void CallHandleResponse(XDocument xDocument)
            {
                HandleResponse(xDocument);
            }
        }

        private const string Data =
            "<subsonic-response xmlns=\"http://subsonic.org/restapi\" status=\"ok\" version=\"1.8.0\"></subsonic-response>";

        private UpdatePlaylistResultWrapper _subject;
        private List<int> _songIdsToAdd;
        private List<int> _songIndexesToRemove;

        [TestInitialize]
        public void Setup()
        {
            _songIdsToAdd = new List<int>();
            _songIndexesToRemove = new List<int>();
            _subject = new UpdatePlaylistResultWrapper(new SubsonicServiceConfiguration(), 1, _songIdsToAdd, _songIndexesToRemove);
        }

        [TestMethod]
        public void ViewNameShouldBeupdatePlaylist()
        {
            _subject.ViewName.Should().Be("updatePlaylist.view");
        }

        [TestMethod]
        public void RequestUrlShouldBeCorrect()
        {
            _songIdsToAdd.AddRange(Enumerable.Range(0, 2));
            _songIndexesToRemove.AddRange(Enumerable.Range(2, 2));

            _subject.RequestUrl.Should().EndWith("&playlistId=1&songIdToAdd=0&songIdToAdd=1&songIndexToRemove=2&songIndexToRemove=3");
        }

        [TestMethod]
        public void HandleResponse_ResponseIsEmpty_ReturnsTrue()
        {
            var result = new UpdatePlaylistResultWrapper(new SubsonicServiceConfiguration(), 1, _songIdsToAdd, _songIndexesToRemove);

            result.CallHandleResponse(XDocument.Load(new StringReader(Data)));

            result.Result.Should().BeTrue();
        }
    }
}