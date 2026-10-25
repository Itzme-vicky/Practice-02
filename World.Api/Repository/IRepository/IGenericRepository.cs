using System.Linq.Expressions;

namespace World.Api.Repository.IRepository
{
    public interface IGenericRepository <T> where T : class
    {
        Task<List<T>> GetAll();
        Task<T> GetById(int id);
        Task Create(T entity);
        Task Delete(T entity);
        Task Save();

        bool IsRecordExsits(Expression <Func<T,bool>> condition);
    }
}
