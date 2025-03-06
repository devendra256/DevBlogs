using DevBlogs.Web.Models.ViewModels;
using DevBlogs.Web.Repository.UserRepository;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;

namespace DevBlogs.Web.Controllers
{
    [Authorize(Roles = "Admin")]
    public class UsersController : Controller
    {
        private readonly IUserRepository _userRepository;
        private readonly UserManager<IdentityUser> _userManager;

        public UsersController(IUserRepository userRepository, UserManager<IdentityUser> userManager)
        {
            _userRepository = userRepository;
            _userManager = userManager;
        }

        [HttpGet]
        public async Task<IActionResult> List()
        {
            var allUsers = await _userRepository.GetAllAsync();

            var userViewModel = new UsersViewModel();
            userViewModel.Users = new List<User>();
            foreach (var user in allUsers)
            {
                userViewModel.Users.Add(new User()
                {
                    Id = Guid.Parse(user.Id),
                    Email = user.Email,
                    UserName = user.UserName
                });
            }
            return View(userViewModel);
        }

        [HttpPost]
        public async Task<IActionResult> List(UsersViewModel usersViewModel)
        {
            var identityUser = new IdentityUser
            {
                UserName = usersViewModel.UserName,
                Email = usersViewModel.Email,
            };

            var identityResult = await _userManager.CreateAsync(identityUser, usersViewModel.Password);
            if (identityResult != null && identityResult.Succeeded)
            {
                var roles = new List<string> { "User" };
                if (usersViewModel.AdminRoleCheckbox)
                {
                    roles.Add("Admin");
                }

                identityResult = await _userManager.AddToRolesAsync(identityUser, roles);
                if (identityResult != null && identityResult.Succeeded)
                {
                    return RedirectToAction("List", "Users");
                }
            }
            return View(usersViewModel);
        }

        [HttpPost]
        public async Task<IActionResult> Delete(Guid id)
        {
            var identityUser = await _userManager.FindByIdAsync(id.ToString());
            if (identityUser != null)
            {
                // delete user
                var result = await _userManager.DeleteAsync(identityUser);
                if (result != null && result.Succeeded)
                {
                    return RedirectToAction("List", "Users");
                }
            }
            return RedirectToAction("Add", "AdminTags");
        }
    }
}
