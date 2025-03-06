using DevBlogs.Web.Models.Domain;

namespace DevBlogs.Web.Repository.TagRepository
{
    public interface ITagRepository
    {
        Task<IEnumerable<Tag>> GetAllAsync(string? searchTerm = null,
            string? sortBy = null, 
            string? sortDirection = null);
        Task<Tag?> GetAsync(Guid id);
        Task<Tag> AddAsync(Tag tag);
        Task<Tag?> UpdateAsync(Tag tag);
        Task<Tag?> DeleteAsync(Guid id);

    }
}
