using DevBlogs.Web.Models.Domain;
using Microsoft.AspNetCore.Identity;

namespace DevBlogs.Web.Repository.UserRepository
{
    public interface IUserRepository
    {
        Task<IEnumerable<IdentityUser>> GetAllAsync();
    }
}
