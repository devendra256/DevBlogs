using DevBlogs.Web.Models.Domain;
using DevBlogs.Web.Models.ViewModels;
using Microsoft.AspNetCore.Mvc;

namespace DevBlogs.Web.Repository.BlogPostsRepository
{
    public interface IBlogPostRepository
    {
        Task<IEnumerable<BlogPost>> GetAllAsync();
        Task<BlogPost> AddAsync(BlogPost blogPost);
        Task<BlogPost?> GetAsync(Guid id);
        Task<BlogPost?> GetByUrlHandle(string urlHandle);
        Task<BlogPost?> UpdateBlogPost(BlogPost blogPost);
        Task<BlogPost?> DeleteBlogPost(Guid id);
    }
}
