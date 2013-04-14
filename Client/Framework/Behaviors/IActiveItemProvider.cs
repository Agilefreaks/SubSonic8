using System.ComponentModel;

namespace Subsonic8.Framework.Behaviors
{
    public interface IActiveItemProvider : INotifyPropertyChanged
    {
        object ActiveItem { get; }
    }
}