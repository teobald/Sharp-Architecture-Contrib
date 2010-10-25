namespace Tests.NHibernateTests
{
    using System;
    using System.IO;
    using System.Reflection;

    using FluentNHibernate.Automapping;

    using NHibernate.Cfg;
    using NHibernate.Tool.hbm2ddl;

    using SharpArch.Core.PersistenceSupport;
    using SharpArch.Data.NHibernate;
    using SharpArch.Data.NHibernate.FluentNHibernate;

    using global::SharpArchContrib.Data.NHibernate;

    using Tests.DomainModel.Entities;

    public class TransactionTestsBase
    {
        protected IRepository<TestEntity> testEntityRepository;

        public void SetUp()
        {
            this.InitializeDatabase();
            this.InitializeData();
        }

        public void TearDown()
        {
            this.Shutdown();
        }

        protected virtual void InitializeData()
        {
            this.testEntityRepository = new Repository<TestEntity>();
        }

        private AutoPersistenceModel GetAutoPersistenceModel(string[] assemblies)
        {
            foreach (var asmName in assemblies)
            {
                var asm = Assembly.Load(asmName);
                var asmTypes = asm.GetTypes();

                foreach (var asmType in asmTypes)
                {
                    if (typeof(IAutoPersistenceModelGenerator).IsAssignableFrom(asmType))
                    {
                        var generator = Activator.CreateInstance(asmType) as IAutoPersistenceModelGenerator;
                        return generator.Generate();
                    }
                }
            }

            return null;
        }

        private string[] GetMappingAssemblies()
        {
            return new[] { "SharpArchContrib.Tests" };
        }

        private void InitializeDatabase()
        {
            if (File.Exists("db.dat"))
            {
                File.Delete("db.dat");
            }

            var cfg = this.InitializeNHibernateSession();
            var connection = NHibernateSession.Current.Connection;
            new SchemaExport(cfg).Execute(false, true, false, connection, null);
        }

        private Configuration InitializeNHibernateSession()
        {
            var mappingAssemblies = this.GetMappingAssemblies();
            var autoPersistenceModel = this.GetAutoPersistenceModel(mappingAssemblies);
            return NHibernateSession.Init(
                new ThreadSessionStorage(), mappingAssemblies, autoPersistenceModel, "HibernateFile.cfg.xml");
        }

        private void Shutdown()
        {
            NHibernateSession.CloseAllSessions();
            NHibernateSession.Reset();
        }
    }
}