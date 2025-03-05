using DevBlogs.Web.Models.Domain;
using DevBlogs.Web.Models.ViewModels;
using DevBlogs.Web.Repository.BlogPostCommentRepository;
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
        private readonly IBlogPostCommentRepository _blogPostCommentRepository;
        private readonly IBlogPostLikeRepository _blogPostLikeRepository;

        public BlogsController(IBlogPostRepository blogPostRepository, IBlogPostLikeRepository blogPostLikeRepository,
            SignInManager<IdentityUser> signInManager,
            UserManager<IdentityUser> userManager,
            IBlogPostCommentRepository blogPostCommentRepository)
        {
            _blogPostRepository = blogPostRepository;
            _signInManager = signInManager;
            _userManager = userManager;
            _blogPostCommentRepository = blogPostCommentRepository;
            _blogPostLikeRepository = blogPostLikeRepository;
        }

        [HttpGet]
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

            // get all comments for specific post
            var blogCommentsDomainModel = await _blogPostCommentRepository.GetCommentsByIdAsync(blogPost.Id);
            var blogCommentsForView = new List<BlogComment>();
            foreach (var blogComment in blogCommentsDomainModel)
            {
                blogCommentsForView.Add(new BlogComment
                {
                    DateAdded = blogComment.DateAdded,
                    Description = blogComment.Description,
                    Username = (await _userManager.FindByIdAsync(blogComment.UserId.ToString())).UserName
                });
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
                    IsLiked = isLiked,
                    Comments = blogCommentsForView
                };
                blogPostViewModel.TotalLikes = await _blogPostLikeRepository.GetTotalLikes(blogPost.Id);
                return View(blogPostViewModel);
            }
            return View(null);
        }

        [HttpPost]
        public async Task<IActionResult> Index(BlogPostViewModel blogPostViewModel)
        {
            if (_signInManager.IsSignedIn(User))
            {
                var commentModel = new BlogPostComment()
                {
                    BlogPostId = blogPostViewModel.Id,
                    Description = blogPostViewModel.CommentDescription,
                    UserId = Guid.Parse(_userManager.GetUserId(User)),
                    DateAdded = DateTime.Now
                };
                await _blogPostCommentRepository.AddCommentAsync(commentModel);
                return RedirectToAction("Index", "Blogs", new { urlHandle = blogPostViewModel.UrlHandle });
            }
            return View();
        }
    }
}
