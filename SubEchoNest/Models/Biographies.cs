namespace SubEchoNest.Models
{
    using System.Collections.Generic;
    using System.Linq;
    using System.Xml.Serialization;

    [XmlRoot("biographies")]
    public class Biographies
    {
        private const string LastFmSiteName = "last.fm";
        private const string WikipediaSiteName = "wikipedia";

        public Biographies()
        {
            Items = new List<Biography>();
        }

        [XmlElement("biography")]
        public List<Biography> Items { get; set; }

        [XmlIgnore]
        public Biography PreferredBiography
        {
            get
            {
                return Items.OrderByDescending(GetScore).FirstOrDefault();
            }
        }

        private int GetScore(Biography biography)
        {
            var maximumBiographyLength = Items.Max(item => item.Text != null ? item.Text.Length : 0);
            var score = BiographyPrioritiesEnum.None;
            if (biography.Site == LastFmSiteName)
            {
                score |= BiographyPrioritiesEnum.LastFm;
            }

            if (biography.Site == WikipediaSiteName)
            {
                score |= BiographyPrioritiesEnum.Wikipedia;
            }

            if (biography.Text != null && biography.Text.Length == maximumBiographyLength)
            {
                score |= BiographyPrioritiesEnum.LongestText;
            }

            return (int)score;
        }
    }
}
