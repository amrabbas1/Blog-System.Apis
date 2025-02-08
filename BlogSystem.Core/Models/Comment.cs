using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BlogSystem.Core.Models
{
    public class Comment
    {
        public string Id { get; set; }
        public string Content { get; set; }
        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
        public string? PostId { get; set; }//FK
        public BlogPost BlogPost { get; set; }
        public string? AuthorId { get; set; }//FK
        public User Author { get; set; }
    }
}
