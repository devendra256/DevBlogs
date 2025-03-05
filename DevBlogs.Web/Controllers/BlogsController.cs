using DevBlogs.Web.Models.ViewModels;
using DevBlogs.Web.Repository.BlogPostLikeRepository;
using DevBlogs.Web.Repository.BlogPostsRepository;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;

namespace DevBlogs.Web.Controllers
{
    public class BlogsController : Controller
    {
        private readonly IBlogPostRepository _blogPostRepository;
        private readonly SignInManager<IdentityUser> _signInManager;
        private readonly UserManager<IdentityUser> _userManager;
        private readonly IBlogPostLikeRepository _blogPostLikeRepository;

        public BlogsController(IBlogPostRepository blogPostRepository, IBlogPostLikeRepository blogPostLikeRepository,
            SignInManager<IdentityUser> signInManager, UserManager<IdentityUser> userManager)
        {
            _blogPostRepository = blogPostRepository;
            _signInManager = signInManager;
            _userManager = userManager;
            _blogPostLikeRepository = blogPostLikeRepository;
        }
        public async Task<IActionResult> Index(string urlHandle)
        {
            var isLiked = false;
            var blogPost = await _blogPostRepository.GetByUrlHandle(urlHandle);
            var blogPostViewModel = new BlogPostViewModel();

            // check if user is signed in
            if (_signInManager.IsSignedIn(User))
            {
                // fetch list of likes for current blog post
                var currentBlogLikesList = await _blogPostLikeRepository.GetLikesForBlogPost(blogPost.Id);

                // get current user & check if liked it or not
                var currentUserId = _userManager.GetUserId(User);
                if (currentUserId != null)
                {
                    var isLikedByCurrentUser = currentBlogLikesList.FirstOrDefault(x => x.UserId == Guid.Parse(currentUserId));
                    isLiked = isLikedByCurrentUser != null;
                }
            }

            if (blogPost != null)
            {
                blogPostViewModel = new BlogPostViewModel()
                {
                    Id = blogPost.Id,
                    Author = blogPost.Author,
                    Heading = blogPost.Heading,
                    Content = blogPost.Content,
                    FeaturedImageUrl = blogPost.FeaturedImageUrl,
                    PageTitle = blogPost.PageTitle,
                    PublishedDate = blogPost.PublishedDate,
                    ShortDescription = blogPost.ShortDescription,
                    UrlHandle = blogPost.UrlHandle,
                    Visible = blogPost.Visible,
                    Tags = blogPost.Tags,
                    IsLiked = isLiked
                };
                blogPostViewModel.TotalLikes = await _blogPostLikeRepository.GetTotalLikes(blogPost.Id);
                return View(blogPostViewModel);
            }
            return View(null);
        }
    }
}
