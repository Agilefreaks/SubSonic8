namespace Subsonic8.Framework.Interfaces
{
    using Subsonic8.BottomBar;

    public interface IBottomBarViewModelProvider
    {
        #region Public Properties

        IBottomBarViewModel BottomBar { get; set; }

        #endregion
    }
}