namespace Client.Common.Results
{
    using System.Collections.Generic;
    using System.Linq;
    using System.Xml.Linq;
    using System.Xml.Serialization;
    using Client.Common.Models.Subsonic;
    using Client.Common.Services.DataStructures.SubsonicService;

    public class GetRandomSongsResult : ServiceResultBase<IList<Song>>, IGetRandomSongsResult
    {
        public GetRandomSongsResult(ISubsonicServiceConfiguration configuration, int numberOfSongs)
            : base(configuration)
        {
            NumberOfSongs = numberOfSongs;
        }

        public override string ResourcePath
        {
            get
            {
                return "getRandomSongs.view ";
            }
        }

        public int NumberOfSongs { get; private set; }

        public override void HandleResponse(XDocument xDocument)
        {
            var xmlSerializer = new XmlSerializer(typeof(Song));
            Result =
                xDocument.Element(Namespace + "subsonic-response")
                         .Element(Namespace + "randomSongs")
                         .Descendants(Namespace + "song")
                         .Select(
                             song =>
                             {
                                 using (var xmlReader = song.CreateReader())
                                 {
                                     return (Song)xmlSerializer.Deserialize(xmlReader);
                                 }
                             }).ToList();
        }
    }
}