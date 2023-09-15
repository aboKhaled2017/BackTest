using BackTest.Configurations;
using BackTest.DataModels;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;

namespace BackTest
{
    /// <summary>
    /// class which is used for managing the data at the start of the application
    /// </summary>
    public sealed class DataSeederManager:IDisposable
    {
        private readonly AppDbContext _db;
        private readonly IServiceProvider _sp;
        private readonly SeederSettings _seederSettings;
        private static readonly string[] Names =
        {
            "John", "Alice", "Robert", "Emily", "Michael", "Sophia", "David", "Olivia", "William", "Emma"
        };

        public DataSeederManager(IServiceProvider sp, IOptions<SeederSettings> seedingOptions)
        {
            _sp = sp;
            _db = _sp.CreateScope().ServiceProvider.GetRequiredService<AppDbContext>();
            _seederSettings =seedingOptions.Value;
        }
        public async Task CleanData()
        {

            if (!_seederSettings.EnableSeeding) //data cleaning is disabled from app serrings
                return;

            var drivers = await _db.Drivers.ToListAsync();

            _db.RemoveRange(drivers);

            await _db.SaveChangesAsync();
        }
        public async Task Seed10RandomDrivers()
        {
            if (!_seederSettings.EnableSeeding) //seeding is disabled from app serrings
                return;
            var x = _db.Drivers.Count();
            var randomNum = new Random(1);

            List<Driver> drivers = new();

            foreach (var name in Names)
            {
                drivers.Add(Driver.Create(name, Names[randomNum.Next(0,9)],$"{name}@test.com",$"20115250643{randomNum.Next(0, 9)}"));
            }

            _db.AddRange(drivers);

            await _db.SaveChangesAsync();
        }

        public void Dispose()
        {
            _db.Dispose();
        }
    }
}
