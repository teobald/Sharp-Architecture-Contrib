namespace SharpArchContrib.Data.NHibernate
{
    using System.Collections.Generic;

    using global::NHibernate;
    using global::NHibernate.Search;

    using SharpArch.Core.PersistenceSupport;
    using SharpArch.Data.NHibernate;

    using SharpArchContrib.Core.Search;

    using NHSearch = global::NHibernate.Search;

    public static class LuceneExtensions
    {
        public static IList<T> Search<T, TId>(this IRepositoryWithTypedId<T, TId> repository, string searchQuery) where T : ISearchable
        {
            ISession session = NHibernateSession.CurrentFor(SessionFactoryAttribute.GetKeyFrom(repository));
            using (IFullTextSession fullTextSession = NHSearch.Search.CreateFullTextSession(session))
            {
                return fullTextSession.CreateFullTextQuery<T>(searchQuery).List<T>();
            }
        }
    }
}
