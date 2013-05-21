using Client.Common.Models.Subsonic;

namespace Client.Common.Results
{
    public interface IPingResult : IEmptyResponseResult
    {
        Error ApiError { get; }
    }
}