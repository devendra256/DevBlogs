using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace DevBlogs.Web.Data
{
    public class AuthDbContext : IdentityDbContext
    {
        public AuthDbContext(DbContextOptions<AuthDbContext> options) : base(options) { }

        protected override void OnModelCreating(ModelBuilder builder)
        {
            base.OnModelCreating(builder);

            // seed roles - user, admin & superadmin
            var userRoleId = "f6b77dc2-7a3b-484a-82b6-ac90ecb44fd1";
            var adminRoleId = "0fbb095b-a841-46ed-8262-09314c4ecc7c";
            var superAdminRoleId = "3c2a00ba-42d6-4aff-a81e-5e9dc95c6799";

            var roles = new List<IdentityRole>
            {
                new IdentityRole()
                {
                    Id = userRoleId,
                    Name = "User",
                    NormalizedName = "User",
                    ConcurrencyStamp = userRoleId
                },
                new IdentityRole
                {
                    Id = adminRoleId,
                    Name = "Admin",
                    NormalizedName = "Admin",
                    ConcurrencyStamp = adminRoleId
                },
                new IdentityRole
                {
                    Id = superAdminRoleId,
                    Name = "SuperAdmin",
                    NormalizedName = "SuperAdmin",
                    ConcurrencyStamp= superAdminRoleId
                }
            };

            builder.Entity<IdentityRole>().HasData(roles);

            // seed superadmin
            var superAdminUserId = "ef898818-485e-4741-8dda-43b0395c7439";
            var superAdminUser = new IdentityUser
            {
                Id = superAdminUserId,
                Email = "superadmin@devblogs.com",
                UserName = "superadmin@devblogs.com",
                NormalizedEmail = "superadmin@devblogs.com".ToUpper(),
                NormalizedUserName = "superadmin@devblogs.com".ToUpper(),
            };

            superAdminUser.PasswordHash = new PasswordHasher<IdentityUser>()
                .HashPassword(superAdminUser, "Superadmin@123");

            builder.Entity<IdentityUser>().HasData(superAdminUser);


            var superAdminRoles = new List<IdentityUserRole<string>>
            {
                new IdentityUserRole<string>
                {
                    RoleId = adminRoleId,
                    UserId = superAdminUserId
                },
                new IdentityUserRole<string>
                {
                    RoleId = superAdminRoleId,
                    UserId = superAdminUserId
                },
                new IdentityUserRole<string>
                {
                    RoleId = userRoleId,
                    UserId = superAdminUserId
                }
            };

            builder.Entity<IdentityUserRole<string>>().HasData(superAdminRoles);
        }
    }
}
