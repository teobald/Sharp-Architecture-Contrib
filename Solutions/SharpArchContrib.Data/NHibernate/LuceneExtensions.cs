namespace SharpArchContrib.Data.NHibernate
{
    using System.Collections.Generic;

    using global::NHibernate;
    using global::NHibernate.Search;

    using Lucene.Net.Analysis.Standard;
    using Lucene.Net.QueryParsers;
    using Lucene.Net.Search;

    using SharpArch.Core.PersistenceSupport;
    using SharpArch.Data.NHibernate;

    using SharpArchContrib.Core.Search;

    public static class LuceneExtensions
    {
        public static IList<T> Search<T, TId>(this IRepositoryWithTypedId<T, TId> repositoryWithTypedId, string searchQuery) where T : ISearchable
        {
            var parser = new MultiFieldQueryParser(new[] { "Query" }, new StandardAnalyzer());
            Query luceneQuery = parser.Parse(searchQuery);
            IFullTextSession session = global::NHibernate.Search.Search.CreateFullTextSession(NHibernateSession.Current);
            IQuery fullTextQuery = session.CreateFullTextQuery(luceneQuery, new[] { typeof(T) });
            IList<T> results = fullTextQuery.List<T>();
            return results;
        }
    }
}
