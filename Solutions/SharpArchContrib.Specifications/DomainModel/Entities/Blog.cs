namespace SharpArchContrib.Specifications.DomainModel.Entities
{
    using System.Collections.Generic;

    using NHibernate.Search.Attributes;

    [Indexed]
    public class Blog : SearchableEntity
    {
        [Field(Index.Tokenized, Store = Store.Yes)]
        public virtual string Name { get; set; }

        [Field(Index.Tokenized)]
        public virtual string Description { get; set; }

        [IndexedEmbedded]
        public virtual IList<Post> Posts { get; set; }
    }
}
