namespace Client.Common.Tests.Results
{
    using System.IO;
    using System.Xml.Linq;
    using Client.Common.Results;
    using Client.Common.Services.DataStructures.SubsonicService;
    using FluentAssertions;
    using Microsoft.VisualStudio.TestPlatform.UnitTestFramework;

    [TestClass]
    public class GetRandomSongsResultTests
    {
        private const string Data =
            "<?xml version=\"1.0\" encoding=\"UTF-8\"?>\r\n\r\n<subsonic-response xmlns=\"http://subsonic.org/restapi\"\r\n                   status=\"ok\" version=\"1.4.0\">\r\n\r\n    <randomSongs>\r\n        <song id=\"111\" parent=\"11\" title=\"Dancing Queen\" isDir=\"false\"\r\n              album=\"Arrival\" artist=\"ABBA\" track=\"7\" year=\"1978\" genre=\"Pop\" coverArt=\"24\"\r\n              size=\"8421341\" contentType=\"audio/mpeg\" suffix=\"mp3\" duration=\"146\" bitRate=\"128\"\r\n              path=\"ABBA/Arrival/Dancing Queen.mp3\"/>\r\n\r\n        <song id=\"112\" parent=\"11\" title=\"Money, Money, Money\" isDir=\"false\"\r\n              album=\"Arrival\" artist=\"ABBA\" track=\"7\" year=\"1978\" genre=\"Pop\" coverArt=\"25\"\r\n              size=\"4910028\" contentType=\"audio/flac\" suffix=\"flac\"\r\n              transcodedContentType=\"audio/mpeg\" transcodedSuffix=\"mp3\"  duration=\"208\" bitRate=\"128\"\r\n              path=\"ABBA/Arrival/Money, Money, Money.mp3\"/>\r\n    </randomSongs>\r\n\r\n</subsonic-response>";

        private ResultWrapper _subject;

        [TestInitialize]
        public void Setup()
        {
            _subject = new ResultWrapper(new SubsonicServiceConfiguration(), 10);
        }

        [TestMethod]
        public void HandleResponse_Always_ShouldBeAbleToParseAValidResponse()
        {
            _subject.CallHandleResponse(XDocument.Load(new StringReader(Data)));

            _subject.Result.Should().NotBeNull();
            _subject.Result.Count.Should().Be(2);
            _subject.Result[0].Id.Should().Be(111);
        }

        internal class ResultWrapper : GetRandomSongsResult
        {
            public ResultWrapper(ISubsonicServiceConfiguration configuration, int numberOfSongs)
                : base(configuration, numberOfSongs)
            {
            }

            public void CallHandleResponse(XDocument xmlDocument)
            {
                HandleResponse(xmlDocument);
            }
        }
    }
}