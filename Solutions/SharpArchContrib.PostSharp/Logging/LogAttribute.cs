using System;
using log4net;
using Microsoft.Practices.ServiceLocation;
using PostSharp.Extensibility;
using PostSharp.Aspects;
using SharpArchContrib.Core.Logging;

namespace SharpArchContrib.PostSharp.Logging {
    [Serializable]
    [AttributeUsage(AttributeTargets.Assembly | AttributeTargets.Class | AttributeTargets.Method, AllowMultiple = false,
        Inherited = false)]
    [MulticastAttributeUsage(
        MulticastTargets.Method | MulticastTargets.InstanceConstructor | MulticastTargets.StaticConstructor,
        AllowMultiple = true)]
    public class LogAttribute : OnMethodBoundaryAspect {
        private IMethodLogger methodLogger;

        public LogAttribute() {
            Settings = new LogAttributeSettings(LoggingLevel.Debug, LoggingLevel.Debug, LoggingLevel.Error);
        }

        public LoggingLevel EntryLevel {
            get { return Settings.EntryLevel; }
            set { Settings.EntryLevel = value; }
        }

        public LoggingLevel SuccessLevel {
            get { return Settings.SuccessLevel; }
            set { Settings.SuccessLevel = value; }
        }

        public LoggingLevel ExceptionLevel {
            get { return Settings.ExceptionLevel; }
            set { Settings.ExceptionLevel = value; }
        }

        public LogAttributeSettings Settings { get; set; }

        private IMethodLogger MethodLogger {
            get {
                if (methodLogger == null) {
                    methodLogger = ServiceLocator.Current.GetInstance<IMethodLogger>();
                }
                return methodLogger;
            }
        }

        public override void OnEntry(MethodExecutionArgs eventArgs) {
            this.MethodLogger.LogEntry(eventArgs.Method, eventArgs.Arguments.ToArray(), this.EntryLevel);
        }

        public override void OnSuccess(MethodExecutionArgs eventArgs) {
            methodLogger.LogSuccess(eventArgs.Method, eventArgs.ReturnValue, SuccessLevel);
        }

        public override void OnException(MethodExecutionArgs eventArgs) {
            methodLogger.LogException(eventArgs.Method, eventArgs.Exception, ExceptionLevel);
        }

        private bool ShouldLog(ILog logger, LoggingLevel loggingLevel, MethodExecutionArgs args) {
            if (args != null && args.Method != null && args.Method.Name != null) {
                return logger.IsEnabledFor(loggingLevel);
            }

            return false;
        }
    }
}