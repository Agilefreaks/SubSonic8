namespace Subsonic8.Main
{
    using Subsonic8.Framework.ViewModel;

    public interface IMainViewModel : ICollectionViewModel<bool>
    {
        #region Public Methods and Operators

        void Populate();

        #endregion
    }
}