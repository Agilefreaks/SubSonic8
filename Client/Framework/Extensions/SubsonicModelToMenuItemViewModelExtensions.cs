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
                           Type = string.Format("{0}(s)", item.Type.ToString()),
                           Title = description.Item1,
                           Subtitle = description.Item2,
                           Item = item
                       };
        }
    }
}