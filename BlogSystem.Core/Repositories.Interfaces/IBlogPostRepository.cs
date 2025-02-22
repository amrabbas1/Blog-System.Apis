using BlogSystem.Core.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BlogSystem.Core.Repositories.Interfaces
{
    public interface IBlogPostRepository : IGenericRepository<BlogPost>
    {
        public Task<User?> GetAuthorByEmailAsync(string email);
        public Task<Category> GetCategoryAsync(string categoryName);
        public Task<List<Tag>> GetTagsAsync(ICollection<string> tagsName);
    }
}
