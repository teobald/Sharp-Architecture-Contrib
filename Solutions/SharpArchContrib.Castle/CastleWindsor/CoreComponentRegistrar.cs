namespace SharpArchContrib.Core.CastleWindsor
{
    using global::Castle.MicroKernel.Registration;

    using global::Castle.Windsor;

    using SharpArchContrib.Core.Logging;

    public static class CoreComponentRegistrar
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