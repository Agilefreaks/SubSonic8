using Client.Common.Models;
using Subsonic8.Framework.ViewModel;
using Microsoft.VisualStudio.TestPlatform.UnitTestFramework;

namespace Client.Tests.Framework.ViewModel
{
    [TestClass]
    public abstract class DetailViewModelBaseTests<TSubsonicModel, TViewModel> : CollectionViewModelBaseTests<TViewModel, int>
        where TViewModel : IDetailViewModel<TSubsonicModel>, new()
        where TSubsonicModel : ISubsonicModel
    {
    }
}
