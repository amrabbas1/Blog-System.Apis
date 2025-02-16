using BlogSystem.Core.Models;

namespace BlogSystem.Apis.DTOs
{
    public class BlogPostToReturnDto
    {
        public string Title { get; set; }
        public string Content { get; set; }
        public string? AuthorName { get; set; }
        public DateTime CreatedAt { get; set; } //= DateTime.UtcNow;
        public DateTime? UpdatedAt { get; set; }
        public PostStatus Status { get; set; }
        public string? CategoryName { get; set; }
        public ICollection<string>? TagsName { get; set; }
        //public ICollection<CommentDto> Comments { get; set; }
    }
}
