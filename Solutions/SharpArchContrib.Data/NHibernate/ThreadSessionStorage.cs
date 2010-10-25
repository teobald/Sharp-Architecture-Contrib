namespace SharpArchContrib.Data.NHibernate
{
    using System;
    using System.Collections.Generic;
    using System.Threading;

    using global::NHibernate;

    using SharpArch.Data.NHibernate;

    public class ThreadSessionStorage : IUnitOfWorkSessionStorage
    {
        private readonly ThreadSafeDictionary<string, SimpleSessionStorage> perThreadSessionStorage =
            new ThreadSafeDictionary<string, SimpleSessionStorage>();

        public IEnumerable<ISession> GetAllSessions()
        {
            return this.GetSimpleSessionStorageForThread().GetAllSessions();
        }

        public ISession GetSessionForKey(string factoryKey)
        {
            return this.GetSimpleSessionStorageForThread().GetSessionForKey(factoryKey);
        }

        public void SetSessionForKey(string factoryKey, ISession session)
        {
            this.GetSimpleSessionStorageForThread().SetSessionForKey(factoryKey, session);
        }

        public void EndUnitOfWork(bool closeSessions)
        {
            if (closeSessions)
            {
                NHibernateSession.CloseAllSessions();
                this.perThreadSessionStorage.Remove(this.GetCurrentThreadName());
            }
            else
            {
                foreach (var session in this.GetAllSessions())
                {
                    session.Clear();
                }
            }
        }

        private string GetCurrentThreadName()
        {
            if (Thread.CurrentThread.Name == null)
            {
                Thread.CurrentThread.Name = Guid.NewGuid().ToString();
            }

            return Thread.CurrentThread.Name;
        }

        private SimpleSessionStorage GetSimpleSessionStorageForThread()
        {
            var currentThreadName = this.GetCurrentThreadName();
            SimpleSessionStorage sessionStorage;
            if (!this.perThreadSessionStorage.TryGetValue(currentThreadName, out sessionStorage))
            {
                sessionStorage = new SimpleSessionStorage();
                this.perThreadSessionStorage.Add(currentThreadName, sessionStorage);
            }

            return sessionStorage;
        }
    }
}