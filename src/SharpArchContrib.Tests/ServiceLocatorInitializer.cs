using System;
using Castle.MicroKernel.Registration;
using Castle.Windsor;
using CommonServiceLocator.WindsorAdapter;
using Microsoft.Practices.ServiceLocation;
using SharpArch.Data.NHibernate;
using SharpArchContrib.Castle.CastleWindsor;
using SharpArchContrib.Data.NHibernate;
using Tests.NHibernateTests;
using Tests.SharpArchContrib.Castle.Logging;
using Tests.SharpArchContrib.Castle.NHibernate;

namespace Tests {
    public static class ServiceLocatorInitializer {
        public static void Init() {
            Init(typeof(NHibernateTransactionManager));
        }

        public static void Init(Type transactionManagerType) {
            IWindsorContainer container = new WindsorContainer();
            ComponentRegistrar.AddComponentsTo(container, transactionManagerType);
            RegisterTestServices(container);
            ServiceLocator.SetLocatorProvider(() => new WindsorServiceLocator(container));
        }

        private static void RegisterTestServices(IWindsorContainer container) {
            container.Register(Component.For<ISessionFactoryKeyProvider>()
                .ImplementedBy<DefaultSessionFactoryKeyProvider>()
                .Named("SessionFactoryKeyProvider"));
            container.Register(Component.For<ILogTestClass>()
                .ImplementedBy<LogTestClass>()
                .Forward<IAmForwarded>()
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
        }
    }
}