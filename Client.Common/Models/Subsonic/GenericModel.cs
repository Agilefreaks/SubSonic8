using System;

namespace Client.Common.Models.Subsonic
{
    public class GenericSubsonicModel : ISubsonicModel
    {
        public int Id { get; set; }

        public SubsonicModelTypeEnum Type { get; set; }

        public string CoverArt { get; set; }

        public Tuple<string, string> Description { get; set; }

        public GenericSubsonicModel()
        {
        }

        public GenericSubsonicModel(ISubsonicModel subsonicModel)
        {
            Id = subsonicModel.Id;
            Type = subsonicModel.Type;
            CoverArt = subsonicModel.CoverArt;
            Description = subsonicModel.GetDescription();
        }

        public Tuple<string, string> GetDescription()
        {
            return Description;
        }
    }
}