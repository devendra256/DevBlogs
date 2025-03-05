using DevBlogs.Web.Models.Domain;

namespace DevBlogs.Web.Models.ViewModels
{
    public class BlogPostViewModel
    {
        public Guid Id { get; set; }
        public string Heading { get; set; }
        public string PageTitle { get; set; }
        public string Content { get; set; }
        public string ShortDescription { get; set; }
        public string FeaturedImageUrl { get; set; }
        public string UrlHandle { get; set; }
        public string Author { get; set; }
        public bool Visible { get; set; }
        public DateTime PublishedDate { get; set; }
        public int TotalLikes { get; set; }
        public bool IsLiked { get; set; }
        public ICollection<Tag> Tags { get; set; }
    }
}
