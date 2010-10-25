namespace Tests.DomainModel.NHibernateMaps
{
    using System;

    using FluentNHibernate.Automapping;
    using FluentNHibernate.Automapping.Alterations;

    using Tests.DomainModel.Entities;

    public class TestEntityMap : IAutoMappingOverride<TestEntity>
    {
        [CLSCompliant(false)]
        public void Override(AutoMapping<TestEntity> mapping)
        {
            mapping.Map(c => c.Name).Unique();
        }
    }
}