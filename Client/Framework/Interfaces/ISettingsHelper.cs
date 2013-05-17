using System.Threading.Tasks;
using Windows.Security.Credentials;

namespace Subsonic8.Framework.Interfaces
{
    public interface ISettingsHelper
    {
        Task LoadSettings();

        void UpdateCredentialsInVault(PasswordCredential credential);

        PasswordCredential GetCredentialsFromVault();
    }
}