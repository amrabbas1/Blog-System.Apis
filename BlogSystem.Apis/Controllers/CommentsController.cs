using AutoMapper;
using BlogSystem.Apis.DTOs;
using BlogSystem.Apis.Errors;
using BlogSystem.Core.Repositories.Interfaces;
using BlogSystem.Core.Specifications;
using BlogSystem.Core;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using BlogSystem.Core.Models;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using System.Security.Claims;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Hosting;

namespace BlogSystem.Apis.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class CommentsController : ControllerBase
    {
        private readonly IGenericRepository<Comment> _genericRepository;
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;
        private readonly UserManager<User> _userManager;

        public CommentsController(IGenericRepository<Comment> genericRepository, IUnitOfWork unitOfWork, IMapper mapper, UserManager<User> userManager)
        {
            _genericRepository = genericRepository;
            _unitOfWork = unitOfWork;
            _mapper = mapper;
            _userManager = userManager;
        }

        [HttpPost]
        [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
        public async Task<ActionResult<CommentToReturnDto>> CreateComment(CommentDto model)
        {
            var post = await _unitOfWork.Repository<BlogPost>().GetAsync(model.PostId);
            if (post == null) return NotFound(new ApiErrorResponse(404, "Blog post not found"));
            var userEmail = User.FindFirstValue(ClaimTypes.Email);
            var author = await _userManager.FindByEmailAsync(userEmail);
            if (author == null) return BadRequest(new ApiErrorResponse(400));

            var comment = new Comment
            {
                Content = model.Content,
                PostId = post.Id,
                BlogPost = post,
                AuthorId = author.Id,
                Author = author
            };

            await _genericRepository.AddAsync(comment);

            await _unitOfWork.CompleteAsync();

            var commentToReturn = new CommentToReturnDto
            {
                Content = model.Content,
                AuthorName = author.UserName
            };

            return Ok(commentToReturn);
        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<CommentToReturnDto>>> GetAllComments(int id)
        {
            var spec = new BlogPostSpecifications(id);

            if (spec is null) return BadRequest(new ApiErrorResponse(400));

            var post = await _unitOfWork.Repository<BlogPost>().GetWithSpecAsync(spec);

            if (post is null) return NotFound(new ApiErrorResponse(400));

            var comments = post.Comments.OrderByDescending(C => C.CreatedAt);

            var commentsDto = _mapper.Map<IEnumerable<CommentToReturnDto>>(comments);

            return Ok(commentsDto);
        }

        [HttpDelete]
        [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
        public async Task<IActionResult> DeleteComment(int id)
        {
            var deletedComment = await _genericRepository.GetAsync(id);

            if (deletedComment is null) return BadRequest(new ApiErrorResponse(400));

            _genericRepository.Delete(deletedComment);

            await _unitOfWork.CompleteAsync();

            return Ok();
        }
    }
}
