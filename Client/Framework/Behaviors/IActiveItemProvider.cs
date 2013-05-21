namespace Subsonic8.Framework.Behaviors
{
    using System.ComponentModel;

    public interface IActiveItemProvider : INotifyPropertyChanged
    {
        #region Public Properties

        object ActiveItem { get; }

        #endregion
    }
}