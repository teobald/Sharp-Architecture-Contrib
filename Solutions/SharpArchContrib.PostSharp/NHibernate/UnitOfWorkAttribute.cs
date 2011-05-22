namespace SharpArchContrib.PostSharp.NHibernate
{
    using System;

    using global::PostSharp.Aspects;
    using global::PostSharp.Extensibility;

    using SharpArch.NHibernate;

    using SharpArchContrib.Data.NHibernate;

    [Serializable]
    [AttributeUsage(AttributeTargets.Class | AttributeTargets.Method, AllowMultiple = false, Inherited = false)]
    [MulticastAttributeUsage(MulticastTargets.Method, AllowMultiple = true)]
    public sealed class UnitOfWorkAttribute : TransactionAttribute
    {
        #region Constructors and Destructors

        public UnitOfWorkAttribute()
        {
            this.attributeSettings = new UnitOfWorkAttributeSettings();
        }

        #endregion

        #region Properties

        public bool CloseSessions
        {
            get
            {
                return this.UnitOfWorkSettings.CloseSessions;
            }

            set
            {
                this.UnitOfWorkSettings.CloseSessions = value;
            }
        }

        public UnitOfWorkAttributeSettings UnitOfWorkSettings
        {
            get
            {
                return (UnitOfWorkAttributeSettings)this.attributeSettings;
            }

            set
            {
                this.Settings = value;
            }
        }

        #endregion

        #region Methods

        protected override object CloseUnitOfWork(object transactionState, bool commit)
        {
            base.CloseUnitOfWork(transactionState, commit);
            if (this.TransactionManager.TransactionDepth == 0)
            {
                var sessionStorage = NHibernateSession.Storage as IUnitOfWorkSessionStorage;
                if (sessionStorage != null)
                {
                    sessionStorage.EndUnitOfWork(this.CloseSessions);
                }
            }

            return transactionState;
        }

        #endregion
    }
}