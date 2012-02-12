namespace SharpArchContrib.PostSharp.NHibernate
{
    using System;

    using Microsoft.Practices.ServiceLocation;

    using global::PostSharp.Aspects;
    using global::PostSharp.Extensibility;

    using SharpArch.Domain;
    using SharpArch.NHibernate;

    using SharpArchContrib.Core.Logging;
    using SharpArchContrib.Data.NHibernate;

    [AttributeUsage(AttributeTargets.Class | AttributeTargets.Method, AllowMultiple = false, Inherited = false)]
    [MulticastAttributeUsage(MulticastTargets.Method, AllowMultiple = true)]
    [Serializable]
    public class TransactionAttribute : OnMethodBoundaryAspect, ITransactionAttributeSettings
    {
        #region Constants and Fields

        protected TransactionAttributeSettings attributeSettings;

        private IExceptionLogger exceptionLogger;

        private ITransactionManager transactionManager;

        #endregion

        #region Constructors and Destructors

        public TransactionAttribute()
        {
            this.attributeSettings = new TransactionAttributeSettings();
        }

        #endregion

        #region Properties

        public string FactoryKey
        {
            get
            {
                return this.Settings.FactoryKey;
            }

            set
            {
                if (value == null)
                {
                    throw new PreconditionException("FactoryKey cannot be null");
                }

                this.Settings.FactoryKey = value;
            }
        }

        public bool IsExceptionSilent
        {
            get
            {
                return this.Settings.IsExceptionSilent;
            }

            set
            {
                this.Settings.IsExceptionSilent = value;
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

        public TransactionAttributeSettings Settings
        {
            get
            {
                return this.attributeSettings;
            }

            set
            {
                if (value == null)
                {
                    throw new PreconditionException("Settings must not be null");
                }

                this.attributeSettings = value;
            }
        }

        protected ITransactionManager TransactionManager
        {
            get
            {
                if (this.transactionManager == null)
                {
                    this.transactionManager = ServiceLocator.Current.GetInstance<ITransactionManager>();
                }

                return this.transactionManager;
            }
        }

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

        public sealed override void OnEntry(MethodExecutionArgs eventArgs)
        {
            eventArgs.MethodExecutionTag = this.TransactionManager.PushTransaction(
                this.FactoryKey, eventArgs.MethodExecutionTag);
        }

        public override sealed void OnException(MethodExecutionArgs eventArgs)
        {
            eventArgs.MethodExecutionTag = this.CloseUnitOfWork(eventArgs.MethodExecutionTag, eventArgs.Exception == null);

            if (!(eventArgs.Exception is AbortTransactionException))
            {
                this.ExceptionLogger.LogException(
                    eventArgs.Exception, this.IsExceptionSilent, eventArgs.Method.DeclaringType);
            }

            if (this.TransactionManager.TransactionDepth == 0 &&
                (this.IsExceptionSilent || eventArgs.Exception is AbortTransactionException))
            {
                eventArgs.FlowBehavior = FlowBehavior.Return;
                eventArgs.ReturnValue = this.ReturnValue;
            }
        }

        public sealed override void OnSuccess(MethodExecutionArgs eventArgs)
        {
            eventArgs.MethodExecutionTag = this.CloseUnitOfWork(eventArgs.MethodExecutionTag, eventArgs.Exception == null);
        }

        #endregion

        #region Methods

        protected virtual object CloseUnitOfWork(object transactionState, bool commit)
        {
            if (commit)
            {
                NHibernateSession.CurrentFor(this.FactoryKey).Flush();
                transactionState = this.TransactionManager.CommitTransaction(this.FactoryKey, transactionState);
            }
            else
            {
                transactionState = this.TransactionManager.RollbackTransaction(this.FactoryKey, transactionState);
            }

            transactionState = this.TransactionManager.PopTransaction(this.FactoryKey, transactionState);

            return transactionState;
        }

        #endregion
    }
}