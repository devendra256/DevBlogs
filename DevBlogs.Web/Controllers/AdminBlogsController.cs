using DevBlogs.Web.Models.Domain;
using DevBlogs.Web.Models.ViewModels;
using DevBlogs.Web.Repository.BlogPostsRepository;
using DevBlogs.Web.Repository.TagRepository;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace DevBlogs.Web.Controllers
{
    [Authorize(Roles = "Admin")]
    public class AdminBlogsController : Controller
    {
        private readonly ITagRepository _tagRepository;
        private readonly IBlogPostRepository _blogPostRepository;
        public AdminBlogsController(ITagRepository tagRepository, IBlogPostRepository blogPostRepository)
        {
            _tagRepository = tagRepository;
            _blogPostRepository = blogPostRepository;
        }

        [HttpGet]
        public async Task<IActionResult> Add()
        {
            var tags = await _tagRepository.GetAllAsync();

            var model = new AddBlogPostRequest
            {
                Tags = tags.Select(x => new SelectListItem { Text = x.Name, Value = x.Id.ToString() })
            };
            return View(model);
        }

        [HttpPost]
        public async Task<IActionResult> Add(AddBlogPostRequest addBlogPostRequest)
        {
            var blogPost = new BlogPost
            {
                Author = addBlogPostRequest.Author,
                Content = addBlogPostRequest.Content,
                FeaturedImageUrl = addBlogPostRequest.FeaturedImageUrl,
                Heading = addBlogPostRequest.Heading,
                PageTitle = addBlogPostRequest.PageTitle,
                PublishedDate = addBlogPostRequest.PublishedDate,
                ShortDescription = addBlogPostRequest.ShortDescription,
                UrlHandle = addBlogPostRequest.UrlHandle,
                Visible = addBlogPostRequest.Visible
            };

            var selectedTagsList = new List<Tag>();
            foreach (var selectedTagId in addBlogPostRequest.SelectedTags)
            {
                var existingTag = await _tagRepository.GetAsync(Guid.Parse(selectedTagId));
                if (existingTag != null)
                {
                    selectedTagsList.Add(existingTag);
                }
            }
            blogPost.Tags = selectedTagsList;
            await _blogPostRepository.AddAsync(blogPost);

            return RedirectToAction("Add");
        }

        [HttpGet]
        public async Task<IActionResult> List()
        {
            return View(await _blogPostRepository.GetAllAsync());
        }

        [HttpGet]
        public async Task<IActionResult> Edit(Guid id)
        {
            var existingBlogPost = await _blogPostRepository.GetAsync(id);
            var tagModelDomain = await _tagRepository.GetAllAsync();

            if (existingBlogPost != null)
            {
                var model = new EditBlogPostRequest
                {
                    Id = existingBlogPost.Id,
                    Author = existingBlogPost.Author,
                    Content = existingBlogPost.Content,
                    FeaturedImageUrl = existingBlogPost.FeaturedImageUrl,
                    Heading = existingBlogPost.Heading,
                    PageTitle = existingBlogPost.PageTitle,
                    PublishedDate = existingBlogPost.PublishedDate,
                    ShortDescription = existingBlogPost.ShortDescription,
                    UrlHandle = existingBlogPost.UrlHandle,
                    Visible = existingBlogPost.Visible,
                    Tags = tagModelDomain.Select(x => new SelectListItem { Text = x.Name, Value = x.Id.ToString() }),
                    SelectedTags = tagModelDomain.Select(x => x.Id.ToString()).ToArray()
                };
                return View(model);
            }
            return View(null);
        }

        [HttpPost]
        public async Task<IActionResult> Edit(EditBlogPostRequest editBlogPostRequest)
        {
            var blogPost = new BlogPost
            {
                Id = editBlogPostRequest.Id,
                Author = editBlogPostRequest.Author,
                Content = editBlogPostRequest.Content,
                FeaturedImageUrl = editBlogPostRequest.FeaturedImageUrl,
                Heading = editBlogPostRequest.Heading,
                PageTitle = editBlogPostRequest.PageTitle,
                PublishedDate = editBlogPostRequest.PublishedDate,
                ShortDescription = editBlogPostRequest.ShortDescription,
                UrlHandle = editBlogPostRequest.UrlHandle,
                Visible = editBlogPostRequest.Visible
            };

            var selectedTagList = new List<Tag>();
            foreach (var selectedTag in editBlogPostRequest.SelectedTags)
            {
                if (Guid.TryParse(selectedTag, out var tag))
                {
                    var foundTag = await _tagRepository.GetAsync(tag);
                    if (foundTag != null)
                    {
                        selectedTagList.Add(foundTag);
                    }
                }
            }
            blogPost.Tags = selectedTagList;
            var result = await _blogPostRepository.UpdateBlogPost(blogPost);

            if (result != null)
            {
                // success
                return RedirectToAction("List");
            }
            // failure
            return View(null);
        }

        [HttpPost]
        public async Task<IActionResult> Delete(EditBlogPostRequest editBlogPostRequest)
        {
            var deletedTag = await _blogPostRepository.DeleteBlogPost(editBlogPostRequest.Id);
            if (deletedTag != null)
            {
                // success
                return RedirectToAction("List");
            }
            // failure
            return RedirectToAction("Edit", new {id = editBlogPostRequest.Id});
        }
    }
}
