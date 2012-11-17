using System.Collections.Generic;
using Caliburn.Micro;
using Client.Common;
using Client.Common.Results;
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
        }

        public void IndexClick(ItemClickEventArgs eventArgs)
        {
            var item = ((MenuItemViewModel) eventArgs.ClickedItem).Item;
            NavigationService.NavigateToViewModel<IndexViewModel>(item);
        }

        protected override void OnInitialize()
        {
            base.OnInitialize();
            Populate().Execute();
        }

        public IEnumerable<IResult> Populate()
        {
            var getIndexResult = SubsonicService.GetRootIndex();
            yield return getIndexResult;

            if (getIndexResult.Error != null)
            {
                yield return new MessageDialogResult(getIndexResult.Error.ToString(), "This is a sad day");
            }
            else
            {
                foreach (var index in getIndexResult.Result)
                {
                    MenuItems.Add(new MenuItemViewModel { Title = index.Name, Subtitle = string.Format("{0} artists", index.Artists.Count), Item = index });
                }                
            }
        }
    }
}