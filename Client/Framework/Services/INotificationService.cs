using System.Threading.Tasks;

namespace Subsonic8.Framework.Services
{
    public interface INotificationService<in TOptions>
    {
        Task Show(TOptions options);
    }
}
