using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using XtramileBackend.Data;

namespace XtramileBackend.Repositories
{
    public class Repository<T> : IRepository<T> where T : class
    {
        private readonly AppDBContext _dbContext;

        public Repository(AppDBContext dbContext)
        {
            _dbContext = dbContext ?? throw new ArgumentNullException(nameof(dbContext));
        }

        // Adding an entity asynchronously
        public async Task AddAsync(T entity)
        {
            try
            {
                await _dbContext.Set<T>().AddAsync(entity);
            }
            catch (Exception ex)
            {
                // Handle or log the exception
                Console.WriteLine($"An error occurred while adding an entity: {ex.Message}");
                throw; // Re-throw the exception to propagate it
            }
        }

        // Getting all the records asynchronously
        public async Task<IEnumerable<T>> GetAllAsync()
        {
            try
            {
                return await _dbContext.Set<T>().ToListAsync();
            }
            catch (Exception ex)
            {
                // Handle or log the exception
                Console.WriteLine($"An error occurred while getting all entities: {ex.Message}");
                throw; // Re-throw the exception to propagate it
            }
        }

        // Get an info by id asynchronously
        public async Task<T> GetByIdAsync(int id)
        {
            try
            {
                return await _dbContext.Set<T>().FindAsync(id);
            }
            catch (Exception ex)
            {
                // Handle or log the exception
                Console.WriteLine($"An error occurred while getting an entity by id: {ex.Message}");
                throw; // Re-throw the exception to propagate it
            }
        }

        public void Update(T entity)
        {
            _dbContext.Set<T>().Update(entity);
        }

        public void Delete(T entity)
        {
            _dbContext.Set<T>().Remove(entity);
        }
    }
}
