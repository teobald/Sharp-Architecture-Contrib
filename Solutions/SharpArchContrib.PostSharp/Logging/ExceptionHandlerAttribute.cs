namespace SharpArchContrib.PostSharp.Logging
{
    using System;

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
    public sealed class ExceptionHandlerAttribute : OnExceptionAspect
    {
        #region Constants and Fields

        private IExceptionLogger exceptionLogger;

        #endregion

        #region Constructors and Destructors

        /// <summary>
        ///   Constructor
        /// </summary>
        public ExceptionHandlerAttribute()
        {
            this.Settings = new ExceptionHandlerAttributeSettings();
        }

        #endregion

        #region Properties

        public bool IsSilent
        {
            get
            {
                return this.Settings.IsSilent;
            }

            set
            {
                this.Settings.IsSilent = value;
            }
        }

        public object ReturnValue
        {
            get
            {
                return this.Settings.ReturnValue;
            }

            set
            {
                this.Settings.ReturnValue = value;
            }
        }

        public ExceptionHandlerAttributeSettings Settings { get; set; }

        private IExceptionLogger ExceptionLogger
        {
            get
            {
                if (this.exceptionLogger == null)
                {
                    this.exceptionLogger = ServiceLocator.Current.GetInstance<IExceptionLogger>();
                }

                return this.exceptionLogger;
            }
        }

        #endregion

        #region Public Methods

        public override void OnException(MethodExecutionArgs eventArgs)
        {
            this.ExceptionLogger.LogException(eventArgs.Exception, this.IsSilent, eventArgs.Method.DeclaringType);
            if (this.IsSilent)
            {
                eventArgs.FlowBehavior = FlowBehavior.Return;
                eventArgs.ReturnValue = this.ReturnValue;
            }
        }

        #endregion
    }
}