namespace Tests.SharpArchContrib.Castle.Logging
{
    using System;

    using Microsoft.Practices.ServiceLocation;

    using NUnit.Framework;

    using SharpArch.Testing.NUnit;

    [TestFixture]
    public class ExceptionHandlerAttributeTests
    {
        private IExceptionHandlerTestClass exceptionHandlerTestClass;

        [Test]
        public void LoggedExceptionDoesNotRethrow()
        {
            Assert.DoesNotThrow(() => this.exceptionHandlerTestClass.ThrowExceptionSilent());
        }

        [Test]
        public void LoggedExceptionDoesNotRethrowWithReturn()
        {
            this.exceptionHandlerTestClass.ThrowExceptionSilentWithReturn().ShouldEqual(6f);
        }

        [Test]
        public void LoggedExceptionDoesNotRethrowWithReturnWithLogAttribute()
        {
            this.exceptionHandlerTestClass.ThrowExceptionSilentWithReturnWithLogAttribute().ShouldEqual(6f);
        }

        [Test]
        public void LoggedExceptionRethrows()
        {
            Assert.Throws<NotImplementedException>(() => this.exceptionHandlerTestClass.ThrowException());
        }

        [SetUp]
        public void SetUp()
        {
            this.exceptionHandlerTestClass = ServiceLocator.Current.GetInstance<IExceptionHandlerTestClass>();
        }

        [Test]
        public void ThrowBaseExceptionNoCatch()
        {
            Assert.Throws<Exception>(() => this.exceptionHandlerTestClass.ThrowBaseExceptionNoCatch());
        }

        [Test]
        public void ThrowNotImplementedExceptionCatch()
        {
            this.exceptionHandlerTestClass.ThrowNotImplementedExceptionCatch().ShouldEqual(6f);
        }
    }
}