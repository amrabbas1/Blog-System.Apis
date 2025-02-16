using BlogSystem.Core.Models;

namespace BlogSystem.Apis.DTOs
{
    public class BlogPostDto
    {
        public int? Id { get; set; }
        public string Title { get; set; }
        public string Content { get; set; }
        public PostStatus Status { get; set; }
        public string? CategoryName { get; set; }
        public ICollection<string>? TagsName { get; set; }
    }
}
