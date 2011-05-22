namespace SharpArchContrib.Data.NHibernate
{
    using SharpArch.NHibernate;

    public interface IUnitOfWorkSessionStorage : ISessionStorage
    {
        void EndUnitOfWork(bool closeSessions);
    }
}