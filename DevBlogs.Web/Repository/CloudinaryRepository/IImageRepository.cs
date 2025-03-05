namespace DevBlogs.Web.Repository.CloudinaryRepository
{
    public interface IImageRepository
    {
        Task<string?> UploadImage(IFormFile file);
    }
}
