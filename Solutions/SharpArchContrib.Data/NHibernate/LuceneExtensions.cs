namespace SharpArchContrib.Data.NHibernate
{
    using System.Collections.Generic;

    using global::NHibernate;
    using global::NHibernate.Search;

    using Lucene.Net.Analysis.Standard;
    using Lucene.Net.QueryParsers;
    using Lucene.Net.Search;
    using Lucene.Net.Util;

    using SharpArch.Core.PersistenceSupport;
    using SharpArch.Data.NHibernate;

    using SharpArchContrib.Core.Search;

    using NHSearch = global::NHibernate.Search;

    public static class LuceneExtensions
    {
        public static IList<T> Search<T, TId>(this IRepositoryWithTypedId<T, TId> repository, string searchQuery) where T : ISearchable
        {
            var parser = new MultiFieldQueryParser(Version.LUCENE_29, new[] { "Query" }, new StandardAnalyzer(Version.LUCENE_29));
            Query luceneQuery = parser.Parse(searchQuery);
            ISession session = NHibernateSession.CurrentFor(SessionFactoryAttribute.GetKeyFrom(repository));
            using (IFullTextSession fullTextSession = NHSearch.Search.CreateFullTextSession(session))
            {
                IQuery fullTextQuery = fullTextSession.CreateFullTextQuery(luceneQuery, new[] { typeof(T) });
                IList<T> results = fullTextQuery.List<T>();
                return results;
            }
        }
    }
}
