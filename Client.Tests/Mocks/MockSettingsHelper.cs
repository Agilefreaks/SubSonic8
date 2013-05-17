using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Subsonic8.Framework.Interfaces;
using Windows.Security.Credentials;

namespace Client.Tests.Mocks
{
    public class MockSettingsHelper : ISettingsHelper
    {
        public IList<Tuple<string, IList<object>>> MethodCalls { get; set; }

        public Func<PasswordCredential> OnGetCredentialsFromVault { get; set; }

        public MockSettingsHelper()
        {
            MethodCalls = new List<Tuple<string, IList<object>>>();
        }

        public Task LoadSettings()
        {
            return Task.Factory.StartNew(() => { });
        }

        public void UpdateCredentialsInVault(PasswordCredential credential)
        {
            MethodCalls.Add(new Tuple<string, IList<object>>("UpdateCredentialsInVault", new List<object> { credential }));
        }

        public PasswordCredential GetCredentialsFromVault()
        {
            MethodCalls.Add(new Tuple<string, IList<object>>("GetCredentialsFromVault", new List<object>()));

            return OnGetCredentialsFromVault != null ? OnGetCredentialsFromVault() : new PasswordCredential();
        }
    }
}