using Caliburn.Micro;
using Client.Common.Results;
using Subsonic8.Framework.ViewModel;
using Subsonic8.Index;
using Subsonic8.MenuItem;
using Windows.UI.Xaml.Controls;

namespace Subsonic8.Main
{
    public class MainViewModel : ViewModelBase, IMainViewModel
    {
        public BindableCollection<MenuItemViewModel> MenuItems { get; private set; }

        public MainViewModel()
        {
            MenuItems = new BindableCollection<MenuItemViewModel>();
            UpdateDisplayName();
        }

        public void IndexClick(ItemClickEventArgs eventArgs)
        {
            var item = ((MenuItemViewModel)eventArgs.ClickedItem).Item;
            NavigationService.NavigateToViewModel<IndexViewModel>(item);
        }

        public async void Populate()
        {
            var getIndexResult = SubsonicService.GetRootIndex();
            await getIndexResult.Execute();

            if (getIndexResult.Error != null)
            {
                await new MessageDialogResult(getIndexResult.Error.ToString(), "This is a sad day").Execute();
            }
            else
            {
                SetMenuItems(getIndexResult);
            }
        }

        public void SetMenuItems(IGetRootResult getIndexResult)
        {
            foreach (var index in getIndexResult.Result)
            {
                MenuItems.Add(new MenuItemViewModel
                                  {
                                      Title = index.Name,
                                      Subtitle = string.Format("{0} artists", index.Artists.Count),
                                      Item = index
                                  });
            }
        }

        protected override void OnInitialize()
        {
            base.OnInitialize();
            Populate();
        }

        protected override void UpdateDisplayName()
        {
            DisplayName = "Subsonic8";
        }
    }
}