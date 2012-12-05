namespace Client.Common.ViewModels
{
    public interface IBottomBarViewModel
    {
        bool IsOpened { get; set; }

        void NavigateToPlaylist();
    }
}