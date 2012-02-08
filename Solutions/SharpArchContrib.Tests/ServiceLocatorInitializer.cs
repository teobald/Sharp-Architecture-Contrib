namespace Tests
{
    using System;

    using Castle.MicroKernel.Registration;
    using Castle.Windsor;

    using CommonServiceLocator.WindsorAdapter;

    using Microsoft.Practices.ServiceLocation;

    using SharpArch.Domain.PersistenceSupport;
    using SharpArch.NHibernate;

    using global::SharpArchContrib.Castle.CastleWindsor;

    using global::SharpArchContrib.Data.NHibernate;

    using Tests.NHibernateTests;
    using Tests.SharpArchContrib.Castle.Logging;
    using Tests.SharpArchContrib.Castle.NHibernate;

    public static class ServiceLocatorInitializer
    {
        public static void Init()
        {
            Init(typeof(NHibernateTransactionManager));
        }

        public static void Init(Type transactionManagerType)
        {
            IWindsorContainer container = new WindsorContainer();
            ComponentRegistrar.AddComponentsTo(container, transactionManagerType);
            RegisterTestServices(container);
            ServiceLocator.SetLocatorProvider(() => new WindsorServiceLocator(container));
        }

        private static void RegisterTestServices(IWindsorContainer container)
        {
            container.Register(Component.For<ILogTestClass>()
                .ImplementedBy<LogTestClass>()
                .Named("logTestClass"));
            container.Register(Component.For<ITransactionTestProvider>()
                .ImplementedBy<SystemTransactionTestProvider>()
                .Named("SystemTransactionTestProvider"));
            container.Register(Component.For<ITransactionTestProvider>()
                .ImplementedBy<NHibernateTransactionTestProvider>()
                .Named("NHibernateTransactionTestProvider"));
            container.Register(Component.For<ITransactionTestProvider>()
                .ImplementedBy<SystemUnitOfWorkTestProvider>()
                .Named("SystemUnitOfWorkTestProvider"));
            container.Register(Component.For<ITransactionTestProvider>()
                .ImplementedBy<NHibernateUnitOfWorkTestProvider>()
                .Named("NHibernateUnitOfWorkTestProvider"));
            container.Register(Component.For<IExceptionHandlerTestClass>()
                .ImplementedBy<ExceptionHandlerTestClass>()
                .Named("ExceptionHandlerTestClass"));
            container.Register(Component.For<ISessionFactoryKeyProvider>()
                .ImplementedBy<DefaultSessionFactoryKeyProvider>()
                .Named("SessionFactoryKeyProvider"));
        }
    }
}