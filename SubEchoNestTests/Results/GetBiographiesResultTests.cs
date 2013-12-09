namespace SubEchoNestTests.Results
{
    using System.IO;
    using System.Xml;
    using System.Xml.Linq;
    using FluentAssertions;
    using Microsoft.VisualStudio.TestPlatform.UnitTestFramework;
    using SubEchoNest;
    using SubEchoNest.Results;

    [TestClass]
    public class GetBiographiesResultTests
    {
        private static string XmlResponse
        {
            get
            {
                return @"<?xml version=""1.0"" encoding=""UTF-8""?>
<response>
    <status>
        <version>4.2</version>
        <code>0</code>
        <message>Success</message>
    </status>
    <start>0</start>
    <total>2</total>
    <biographies>
        <biography>
            <text>testContent</text>
            <site>wikipedia</site>
            <url>http://en.wikipedia.org/wiki/Iris_(American_band)</url>
            <license>
                <type>cc-by-sa</type>
                <attribution>wikipedia</attribution>
                <attribution-url>http://en.wikipedia.org/wiki/Iris_(American_band)</attribution-url>
                <url>http://creativecommons.org/licenses/by-sa/3.0/</url>
                <version>3.0</version>
            </license>
        </biography>
    </biographies>
</response>";
            }
        }

        private GetBiographiesResult _subject;

        [TestInitialize]
        public void Setup()
        {
            _subject = new GetBiographiesResult(new Configuration
            {
                ApiKey = "1234",
                BaseUrl = "http://test.com",
            }, "testArtist");
        }

        [TestMethod]
        public void ShouldBeAbleToDeserializeArtistInfo()
        {
            var xDocument = XDocument.Load(XmlReader.Create(new StringReader(XmlResponse)));
            _subject.HandleResponse(xDocument);

            var artistDetails = _subject.Result;
            artistDetails.Items.Count.Should().Be(1);
            artistDetails.Items[0].Text.Should().Be("testContent");
            artistDetails.Items[0].Site.Should().Be("wikipedia");
            artistDetails.Items[0].Url.Should().Be("http://en.wikipedia.org/wiki/Iris_(American_band)");
        }

        [TestMethod]
        public void RequestUrl_Always_ReturnsProperlyFormedUrl()
        {
            _subject.RequestUrl.Should()
                .Be("http://test.com/artist/biographies?api_key=1234&format=xml&name=testArtist&license=cc-by-sa");
        }
    }
}
