namespace Client.Common.Services.DataStructures.SubsonicService
{
    public interface ISubsonicServiceConfiguration
    {
        #region Public Properties

        string BaseUrl { get; set; }

        string EncodedCredentials { get; }

        string Password { get; set; }

        string Username { get; set; }

        #endregion
    }
}