namespace SharpArchContrib.Data.NHibernate
{
    using System;
    using System.Threading;

    using log4net;

    [Serializable]
    public abstract class TransactionManagerBase : ITransactionManager
    {
        private static readonly ILog logger = LogManager.GetLogger(typeof(TransactionManagerBase));

        [ThreadStatic]
        private static int transactionDepth;

        public abstract string Name { get; }

        public int TransactionDepth
        {
            get
            {
                return transactionDepth;
            }
        }

        public abstract object CommitTransaction(string factoryKey, object transactionState);

        public virtual object PopTransaction(string factoryKey, object transactionState)
        {
            Interlocked.Decrement(ref transactionDepth);
            this.Log(string.Format("Pop Transaction to Depth {0}", transactionDepth));
            return transactionState;
        }

        public virtual object PushTransaction(string factoryKey, object transactionState)
        {
            Interlocked.Increment(ref transactionDepth);
            this.Log(string.Format("Push Transaction to Depth {0}", transactionDepth));
            return transactionState;
        }

        public abstract object RollbackTransaction(string factoryKey, object transactionState);

        public abstract bool TransactionIsActive(string factoryKey);

        protected void Log(string message)
        {
            logger.Debug(string.Format("{0}: {1}", this.Name, message));
        }
    }
}