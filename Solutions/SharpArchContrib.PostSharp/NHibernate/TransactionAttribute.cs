using System;
using Microsoft.Practices.ServiceLocation;
using PostSharp.Extensibility;
using PostSharp.Aspects;
using SharpArch.Core;
using SharpArch.Data.NHibernate;
using SharpArchContrib.Core.Logging;
using SharpArchContrib.Data.NHibernate;

namespace SharpArchContrib.PostSharp.NHibernate {
    [AttributeUsage(AttributeTargets.Class | AttributeTargets.Method, AllowMultiple = false, Inherited = false)]
    [MulticastAttributeUsage(MulticastTargets.Method, AllowMultiple = true)]
    [Serializable]
    public class TransactionAttribute : OnMethodBoundaryAspect, ITransactionAttributeSettings {
        private IExceptionLogger exceptionLogger;
        protected TransactionAttributeSettings settings;
        private ITransactionManager transactionManager;

        public TransactionAttribute() {
            settings = new TransactionAttributeSettings();
        }

        public string FactoryKey {
            get { return Settings.FactoryKey; }
            set {
                if (value == null) {
                    throw new PreconditionException("FactoryKey cannot be null");
                }
                Settings.FactoryKey = value;
            }
        }

        public bool IsExceptionSilent {
            get { return Settings.IsExceptionSilent; }
            set { Settings.IsExceptionSilent = value; }
        }

        public object ReturnValue {
            get { return Settings.ReturnValue; }
            set { Settings.ReturnValue = value; }
        }

        protected ITransactionManager TransactionManager {
            get {
                if (transactionManager == null) {
                    transactionManager = ServiceLocator.Current.GetInstance<ITransactionManager>();
                }
                return transactionManager;
            }
        }

        private IExceptionLogger ExceptionLogger {
            get {
                if (exceptionLogger == null) {
                    exceptionLogger = ServiceLocator.Current.GetInstance<IExceptionLogger>();
                }
                return exceptionLogger;
            }
        }

        #region ITransactionAttributeSettings Members

        public TransactionAttributeSettings Settings {
            get { return settings; }
            set {
                if (value == null) {
                    throw new PreconditionException("Settings must not be null");
                }
                settings = value;
            }
        }

        #endregion

        public override void OnEntry(MethodExecutionArgs eventArgs) {
            eventArgs.MethodExecutionTag = TransactionManager.PushTransaction(FactoryKey, eventArgs.MethodExecutionTag);
        }

        public override void OnException(MethodExecutionArgs eventArgs) {
            eventArgs.MethodExecutionTag = CloseUnitOfWork(eventArgs);
            if (!(eventArgs.Exception is AbortTransactionException)) {
                ExceptionLogger.LogException(eventArgs.Exception, IsExceptionSilent, eventArgs.Method.DeclaringType);
            }
            if (TransactionManager.TransactionDepth == 0 &&
                (IsExceptionSilent || eventArgs.Exception is AbortTransactionException)) {
                eventArgs.FlowBehavior = FlowBehavior.Return;
                eventArgs.ReturnValue = ReturnValue;
            }
        }

        public override void OnSuccess(MethodExecutionArgs eventArgs) {
            eventArgs.MethodExecutionTag = CloseUnitOfWork(eventArgs);
        }

        protected virtual object CloseUnitOfWork(MethodExecutionArgs eventArgs) {
            object transactionState = eventArgs.MethodExecutionTag;
            if (eventArgs.Exception == null) {
                NHibernateSession.CurrentFor(FactoryKey).Flush();
                transactionState = TransactionManager.CommitTransaction(FactoryKey, transactionState);
            }
            else {
                transactionState = TransactionManager.RollbackTransaction(FactoryKey, transactionState);
            }
            transactionState = TransactionManager.PopTransaction(FactoryKey, transactionState);

            return transactionState;
        }
    }
}