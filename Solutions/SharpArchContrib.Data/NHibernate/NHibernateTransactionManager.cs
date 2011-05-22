namespace SharpArchContrib.Data.NHibernate
{
    using System;

    using SharpArch.NHibernate;

    /// <summary>
    ///   Provides support for System.Transaction transactions
    /// </summary>
    [Serializable]
    public class NHibernateTransactionManager : TransactionManagerBase
    {
        public override string Name
        {
            get
            {
                return "NHibernate TransactionManager";
            }
        }

        public override object CommitTransaction(string factoryKey, object transactionState)
        {
            var transaction = NHibernateSession.CurrentFor(factoryKey).Transaction;
            if (this.TransactionDepth == 1 && transaction.IsActive)
            {
                transaction.Commit();
            }

            return transactionState;
        }

        public override object PushTransaction(string factoryKey, object transactionState)
        {
            transactionState = base.PushTransaction(factoryKey, transactionState);

            var transaction = NHibernateSession.CurrentFor(factoryKey).Transaction;
            if (!transaction.IsActive)
            {
                transaction.Begin();
            }

            return transactionState;
        }

        public override object RollbackTransaction(string factoryKey, object transactionState)
        {
            var transaction = NHibernateSession.CurrentFor(factoryKey).Transaction;
            if (this.TransactionDepth == 1 && transaction.IsActive)
            {
                transaction.Rollback();
            }

            return transactionState;
        }

        public override bool TransactionIsActive(string factoryKey)
        {
            var transaction = NHibernateSession.CurrentFor(factoryKey).Transaction;
            return transaction != null && transaction.IsActive;
        }
    }
}