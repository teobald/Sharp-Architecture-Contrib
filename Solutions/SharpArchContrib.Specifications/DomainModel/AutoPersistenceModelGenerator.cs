namespace SharpArchContrib.Specifications.DomainModel
{
    using System;
    using System.Linq;

    using FluentNHibernate.Automapping;
    using FluentNHibernate.Conventions;

    using SharpArch.Core.DomainModel;
    using SharpArch.Data.NHibernate.FluentNHibernate;

    using SharpArchContrib.Specifications.DomainModel.Conventions;
    using SharpArchContrib.Specifications.DomainModel.Entities;

    public class AutoPersistenceModelGenerator : IAutoPersistenceModelGenerator
    {
        public AutoPersistenceModel Generate()
        {
            var mappings = new AutoPersistenceModel();
            mappings.AddEntityAssembly(typeof(Blog).Assembly).Where(this.GetAutoMappingFilter);
            mappings.Conventions.Setup(this.GetConventions());
            mappings.Setup(this.GetSetup());
            mappings.IgnoreBase<SearchableEntity>();
            mappings.IgnoreBase(typeof(EntityWithTypedId<>));
            mappings.UseOverridesFromAssemblyOf<AutoPersistenceModelGenerator>();
            return mappings;
        }

        /// <summary>
        ///   Provides a filter for only including types which inherit from the IEntityWithTypedId interface.
        /// </summary>
        private bool GetAutoMappingFilter(Type t)
        {
            return
                t.GetInterfaces().Any(
                    x => x.IsGenericType && x.GetGenericTypeDefinition() == typeof(IEntityWithTypedId<>));
        }

        private Action<IConventionFinder> GetConventions()
        {
            return c =>
                {
                    c.Add<PrimaryKeyConvention>();
                    c.Add<ReferenceConvention>();
                    c.Add<HasManyConvention>();
                    c.Add<TableNameConvention>();
                };
        }

        private Action<AutoMappingExpressions> GetSetup()
        {
            return c => { c.FindIdentity = type => type.Name == "Id"; };
        }
    }
}