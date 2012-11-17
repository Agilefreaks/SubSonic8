using System;
using Caliburn.Micro;

namespace Client.Common.Results
{
    public interface IResultBase : IResult
    {
        Exception Error { get; set; }
    }
}