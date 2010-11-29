namespace SharpArchContrib.Specifications.DomainModel.Conventions
{
    using System;

    using FluentNHibernate.Conventions;
    using FluentNHibernate.Conventions.Instances;

    public class ReferenceConvention : IReferenceConvention
    {
        public void Apply(IManyToOneInstance instance)
        {
            instance.Column(instance.Property.Name + "Id");
        }
    }
}