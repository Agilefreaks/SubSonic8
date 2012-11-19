using System;

namespace Client.Common.Results
{
    public interface IResultBase : ITaskResult
    {
        Exception Error { get; set; }
    }
}