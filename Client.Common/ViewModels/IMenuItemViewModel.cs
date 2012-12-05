using Client.Common.Models;

namespace Client.Common.ViewModels
{
    public interface IMenuItemViewModel
    {
        string Title { get; set; }
        string Subtitle { get; set; }
        ISubsonicModel Item { get; set; }
        string Type { get; set; }
    }
}