namespace Client.Common
{
    using System;
    using MetroLog;

    public static class ObjectExtensionMethods
    {
        #region Public Methods and Operators

        public static void Log<T>(this T source, string message)
        {
            LogManagerFactory.DefaultLogManager.GetLogger<T>().Info(message);
        }

        public static void Log<T>(this T source, Exception exception)
        {
            LogManagerFactory.DefaultLogManager.GetLogger<T>().Error("Exception", exception);
        }

        #endregion
    }
}