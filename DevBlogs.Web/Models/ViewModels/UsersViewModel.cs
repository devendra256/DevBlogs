namespace DevBlogs.Web.Models.ViewModels
{
    public class UsersViewModel
    {
        public ICollection<User> Users { get; set; }

        public string UserName { get; set; }
        public string Email { get; set; }
        public string Password { get; set; }

        public bool AdminRoleCheckbox { get; set; }
    }
}
