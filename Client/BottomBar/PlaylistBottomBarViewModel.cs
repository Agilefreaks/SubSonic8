namespace Subsonic8.BottomBar
{
    using System;
    using System.Collections.Specialized;
    using System.Linq;
    using System.Threading.Tasks;
    using Callisto.Controls;
    using Client.Common.Services;
    using MugenInjection.Attributes;
    using Subsonic8.Framework.Services;
    using Subsonic8.MenuItem;
    using Windows.UI.Xaml;
    using Windows.UI.Xaml.Controls;
    using Windows.UI.Xaml.Controls.Primitives;
    using Action = System.Action;
    using Flyout = Callisto.Controls.Flyout;

    public class PlaylistBottomBarViewModel : BottomBarViewModelBase, IPlaylistBottomBarViewModel
    {
        #region Fields

        private Flyout _renamePlaylistFlyout;

        #endregion

        #region Constructors and Destructors

        #endregion

        #region Public Properties

        public bool CanDeletePlaylist
        {
            get
            {
                return SelectedItems.Any();
            }
        }

        [Inject]
        public IDialogNotificationService NotificationService { get; set; }

        public Action OnPlaylistDeleted { get; set; }

        public MenuItemViewModel SelectedItem
        {
            get
            {
                return (MenuItemViewModel)SelectedItems[0];
            }
        }

        [Inject]
        public ISubsonicService SubsonicService { get; set; }

        #endregion

        #region Public Methods and Operators

        public async Task DeletePlaylist()
        {
            var playlistId = ((MenuItemViewModel)SelectedItems[0]).Item.Id;
            await
                SubsonicService.DeletePlaylist(playlistId)
                               .WithErrorHandler(this)
                               .OnSuccess(result => OnPlaylistDeleted())
                               .Execute();
        }

        public async Task HandleError(Exception error)
        {
            await NotificationService.Show(new DialogNotificationOptions { Message = error.ToString(), });
        }

        public async Task RenamePlaylist(string newName)
        {
            var playlistId = SelectedItem.Item.Id;
            await
                SubsonicService.RenamePlaylist(playlistId, newName)
                               .WithErrorHandler(this)
                               .OnSuccess(result => HandleRenameFinished(newName, result))
                               .Execute();
        }

        public void ShowRenameDialog(object sender)
        {
            var view = GetView() as PlaylistBottomBarView;
            if (view == null)
            {
                return;
            }

            var layoutRoot = view.GetLayoutRoot();
            _renamePlaylistFlyout = GenerateFlyout(sender);
            HookupFlyout(layoutRoot, _renamePlaylistFlyout);
            layoutRoot.Children.Add(_renamePlaylistFlyout.HostPopup);
        }

        #endregion

        #region Methods

        protected override void OnSelectedItemsChanged(
            object sender, NotifyCollectionChangedEventArgs notifyCollectionChangedEventArgs)
        {
            base.OnSelectedItemsChanged(sender, notifyCollectionChangedEventArgs);
            NotifyOfPropertyChange(() => CanDeletePlaylist);
        }

        private static void HookupFlyout(Grid layoutRoot, Flyout flyout)
        {
            EventHandler<object> eventHandler = null;
            eventHandler = (s, e) =>
                {
                    layoutRoot.Children.Remove(flyout);
                    flyout.Closed -= eventHandler;
                };
            flyout.Closed += eventHandler;
        }

        private Flyout GenerateFlyout(object sender)
        {
            var renamePlaylistDialog = new RenamePlaylistDialog
                                           {
                                               OnOkClick = async newName => await RenamePlaylist(newName),
                                               PlaylistName = SelectedItem.Title
                                           };
            var flyout = new Flyout
                             {
                                 Placement = PlacementMode.Top,
                                 PlacementTarget = (UIElement)sender,
                                 Content = renamePlaylistDialog,
                                 IsOpen = true
                             };

            return flyout;
        }

        private void HandleRenameFinished(string newName, bool result)
        {
            if (result)
            {
                SelectedItem.Title = newName;
            }
        }

        #endregion
    }
}