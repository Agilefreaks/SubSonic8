using System.Collections.Generic;
using Client.Common.Models.Subsonic;

namespace Client.Common.Results
{
    public interface IGetRootResult : IServiceResultBase
    {
        IList<IndexItem> Result { get; set; }
    }
}