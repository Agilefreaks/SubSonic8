namespace Client.Common.Services.DataStructures.SubsonicService
{
    using global::Common.Results;

    public interface ISubsonicServiceConfiguration : IConfiguration
    {
        #region Public Properties

        string EncodedCredentials { get; }

        string Password { get; set; }

        string Username { get; set; }

        string EncodedPassword { get; }

        #endregion
    }
}