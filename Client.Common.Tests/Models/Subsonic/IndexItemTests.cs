using System.Collections.Generic;
using Client.Common.Models.Subsonic;
using FluentAssertions;
using Microsoft.VisualStudio.TestPlatform.UnitTestFramework;

namespace Client.Common.Tests.Models.Subsonic
{
    [TestClass]
    public class IndexItemTests
    {
        [TestMethod]
        public void Deserialize_Always_ShouldBeAbleToDeserializeASerializedItem()
        {
            var indexItem = new IndexItem
                {
                    Artists = new List<Artist> { new Artist { Id = 4, Name = "a", CoverArt = "b" } },
                    Id = 3,
                    Name = "test",
                    CoverArt = "asdas"
                };
            var serializedItem = indexItem.Serialize();

            var deserializedItem = IndexItem.Deserialize(serializedItem);

            deserializedItem.Id.Should().Be(3);
            deserializedItem.Name.Should().Be("test");
            deserializedItem.CoverArt.Should().Be("asdas");
            deserializedItem.Artists.Count.Should().Be(1);
            deserializedItem.Artists[0].Id.Should().Be(4);
            deserializedItem.Artists[0].Name.Should().Be("a");
            deserializedItem.Artists[0].CoverArt.Should().Be("b");
        }
    }
}
