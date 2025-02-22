using BlogSystem.Core.Models;

namespace BlogSystem.Apis.DTOs
{
    public class CommentDto
    {
        public string Content { get; set; }
        public int PostId { get; set; }
    }
}
