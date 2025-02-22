using BlogSystem.Core.Models;

namespace BlogSystem.Apis.DTOs
{
    public class CommentToReturnDto
    {
        public string Content { get; set; }
        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
        public string AuthorName { get; set; }
    }
}
