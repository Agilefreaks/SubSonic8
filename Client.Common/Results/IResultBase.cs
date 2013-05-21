namespace Client.Common.Results
{
    using System;

    public interface IResultBase : ITaskResult
    {
        #region Public Properties

        Exception Error { get; set; }

        #endregion
    }
}