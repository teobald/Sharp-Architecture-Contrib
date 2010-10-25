namespace Tests.NHibernateTests
{
    using SharpArch.Core.PersistenceSupport;

    using Tests.DomainModel.Entities;

    public interface ITransactionTestProvider
    {
        string Name { get; }

        IRepository<TestEntity> TestEntityRepository { get; set; }

        void CheckNumberOfEntities(int numberOfEntities);

        void DoCommit(string testEntityName);

        void DoCommitSilenceException(string testEntityName);

        void DoNestedCommit();

        void DoNestedForceRollback();

        void DoNestedInnerForceRollback();

        void DoRollback();

        void InitTransactionManager();
    }
}