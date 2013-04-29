using Client.Common.Models;

namespace Subsonic8.Framework.ViewModel
{
    public interface IDetailViewModel<T> : ICollectionViewModel<int>
        where T : ISubsonicModel
    {
        T Item { get; set; }
    }
}