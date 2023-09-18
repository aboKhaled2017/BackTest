using Backend.DataModels;
using Microsoft.EntityFrameworkCore;

namespace Backend.Repos
{
    public sealed class NamesProcessingRepo : INamesProcessingRepo
    {
        private readonly AppDbContext _db;

        public NamesProcessingRepo(AppDbContext db)
        {
            _db = db;
        }

        public async Task Insert10NamesAsync(List<string> names)
        {
            var entities = names.Select(n => NameIdentifier.Create(n));
            
            _db.AddRange(entities);

            await _db.SaveChangesAsync();
        }

        public async Task<List<NameIdentifier>> GetAllNamesSortedAsync()
        {
            return await _db.Names
                .OrderBy(x => x.Value)
                .ToListAsync();
        }

        public async Task<NameIdentifier> GetNameSpellingAlphapetized(int index)
        {
            return await _db.Names.FindAsync(index);
        }
    }
}
