using System;

namespace Client.Common.Models
{
    public interface ISubsonicModel : IId
    {
        SubsonicModelTypeEnum Type { get; }

        string Name { get; }

        Tuple<string, string> GetDescription();
    }
}
