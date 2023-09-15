using Backend.DataModels;
using Microsoft.EntityFrameworkCore;

namespace Backend.Repos
{
    public sealed class DriverRepo: IDriverRepo
    {
        private readonly AppDbContext _db;

        public DriverRepo(AppDbContext db)
        {
            _db = db;
        }

        public async Task<Driver> GetDriverByIdAsync(int id)
        {
            return await _db.Drivers.FirstOrDefaultAsync(x=>x.Id==id);
        }

        public async Task<List<Driver>> GetAllDriversAsync()
        {
            return await _db.Drivers.ToListAsync();
        }

        public async Task CreateDriverAsync(Driver driver)
        {
            _db.Drivers.Add(driver);

            await _db.SaveChangesAsync();
        }

        public async Task UpdateDriverAsync(Driver driver)
        {
            _db.Update(driver);

            await _db.SaveChangesAsync();
        }

        public async Task DeleteDriverAsync(Driver driver)
        {
            _db.Remove(driver);

            await _db.SaveChangesAsync();
        }
    }
}
