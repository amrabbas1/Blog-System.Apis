using BlogSystem.Core.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;

namespace BlogSystem.Repository.Data
{
    public static class BlogDbContextSeed
    {
        public async static Task SeedAsync(BlogDbContext _context)
        {
            if (_context.Tags.Count() == 0)
            {
                var tagsData = File.ReadAllText(@"..\BlogSystem.Repository\Data\\DataSeed\\Tags.json");

                var tags = JsonSerializer.Deserialize<List<Tag>>(tagsData);

                if (tags is not null && tags.Count() > 0)
                {
                    await _context.Tags.AddRangeAsync(tags);
                    await _context.SaveChangesAsync();
                }
            }

            if (_context.Categories.Count() == 0)
            {
                var categoriesData = File.ReadAllText(@"..\BlogSystem.Repository\Data\\DataSeed\\Categories.json");

                var categories = JsonSerializer.Deserialize<List<Category>>(categoriesData);

                if (categories is not null && categories.Count() > 0)
                {
                    await _context.Categories.AddRangeAsync(categories);
                    await _context.SaveChangesAsync();
                }
            }
        }
    }
}
