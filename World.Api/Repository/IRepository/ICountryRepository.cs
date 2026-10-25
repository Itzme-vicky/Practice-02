using World.Api.Models;

namespace World.Api.Repository.IRepository
{
    public interface ICountryRepository
    {
        Task<List<Country>> GetAll();
        Task<Country> GetById(int id);

        Task Create(Country entity);
        Task Update(Country entity);
        Task Delete(Country entity);
        Task Save();

        bool IsCountryExists(string name);
    }
}
