using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Caliburn.Micro;
using Client.Common.Models;
using Client.Common.Models.Subsonic;
using Client.Common.Results;
using Subsonic8.Framework.ViewModel;
using Subsonic8.MenuItem;

namespace Subsonic8.Index
{
    public class IndexViewModel : DetailViewModelBase<IndexItem>, IIndexViewModel
    {
        public IndexViewModel()
        {
            MenuItems = new BindableCollection<MenuItemViewModel>();
        }

        protected override IServiceResultBase<IndexItem> GetResult(int id)
        {
            return SubsonicService.GetIndex(id);
        }

        protected override IEnumerable<IMediaModel> GetItemsToDisplay(IndexItem result)
        {
            return result.Artists;
        }

        protected override async Task AfterPopulate(int id)
        {
            var result = SubsonicService.GetMusicFolders();
            await result.WithErrorHandler(this).OnSuccess(r => SetIndexName(r, id)).Execute();
        }

        private void SetIndexName(IEnumerable<MusicFolder> musicFolders, int id)
        {
            var rootFolder = musicFolders.First(f => f.Id == id);
            Item.Name = rootFolder != null ? rootFolder.Name : "Unknown";
        }
    }
}