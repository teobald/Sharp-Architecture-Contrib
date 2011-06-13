namespace Tests.SharpArchContrib.PostSharp.Logging
{
    using System;

    using NUnit.Framework;

    using SharpArch.Testing.NUnit;

    using global::SharpArchContrib.Core.Logging;
    using global::SharpArchContrib.PostSharp.Logging;

    [TestFixture]
    public class ExceptionHandlerAttributeTests
    {
        [Test]
        public void LoggedExceptionDoesNotRethrow()
        {
            Assert.DoesNotThrow(() => this.ThrowExceptionSilent());
        }

        [Test]
        public void LoggedExceptionDoesNotRethrowWithReturn()
        {
            this.ThrowExceptionSilentWithReturn().ShouldEqual(6f);
        }

        [Test]
        public void LoggedExceptionDoesNotRethrowWithReturnWithLogAttribute()
        {
            this.ThrowExceptionSilentWithReturnWithLogAttribute().ShouldEqual(6f);
        }

        [Test]
        public void LoggedExceptionRethrows()
        {
            Assert.Throws<NotImplementedException>(() => this.ThrowException());
        }

        [ExceptionHandler]
        private void ThrowException()
        {
            throw new NotImplementedException();
        }

        [ExceptionHandler(IsSilent = true, ReturnValue = 6f)]
        private void ThrowExceptionSilent()
        {
            throw new NotImplementedException();
        }

        [ExceptionHandler(IsSilent = true, ReturnValue = 6f)]
        private float ThrowExceptionSilentWithReturn()
        {
            throw new NotImplementedException();
        }

        [ExceptionHandler(IsSilent = true, ReturnValue = 6f, AspectPriority = 1)]
        [Log(ExceptionLevel = LoggingLevel.Error, AspectPriority = 2)]
        private float ThrowExceptionSilentWithReturnWithLogAttribute()
        {
            throw new NotImplementedException();
        }
    }
}