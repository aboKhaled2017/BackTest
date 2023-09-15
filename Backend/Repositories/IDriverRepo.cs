using Backend.DataModels;

namespace Backend.Repos
{
    public interface IDriverRepo
    {
        Task CreateDriverAsync(Driver driver);
        Task DeleteDriverAsync(Driver driver);
        Task<List<Driver>> GetAllDriversAsync();
        Task<Driver> GetDriverByIdAsync(int id);
        Task UpdateDriverAsync(Driver driver);
    }
}
