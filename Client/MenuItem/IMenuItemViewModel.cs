using Client.Common.Models;

namespace Subsonic8.MenuItem
{
    public interface IMenuItemViewModel
    {
        string Title { get; set; }

        string Subtitle { get; set; }

        ISubsonicModel Item { get; set; }

        string Type { get; set; }

        string CoverArt { get; }

        string CoverArtId { get; set; }
    }
}