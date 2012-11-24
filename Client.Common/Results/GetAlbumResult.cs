﻿using System.Xml.Linq;
using System.Xml.Serialization;
using Client.Common.Models.Subsonic;
using Client.Common.Services;

namespace Client.Common.Results
{
    public class GetAlbumResult : ServiceResultBase<Album>, IGetAlbumResult
    {
        private readonly int _id;

        public int Id
        {
            get { return _id; }
        }

        public override string ViewName { get { return "getAlbum.view"; } }

        public override string RequestUrl
        {
            get
            {
                return string.Concat(base.RequestUrl, string.Format("&id={0}", Id));
            }
        }

        public GetAlbumResult(ISubsonicServiceConfiguration configuration, int id)
            : base(configuration)
        {
            _id = id;
        }

        protected override void HandleResponse(XDocument xDocument)
        {
            var xmlSerializer = new XmlSerializer(typeof(Album), new[] { typeof(Album) });
            var xElement = xDocument.Element(Namespace + "subsonic-response").Element(Namespace + "album");
            Result = (Album)xmlSerializer.Deserialize(xElement.CreateReader());
        }
    }
}
