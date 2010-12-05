namespace SharpArchContrib.Castle.NHibernate
{
    using System;

    using global::Castle.DynamicProxy;

    using SharpArch.Data.NHibernate;

    using SharpArchContrib.Core;
    using SharpArchContrib.Core.Logging;
    using SharpArchContrib.Data.NHibernate;

    public class TransactionInterceptor : IInterceptor
    {
        protected readonly IExceptionLogger exceptionLogger;

        protected readonly ITransactionManager transactionManager;

        public TransactionInterceptor(ITransactionManager transactionManager, IExceptionLogger exceptionLogger)
        {
            ParameterCheck.ParameterRequired(transactionManager, "transactionManager");
            ParameterCheck.ParameterRequired(exceptionLogger, "exceptionLogger");

            this.transactionManager = transactionManager;
            this.exceptionLogger = exceptionLogger;
        }

        public void Intercept(IInvocation invocation)
        {
            var methodInfo = invocation.MethodInvocationTarget;
            if (methodInfo == null)
            {
                methodInfo = invocation.Method;
            }

            // we take the settings from the first attribute we find searching method first
            // If there is at least one attribute, the call gets wrapped with a transaction
            var attributeType = this.GetAttributeType();
            var classAttributes =
                (ITransactionAttributeSettings[])methodInfo.ReflectedType.GetCustomAttributes(attributeType, false);
            var methodAttributes = (ITransactionAttributeSettings[])methodInfo.GetCustomAttributes(attributeType, false);
            if (classAttributes.Length == 0 && methodAttributes.Length == 0)
            {
                invocation.Proceed();
            }
            else
            {
                var transactionAttributeSettings = this.GetTransactionAttributeSettings(
                    methodAttributes, classAttributes);

                var transactionState = this.OnEntry(transactionAttributeSettings, null);
                try
                {
                    invocation.Proceed();
                }
                catch (Exception err)
                {
                    this.CloseUnitOfWork(transactionAttributeSettings, transactionState, err);
                    if (!(err is AbortTransactionException))
                    {
                        this.exceptionLogger.LogException(
                            err, transactionAttributeSettings.IsExceptionSilent, methodInfo.ReflectedType);
                    }

                    if (this.transactionManager.TransactionDepth == 0 &&
                        (transactionAttributeSettings.IsExceptionSilent || err is AbortTransactionException))
                    {
                        invocation.ReturnValue = transactionAttributeSettings.ReturnValue;
                        return;
                    }

                    throw;
                }

                transactionState = this.OnSuccess(transactionAttributeSettings, transactionState);
            }
        }

        protected virtual object CloseUnitOfWork(
            TransactionAttributeSettings transactionAttributeSettings, object transactionState, Exception err)
        {
            var factoryKey = transactionAttributeSettings.FactoryKey;
            if (err == null)
            {
                try
                {
                    NHibernateSession.CurrentFor(factoryKey).Flush();
                    transactionState = this.transactionManager.CommitTransaction(factoryKey, transactionState);
                }
                catch (Exception)
                {
                    transactionState = this.transactionManager.RollbackTransaction(factoryKey, transactionState);
                    transactionState = this.transactionManager.PopTransaction(factoryKey, transactionState);
                    throw;
                }
            }
            else
            {
                transactionState = this.transactionManager.RollbackTransaction(factoryKey, transactionState);
            }

            transactionState = this.transactionManager.PopTransaction(factoryKey, transactionState);

            return transactionState;
        }

        protected virtual Type GetAttributeType()
        {
            return typeof(TransactionAttribute);
        }

        private TransactionAttributeSettings GetTransactionAttributeSettings(
            ITransactionAttributeSettings[] methodAttributes, ITransactionAttributeSettings[] classAttributes)
        {
            var transactionAttributeSettings = new TransactionAttributeSettings();
            if (methodAttributes.Length > 0)
            {
                transactionAttributeSettings = methodAttributes[methodAttributes.Length - 1].Settings;
            }
            else if (classAttributes.Length > 0)
            {
                transactionAttributeSettings = classAttributes[classAttributes.Length - 1].Settings;
            }

            return transactionAttributeSettings;
        }

        private object OnEntry(TransactionAttributeSettings transactionAttributeSettings, object transactionState)
        {
            return this.transactionManager.PushTransaction(transactionAttributeSettings.FactoryKey, transactionState);
        }

        private object OnSuccess(TransactionAttributeSettings transactionAttributeSettings, object transactionState)
        {
            return this.CloseUnitOfWork(transactionAttributeSettings, transactionState, null);
        }
    }
}