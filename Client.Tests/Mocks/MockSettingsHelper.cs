namespace Client.Tests.Mocks
{
    using System;
    using System.Collections.Generic;
    using System.Threading.Tasks;
    using Subsonic8.Framework.Interfaces;
    using Windows.Security.Credentials;

    public class MockSettingsHelper : ISettingsHelper
    {
        #region Constructors and Destructors

        public MockSettingsHelper()
        {
            MethodCalls = new List<Tuple<string, IList<object>>>();
        }

        #endregion

        #region Public Properties

        public IList<Tuple<string, IList<object>>> MethodCalls { get; set; }

        public Func<PasswordCredential> OnGetCredentialsFromVault { get; set; }

        #endregion

        #region Public Methods and Operators

        public PasswordCredential GetCredentialsFromVault()
        {
            MethodCalls.Add(new Tuple<string, IList<object>>("GetCredentialsFromVault", new List<object>()));

            return OnGetCredentialsFromVault != null ? OnGetCredentialsFromVault() : new PasswordCredential();
        }

        public Task LoadSettings()
        {
            return Task.Factory.StartNew(() => { });
        }

        public void UpdateCredentialsInVault(PasswordCredential credential)
        {
            MethodCalls.Add(
                new Tuple<string, IList<object>>("UpdateCredentialsInVault", new List<object> { credential }));
        }

        #endregion
    }
}