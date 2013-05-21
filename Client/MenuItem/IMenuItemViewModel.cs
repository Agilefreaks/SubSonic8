namespace Subsonic8.MenuItem
{
    using Client.Common.Models;

    public interface IMenuItemViewModel
    {
        #region Public Properties

        string CoverArt { get; }

        string CoverArtId { get; set; }

        ISubsonicModel Item { get; set; }

        string Subtitle { get; set; }

        string Title { get; set; }

        string Type { get; set; }

        #endregion
    }
}