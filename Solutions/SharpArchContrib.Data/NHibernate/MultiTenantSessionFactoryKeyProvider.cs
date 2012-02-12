namespace SharpArchContrib.Data.NHibernate
{
    using System;
    using System.Linq;

    using SharpArch.NHibernate;

    using SharpArchContrib.Core.Extensions;
    using SharpArchContrib.Core.MultiTenant;

    public class MultiTenantSessionFactoryKeyProvider : ISessionFactoryKeyProvider
    {
        private readonly ITenantContext tenantContext;

        public MultiTenantSessionFactoryKeyProvider(ITenantContext tenantContext)
        {
            this.tenantContext = tenantContext;
        }

        public string GetKey()
        {
            var key = this.tenantContext.Key;
            return string.IsNullOrEmpty(key) ? NHibernateSession.DefaultFactoryKey : key;
        }

        public string GetKeyFrom(object anObject)
        {
            var type = anObject.GetType();
            var isMultiTenant = type.IsImplementationOf<IMultiTenantQuery>() ||
                                type.IsImplementationOf<IMultiTenantRepository>() ||
                                IsRepositoryForMultiTenantEntity(type);
            return isMultiTenant ? GetKey() : NHibernateSession.DefaultFactoryKey;
        }

        public bool IsRepositoryForMultiTenantEntity(Type type)
        {
            if (!type.IsGenericType)
            {
                return false;
            }

            var genericTypes = type.GetGenericArguments();
            if (!genericTypes.Any())
            {
                return false;
            }

            var firstGenericType = genericTypes[0];
            return firstGenericType.IsImplementationOf<IMultiTenantEntity>();
        }
    }
}