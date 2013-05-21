namespace Client.Common.Models
{
    public interface ISongModel : IMediaModel
    {
        #region Public Properties

        string Artist { get; }

        int Duration { get; }

        #endregion
    }
}