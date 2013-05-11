using System;

namespace Client.Common.Models.Subsonic
{
    public sealed class GenericMediaModel : MediaModelBase
    {
        private readonly SubsonicModelTypeEnum _type;
        private readonly Tuple<string, string> _description;

        public override SubsonicModelTypeEnum Type
        {
            get { return _type; }
        }

        public GenericMediaModel(SubsonicModelTypeEnum type)
        {
            _type = type;
        }

        public GenericMediaModel(ISubsonicModel subsonicModel)
            : this(subsonicModel.Type)
        {
            Id = subsonicModel.Id;
            Name = subsonicModel.Name;
            _description = subsonicModel.GetDescription();
        }

        public override Tuple<string, string> GetDescription()
        {
            return _description;
        }
    }
}