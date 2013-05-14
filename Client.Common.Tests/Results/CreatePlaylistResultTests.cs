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
    public class CreatePlaylistResultTests
    {
        internal class CreatePlaylistResultWrapper : CreatePlaylistResult
        {
            public CreatePlaylistResultWrapper(ISubsonicServiceConfiguration configuration, string name, IEnumerable<int> songIds)
                : base(configuration, name, songIds)
            {
            }

            public void CallHandleResponse(XDocument xDocument)
            {
                HandleResponse(xDocument);
            }
        }

        private const string Data =
            "<subsonic-response xmlns=\"http://subsonic.org/restapi\" status=\"ok\" version=\"1.8.0\"></subsonic-response>";

        private CreatePlaylistResultWrapper _subject;
        private List<int> _songIds;

        [TestInitialize]
        public void Setup()
        {
            _songIds = new List<int>();
            _subject = new CreatePlaylistResultWrapper(new SubsonicServiceConfiguration(), "test playlist", _songIds);
        }

        [TestMethod]
        public void ViewNameShouldBesavePlaylist()
        {
            _subject.ViewName.Should().Be("savePlaylist.view");
        }

        [TestMethod]
        public void RequestUrlShouldBeCorrect()
        {
            _songIds.AddRange(Enumerable.Range(0, 5));

            _subject.RequestUrl.Should().EndWith("&name=test+playlist&songId=0&songId=1&songId=2&songId=3&songId=4");
        }

        [TestMethod]
        public void HandleResponse_ResponseIsEmpty_ReturnsTrue()
        {
            var result = new CreatePlaylistResultWrapper(new SubsonicServiceConfiguration(), string.Empty, new int[0]);

            result.CallHandleResponse(XDocument.Load(new StringReader(Data)));

            result.Result.Should().BeTrue();
        }
    }
}