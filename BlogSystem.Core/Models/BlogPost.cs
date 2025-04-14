using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BlogSystem.Core.Models
{
    public class BlogPost
    {
        public int Id { get; set; }
        public string Title { get; set; }
        public string Content { get; set; }
        public string? AuthorId { get; set; }//FK
        public User? Author { get; set; }
        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
        public DateTime? UpdatedAt { get; set; }
        public PostStatus Status { get; set; } // Published, Draft, Archived
        public int? CategoryId { get; set; }//FK
        public Category? Category { get; set; }
        public ICollection<BlogPostTag>? Tags { get; set; }
        public ICollection<Comment>? Comments { get; set; }
    }
}
