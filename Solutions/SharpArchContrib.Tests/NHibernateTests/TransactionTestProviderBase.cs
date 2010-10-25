namespace Tests.NHibernateTests
{
    using System;

    using SharpArch.Core.PersistenceSupport;
    using SharpArch.Data.NHibernate;
    using SharpArch.Testing.NUnit;

    using global::SharpArchContrib.Data.NHibernate;

    using Tests.DomainModel.Entities;

    public abstract class TransactionTestProviderBase
    {
        public IRepository<TestEntity> TestEntityRepository { get; set; }

        protected abstract string TestEntityName { get; }

        public void CheckNumberOfEntities(int numberOfEntities)
        {
            var testEntityRepository = new Repository<TestEntity>();
            var testEntities = testEntityRepository.GetAll();
            testEntities.Count.ShouldEqual(numberOfEntities);
        }

        public virtual void Commit(string testEntityName)
        {
            this.DoCommit(testEntityName);
            this.CheckNumberOfEntities(1);
        }

        public virtual void DoCommit(string testEntityName)
        {
            this.InsertTestEntity(testEntityName);
        }

        public virtual void DoCommitSilenceException(string testEntityName)
        {
            this.InsertTestEntity(testEntityName);
            throw new Exception("Unknown Issue");
        }

        public virtual void DoNestedCommit()
        {
            this.InsertTestEntity(this.TestEntityName + "Outer");
            this.DoCommit(this.TestEntityName);
        }

        public virtual void DoNestedForceRollback()
        {
            this.InsertTestEntity(this.TestEntityName + "Outer");
            this.DoCommit(this.TestEntityName);
            throw new AbortTransactionException();
        }

        public virtual void DoNestedInnerForceRollback()
        {
            this.InsertTestEntity(this.TestEntityName + "Outer");
            this.DoRollback();
        }

        public virtual void DoRollback()
        {
            this.InsertTestEntity(this.TestEntityName);
            throw new AbortTransactionException();
        }

        protected void InsertTestEntity(string name)
        {
            var testEntityRepository = new Repository<TestEntity>();
            var testEntity = new TestEntity { Name = name };
            testEntityRepository.SaveOrUpdate(testEntity);
            NHibernateSession.Current.Evict(testEntity);
        }
    }
}