using Client.Common.Models;
using Subsonic8.MenuItem;

namespace Subsonic8.Framework.Extensions
{
    public static class SubsonicModelToMenuItemViewModelExtensions
    {
        public static MenuItemViewModel AsMenuItemViewModel<T>(this T item)
            where T : ISubsonicModel
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
    }
}