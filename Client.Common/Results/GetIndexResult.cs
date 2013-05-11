﻿using System.Linq;
using System.Xml.Linq;
using System.Xml.Serialization;
using Client.Common.Models.Subsonic;
using Client.Common.Services.DataStructures.SubsonicService;

namespace Client.Common.Results
{
    public class GetIndexResult : ServiceResultBase<IndexItem>, IGetIndexResult
    {
        public int MusicFolderId { get; private set; }

        public override string ViewName
        {
            get { return "getIndexes.view"; }
        }

        public override string RequestUrl
        {
            get
            {
                return string.Concat(base.RequestUrl, string.Format("&musicFolderId={0}", MusicFolderId));
            }
        }

        public GetIndexResult(ISubsonicServiceConfiguration configuration, int musicFolderId)
            : base(configuration)
        {
            MusicFolderId = musicFolderId;
        }

        protected override void HandleResponse(XDocument xDocument)
        {
            var xmlSerializer = new XmlSerializer(typeof(IndexItem), new[] { typeof(Artist) });
            var indexItems = (xDocument.Element(Namespace + "subsonic-response")
                                       .Element(Namespace + "indexes")
                                       .Descendants(Namespace + "index")
                                       .Select(musicFolder =>
                                           {
                                               using (var xmlReader = musicFolder.CreateReader())
                                               {
                                                   return (IndexItem)xmlSerializer.Deserialize(xmlReader);
                                               }
                                           })).ToList();
            Result = new IndexItem
                {
                    Name = string.Empty,
                    Id = MusicFolderId,
                    Artists = indexItems.SelectMany(ii => ii.Artists).ToList()
                };
        }
    }
}