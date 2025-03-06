using DevBlogs.Web.Data;
using DevBlogs.Web.Models.Domain;
using Microsoft.EntityFrameworkCore;

namespace DevBlogs.Web.Repository.TagRepository
{
    public class TagRepository : ITagRepository
    {
        private readonly DevBlogsDbContext _devBlogsDbContext;

        public TagRepository(DevBlogsDbContext devBlogsDbContext)
        {
            _devBlogsDbContext = devBlogsDbContext;
        }

        public async Task<Tag> AddAsync(Tag tag)
        {
            await _devBlogsDbContext.Tags.AddAsync(tag);
            await _devBlogsDbContext.SaveChangesAsync();
            return tag;
        }

        public async Task<Tag?> DeleteAsync(Guid id)
        {
            var deletedTag = await _devBlogsDbContext.Tags.FindAsync(id);
            if (deletedTag != null)
            {
                _devBlogsDbContext.Tags.Remove(deletedTag);
                await _devBlogsDbContext.SaveChangesAsync();
                return deletedTag;
            }
            return null;
        }

        public async Task<IEnumerable<Tag>> GetAllAsync(string? searchTerm = null,
            string? sortBy = null, string? sortDirection = null)
        {
            var query = _devBlogsDbContext.Tags.AsQueryable();

            // filtering
            if (!string.IsNullOrWhiteSpace(searchTerm))
            {
                query = query.Where(x => x.Name.Contains(searchTerm) || x.DisplayName.Contains(searchTerm));
            }

            // sorting
            if (!string.IsNullOrWhiteSpace(sortBy))
            {
                var isDesc = string.Equals(sortDirection, "Desc", StringComparison.OrdinalIgnoreCase);

                if (string.Equals(sortBy, "Name", StringComparison.OrdinalIgnoreCase))
                {
                    query = isDesc ? query.OrderByDescending(x => x.Name) : query.OrderBy(x => x.Name);
                }
                if (string.Equals(sortBy, "DisplayName", StringComparison.OrdinalIgnoreCase))
                {
                    query = isDesc ? query.OrderByDescending(x => x.DisplayName) : query.OrderBy(x => x.Name);
                }
            }

            return await query.ToListAsync();
        }

        public async Task<Tag?> GetAsync(Guid id)
        {
            return await _devBlogsDbContext.Tags.FindAsync(id);
        }

        public async Task<Tag?> UpdateAsync(Tag tag)
        {
            var existingTag = await _devBlogsDbContext.Tags.FindAsync(tag.Id);
            if (existingTag != null)
            {
                existingTag.Name = tag.Name;
                existingTag.DisplayName = tag.DisplayName;
                await _devBlogsDbContext.SaveChangesAsync();

                return existingTag;
            }
            return null;
        }
    }
}
