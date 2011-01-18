namespace SharpArchContrib.Data.NHibernate.Search
{
    using System;
    using System.IO;

    using global::NHibernate.Search;
    using global::NHibernate.Search.Cfg;

    using Lucene.Net.Analysis.Standard;
    using Lucene.Net.Index;
    using Lucene.Net.Store;

    using SharpArch.Core.PersistenceSupport;
    using SharpArch.Data.NHibernate;

    using SharpArchContrib.Core.Search;

    using Version = Lucene.Net.Util.Version;

    public class IndexBuilder<T> : IIndexBuilder<T> where T : ISearchable
    {
        private readonly IRepositoryWithTypedId<T, int> repository;
        private readonly string factoryKey;

        public IndexBuilder(IRepositoryWithTypedId<T, int> repository)
        {
            this.repository = repository;
            this.factoryKey = SessionFactoryAttribute.GetKeyFrom(repository);
        }

        public virtual void BuildSearchIndex()
        {
            this.CreateIndexDirectory();

            foreach (var instance in this.repository.GetAll())
            {
                this.AddToIndex(instance);
            }
        }

        public virtual void CreateIndexDirectory()
        {
            FSDirectory entityDirectory = null;
            IndexWriter writer = null;
            var entityType = typeof(T);

            DirectoryInfo entityIndexDirectory = this.GetEntityIndexDirectory(entityType);
            if (entityIndexDirectory.Exists)
            {
                entityIndexDirectory.Delete(true);
            }

            try
            {
                entityDirectory = FSDirectory.Open(entityIndexDirectory);
                writer = new IndexWriter(entityDirectory, new StandardAnalyzer(Version.LUCENE_29), true, IndexWriter.MaxFieldLength.UNLIMITED);
            }
            finally
            {
                if (entityDirectory != null)
                {
                    entityDirectory.Close();
                }

                if (writer != null)
                {
                    writer.Close();
                }
            }
        }

        public virtual void AddToIndex(T entity)
        {
            using (var fullTextSession = Search.CreateFullTextSession(NHibernateSession.CurrentFor(this.factoryKey)))
            {
                fullTextSession.Index(entity);
            }
        }

        public virtual DirectoryInfo GetEntityIndexDirectory(Type entityType)
        {
            INHSConfiguration nhsConfiguration = GetSearchConfiguration(this.factoryKey);
            string property = nhsConfiguration.Properties["hibernate.search.default.indexBase"];
            var fi = new FileInfo(property);
            var indexDirectoryPath = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, fi.Name, entityType.Name);
            return new DirectoryInfo(indexDirectoryPath);
        }

        private static INHSConfiguration GetSearchConfiguration(string factoryKey)
        {
            INHSConfigCollection nhsConfigCollection = CfgHelper.LoadConfiguration();
            INHSConfiguration nhsConfiguration = NHibernateSession.IsConfiguredForMultipleDatabases() ? 
                nhsConfigCollection.GetConfiguration(factoryKey) : nhsConfigCollection.DefaultConfiguration;

            return nhsConfiguration;
        }  
    }
}