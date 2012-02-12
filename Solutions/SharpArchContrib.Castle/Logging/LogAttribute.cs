namespace SharpArchContrib.Castle.Logging
{
    using System;

    using SharpArchContrib.Core.Logging;

    [AttributeUsage(AttributeTargets.Assembly | AttributeTargets.Class | AttributeTargets.Method, AllowMultiple = false, 
        Inherited = false)]
    public class LogAttribute : Attribute
    {
        public LogAttribute()
        {
            this.Settings = new LogAttributeSettings(LoggingLevel.Debug, LoggingLevel.Debug, LoggingLevel.Error);
        }

        public LoggingLevel EntryLevel
        {
            get
            {
                return this.Settings.EntryLevel;
            }

            set
            {
                this.Settings.EntryLevel = value;
            }
        }

        public LoggingLevel ExceptionLevel
        {
            get
            {
                return this.Settings.ExceptionLevel;
            }

            set
            {
                this.Settings.ExceptionLevel = value;
            }
        }

        public LogAttributeSettings Settings { get; set; }

        public LoggingLevel SuccessLevel
        {
            get
            {
                return this.Settings.SuccessLevel;
            }

            set
            {
                this.Settings.SuccessLevel = value;
            }
        }
    }
}