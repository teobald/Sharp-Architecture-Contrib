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

    public class IndexBuilder<T> : IIndexBuilder<T>
    {
        private readonly IRepositoryWithTypedId<T, int> repository;

        public IndexBuilder(IRepositoryWithTypedId<T, int> repository)
        {
            this.repository = repository;
        }

        public virtual void BuildSearchIndex()
        {
            FSDirectory entityDirectory = null;
            IndexWriter writer = null;
            var entityType = typeof(T);

            var indexDirectory = new DirectoryInfo(GetIndexDirectory());

            if (indexDirectory.Exists)
            {
                indexDirectory.Delete(true);
            }

            try
            {
                entityDirectory = FSDirectory.GetDirectory(Path.Combine(indexDirectory.FullName, entityType.Name), true);
                writer = new IndexWriter(entityDirectory, new StandardAnalyzer(), true);
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

            IFullTextSession fullTextSession = Search.CreateFullTextSession(NHibernateSession.Current);

            foreach (var instance in this.repository.GetAll())
            {
                fullTextSession.Index(instance);
            }
        }

        private static string GetIndexDirectory()
        {
            INHSConfigCollection nhsConfigCollection = CfgHelper.LoadConfiguration();
            string property = nhsConfigCollection.DefaultConfiguration.Properties["hibernate.search.default.indexBase"];
            var fi = new FileInfo(property);
            return Path.Combine(AppDomain.CurrentDomain.BaseDirectory, fi.Name);
        }
    }
}