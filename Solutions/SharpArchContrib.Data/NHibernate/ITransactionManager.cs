namespace SharpArchContrib.Data.NHibernate
{
    public interface ITransactionManager
    {
        string Name { get; }

        int TransactionDepth { get; }

        object CommitTransaction(string factoryKey, object transactionState);

        object PopTransaction(string factoryKey, object transactionState);

        object PushTransaction(string factoryKey, object transactionState);

        object RollbackTransaction(string factoryKey, object transactionState);

        bool TransactionIsActive(string factoryKey);
    }
}