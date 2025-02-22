using BlogSystem.Core.Models;
using BlogSystem.Core.Repositories.Interfaces;
using BlogSystem.Repository.Data;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BlogSystem.Repository.Repositories
{
    public class BlogPostRepository : GenericRepository<BlogPost>, IBlogPostRepository
    {
        private readonly BlogDbContext _context;
        private readonly UserManager<User> _userManager;

        public BlogPostRepository(BlogDbContext context, UserManager<User> userManager) : base(context)
        {
            _context = context;
            _userManager = userManager;
        }

        public async Task<User?> GetAuthorByEmailAsync(string email)
        {
            if (string.IsNullOrEmpty(email))
                return null;

            return await _userManager.FindByEmailAsync(email);
        }
        public async Task<Category> GetCategoryAsync(string categoryName)
        {
            if (string.IsNullOrEmpty(categoryName)) return null;
            return await _context.Categories.FirstOrDefaultAsync(c => c.Name == categoryName);
        }

        public async Task<List<Tag>> GetTagsAsync(ICollection<string> tagsName)
        {
            return tagsName != null && tagsName.Any()
                ? await _context.Tags.Where(t => tagsName.Contains(t.Name)).ToListAsync()
                : new List<Tag>();
        }

    }

}
