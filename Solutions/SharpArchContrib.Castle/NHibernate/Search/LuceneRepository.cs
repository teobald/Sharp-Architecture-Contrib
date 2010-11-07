namespace SharpArchContrib.Castle.NHibernate.Search
{
    using System;
    using System.Collections.Generic;
    using System.IO;

    using global::NHibernate;
    using global::NHibernate.Search;
    using global::NHibernate.Search.Cfg;

    using Lucene.Net.Analysis.Standard;
    using Lucene.Net.Index;
    using Lucene.Net.QueryParsers;
    using Lucene.Net.Search;
    using Lucene.Net.Store;

    using SharpArch.Data.NHibernate;

    using SharpArchContrib.Core.LuceneSupport;

    public class LuceneRepository<T> : LuceneRepositoryWithTypeId<T, int>
    {
    }


    public class LuceneRepositoryWithTypeId<T, IdT> : RepositoryWithTypedId<T, IdT>, ILuceneRepositoryWithTypedId<T, IdT>
    {
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

            IFullTextSession fullTextSession = Search.CreateFullTextSession(this.Session);
            foreach (var instance in Session.CreateCriteria(typeof(T)).List<T>())
            {
                fullTextSession.Index(instance);
            }
        }

        public IList<T> Query(string searchQuery)
        {
            var parser = new MultiFieldQueryParser(new[] { "Query" }, new StandardAnalyzer());
            Query luceneQuery = parser.Parse(searchQuery);
            IFullTextSession session = Search.CreateFullTextSession(this.Session);
            IQuery fullTextQuery = session.CreateFullTextQuery(luceneQuery, new[] { typeof(T) });
            IList<T> results = fullTextQuery.List<T>();

            return results;
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
