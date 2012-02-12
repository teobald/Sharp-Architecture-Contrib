namespace Tests.DomainModel.Conventions
{
    using System;

    using FluentNHibernate.Conventions;
    using FluentNHibernate.Conventions.Instances;

    public class TableNameConvention : IClassConvention
    {
        [CLSCompliant(false)]
        public void Apply(IClassInstance instance)
        {
            instance.Table(instance.EntityType.Name);
        }
    }
}