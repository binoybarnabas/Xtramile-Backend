using XtramileBackend.Data;

namespace XtramileBackend.Repositories
{
    public class Repository<T>: IRepository<T> where T : class
    {
        private readonly AppDBContext _dbContext;
        public Repository(AppDBContext dBContext) { 
        _dbContext = dBContext;
        }

        //Adding an entity
        public void Add(T entity) { 
              _dbContext.Set<T>().Add(entity);
        }

        //Getting all the records
        public IEnumerable<T> GetAll() { 
        return _dbContext.Set<T>().ToList();
        }

        //Get an info by id
        public T GetById(int id) { 
        return _dbContext.Set<T>().Find(id);
        }

        public void Update(T entity) {
            _dbContext.Set<T>().Update(entity);
        }

        public void Delete(T entity) { 
            _dbContext.Set<T>().Remove(entity);
        }
    }
}
