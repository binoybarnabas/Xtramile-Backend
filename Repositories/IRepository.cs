using System.Collections.Generic;

namespace XtramileBackend.Repositories
{
    public interface IRepository<T>
    {
        public void Add(T entity);
        public IEnumerable<T> GetAll();
        public T GetById(int id);
        public void Update(T entity);
        public void Delete(T entity);
    }
}
