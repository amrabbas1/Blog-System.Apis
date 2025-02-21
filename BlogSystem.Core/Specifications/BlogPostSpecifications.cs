using BlogSystem.Core.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace BlogSystem.Core.Specifications
{
    public class BlogPostSpecifications : BaseSpecifications<BlogPost>
    {
        public BlogPostSpecifications(int id) : base(P => P.Id == id)
        {
            ApplyIncludes();
        }
        public BlogPostSpecifications(BlogPostSpecParams blogPostSpec) : base(
            B =>
            (string.IsNullOrEmpty(blogPostSpec.SearchByTag) || B.Tags.Any(tag => tag.Name.ToLower().Contains(blogPostSpec.SearchByTag.ToLower())))
            &&
            (string.IsNullOrEmpty(blogPostSpec.SearchByCategory) || B.Category.Name.ToLower().Contains(blogPostSpec.SearchByCategory.ToLower()))
            &&
            (string.IsNullOrEmpty(blogPostSpec.SearchByTitle) || B.Title.ToLower().Contains(blogPostSpec.SearchByTitle.ToLower()))
            &&
            (string.IsNullOrEmpty(blogPostSpec.FilterByStatus) || B.Status == GetPostStatus(blogPostSpec.FilterByStatus))

            )
        {
            ApplyIncludes();
        }
        private static PostStatus? GetPostStatus(string status)
        {
            return Enum.TryParse<PostStatus>(status, true, out var parsedStatus) ? parsedStatus : null;
        }
        private void ApplyIncludes()
        {
            Includes.Add(P => P.Category);
            Includes.Add(P => P.Author);
            Includes.Add(P => P.Tags);
            Includes.Add(P => P.Comments);
        }
    }
}
