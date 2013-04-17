namespace Client.Common.Services.DataStructures.SubsonicService
{
    public interface ISubsonicServiceConfiguration
    {
        string Username { get; set; }

        string Password { get; set; }

        string BaseUrl { get; set; }

        string EncodedCredentials { get; }
    }
}