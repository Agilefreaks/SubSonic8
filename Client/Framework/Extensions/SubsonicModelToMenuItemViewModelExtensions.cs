namespace Subsonic8.Framework.Extensions
{
    using Client.Common.Models;
    using Subsonic8.MenuItem;

    public static class SubsonicModelToMenuItemViewModelExtensions
    {
        #region Public Methods and Operators

        public static MenuItemViewModel AsMenuItemViewModel<T>(this T item) where T : IMediaModel
        {
            var description = item.GetDescription();
            return new MenuItemViewModel
                       {
                           Title = description.Item1, 
                           CoverArtId = item.CoverArt, 
                           Type = string.Format("{0}(s)", item.Type.ToString()), 
                           Subtitle = description.Item2, 
                           Item = item
                       };
        }

        #endregion
    }
}