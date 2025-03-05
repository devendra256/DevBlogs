using System.Net;
using DevBlogs.Web.Repository.CloudinaryRepository;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace DevBlogs.Web.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ImageController : ControllerBase
    {
        private readonly IImageRepository _imageRepository;
        public ImageController(IImageRepository imageRepository)
        {
            _imageRepository = imageRepository;
        }

        [HttpPost]
        public async Task<IActionResult> UploadImage(IFormFile formFile)
        {
            var imageUrl = await _imageRepository.UploadImage(formFile);
            if (imageUrl != null)
            {
                return new JsonResult(new { imageUrl = imageUrl });
            }
            return Problem("Something went wrong", null, (int) HttpStatusCode.InternalServerError);
        }
    }
}
