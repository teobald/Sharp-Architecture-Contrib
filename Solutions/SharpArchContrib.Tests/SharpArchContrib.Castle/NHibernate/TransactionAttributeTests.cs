namespace Tests.SharpArchContrib.Castle.NHibernate
{
    using System;

    using NUnit.Framework;

    using SharpArch.Testing.NUnit;

    using global::SharpArchContrib.Castle.NHibernate;

    [TestFixture]
    public class TransactionAttributeTests
    {
        [Test]
        public void Gathering_Transaction_Attributes_Does_Not_Gather_UnitOfWork()
        {
            foreach (var methodInfo in typeof(TestClass).GetMethods())
            {
                if (methodInfo.Name == "Transaction" || methodInfo.Name == "UnitOfWork")
                {
                    var attributes = (Attribute[])methodInfo.GetCustomAttributes(typeof(TransactionAttribute), false);
                    attributes.Length.ShouldEqual(1);
                    foreach (var attribute in attributes)
                    {
                        attribute.ShouldBeOfType(typeof(TransactionAttribute));
                        attribute.ShouldNotBeOfType(typeof(UnitOfWorkAttribute));
                    }
                }
            }
        }

        public class TestClass
        {
            [Transaction]
            [UnitOfWork]
            public void Transaction()
            {
            }

            [UnitOfWork]
            [Transaction]
            public void UnitOfWork()
            {
            }
        }
    }
}