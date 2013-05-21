namespace Client.Common.Models
{
    using System;

    public interface ISubsonicModel : IId
    {
        #region Public Properties

        string Name { get; }

        SubsonicModelTypeEnum Type { get; }

        #endregion

        #region Public Methods and Operators

        Tuple<string, string> GetDescription();

        #endregion
    }
}