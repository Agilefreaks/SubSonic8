namespace Client.Common.Models.Subsonic
{
    using System;

    public sealed class GenericMediaModel : MediaModelBase
    {
        #region Fields

        private readonly Tuple<string, string> _description;

        private readonly SubsonicModelTypeEnum _type;

        #endregion

        #region Constructors and Destructors

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

        #endregion

        #region Public Properties

        public override SubsonicModelTypeEnum Type
        {
            get
            {
                return _type;
            }
        }

        #endregion

        #region Public Methods and Operators

        public override Tuple<string, string> GetDescription()
        {
            return _description;
        }

        #endregion
    }
}