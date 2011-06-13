namespace SharpArchContrib.Core.CastleWindsor
{
    using Castle.MicroKernel.Registration;
    using Castle.Windsor;

    using SharpArchContrib.Core.Logging;

    public static class ComponentRegistrar
    {
        public static void AddComponentsTo(IWindsorContainer container)
        {
            ParameterCheck.ParameterRequired(container, "container");

            if (!container.Kernel.HasComponent("ExceptionLogger"))
            {
                container.Register(Component.For<IExceptionLogger>()
                    .ImplementedBy<ExceptionLogger>()
                    .Named("ExceptionLogger"));
                container.Register(Component.For<IMethodLogger>()
                    .ImplementedBy<MethodLogger>()
                    .Named("MethodLogger"));
            }
        }
    }
}