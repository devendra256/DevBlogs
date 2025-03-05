using DevBlogs.Web.Data;
using DevBlogs.Web.Models.Domain;
using Microsoft.EntityFrameworkCore;

namespace DevBlogs.Web.Repository.BlogPostCommentRepository
{
    public class BlogPostCommentRepository : IBlogPostCommentRepository
    {
        private readonly DevBlogsDbContext _devBlogsDbContext;

        public BlogPostCommentRepository(DevBlogsDbContext devBlogsDbContext)
        {
            _devBlogsDbContext = devBlogsDbContext;
        }

        public async Task<BlogPostComment> AddCommentAsync(BlogPostComment comment)
        {
            await _devBlogsDbContext.BlogPostComments.AddAsync(comment);
            await _devBlogsDbContext.SaveChangesAsync();
            return comment;
        }

        public async Task<IEnumerable<BlogPostComment>> GetCommentsByIdAsync(Guid blogPostId)
        {
            return await _devBlogsDbContext.BlogPostComments.Where(x => x.BlogPostId == blogPostId).ToListAsync();
        }
    }
}
