using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Caliburn.Micro;
using Client.Common.Models;
using Client.Common.Models.Subsonic;
using Client.Common.Results;
using Subsonic8.Framework.ViewModel;
using Subsonic8.MenuItem;
using Subsonic8.MusicDirectory;
using Windows.UI.Xaml.Controls;

namespace Subsonic8.Index
{
    public class IndexViewModel : DetailViewModelBase<IndexItem>, IIndexViewModel
    {

        public IndexViewModel()
        {
            MenuItems = new BindableCollection<MenuItemViewModel>();
        }


        protected override async void LoadModel()
        {
            base.LoadModel();
            await SubsonicService.GetMusicFolders().WithErrorHandler(this).OnSuccess(SetIndexName).Execute();
        }

        protected override IServiceResultBase<IndexItem> GetResult(int id)
        {
            return SubsonicService.GetIndex(Parameter.Id);
        }

        protected override IEnumerable<ISubsonicModel> GetItemsToDisplay()
        {
            return Item.Artists;
        }

        private void SetIndexName(IList<MusicFolder> musicFolders)
        {
            var result = SubsonicService.GetMusicFolders();
            var rootFolder = result.Result.First(f => f.Id == Parameter.Id);
            Item.Name = rootFolder != null ? rootFolder.Name : "Unknown";
        }
    }
}