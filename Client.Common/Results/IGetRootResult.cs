using System.Collections.Generic;
using Caliburn.Micro;
using Client.Common.Models.Subsonic;

namespace Client.Common.Results
{
    public interface IGetRootResult : IResult
    {
        IList<IndexItem> Result { get; set; }
    }
}