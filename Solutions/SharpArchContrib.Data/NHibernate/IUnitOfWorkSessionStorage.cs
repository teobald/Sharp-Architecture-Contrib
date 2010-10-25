namespace SharpArchContrib.Data.NHibernate
{
    using SharpArch.Data.NHibernate;

    public interface IUnitOfWorkSessionStorage : ISessionStorage
    {
        void EndUnitOfWork(bool closeSessions);
    }
}