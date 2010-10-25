namespace SharpArchContrib.Data.NHibernate
{
    using System;

    [Serializable]
    public class UnitOfWorkAttributeSettings : TransactionAttributeSettings
    {
        public UnitOfWorkAttributeSettings()
        {
            this.CloseSessions = true;
        }

        public bool CloseSessions { get; set; }
    }
}