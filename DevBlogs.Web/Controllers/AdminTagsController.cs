using DevBlogs.Web.Models.Domain;
using DevBlogs.Web.Models.ViewModels;
using DevBlogs.Web.Repository.TagRepository;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace DevBlogs.Web.Controllers
{
    [Authorize(Roles = "Admin")]
    public class AdminTagsController : Controller
    {
        private readonly ITagRepository _tagRepository;
        public AdminTagsController(ITagRepository tagRepository)
        {
            _tagRepository = tagRepository;
        }

        [HttpGet]
        public IActionResult Add()
        {
            return View();
        }

        [HttpPost]
        [ActionName("Add")]
        public async Task<IActionResult> AddAsync(AddTagRequest addTagRequest)
        {
            ValidateTagNames(addTagRequest);
            if (ModelState.IsValid == false)
            {
                return View();
            }
            Tag tag = new Tag
            {
                Name = addTagRequest.Name,
                DisplayName = addTagRequest.DisplayName,
            };
            await _tagRepository.AddAsync(tag);
            return RedirectToAction("List");
        }


        [HttpGet]
        [ActionName("List")]
        public async Task<IActionResult> ListAsync(string? searchQuery)
        {
            ViewBag.SearchQuery = searchQuery;

            var tags = await _tagRepository.GetAllAsync(searchQuery);
            return View(tags);
        }

        [HttpGet]
        [ActionName("Edit")]
        public async Task<IActionResult> EditAsync(Guid Id)
        {
            var existingTag = await _tagRepository.GetAsync(Id);

            if (existingTag != null)
            {
                EditTagRequest editTagRequest = new EditTagRequest
                {
                    Id = existingTag.Id,
                    Name = existingTag.Name,
                    DisplayName = existingTag.DisplayName,
                };
                return View(editTagRequest);
            }
            return View(null);
        }


        [HttpPost]
        [ActionName("Edit")]
        public async Task<IActionResult> EditAsync(EditTagRequest editTagRequest)
        {
            var tag = new Tag()
            {
                Id = editTagRequest.Id,
                Name = editTagRequest.Name,
                DisplayName = editTagRequest.DisplayName
            };
            await _tagRepository.UpdateAsync(tag);
            return RedirectToAction("Edit", new { id = tag.Id });
        }

        [HttpPost]
        [ActionName("Delete")]
        public async Task<IActionResult> Delete(Guid Id)
        {
            var tag = await _tagRepository.DeleteAsync(Id);
            if (tag != null)
            {
                return RedirectToAction("List");
            }
            return RedirectToAction("Edit", new { id = Id });
        }

        public void ValidateTagNames(AddTagRequest addTagRequest)
        {
            if (addTagRequest.Name is not null && addTagRequest.DisplayName is not null)
            {
                if (addTagRequest.Name == addTagRequest.DisplayName)
                {
                    ModelState.AddModelError("DisplayName", "Tag DisplayName can not be same as the Tag Name!");
                }
            }
        }
    }
}
