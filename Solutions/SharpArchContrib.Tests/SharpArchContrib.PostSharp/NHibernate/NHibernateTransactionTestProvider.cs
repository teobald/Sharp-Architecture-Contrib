namespace Tests.SharpArchContrib.PostSharp.NHibernate
{
    using global::SharpArchContrib.Data.NHibernate;
    using global::SharpArchContrib.PostSharp.NHibernate;

    using Tests.NHibernateTests;

    public class NHibernateTransactionTestProvider : TransactionTestProviderBase, ITransactionTestProvider
    {
        public string Name
        {
            get
            {
                return "PostSharp NHibernateTransactionTestProvider";
            }
        }

        protected override string TestEntityName
        {
            get
            {
                return "NHibernateTransactionTest";
            }
        }

        [Transaction]
        public override void DoCommit(string testEntityName)
        {
            base.DoCommit(testEntityName);
        }

        [Transaction(IsExceptionSilent = true)]
        public override void DoCommitSilenceException(string testEntityName)
        {
            base.DoCommitSilenceException(testEntityName);
        }

        [Transaction]
        public override void DoNestedCommit()
        {
            base.DoNestedCommit();
        }

        [Transaction]
        public override void DoNestedForceRollback()
        {
            base.DoNestedInnerForceRollback();
        }

        [Transaction]
        public override void DoNestedInnerForceRollback()
        {
            base.DoNestedInnerForceRollback();
        }

        [Transaction]
        public override void DoRollback()
        {
            base.DoRollback();
        }

        public void InitTransactionManager()
        {
            ServiceLocatorInitializer.Init(typeof(NHibernateTransactionManager));
        }
    }
}