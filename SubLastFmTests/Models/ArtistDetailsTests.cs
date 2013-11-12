namespace SubLastFmTests.Models
{
    using System;
    using System.IO;
    using System.Linq;
    using System.Xml;
    using System.Xml.Linq;
    using System.Xml.Serialization;
    using FluentAssertions;
    using Microsoft.VisualStudio.TestPlatform.UnitTestFramework;
    using SubLastFm.Models;

    [TestClass]
    public class ArtistDetailsTests
    {
        private static string XmlResponse
        {
            get
            {
                return @"<?xml version=""1.0"" encoding=""utf-8""?>
<lfm status=""ok"">
<artist>
  <name>Muse</name>
          <mbid>1695c115-bf3f-4014-9966-2b0c50179193</mbid>
            <bandmembers>
                    <member>
                <name>Matthew Bellamy</name>
                                            </member>
                    <member>
                <name>Chris Wolstenholme</name>
                                            </member>
                    <member>
                <name>Dominic Howard</name>
                                            </member>
            </bandmembers>
        <url>http://www.last.fm/music/Muse</url>
  <image size=""small"">http://userserve-ak.last.fm/serve/34/210128.jpg</image>
  <image size=""medium"">http://userserve-ak.last.fm/serve/64/210128.jpg</image>
  <image size=""large"">http://userserve-ak.last.fm/serve/126/210128.jpg</image>
  <image size=""extralarge"">http://userserve-ak.last.fm/serve/252/210128.jpg</image>
  <image size=""mega"">http://userserve-ak.last.fm/serve/500/210128/Muse.jpg</image>
  <streamable>1</streamable>
  <ontour>1</ontour>
  <stats>
    <listeners>3603651</listeners>
    <playcount>281733911</playcount>
      </stats>

  <similar>
	    <artist>
	  <name>Franz Ferdinand</name>
	  <url>http://www.last.fm/music/Franz+Ferdinand</url>
	  <image size=""small"">http://userserve-ak.last.fm/serve/34/4180192.jpg</image>
	  <image size=""medium"">http://userserve-ak.last.fm/serve/64/4180192.jpg</image>
	  <image size=""large"">http://userserve-ak.last.fm/serve/126/4180192.jpg</image>
	  <image size=""extralarge"">http://userserve-ak.last.fm/serve/252/4180192.jpg</image>
	  <image size=""mega"">http://userserve-ak.last.fm/serve/_/4180192/Franz+Ferdinand.jpg</image>
	  
	        	</artist>
    <artist>
	  <name>30 Seconds to Mars</name>
	  <url>http://www.last.fm/music/30+Seconds+to+Mars</url>
	  <image size=""small"">http://userserve-ak.last.fm/serve/34/47014513.png</image>
	  <image size=""medium"">http://userserve-ak.last.fm/serve/64/47014513.png</image>
	  <image size=""large"">http://userserve-ak.last.fm/serve/126/47014513.png</image>
	  <image size=""extralarge"">http://userserve-ak.last.fm/serve/252/47014513.png</image>
	  <image size=""mega"">http://userserve-ak.last.fm/serve/500/47014513/30+Seconds+to+Mars+TSTMPNG1.png</image>
	  
	        	</artist>
    <artist>
	  <name>Radiohead</name>
	  <url>http://www.last.fm/music/Radiohead</url>
	  <image size=""small"">http://userserve-ak.last.fm/serve/34/30263.jpg</image>
	  <image size=""medium"">http://userserve-ak.last.fm/serve/64/30263.jpg</image>
	  <image size=""large"">http://userserve-ak.last.fm/serve/126/30263.jpg</image>
	  <image size=""extralarge"">http://userserve-ak.last.fm/serve/252/30263.jpg</image>
	  <image size=""mega"">http://userserve-ak.last.fm/serve/_/30263/Radiohead.jpg</image>
	  
	        	</artist>
    <artist>
	  <name>Coldplay</name>
	  <url>http://www.last.fm/music/Coldplay</url>
	  <image size=""small"">http://userserve-ak.last.fm/serve/34/28914.jpg</image>
	  <image size=""medium"">http://userserve-ak.last.fm/serve/64/28914.jpg</image>
	  <image size=""large"">http://userserve-ak.last.fm/serve/126/28914.jpg</image>
	  <image size=""extralarge"">http://userserve-ak.last.fm/serve/252/28914.jpg</image>
	  <image size=""mega"">http://userserve-ak.last.fm/serve/_/28914/Coldplay.jpg</image>
	  
	        	</artist>
    <artist>
	  <name>Placebo</name>
	  <url>http://www.last.fm/music/Placebo</url>
	  <image size=""small"">http://userserve-ak.last.fm/serve/34/28534563.jpg</image>
	  <image size=""medium"">http://userserve-ak.last.fm/serve/64/28534563.jpg</image>
	  <image size=""large"">http://userserve-ak.last.fm/serve/126/28534563.jpg</image>
	  <image size=""extralarge"">http://userserve-ak.last.fm/serve/252/28534563.jpg</image>
	  <image size=""mega"">http://userserve-ak.last.fm/serve/500/28534563/Placebo+threesome.jpg</image>
	  
	        	</artist>
  </similar>
    <tags>
        <tag>
	  <name>alternative rock</name>
	  <url>http://www.last.fm/tag/alternative%20rock</url>
	</tag>
        <tag>
	  <name>rock</name>
	  <url>http://www.last.fm/tag/rock</url>
	</tag>
        <tag>
	  <name>alternative</name>
	  <url>http://www.last.fm/tag/alternative</url>
	</tag>
        <tag>
	  <name>progressive rock</name>
	  <url>http://www.last.fm/tag/progressive%20rock</url>
	</tag>
        <tag>
	  <name>indie</name>
	  <url>http://www.last.fm/tag/indie</url>
	</tag>
      </tags>
      <bio>
        <links>
        <link rel=""original"" href=""http://www.last.fm/music/Muse/+wiki"" />
    </links>
                                    <published>Fri, 7 Sep 2012 23:52:20 +0000</published>
    <summary>
        <![CDATA[        Muse are an alternative rock band from Teignmouth, England, United Kingdom. The band consists of <a href=""http://www.last.fm/music/Matthew+Bellamy"" class=""bbcode_artist"">Matthew Bellamy</a> on lead vocals, piano, keyboard and guitar, <a href=""http://www.last.fm/music/Chris+Wolstenholme"" class=""bbcode_artist"">Chris Wolstenholme</a> on backing vocals and bass guitar, and <a href=""http://www.last.fm/music/Dominic+Howard"" class=""bbcode_artist"">Dominic Howard</a> on drums and percussion.    They have been friends since their formation in early 1994 and changed band names a number of times (such as Gothic Plague, Fixed Penalty, and Rocket Baby Dolls) before adopting the name Muse.

        <a href=""http://www.last.fm/music/Muse"">Read more about Muse on Last.fm</a>.
    ]]>
    </summary>
    <content>
            <![CDATA[        Muse are an alternative rock band from Teignmouth, England, United Kingdom. The band consists of <a href=""http://www.last.fm/music/Matthew+Bellamy"" class=""bbcode_artist"">Matthew Bellamy</a> on lead vocals, piano, keyboard and guitar, <a href=""http://www.last.fm/music/Chris+Wolstenholme"" class=""bbcode_artist"">Chris Wolstenholme</a> on backing vocals and bass guitar, and <a href=""http://www.last.fm/music/Dominic+Howard"" class=""bbcode_artist"">Dominic Howard</a> on drums and percussion.    They have been friends since their formation in early 1994 and changed band names a number of times (such as Gothic Plague, Fixed Penalty, and Rocket Baby Dolls) before adopting the name Muse.

        <a href=""http://www.last.fm/music/Muse"">Read more about Muse on Last.fm</a>.
    
    
User-contributed text is available under the Creative Commons By-SA License and may also be available under the GNU FDL.]]>
    </content>
            <placeformed>Teignmouth, England, United Kingdom</placeformed>                        <yearformed>1994</yearformed>
            <formationlist>
                    <formation>
                <yearfrom>1994</yearfrom>
                <yearto></yearto>
            </formation>
            </formationlist>
      </bio>
  </artist></lfm>
";
            }
        }

        private ArtistDetails _subject;

        [TestInitialize]
        public void Setup()
        {
            _subject = new ArtistDetails();
        }

        [TestMethod]
        public void ShouldBeAbleToDeserializeArtistInfo()
        {
            var xDocument = XDocument.Load(XmlReader.Create(new StringReader(XmlResponse)));
            var element = xDocument.Element("lfm").Descendants().First();
            var xmlSerializer = new XmlSerializer(typeof(ArtistDetails), new[] { typeof(Band), typeof(BandMember), typeof(Image), typeof(TagList), typeof(Tag) });
            var result = xmlSerializer.Deserialize(element.CreateReader());

            var artistDetails = result as ArtistDetails;
            Assert.IsNotNull(artistDetails);
            artistDetails.Name.Should().Be("Muse");
            artistDetails.Band.Should().NotBeNull();
            artistDetails.Band.Members.Count.Should().Be(3);
            artistDetails.Band.Members[0].Name.Should().Be("Matthew Bellamy");
            artistDetails.Band.Members[1].Name.Should().Be("Chris Wolstenholme");
            artistDetails.Band.Members[2].Name.Should().Be("Dominic Howard");
            artistDetails.Url.Should().Be(new Uri("http://www.last.fm/music/Muse"));
            artistDetails.Images.Should().NotBeNull();
            artistDetails.Images.Count.Should().Be(5);
            artistDetails.Images[0].Size.Should().Be(ImageSizeEnum.Small);
            artistDetails.Images[0].Url.Should().Be(new Uri("http://userserve-ak.last.fm/serve/34/210128.jpg"));
            artistDetails.Images[1].Size.Should().Be(ImageSizeEnum.Medium);
            artistDetails.Images[1].Url.Should().Be(new Uri("http://userserve-ak.last.fm/serve/64/210128.jpg"));
            artistDetails.Images[2].Size.Should().Be(ImageSizeEnum.Large);
            artistDetails.Images[2].Url.Should().Be(new Uri("http://userserve-ak.last.fm/serve/126/210128.jpg"));
            artistDetails.Images[3].Size.Should().Be(ImageSizeEnum.ExtraLarge);
            artistDetails.Images[3].Url.Should().Be(new Uri("http://userserve-ak.last.fm/serve/252/210128.jpg"));
            artistDetails.Images[4].Size.Should().Be(ImageSizeEnum.Mega);
            artistDetails.Images[4].Url.Should().Be(new Uri("http://userserve-ak.last.fm/serve/500/210128/Muse.jpg"));
            artistDetails.Tags.Should().NotBeNull();
            artistDetails.Tags.Items.Count().Should().Be(5);
            artistDetails.Tags.Items[0].Name.Should().Be("alternative rock");
            artistDetails.Tags.Items[1].Name.Should().Be("rock");
            artistDetails.Tags.Items[2].Name.Should().Be("alternative");
            artistDetails.Tags.Items[3].Name.Should().Be("progressive rock");
            artistDetails.Tags.Items[4].Name.Should().Be("indie");
        }

        [TestMethod]
        public void LargestImage_ImagesIsEmpty_ReturnsNull()
        {
            _subject.LargestImage().Should().BeNull();
        }

        [TestMethod]
        public void LargestImage_ImagesHasOneElement_ReturnsThatImage()
        {
            var image = new Image();
            _subject.Images.Add(image);

            _subject.LargestImage().Should().Be(image);
        }

        [TestMethod]
        public void LargestImage_ImagesHasMoreThenOneElement_ReturnsTheImageWithTheHighestSize()
        {
            var image1 = new Image { Size = ImageSizeEnum.Medium };
            var image2 = new Image { Size = ImageSizeEnum.Large };
            var image3 = new Image { Size = ImageSizeEnum.Small };
            var image4 = new Image { Size = ImageSizeEnum.Mega };
            var image5 = new Image { Size = ImageSizeEnum.ExtraLarge };
            _subject.Images.AddRange(new[] { image1, image2, image3, image4, image5 });

            _subject.LargestImage().Should().Be(image4);
        }
    }
}
