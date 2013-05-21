namespace Subsonic8.Framework
{
    using System;

    public class SuspensionManagerException : Exception
    {
        #region Constructors and Destructors

        public SuspensionManagerException()
        {
        }

        public SuspensionManagerException(Exception e)
            : base("SuspensionManager failed", e)
        {
        }

        #endregion
    }
}