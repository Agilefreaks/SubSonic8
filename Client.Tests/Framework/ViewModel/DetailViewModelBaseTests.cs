namespace Client.Tests.Framework.ViewModel
{
    using Client.Common.Models;
    using Microsoft.VisualStudio.TestPlatform.UnitTestFramework;
    using Subsonic8.Framework.ViewModel;

    [TestClass]
    public abstract class DetailViewModelBaseTests<TSubsonicModel, TViewModel> :
        CollectionViewModelBaseTests<TViewModel, int>
        where TViewModel : IDetailViewModel<TSubsonicModel>, new() where TSubsonicModel : ISubsonicModel
    {
    }
}