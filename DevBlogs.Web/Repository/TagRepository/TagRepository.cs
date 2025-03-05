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

        public async Task<IEnumerable<Tag>> GetAllAsync()
        {
            return await _devBlogsDbContext.Tags.ToListAsync();
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
