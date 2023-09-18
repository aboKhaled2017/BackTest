using Backend.DataModels;

namespace Backend.Repos
{
    public interface INamesProcessingRepo
    {
        Task<List<NameIdentifier>> GetAllNamesSortedAsync();
        Task<NameIdentifier> GetNameSpellingAlphapetized(int index);
        Task Insert10NamesAsync(List<string> names);
    }
}
