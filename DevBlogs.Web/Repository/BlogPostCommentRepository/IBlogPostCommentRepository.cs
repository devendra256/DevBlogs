using DevBlogs.Web.Models.Domain;

namespace DevBlogs.Web.Repository.BlogPostCommentRepository
{
    public interface IBlogPostCommentRepository
    {
        Task<BlogPostComment> AddCommentAsync(BlogPostComment comment);

        Task<IEnumerable<BlogPostComment>> GetCommentsByIdAsync(Guid blogPostId);
    }
}
