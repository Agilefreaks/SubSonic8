using Caliburn.Micro;

namespace Client.ViewModels
{
    public class MenuViewModel : ViewModelBase
    {
        public BindableCollection<MenuItemViewModel> MenuItems { get; private set; }

        public MenuViewModel(INavigationService navigationService) : base(navigationService)
        {
            MenuItems = new BindableCollection<MenuItemViewModel>();
        }

        protected override void OnInitialize()
        {
            base.OnInitialize();

            MenuItems.Add(new MenuItemViewModel { Title = "Title", Subtitle = "subtitle" });
        }
    }
}