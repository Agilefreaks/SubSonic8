namespace Subsonic8.Settings
{
    using Caliburn.Micro;

    public sealed class PrivacyPolicyViewModel : Screen
    {
        #region Constructors and Destructors

        public PrivacyPolicyViewModel()
        {
            DisplayName = "Privacy policy";
        }

        #endregion

        #region Public Properties

        public string Text
        {
            get
            {
                return @"General Information:
We store on your computer the given username, password and server url required to connect to the subsonic streaming server and also send the credentials along to it in order to authenticate. We do not collect any information about you.

Privacy questions:
If you have any questions or concerns about our privacy policies, please contact us at: 'office@agilefreaks.com'.

Personal information protection:
We take reasonable steps to secure your personally identifiable information against unauthorized access or disclosure. However, no security or encryption method can be guaranteed to protect information from hackers or human error.

Privacy policy information:
This privacy policy was last updated on march 8 2013. If we make any material changes to our policies, we will place a prominent notice in our application.";
            }
        }

        #endregion
    }
}