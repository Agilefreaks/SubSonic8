namespace SubEchoNest.Models
{
    public enum EchoNestStatusEnum
    {
        UnknownError = -1,
        Success,
        ApiKeyIssue,
        RestrictedArea,
        RateLimitExceeded,
        MissingParameter,
        InvalidParameter
    }
}
