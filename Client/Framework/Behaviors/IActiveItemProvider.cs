namespace Subsonic8.Framework.Behaviors
{
    using System.ComponentModel;
    using Client.Common.Models;

    public interface IActiveItemProvider : INotifyPropertyChanged
    {
        #region Public Properties

        PlaylistItem ActiveItem { get; }

        #endregion
    }
}