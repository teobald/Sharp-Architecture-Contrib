namespace Tests.SharpArchContrib.Castle.Logging
{
    public interface IExceptionHandlerTestClass
    {
        float ThrowBaseExceptionNoCatch();

        void ThrowException();

        void ThrowExceptionSilent();

        float ThrowExceptionSilentWithReturn();

        float ThrowExceptionSilentWithReturnWithLogAttribute();

        float ThrowNotImplementedExceptionCatch();
    }
}