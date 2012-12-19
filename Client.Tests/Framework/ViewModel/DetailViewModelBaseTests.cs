using Client.Common.Models;
using Client.Tests.Mocks;
using FluentAssertions;
using Subsonic8.Framework.ViewModel;
using Microsoft.VisualStudio.TestPlatform.UnitTestFramework;

namespace Client.Tests.Framework.ViewModel
{
    [TestClass]
    public abstract class DetailViewModelBaseTests<TSubsonicModel, TViewModel> : ViewModelBaseTests<TViewModel>
        where TViewModel : IDetailViewModel<TSubsonicModel>
        where TSubsonicModel : ISubsonicModel
    {
        protected MockSubsonicService SubsonicService;
    }
}
