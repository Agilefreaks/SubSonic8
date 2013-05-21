namespace Subsonic8.Framework.ViewModel
{
    using Client.Common.Models;

    public interface IDetailViewModel<T> : ICollectionViewModel<int>
        where T : ISubsonicModel
    {
        #region Public Properties

        T Item { get; set; }

        #endregion
    }
}