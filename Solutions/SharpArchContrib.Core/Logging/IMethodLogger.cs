namespace SharpArchContrib.Core.Logging
{
    using System;
    using System.Reflection;

    public interface IMethodLogger
    {
        void LogEntry(MethodBase methodBase, object[] argumentValues, LoggingLevel entryLevel);

        void LogException(MethodBase methodBase, Exception err, LoggingLevel exceptionLevel);

        void LogSuccess(MethodBase methodBase, object returnValue, LoggingLevel successLevel);
    }
}