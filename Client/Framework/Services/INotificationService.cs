namespace Subsonic8.Framework.Services
{
    using System.Threading.Tasks;

    public interface INotificationService<in TOptions>
    {
        #region Public Methods and Operators

        Task Show(TOptions options);

        #endregion
    }
}