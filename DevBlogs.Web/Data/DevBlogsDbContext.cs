using DevBlogs.Web.Models.Domain;
using Microsoft.EntityFrameworkCore;

namespace DevBlogs.Web.Data
{
    public class DevBlogsDbContext : DbContext
    {
        public DevBlogsDbContext(DbContextOptions<DevBlogsDbContext> options) : base(options) { }

        public DbSet<BlogPost> BlogPosts { get; set; }
        public DbSet<Tag> Tags { get; set; }
        public DbSet<BlogPostLike> Likes { get; set; }

    }
}
