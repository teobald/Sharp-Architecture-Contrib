namespace SharpArchContrib.Data.NHibernate
{
    using System;

    using SharpArch.NHibernate;

    [CLSCompliant(false)]
    public interface IUnitOfWorkSessionStorage : ISessionStorage
    {
        void EndUnitOfWork(bool closeSessions);
    }
}