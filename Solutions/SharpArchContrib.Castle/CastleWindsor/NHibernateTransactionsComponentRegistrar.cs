namespace SharpArchContrib.Data.CastleWindsor
{
    using System;

    using global::Castle.MicroKernel.Registration;
    using global::Castle.Windsor;

    using SharpArchContrib.Core;
    using SharpArchContrib.Data.NHibernate;

    public static class NHibernateTransactionsComponentRegistrar
    {
        public static void AddComponentsTo(IWindsorContainer container)
        {
            AddComponentsTo(container, typeof(NHibernateTransactionManager));
        }

        public static void AddComponentsTo(IWindsorContainer container, Type transactionManagerType)
        {
            ParameterCheck.ParameterRequired(container, "container");
            ParameterCheck.ParameterRequired(transactionManagerType, "transactionManagerType");

            if (!container.Kernel.HasComponent("TransactionManager"))
            {
                Core.CastleWindsor.CoreComponentRegistrar.AddComponentsTo(container);
                container.Register(Component.For<ITransactionManager>()
                    .ImplementedBy(transactionManagerType)
                    .Named("TransactionManager"));
            }
        }
    }
}