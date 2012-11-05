using System.Collections.Generic;
using Caliburn.Micro;

namespace Client.Common.Results
{
    public interface IGetIndexResult : IResult
    {
        IEnumerable<Models.Subsonic.Index> Result { get; set; }
    }
}