namespace SharpArchContrib.PostSharp.Logging
{
    using System;

    using log4net;

    using Microsoft.Practices.ServiceLocation;

    using global::PostSharp.Aspects;
    using global::PostSharp.Extensibility;

    using SharpArchContrib.Core.Logging;

    [Serializable]
    [AttributeUsage(AttributeTargets.Assembly | AttributeTargets.Class | AttributeTargets.Method, AllowMultiple = false, 
        Inherited = false)]
    [MulticastAttributeUsage(
        MulticastTargets.Method | MulticastTargets.InstanceConstructor | MulticastTargets.StaticConstructor, 
        AllowMultiple = true)]
    public class LogAttribute : OnMethodBoundaryAspect
    {
        #region Constants and Fields

        private IMethodLogger methodLogger;

        #endregion

        #region Constructors and Destructors

        public LogAttribute()
        {
            this.Settings = new LogAttributeSettings(LoggingLevel.Debug, LoggingLevel.Debug, LoggingLevel.Error);
        }

        #endregion

        #region Properties

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

        private IMethodLogger MethodLogger
        {
            get
            {
                if (this.methodLogger == null)
                {
                    this.methodLogger = ServiceLocator.Current.GetInstance<IMethodLogger>();
                }

                return this.methodLogger;
            }
        }

        #endregion

        #region Public Methods

        public sealed override void OnEntry(MethodExecutionArgs eventArgs)
        {
            this.MethodLogger.LogEntry(eventArgs.Method, eventArgs.Arguments.ToArray(), this.EntryLevel);
        }

        public sealed override void OnException(MethodExecutionArgs eventArgs)
        {
            this.methodLogger.LogException(eventArgs.Method, eventArgs.Exception, this.ExceptionLevel);
        }

        public sealed override void OnSuccess(MethodExecutionArgs eventArgs)
        {
            this.methodLogger.LogSuccess(eventArgs.Method, eventArgs.ReturnValue, this.SuccessLevel);
        }

        #endregion

        #region Methods

        private bool ShouldLog(ILog logger, LoggingLevel loggingLevel, MethodExecutionArgs args)
        {
            if (args != null && args.Method != null && args.Method.Name != null)
            {
                return logger.IsEnabledFor(loggingLevel);
            }

            return false;
        }

        #endregion
    }
}