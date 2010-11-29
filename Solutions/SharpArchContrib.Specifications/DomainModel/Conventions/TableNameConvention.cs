namespace SharpArchContrib.Specifications.DomainModel.Conventions
{
    using System;

    using FluentNHibernate.Conventions;
    using FluentNHibernate.Conventions.Instances;

    public class TableNameConvention : IClassConvention
    {
        public void Apply(IClassInstance instance)
        {
            instance.Table(instance.EntityType.Name);
        }
    }
}