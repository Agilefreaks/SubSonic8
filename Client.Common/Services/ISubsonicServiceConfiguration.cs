namespace Client.Common.Services
{
    public interface ISubsonicServiceConfiguration
    {
        string Username { get; set; }

        string Password { get; set; }

        string BaseUrl { get; set; }
    }
}