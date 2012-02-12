namespace SharpArchContrib.Core.Logging
{
    using System;
    using System.Reflection;
    using System.Text;

    using log4net;

    public class MethodLogger : IMethodLogger
    {
        private readonly IExceptionLogger exceptionLogger;

        public MethodLogger(IExceptionLogger exceptionLogger)
        {
            ParameterCheck.ParameterRequired(exceptionLogger, "exceptionLogger");

            this.exceptionLogger = exceptionLogger;
        }

        public void LogEntry(MethodBase methodBase, object[] argumentValues, LoggingLevel entryLevel)
        {
            var logger = LogManager.GetLogger(methodBase.DeclaringType);
            if (this.ShouldLog(logger, entryLevel, methodBase))
            {
                var logMessage = new StringBuilder();
                logMessage.Append(string.Format("{0}(", methodBase.Name));

                var parameterInfos = methodBase.GetParameters();
                if (argumentValues != null && parameterInfos != null)
                {
                    for (var i = 0; i < argumentValues.Length; i++)
                    {
                        if (i > 0)
                        {
                            logMessage.Append(" ");
                        }

                        logMessage.Append(string.Format("{0}:[{1}]", parameterInfos[i].Name, argumentValues[i]));
                    }
                }

                logMessage.Append(")");
                logger.Log(entryLevel, logMessage.ToString());
            }
        }

        public void LogException(MethodBase methodBase, Exception err, LoggingLevel exceptionLevel)
        {
            var logger = LogManager.GetLogger(methodBase.DeclaringType);
            if (this.ShouldLog(logger, exceptionLevel, methodBase))
            {
                this.exceptionLogger.LogException(err, false, methodBase.DeclaringType);
            }
        }

        public void LogSuccess(MethodBase methodBase, object returnValue, LoggingLevel successLevel)
        {
            var logger = LogManager.GetLogger(methodBase.DeclaringType);
            if (this.ShouldLog(logger, successLevel, methodBase))
            {
                logger.Log(
                    successLevel, 
                    string.Format("{0} Returns:[{1}]", methodBase.Name, returnValue != null ? returnValue.ToString() : string.Empty));
            }
        }

        private bool ShouldLog(ILog logger, LoggingLevel loggingLevel, MethodBase methodBase)
        {
            if (methodBase != null && methodBase.Name != null)
            {
                return logger.IsEnabledFor(loggingLevel);
            }

            return false;
        }
    }
}