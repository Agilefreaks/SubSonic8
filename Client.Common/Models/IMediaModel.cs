namespace Client.Common.Models
{
    public interface IMediaModel : ISubsonicModel
    {
        #region Public Properties

        string CoverArt { get; set; }

        #endregion
    }
}