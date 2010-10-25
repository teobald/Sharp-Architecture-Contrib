namespace SharpArchContrib.Data.NHibernate
{
    using System;

    using SharpArch.Data.NHibernate;

    [Serializable]
    public class TransactionAttributeSettings
    {
        public TransactionAttributeSettings()
        {
            this.FactoryKey = NHibernateSession.DefaultFactoryKey;
            this.IsExceptionSilent = false;
            this.ReturnValue = null;
        }

        public string FactoryKey { get; set; }

        public bool IsExceptionSilent { get; set; }

        public object ReturnValue { get; set; }
    }
}