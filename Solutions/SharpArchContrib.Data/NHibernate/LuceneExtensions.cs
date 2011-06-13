namespace SharpArchContrib.Data.NHibernate
{
    using System;
    using System.Collections.Generic;

    using global::NHibernate;
    using global::NHibernate.Search;

    using SharpArch.NHibernate;
    using SharpArch.NHibernate.Contracts.Repositories;

    using SharpArchContrib.Core.Search;

    using NHSearch = global::NHibernate.Search;

    public static class LuceneExtensions
    {
        [CLSCompliant(false)]
        public static IList<T> Search<T, TId>(this INHibernateRepositoryWithTypedId<T, TId> repository, string searchQuery) where T : ISearchable
        {
            ISession session = NHibernateSession.CurrentFor(SessionFactoryKeyHelper.GetKeyFrom(repository));
            using (IFullTextSession fullTextSession = NHSearch.Search.CreateFullTextSession(session))
            {
                return fullTextSession.CreateFullTextQuery<T>(searchQuery).List<T>();
            }
        }
    }
}
