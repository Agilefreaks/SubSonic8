using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using Caliburn.Micro;
using Client.Common.Models;
using Subsonic8.MenuItem;
using Subsonic8.Messages;
using Subsonic8.Shell;

namespace Subsonic8.BottomBar
{
    public class BottomBarViewModel : IPlaylistBarViewModel
    {
        private readonly IEventAggregator _eventAggregator;

        public ObservableCollection<MenuItemViewModel> SelectedItems { get; set; }

        public bool IsOpened { get; set; }

        public BottomBarViewModel(IEventAggregator eventAggregator)
        {
            _eventAggregator = eventAggregator;
            SelectedItems = new ObservableCollection<MenuItemViewModel>();
        }

        public void AddToPlaylist()
        {
            _eventAggregator.Publish(new PlaylistMessage { Queue = SelectedItems.Select(i => i.Item).ToList() });
            SelectedItems.Clear();
        }
    }
}
