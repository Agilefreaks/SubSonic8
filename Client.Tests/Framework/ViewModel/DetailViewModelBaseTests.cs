using Client.Common.Models;
using Subsonic8.Framework.ViewModel;
using Microsoft.VisualStudio.TestPlatform.UnitTestFramework;

namespace Client.Tests.Framework.ViewModel
{
    [TestClass]
    public abstract class DetailViewModelBaseTests<TSubsonicModel, TViewModel> : ViewModelBaseTests<TViewModel>
        where TViewModel : IDetailViewModel<TSubsonicModel>, new()
        where TSubsonicModel : ISubsonicModel
    {
    }
}
