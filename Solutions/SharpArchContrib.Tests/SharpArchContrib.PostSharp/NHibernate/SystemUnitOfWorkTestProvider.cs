namespace Tests.SharpArchContrib.PostSharp.NHibernate
{
    using global::SharpArchContrib.Data.NHibernate;
    using global::SharpArchContrib.PostSharp.NHibernate;

    using Tests.NHibernateTests;

    public class SystemUnitOfWorkTestProvider : TransactionTestProviderBase, ITransactionTestProvider
    {
        public string Name
        {
            get
            {
                return "PostSharp SystemUnitOfWorkTestProvider";
            }
        }

        protected override string TestEntityName
        {
            get
            {
                return "UnitOfWorkTest";
            }
        }

        [UnitOfWork]
        public override void DoCommit(string testEntityName)
        {
            base.DoCommit(testEntityName);
        }

        [Transaction(IsExceptionSilent = true)]
        public override void DoCommitSilenceException(string testEntityName)
        {
            base.DoCommitSilenceException(testEntityName);
        }

        [UnitOfWork]
        public override void DoNestedCommit()
        {
            base.DoNestedCommit();
        }

        [UnitOfWork]
        public override void DoNestedForceRollback()
        {
            base.DoNestedInnerForceRollback();
        }

        [UnitOfWork]
        public override void DoNestedInnerForceRollback()
        {
            base.DoNestedInnerForceRollback();
        }

        [UnitOfWork]
        public override void DoRollback()
        {
            base.DoRollback();
        }

        public void InitTransactionManager()
        {
            ServiceLocatorInitializer.Init(typeof(SystemTransactionManager));
        }
    }
}