using NHibernate;
using System.Linq;

namespace MarketHouse.Test.Core
{
    public abstract class IDbSessionManager
    {
        public abstract IQueryable<T> Query<T>();

        public abstract void Persist(object obj);

        public abstract void SaveOrUpdate(object obj);

        public abstract object Save(object obj);

        public abstract void Update(object obj);

        public abstract void Delete(object obj);
    }
}
