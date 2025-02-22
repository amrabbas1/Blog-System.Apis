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
                Tags = tags,
            };

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

            var DeletedPost = await _blogPostRepo.GetAsync(model.Id.Value);
            if (DeletedPost is null) return BadRequest(new ApiErrorResponse(StatusCodes.Status400BadRequest));

            var blogPost = new BlogPost
            {
                Title = model.Title,
                Content = model.Content,
                Author = author,
                AuthorId = author.Id,
                CreatedAt = DeletedPost.CreatedAt,
                UpdatedAt = DateTime.UtcNow,
                Status = model.Status,
                Category = category,
                CategoryId = category.Id,
                Tags = tags,
            };

            _blogPostRepo.Delete(DeletedPost);
            await _blogPostRepo.AddAsync(blogPost);
            await _unitOfWork.CompleteAsync();

            var postDto = _mapper.Map<BlogPostToReturnDto>(blogPost);

            return Ok(postDto);
        }

        [HttpDelete("{id}")]
        [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
        public async Task<IActionResult> DeletePost(int? id)
        {
            if (id is null) return BadRequest(new ApiErrorResponse(StatusCodes.Status400BadRequest));

            var DeletedPost = await _blogPostRepo.GetAsync(id.Value);

            _blogPostRepo.Delete(DeletedPost);

            await _unitOfWork.CompleteAsync();

            return Ok();
        }
    }
}
