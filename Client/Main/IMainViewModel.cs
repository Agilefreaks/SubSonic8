namespace Subsonic8.Main
{
    using System.Threading.Tasks;
    using Subsonic8.Framework.ViewModel;

    public interface IMainViewModel : ICollectionViewModel<bool>
    {
        #region Public Methods and Operators

        Task Populate();

        #endregion
    }
}