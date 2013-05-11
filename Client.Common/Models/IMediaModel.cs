namespace Client.Common.Models
{
    public interface IMediaModel : ISubsonicModel
    {
        string CoverArt { get; set; }
    }
}