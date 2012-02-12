namespace SharpArchContrib.Castle.NHibernate
{
    using System;
    using System.Collections.Generic;

    using global::Castle.Core;
    using global::Castle.MicroKernel;

    public class TransactionFacility : AttributeControlledFacilityBase
    {
        public TransactionFacility()
            : base(typeof(TransactionInterceptor), LifestyleType.Transient)
        {
        }

        protected override List<Attribute> GetAttributes(IHandler handler)
        {
            var attributes = new List<Attribute>();
            this.AddClassLevelAttributes(attributes, handler);
            this.AddMethodLevelAttributes(attributes, handler);

            return attributes;
        }

        private void AddClassLevelAttributes(List<Attribute> attributes, IHandler handler)
        {
            attributes.AddRange(
                (Attribute[])
                handler.ComponentModel.Implementation.GetCustomAttributes(typeof(TransactionAttribute), false));
        }

        private void AddMethodLevelAttributes(List<Attribute> attributes, IHandler handler)
        {
            foreach (var methodInfo in handler.ComponentModel.Implementation.GetMethods())
            {
                attributes.AddRange((Attribute[])methodInfo.GetCustomAttributes(typeof(TransactionAttribute), false));
            }
        }
    }
}