namespace Tests.SharpArchContrib.Castle.Logging
{
    using System;

    using global::SharpArchContrib.Castle.Logging;
    using global::SharpArchContrib.Core.Logging;

    public class ExceptionHandlerTestClass : IExceptionHandlerTestClass
    {
        [ExceptionHandler(ExceptionType = typeof(NotImplementedException))]
        public float ThrowBaseExceptionNoCatch()
        {
            throw new Exception();
        }

        [ExceptionHandler]
        public void ThrowException()
        {
            throw new NotImplementedException();
        }

        [ExceptionHandler(IsSilent = true, ReturnValue = 6f)]
        public void ThrowExceptionSilent()
        {
            throw new NotImplementedException();
        }

        [ExceptionHandler(IsSilent = true, ReturnValue = 6f)]
        public float ThrowExceptionSilentWithReturn()
        {
            throw new NotImplementedException();
        }

        [ExceptionHandler(IsSilent = true, ReturnValue = 6f)]
        [Log(ExceptionLevel = LoggingLevel.Error)]
        public float ThrowExceptionSilentWithReturnWithLogAttribute()
        {
            throw new NotImplementedException();
        }

        [ExceptionHandler(IsSilent = true, ExceptionType = typeof(NotImplementedException), ReturnValue = 6f)]
        public float ThrowNotImplementedExceptionCatch()
        {
            throw new NotImplementedException();
        }
    }
}