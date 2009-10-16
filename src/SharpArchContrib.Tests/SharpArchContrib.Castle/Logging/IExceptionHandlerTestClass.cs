namespace Tests.SharpArchContrib.Castle.Logging {
    public interface IExceptionHandlerTestClass {
        void ThrowException();
        float ThrowExceptionSilentWithReturn();
        void ThrowExceptionSilent();
        float ThrowExceptionSilentWithReturnWithLogAttribute();
    }
}