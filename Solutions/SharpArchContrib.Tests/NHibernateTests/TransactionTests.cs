namespace Tests.NHibernateTests
{
    using System;
    using System.Collections.Generic;

    using log4net;

    using Microsoft.Practices.ServiceLocation;

    using NHibernate.Exceptions;

    using NUnit.Framework;

    using global::SharpArchContrib.Data.NHibernate;

    [TestFixture]
    [Category(TestCategories.FileDbTests)]
    public class TransactionTests : TransactionTestsBase
    {
        private const string TestEntityName = "TransactionTest";

        private List<ITransactionTestProvider> transactionTestProviders = new List<ITransactionTestProvider>();

        public TransactionTests()
        {
            ServiceLocatorInitializer.Init(typeof(SystemTransactionManager));

            // transactionTestProviders.Add(new SharpArchContrib.PostSharp.NHibernate.SystemTransactionTestProvider());
            // transactionTestProviders.Add(new SharpArchContrib.PostSharp.NHibernate.SystemUnitOfWorkTestProvider());
            this.transactionTestProviders.Add(
                ServiceLocator.Current.GetInstance<ITransactionTestProvider>("SystemTransactionTestProvider"));
            this.transactionTestProviders.Add(
                ServiceLocator.Current.GetInstance<ITransactionTestProvider>("SystemUnitOfWorkTestProvider"));

            ServiceLocatorInitializer.Init(typeof(NHibernateTransactionManager));

            // transactionTestProviders.Add(new SharpArchContrib.PostSharp.NHibernate.NHibernateTransactionTestProvider());
            // transactionTestProviders.Add(new SharpArchContrib.PostSharp.NHibernate.NHibernateUnitOfWorkTestProvider());
            this.transactionTestProviders.Add(
                ServiceLocator.Current.GetInstance<ITransactionTestProvider>("NHibernateTransactionTestProvider"));
            this.transactionTestProviders.Add(
                ServiceLocator.Current.GetInstance<ITransactionTestProvider>("NHibernateUnitOfWorkTestProvider"));
        }

        [Test]
        public void MultipleOperations()
        {
            this.PerformTest("MultipleOperations", testProvider => this.PerformMultipleOperations(testProvider));
        }

        [Test]
        public void MultipleOperationsRollbackFirst()
        {
            this.PerformTest(
                "MultipleOperationsRollbackFirst", 
                testProvider => this.PerformMultipleOperationsRollbackFirst(testProvider));
        }

        [Test]
        public void MultipleOperationsRollbackLast()
        {
            this.PerformTest(
                "MultipleOperationsRollbackLast", 
                testProvider => this.PerformMultipleOperationsRollbackLast(testProvider));
        }

        [Test]
        public void NestedCommit()
        {
            this.PerformTest("NestedCommit", testProvider => this.PerformNestedCommit(testProvider));
        }

        [Test]
        public void NestedForceRollback()
        {
            this.PerformTest("NestedForceRollback", testProvider => this.PerformNestedForceRollback(testProvider));
        }

        [Test]
        public void NestedInnerForceRollback()
        {
            this.PerformTest(
                "NestedInnerForceRollback", testProvider => this.PerformNestedInnerForceRollback(testProvider));
        }

        public void PerformTest(string testName, Func<ITransactionTestProvider, bool> function)
        {
            var logger = LogManager.GetLogger(this.GetType());
            logger.Debug(string.Format("Starting test {0}", testName));
            try
            {
                foreach (var transactionTestProvider in this.transactionTestProviders)
                {
                    this.SetUp();
                    try
                    {
                        logger.Debug(string.Format("Transaction Provider: {0}", transactionTestProvider.Name));
                        try
                        {
                            function(transactionTestProvider);
                        }
                        catch (Exception err)
                        {
                            logger.Debug(err);
                            throw;
                        }
                        finally
                        {
                            logger.Debug(
                                string.Format(
                                    "*** Completed Work With Transaction Provider: {0}", transactionTestProvider.Name));
                        }
                    }
                    finally
                    {
                        this.TearDown();
                    }
                }
            }
            finally
            {
                logger.Debug(string.Format("*** Completed test {0}", testName));
            }
        }

        [Test]
        public void Rollback()
        {
            this.PerformTest("Rollback", testProvider => this.PerformRollback(testProvider));
        }

        [Test]
        public void RollsbackOnException()
        {
            this.PerformTest("RollsbackOnException", testProvider => this.PerformRollsbackOnException(testProvider));
        }

        [Test]
        public void RollsbackOnExceptionWithSilentException()
        {
            this.PerformTest(
                "RollsbackOnExceptionWithSilentException", 
                testProvider => this.PerformRollsbackOnExceptionWithSilentException(testProvider));
        }

        [Test]
        public void SingleOperation()
        {
            this.PerformTest("SingleOperation", testProvider => this.PerformSingleOperation(testProvider));
        }

        protected override void InitializeData()
        {
            base.InitializeData();
            this.transactionTestProviders = this.GenerateTransactionManagers();
        }

        private List<ITransactionTestProvider> GenerateTransactionManagers()
        {
            foreach (var transactionTestProvider in
                this.transactionTestProviders)
            {
                transactionTestProvider.TestEntityRepository = this.testEntityRepository;
            }

            return this.transactionTestProviders;
        }

        // Tests call Setup and TearDown manually for each iteration of the loop since
        // we want a clean database for each iteration.  We could use the parameterized
        // test feature of Nunit 2.5 but, unfortunately that doesn't work with all test runners (e.g. Resharper)

        private bool PerformMultipleOperations(ITransactionTestProvider transactionTestProvider)
        {
            transactionTestProvider.InitTransactionManager();
            transactionTestProvider.DoCommit(TestEntityName);
            transactionTestProvider.CheckNumberOfEntities(1);

            transactionTestProvider.DoCommit(TestEntityName + "1");
            transactionTestProvider.CheckNumberOfEntities(2);
            return true;
        }

        private bool PerformMultipleOperationsRollbackFirst(ITransactionTestProvider transactionTestProvider)
        {
            transactionTestProvider.InitTransactionManager();
            transactionTestProvider.DoRollback();
            transactionTestProvider.CheckNumberOfEntities(0);

            transactionTestProvider.DoCommit(TestEntityName);
            transactionTestProvider.CheckNumberOfEntities(1);

            return true;
        }

        private bool PerformMultipleOperationsRollbackLast(ITransactionTestProvider transactionTestProvider)
        {
            transactionTestProvider.InitTransactionManager();
            transactionTestProvider.DoCommit(TestEntityName + "1");
            transactionTestProvider.CheckNumberOfEntities(1);

            transactionTestProvider.DoRollback();
            transactionTestProvider.CheckNumberOfEntities(1);

            return true;
        }

        private bool PerformNestedCommit(ITransactionTestProvider transactionTestProvider)
        {
            transactionTestProvider.InitTransactionManager();
            transactionTestProvider.DoNestedCommit();
            transactionTestProvider.CheckNumberOfEntities(2);

            return true;
        }

        private bool PerformNestedForceRollback(ITransactionTestProvider transactionTestProvider)
        {
            transactionTestProvider.InitTransactionManager();
            transactionTestProvider.DoNestedForceRollback();
            transactionTestProvider.CheckNumberOfEntities(0);

            return true;
        }

        private bool PerformNestedInnerForceRollback(ITransactionTestProvider transactionTestProvider)
        {
            transactionTestProvider.InitTransactionManager();
            transactionTestProvider.DoNestedInnerForceRollback();
            transactionTestProvider.CheckNumberOfEntities(0);

            return true;
        }

        private bool PerformRollback(ITransactionTestProvider transactionTestProvider)
        {
            transactionTestProvider.InitTransactionManager();
            transactionTestProvider.DoRollback();
            transactionTestProvider.CheckNumberOfEntities(0);

            return true;
        }

        private bool PerformRollsbackOnException(ITransactionTestProvider transactionTestProvider)
        {
            transactionTestProvider.InitTransactionManager();
            transactionTestProvider.DoCommit(TestEntityName);
            transactionTestProvider.CheckNumberOfEntities(1);

            Assert.Throws<GenericADOException>(() => transactionTestProvider.DoCommit(TestEntityName));
            transactionTestProvider.CheckNumberOfEntities(1);

            return true;
        }

        private bool PerformRollsbackOnExceptionWithSilentException(ITransactionTestProvider transactionTestProvider)
        {
            transactionTestProvider.InitTransactionManager();
            transactionTestProvider.DoCommit(TestEntityName);
            transactionTestProvider.CheckNumberOfEntities(1);

            transactionTestProvider.DoCommitSilenceException(TestEntityName);
            transactionTestProvider.CheckNumberOfEntities(1);

            return true;
        }

        private bool PerformSingleOperation(ITransactionTestProvider transactionTestProvider)
        {
            transactionTestProvider.InitTransactionManager();
            transactionTestProvider.DoCommit(TestEntityName);
            transactionTestProvider.CheckNumberOfEntities(1);

            return true;
        }
    }
}