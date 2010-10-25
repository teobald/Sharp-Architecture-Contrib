namespace SharpArchContrib.Core.Logging
{
    using System;

    public interface IExceptionLogger
    {
        void LogException(Exception err, bool isSilent, Type throwingType);
    }
}