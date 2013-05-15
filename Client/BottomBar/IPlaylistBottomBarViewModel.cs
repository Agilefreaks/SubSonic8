using System;
using Client.Common.Results;

namespace Subsonic8.BottomBar
{
    public interface IPlaylistBottomBarViewModel : IBottomBarViewModel, IErrorHandler
    {
        Action OnPlaylistDeleted { get; set; }
    }
}