namespace SharpArchContrib.Core.LuceneSupport
{
    using System.Collections.Generic;

    using SharpArch.Core.PersistenceSupport;

    public interface ILuceneRepository<T> : IRepositoryWithTypedId<T, int>
    {
    }

    public interface ILuceneRepositoryWithTypedId<T, IdT> : IRepositoryWithTypedId<T, IdT>
    {
        void BuildSearchIndex();

        IList<T> Query(string query);
    }
}
