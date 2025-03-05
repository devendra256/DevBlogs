
using DevBlogs.Web.Data;
using DevBlogs.Web.Models.Domain;
using Microsoft.EntityFrameworkCore;

namespace DevBlogs.Web.Repository.BlogPostLikeRepository
{
    public class BlogPostLikeRepository : IBlogPostLikeRepository
    {
        private readonly DevBlogsDbContext _devBlogsDbContext;

        public BlogPostLikeRepository(DevBlogsDbContext devBlogsDbContext)
        {
            _devBlogsDbContext = devBlogsDbContext;
        }

        public async Task<BlogPostLike> AddLikeToBlogPost(BlogPostLike blogPostLike)
        {
            await _devBlogsDbContext.Likes.AddAsync(blogPostLike);
            await _devBlogsDbContext.SaveChangesAsync();
            return blogPostLike;
        }

        public async Task<int> GetTotalLikes(Guid blogPostId)
        {
            return await _devBlogsDbContext.Likes.CountAsync(x => x.BlogPostId == blogPostId);
        }

        public async Task<IEnumerable<BlogPostLike>> GetLikesForBlogPost(Guid blogPostId)
        {
            return await _devBlogsDbContext.Likes.Where(x => x.BlogPostId == blogPostId).ToListAsync();
        }
    }
}
