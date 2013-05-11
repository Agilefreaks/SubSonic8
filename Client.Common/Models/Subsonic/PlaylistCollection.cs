using System.Collections.Generic;
using System.Xml.Serialization;

namespace Client.Common.Models.Subsonic
{
    [XmlRoot(ElementName = "playlists", Namespace = "http://subsonic.org/restapi")]
    public class PlaylistCollection
    {
        [XmlElement(ElementName = "playlist", Namespace = "http://subsonic.org/restapi")]
        public List<Playlist> Playlists { get; set; }
    }
}