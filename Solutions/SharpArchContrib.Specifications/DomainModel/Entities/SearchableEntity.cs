namespace SharpArchContrib.Specifications.DomainModel.Entities
{
    using NHibernate.Search.Attributes;

    using SharpArch.Core.DomainModel;

    using SharpArchContrib.Core.Search;

    public class SearchableEntity : Entity, ISearchable
    {
        [DocumentId]
        public override int Id
        {
            get
            {
                return base.Id;
            }
            protected set
            {
                base.Id = value;
            }
        }
    }
}