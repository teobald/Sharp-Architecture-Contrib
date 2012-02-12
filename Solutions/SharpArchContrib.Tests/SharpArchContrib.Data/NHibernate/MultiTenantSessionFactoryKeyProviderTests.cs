namespace Tests.SharpArchContrib.Data.NHibernate
{
    using NUnit.Framework;

    using Rhino.Mocks;

    using SharpArch.Domain.DomainModel;
    using SharpArch.Domain.PersistenceSupport;
    using SharpArch.NHibernate;
    using SharpArch.Testing.NUnit;

    using global::SharpArchContrib.Core.MultiTenant;
    using global::SharpArchContrib.Data.NHibernate;

    using Tests.DomainModel.Entities;

    public class TestMultiTenantEntity : Entity, IMultiTenantEntity
    {
    }

    public interface ITestRepository : IRepository<TestEntity>
    {
    }

    public interface ITestMultiTenantRepository : IRepository<TestMultiTenantEntity>, IMultiTenantRepository
    {
    }

    public class TestRepository : NHibernateRepository<TestEntity>, ITestRepository
    {
    }

    public class TestMultiTenantRepository : NHibernateRepository<TestMultiTenantEntity>, ITestMultiTenantRepository
    {
    }

    [TestFixture]
    public class MultiTenantSessionFactoryKeyProviderTests
    {
        private MultiTenantSessionFactoryKeyProvider provider;


        [SetUp]
        public void Setup()
        {
            var tenantContext = MockRepository.GenerateStub<ITenantContext>();
            this.provider = new MultiTenantSessionFactoryKeyProvider(tenantContext);
        }

        [Test]
        public void IsRepositoryForMultiTenantEntityReturnsFlaseForRepositoryForEntity()
        {
            var isRepositoryForMultiTenantEntity =
                this.provider.IsRepositoryForMultiTenantEntity(typeof(IRepository<TestEntity>));
            isRepositoryForMultiTenantEntity.ShouldEqual(false);
        }

        [Test]
        public void IsRepositoryForMultiTenantEntityReturnsTrueForRepositoryForMultiTenantEntity()
        {
            var isRepositoryForMultiTenantEntity =
                this.provider.IsRepositoryForMultiTenantEntity(typeof(IRepository<TestMultiTenantEntity>));
            isRepositoryForMultiTenantEntity.ShouldEqual(true);
        }
    }
}