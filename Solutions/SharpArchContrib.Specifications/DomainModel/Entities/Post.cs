namespace SharpArchContrib.Specifications.DomainModel.Entities
{
    using NHibernate.Search.Attributes;

    [Indexed]
    public class Post : SearchableEntity
    {
        [Field(Index.Tokenized, Store = Store.Yes)]
        public virtual string Title { get; set; }


        [Field(Index.Tokenized, Store = Store.Yes)]
        public virtual string Body { get; set; }
    }
}
