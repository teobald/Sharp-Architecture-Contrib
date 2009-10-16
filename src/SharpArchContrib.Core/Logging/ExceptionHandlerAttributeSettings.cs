using System;

namespace SharpArchContrib.Core.Logging {
    [Serializable]
    public class ExceptionHandlerAttributeSettings {
        public ExceptionHandlerAttributeSettings() {
            IsSilent = false;
            ReturnValue = null;
        }

        public bool IsSilent { get; set; }
        public object ReturnValue { get; set; }
    }
}