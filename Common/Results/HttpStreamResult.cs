namespace Common.Results
{
    using System;
    using System.IO;

    public class HttpStreamResult
    {
        #region Public Properties

        public Exception Exception { get; set; }

        public Stream Stream { get; set; }

        #endregion
    }
}