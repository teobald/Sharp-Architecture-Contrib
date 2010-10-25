namespace SharpArchContrib.Castle.Logging
{
    using System;

    using SharpArchContrib.Core.Logging;

    [AttributeUsage(AttributeTargets.Assembly | AttributeTargets.Class | AttributeTargets.Method, AllowMultiple = false, 
        Inherited = false)]
    public class ExceptionHandlerAttribute : Attribute
    {
        public ExceptionHandlerAttribute()
        {
            this.Settings = new ExceptionHandlerAttributeSettings();
        }

        public Type ExceptionType
        {
            get
            {
                return this.Settings.ExceptionType;
            }

            set
            {
                this.Settings.ExceptionType = value;
            }
        }

        public bool IsSilent
        {
            get
            {
                return this.Settings.IsSilent;
            }

            set
            {
                this.Settings.IsSilent = value;
            }
        }

        public object ReturnValue
        {
            get
            {
                return this.Settings.ReturnValue;
            }

            set
            {
                this.Settings.ReturnValue = value;
            }
        }

        public ExceptionHandlerAttributeSettings Settings { get; set; }
    }
}