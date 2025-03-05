using DevBlogs.Web.Models.Domain;
using Microsoft.AspNetCore.Mvc;

namespace DevBlogs.Web.Repository.BlogPostLikeRepository
{
    public interface IBlogPostLikeRepository
    {
        // return likes count
        Task<int> GetTotalLikes(Guid blogPostId);
        
        // add like to blogpost
        Task<BlogPostLike> AddLikeToBlogPost(BlogPostLike blogPostLike);

        // return list of likes for a specific post
        Task<IEnumerable<BlogPostLike>> GetLikesForBlogPost(Guid blogPostId);

    }
}
