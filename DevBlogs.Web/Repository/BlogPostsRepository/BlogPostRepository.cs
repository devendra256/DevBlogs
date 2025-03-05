using DevBlogs.Web.Data;
using DevBlogs.Web.Models.Domain;
using Microsoft.EntityFrameworkCore;

namespace DevBlogs.Web.Repository.BlogPostsRepository
{
    public class BlogPostRepository : IBlogPostRepository
    {
        private readonly DevBlogsDbContext _devBlogsDbContext;
        public BlogPostRepository(DevBlogsDbContext devBlogsDbContext)
        {
            _devBlogsDbContext = devBlogsDbContext;
        }
        public async Task<IEnumerable<BlogPost>> GetAllAsync()
        {
            return await _devBlogsDbContext.BlogPosts.Include(x => x.Tags).ToListAsync();
        }

        public async Task<BlogPost> AddAsync(BlogPost blogPost)
        {
            await _devBlogsDbContext.AddAsync(blogPost);
            await _devBlogsDbContext.SaveChangesAsync();
            return blogPost;
        }

        public async Task<BlogPost?> DeleteBlogPost(Guid id)
        {

            var deletedPost = await _devBlogsDbContext.BlogPosts.FindAsync(id);
            if (deletedPost != null)
            {
                _devBlogsDbContext.BlogPosts.Remove(deletedPost);
                await _devBlogsDbContext.SaveChangesAsync();
                return deletedPost; 
            }
            return null;
        }

        public async Task<BlogPost?> GetAsync(Guid id)
        {
            return await _devBlogsDbContext.BlogPosts.Include(x => x.Tags).FirstOrDefaultAsync(x => x.Id == id);
        }

        public async Task<BlogPost?> UpdateBlogPost(BlogPost blogPost)
        {

            var existingBlogPost = await _devBlogsDbContext.BlogPosts.Include(x => x.Tags).FirstOrDefaultAsync(x => x.Id == blogPost.Id);
            if (existingBlogPost != null)
            {
                existingBlogPost.Id = blogPost.Id;
                existingBlogPost.Heading = blogPost.Heading;
                existingBlogPost.PageTitle = blogPost.PageTitle;
                existingBlogPost.ShortDescription = blogPost.ShortDescription;
                existingBlogPost.Author = blogPost.Author;
                existingBlogPost.PublishedDate = blogPost.PublishedDate;
                existingBlogPost.Visible = blogPost.Visible;
                existingBlogPost.Content = blogPost.Content;
                existingBlogPost.FeaturedImageUrl = blogPost.FeaturedImageUrl;
                existingBlogPost.UrlHandle = blogPost.UrlHandle;
                existingBlogPost.Tags = blogPost.Tags;

                await _devBlogsDbContext.SaveChangesAsync();
                return existingBlogPost;
            }
            return null;
        }

        public async Task<BlogPost?> GetByUrlHandle(string urlHandle)
        {
            var blogPost = await _devBlogsDbContext.BlogPosts.Include("Tags").FirstOrDefaultAsync(x => x.UrlHandle == urlHandle);

            if (blogPost != null)
            {
                return blogPost;
            }
            return null;
        }
    }
}
