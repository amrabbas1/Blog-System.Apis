using AutoMapper;
using BlogSystem.Apis.DTOs;
using BlogSystem.Apis.Errors;
using BlogSystem.Core;
using BlogSystem.Core.Models;
using BlogSystem.Core.Repositories.Interfaces;
using BlogSystem.Core.Specifications;
using BlogSystem.Repository.Data;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Hosting;
using System.Security.Claims;

namespace BlogSystem.Apis.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class PostsController : ControllerBase
    {
        private readonly IBlogPostRepository _blogPostRepo;
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;

        public PostsController(IBlogPostRepository blogPostRepo, IUnitOfWork unitOfWork, IMapper mapper)
        {
            _blogPostRepo = blogPostRepo;
            _unitOfWork = unitOfWork;
            _mapper = mapper;
        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<BlogPostToReturnDto>>> GetAllPosts([FromQuery] BlogPostSpecParams blogPostSpec)
        {
            var spec = new BlogPostSpecifications(blogPostSpec);
            var posts = await _blogPostRepo.GetAllWithSpecAsync(spec);
            if (posts == null) return BadRequest(new ApiErrorResponse(StatusCodes.Status400BadRequest));
            var postDtos = _mapper.Map<IEnumerable<BlogPostToReturnDto>>(posts);

            return Ok(postDtos);
        }

        [HttpGet("{id}")]//Get BaseUrl/api/Products
        public async Task<IActionResult> GetPostById(int? id)
        {
            if (id is null) return BadRequest(new ApiErrorResponse(400));

            var spec = new BlogPostSpecifications(id.Value);

            var post = await _blogPostRepo.GetWithSpecAsync(spec);

            if (post is null) return NotFound(new ApiErrorResponse(400));

            var postDto = _mapper.Map<BlogPostToReturnDto>(post);

            return Ok(postDto);
        }

        [HttpPost]
        [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
        public async Task<ActionResult<BlogPostToReturnDto>> CreatePost(BlogPostDto model)
        {
            var userEmail = User.FindFirstValue(ClaimTypes.Email);
            var author = await _blogPostRepo.GetAuthorByEmailAsync(userEmail);
            if (author is null) return BadRequest(new ApiErrorResponse(401));

            var category = await _blogPostRepo.GetCategoryAsync(model.CategoryName);
            if (category == null) return NotFound(new ApiErrorResponse(404, "Category not found."));

            List<Tag> tags = await _blogPostRepo.GetTagsAsync(model.TagsName);

            var blogPost = new BlogPost
            {
                Title = model.Title,
                Content = model.Content,
                Author = author,
                AuthorId = author.Id,
                CreatedAt = DateTime.UtcNow,
                Status = model.Status,
                Category = category,
                CategoryId = category.Id,
                Tags = model.TagIds?.Select(tagId => new BlogPostTag { TagId = tagId }).ToList() ?? new List<BlogPostTag>()
            };

            blogPost.Tags = tags.Select(tag => new BlogPostTag
            {
                TagId = tag.Id,
                BlogPostId = blogPost.Id
            }).ToList();

            await _blogPostRepo.AddAsync(blogPost);
            await _unitOfWork.CompleteAsync();

            var postDto = _mapper.Map<BlogPostToReturnDto>(blogPost);

            return Ok(postDto);
        }
        [HttpPut]
        [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
        public async Task<ActionResult<BlogPostToReturnDto>> UpdatePost(BlogPostDto model)
        {
            if (model is null || model.Id is null) return BadRequest(new ApiErrorResponse(StatusCodes.Status400BadRequest));

            var userEmail = User.FindFirstValue(ClaimTypes.Email);
            var author = await _blogPostRepo.GetAuthorByEmailAsync(userEmail);
            if (author is null) return BadRequest(new ApiErrorResponse(401));

            var category = await _blogPostRepo.GetCategoryAsync(model.CategoryName);
            if (category == null) return NotFound(new ApiErrorResponse(404, "Category not found."));

            List<Tag> tags = await _blogPostRepo.GetTagsAsync(model.TagsName);

            var spec = new BlogPostSpecifications(model.Id.Value);

            var post = await _blogPostRepo.GetWithSpecAsync(spec);
            if (post is null) return BadRequest(new ApiErrorResponse(StatusCodes.Status400BadRequest));

            post.Title = model.Title;
            post.Content = model.Content;
            post.Author = author;
            post.AuthorId = author.Id;
            post.UpdatedAt = DateTime.UtcNow;
            post.Status = model.Status;
            post.Category = category;
            post.CategoryId = category.Id;
            post.Tags = model.TagIds?.Select(tagId => new BlogPostTag { TagId = tagId }).ToList() ?? new List<BlogPostTag>();

            var blogPostTagsRepo = _unitOfWork.Repository<BlogPostTag>();
            var existingTags = await blogPostTagsRepo.GetAllAsync();

            var postTags = existingTags.Where(t => t.BlogPostId == model.Id.Value);

            foreach (var tag in postTags)
            {
                blogPostTagsRepo.Delete(tag);
            }

            post.Tags = tags.Select(tag => new BlogPostTag
            {
                TagId = tag.Id,
                BlogPostId = post.Id
            }).ToList();

            _blogPostRepo.Update(post);
            await _unitOfWork.CompleteAsync();

            var postDto = _mapper.Map<BlogPostToReturnDto>(post);

            return Ok(postDto);
        }

        [HttpDelete]
        [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
        public async Task<IActionResult> DeletePost(int id)
        {
            var DeletedPost = await _blogPostRepo.GetAsync(id);
            
            if (DeletedPost is null) return BadRequest(new ApiErrorResponse(400));

            _blogPostRepo.Delete(DeletedPost);

            await _unitOfWork.CompleteAsync();

            return Ok();
        }
    }
}
