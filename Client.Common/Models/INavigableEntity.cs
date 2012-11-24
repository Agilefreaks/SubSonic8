namespace Client.Common.Models
{
    public interface INavigableEntity : IIdentifiableEntity
    {
        NavigableTypeEnum Type { get; }
    }
}
