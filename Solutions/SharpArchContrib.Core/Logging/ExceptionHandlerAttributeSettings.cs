namespace SharpArchContrib.Core.Logging
{
    using System;

    [Serializable]
    public class ExceptionHandlerAttributeSettings
    {
        public ExceptionHandlerAttributeSettings()
        {
            this.IsSilent = false;
            this.ReturnValue = null;
        }

        public Type ExceptionType { get; set; }

        public bool IsSilent { get; set; }

        public object ReturnValue { get; set; }
    }
}