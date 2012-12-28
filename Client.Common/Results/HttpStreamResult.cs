using System;
using System.IO;

namespace Client.Common.Results
{
    public class HttpStreamResult
    {
        public Stream Stream { get; set; }

        public Exception Exception { get; set; }
    }
}