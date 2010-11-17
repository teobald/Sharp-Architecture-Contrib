namespace SharpArchContrib.Data.NHibernate.Search
{
    public interface IIndexBuilder<T>
    {
        void BuildSearchIndex();
    }
}