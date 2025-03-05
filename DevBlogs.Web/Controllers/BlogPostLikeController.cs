using DevBlogs.Web.Models.Domain;
using DevBlogs.Web.Models.ViewModels;
using DevBlogs.Web.Repository.BlogPostLikeRepository;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace DevBlogs.Web.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class BlogPostLikeController : ControllerBase
    {
        private readonly IBlogPostLikeRepository _blogPostLikeRepository;

        public BlogPostLikeController(IBlogPostLikeRepository blogPostLikeRepository)
        {
            _blogPostLikeRepository = blogPostLikeRepository;
        }

        [HttpGet]
        [Route("{blogPostId:Guid}/totallikes")]
        public async Task<IActionResult> TotalLikes([FromRoute] Guid blogPostId)
        {
            var totalLikes = await _blogPostLikeRepository.GetTotalLikes(blogPostId);
            return Ok(totalLikes);
        }

        [HttpPost]
        // always mention route name explicitly
        [Route("Add")]
        public async Task<IActionResult> AddLike([FromBody] AddLikeRequest addLikeRequest)
        {
            var blogPostLikeModel = new BlogPostLike()
            {
                BlogPostId = addLikeRequest.BlogPostId,
                UserId = addLikeRequest.UserId,
            };
            await _blogPostLikeRepository.AddLikeToBlogPost(blogPostLikeModel);
            return Ok();
        }



    }
}
