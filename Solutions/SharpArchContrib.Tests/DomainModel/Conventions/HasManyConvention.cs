namespace Tests.DomainModel.Conventions
{
    using System;

    using FluentNHibernate.Conventions;
    using FluentNHibernate.Conventions.Instances;

    public class HasManyConvention : IHasManyConvention
    {
        [CLSCompliant(false)]
        public void Apply(IOneToManyCollectionInstance instance)
        {
            instance.Key.Column(instance.EntityType.Name + "Id");
        }
    }
}