namespace SharpArchContrib.Core.Logging
{
    using System;

    using log4net;

    public class ExceptionLogger : IExceptionLogger
    {
        public void LogException(Exception err, bool isSilent, Type throwingType)
        {
            var logger = LogManager.GetLogger(throwingType);
            string message = null;
            if (isSilent)
            {
                message = "[SILENT]";
            }

            logger.Log(LoggingLevel.Error, message, err);
        }
    }
}