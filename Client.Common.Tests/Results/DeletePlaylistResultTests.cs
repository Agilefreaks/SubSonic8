using System.IO;
using System.Xml.Linq;
using Client.Common.Results;
using Client.Common.Services.DataStructures.SubsonicService;
using FluentAssertions;
using Microsoft.VisualStudio.TestPlatform.UnitTestFramework;

namespace Client.Common.Tests.Results
{
    [TestClass]
    public class DeletePlaylistResultTests
    {
        internal class DeletePlaylistResultWrapper : DeletePlaylistResult
        {
            public DeletePlaylistResultWrapper(ISubsonicServiceConfiguration configuration, int id)
                : base(configuration, id)
            {
            }

            public void CallHandleResponse(XDocument xDocument)
            {
                HandleResponse(xDocument);
            }
        }

        protected readonly XNamespace Namespace = "http://subsonic.org/restapi";

        private DeletePlaylistResultWrapper _subject;
        private const string Data =
            "<subsonic-response xmlns=\"http://subsonic.org/restapi\" status=\"ok\" version=\"1.8.0\"></subsonic-response>";

        [TestInitialize]
        public void Setup()
        {
            _subject = new DeletePlaylistResultWrapper(new SubsonicServiceConfiguration(), 1);
        }

        [TestMethod]
        public void ViewNameShouldBegetMusicDirectory()
        {
            _subject.ViewName.Should().Be("deletePlaylist.view");
        }

        [TestMethod]
        public void RequestUrlShouldBeCorrect()
        {
            _subject.RequestUrl.Should().EndWith("&id=1");
        }

        [TestMethod]
        public void HandleResponse_Always_CanDeserializeAPlaylistEntryProperly()
        {
            var result = new DeletePlaylistResultWrapper(new SubsonicServiceConfiguration(), 1);

            result.CallHandleResponse(XDocument.Load(new StringReader(Data)));

            result.Result.Should().BeTrue();
        }
    }
}