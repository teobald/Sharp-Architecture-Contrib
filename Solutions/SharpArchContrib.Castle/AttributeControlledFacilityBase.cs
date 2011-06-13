namespace SharpArchContrib.Castle
{
    using System;
    using System.Collections.Generic;

    using global::Castle.Core;
    using global::Castle.DynamicProxy;
    using global::Castle.MicroKernel;
    using global::Castle.MicroKernel.Facilities;
    using global::Castle.MicroKernel.Registration;

    using SharpArchContrib.Core;

    public abstract class AttributeControlledFacilityBase : AbstractFacility
    {
        private readonly Type interceptorType;

        private readonly LifestyleType lifestyleType;

        public AttributeControlledFacilityBase(Type interceptorType, LifestyleType lifestyleType)
        {
            ParameterCheck.ParameterRequired(interceptorType, "interceptorType");

            this.interceptorType = interceptorType;
            this.lifestyleType = lifestyleType;
        }

        protected abstract List<Attribute> GetAttributes(IHandler handler);

        protected override void Init()
        {
            this.Kernel.Register(Component.For<IInterceptor>()
                .ImplementedBy(this.interceptorType)
                .LifeStyle.Is(this.lifestyleType)
                .Named(this.interceptorType.Name));
            this.Kernel.ComponentRegistered += this.KernelComponentRegistered;
        }

        private bool AddInterceptorIfNeeded(IHandler handler, List<Attribute> attributes)
        {
            foreach (var attribute in attributes)
            {
                handler.ComponentModel.Interceptors.Add(new InterceptorReference(this.interceptorType.Name));
                return true;
            }

            return false;
        }

        private void KernelComponentRegistered(string key, IHandler handler)
        {
            this.AddInterceptorIfNeeded(handler, this.GetAttributes(handler));
        }
    }
}