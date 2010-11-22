namespace SharpArchContrib.Specifications.DomainModel.Conventions
{
    using System;

    using FluentNHibernate.Conventions;
    using FluentNHibernate.Conventions.Instances;

    public class PrimaryKeyConvention : IIdConvention
    {
        public void Apply(IIdentityInstance instance)
        {
            instance.Column(instance.EntityType.Name + "Id");
            instance.UnsavedValue("0");
            instance.GeneratedBy.HiLo("0");
        }
    }
}