using BackTest.DataModels;

namespace BackTest.Services
{
    public interface IDriverService
    {
        Task CreateDriverAsync(Driver driver);
        Task DeleteDriverAsync(Driver driver);
        Task<List<Driver>> GetAllDriversAsync();
        Task<Driver> GetDriverByIdAsync(int id);
        Task UpdateDriverAsync(Driver driver);
    }
}
