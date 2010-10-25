namespace SharpArchContrib.Castle.Logging
{
    using System;
    using System.Collections.Generic;

    using global::Castle.Core;
    using global::Castle.MicroKernel;

    public class LogFacility : AttributeControlledFacilityBase
    {
        public LogFacility()
            : base(typeof(LogInterceptor), LifestyleType.Singleton)
        {
        }

        protected override List<Attribute> GetAttributes(IHandler handler)
        {
            var attributes = new List<Attribute>();
            this.AddAssemblyLevelAttributes(attributes, handler);
            this.AddClassLevelAttributes(attributes, handler);
            this.AddMethodLevelAttributes(attributes, handler);

            return attributes;
        }

        private void AddAssemblyLevelAttributes(List<Attribute> attributes, IHandler handler)
        {
            attributes.AddRange(
                (Attribute[])
                handler.ComponentModel.Implementation.Assembly.GetCustomAttributes(typeof(LogAttribute), false));
        }

        private void AddClassLevelAttributes(List<Attribute> attributes, IHandler handler)
        {
            attributes.AddRange(
                (Attribute[])handler.ComponentModel.Implementation.GetCustomAttributes(typeof(LogAttribute), false));
        }

        private void AddMethodLevelAttributes(List<Attribute> attributes, IHandler handler)
        {
            foreach (var methodInfo in handler.ComponentModel.Implementation.GetMethods())
            {
                attributes.AddRange((Attribute[])methodInfo.GetCustomAttributes(typeof(LogAttribute), false));
            }
        }
    }
}