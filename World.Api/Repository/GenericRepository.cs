using Microsoft.EntityFrameworkCore;
using System.Linq.Expressions;
using World.Api.Data;
using World.Api.Repository.IRepository;

namespace World.Api.Repository
{
    public class GenericRepository<T> : IGenericRepository<T> where T : class
    {
        private readonly ApplicationDbContext _dbContext;

        public GenericRepository(ApplicationDbContext dbContext)
        {
            _dbContext = dbContext;
        }
        public async Task Create(T entity)
        {
            await _dbContext.AddAsync(entity);
            await Save();
        }

        public async Task Delete(T entity)
        {
            _dbContext.Remove(entity);
            await Save();
        }

        public async Task<List<T>> GetAll()
        {
            return await _dbContext.Set<T>().ToListAsync();
        }

        public async Task<T> GetById(int id)
        {
            return await _dbContext.Set<T>().FindAsync(id);
        }

        public bool IsRecordExsits(Expression<Func<T, bool>> condition)
        {
           var result = _dbContext.Set<T>().AsQueryable().Where(condition).Any();
            return result;
        }

        public async Task Save()
        {
           await _dbContext.SaveChangesAsync();
        }
    }
}
