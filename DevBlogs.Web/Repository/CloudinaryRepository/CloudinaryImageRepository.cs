
using CloudinaryDotNet;
using CloudinaryDotNet.Actions;
using Microsoft.AspNetCore.Http.HttpResults;

namespace DevBlogs.Web.Repository.CloudinaryRepository
{
    public class CloudinaryImageRepository : IImageRepository
    {
        private readonly IConfiguration _configuration;

        private readonly Account _account;

        public CloudinaryImageRepository(IConfiguration configuration)
        {
            _configuration = configuration;
            _account = new Account(
                _configuration.GetSection("Cloudinary")["CloudName"],
                _configuration.GetSection("Cloudinary")["APIKey"],
                _configuration.GetSection("Cloudinary")["APISecret"]
                );
        }

        public async Task<string?> UploadImage(IFormFile file)
        {
            Cloudinary cloudinaryClient = new Cloudinary(_account);

            ImageUploadParams imageUploadParams = new ImageUploadParams()
            {
                File = new FileDescription(file.FileName, file.OpenReadStream()),
                DisplayName = file.FileName
            };

            var uploadResult = await cloudinaryClient.UploadAsync(imageUploadParams);

            if(uploadResult != null && uploadResult.StatusCode == System.Net.HttpStatusCode.OK)
            {
                return uploadResult.SecureUrl.ToString();
            }
            return null;
        }
    }
}
