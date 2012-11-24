﻿using System;
using System.Xml.Serialization;

namespace Client.Common.Models.Subsonic
{
    public abstract class SubsonicModelBase : ISubsonicModel
    {
        [XmlAttribute("id")]
        public int Id { get; set; }

        public abstract SubsonicModelTypeEnum Type { get; }

        public virtual Tuple<string, string> GetDescription()
        {
            return new Tuple<string, string>(GetType().Name, Id.ToString());
        }
    }
}