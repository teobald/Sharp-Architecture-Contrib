namespace SharpArchContrib.Core.Logging
{
    using System;

    [Serializable]
    public class LogAttributeSettings
    {
        public LogAttributeSettings()
            : this(LoggingLevel.Off, LoggingLevel.Off, LoggingLevel.Off)
        {
        }

        public LogAttributeSettings(LoggingLevel entryLevel, LoggingLevel successLevel, LoggingLevel exceptionLevel)
        {
            this.EntryLevel = entryLevel;
            this.SuccessLevel = successLevel;
            this.ExceptionLevel = exceptionLevel;
        }

        public LoggingLevel EntryLevel { get; set; }

        public LoggingLevel ExceptionLevel { get; set; }

        public LoggingLevel SuccessLevel { get; set; }
    }
}