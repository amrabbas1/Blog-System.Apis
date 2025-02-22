using AutoMapper;
using BlogSystem.Apis.DTOs;
using BlogSystem.Core.Models;
using static System.Runtime.InteropServices.JavaScript.JSType;

namespace BlogSystem.Apis.Mapping
{
    public class MappingProfile : Profile
    {
        public MappingProfile()
        {
            CreateMap<BlogPost, BlogPostDto>()
           .ForMember(dest => dest.CategoryName, opt => opt.MapFrom(src => src.Category.Name))
           .ForMember(dest => dest.TagsName, opt => opt.MapFrom(src => src.Tags.Select(t => t.Name)));

            CreateMap<BlogPost, BlogPostToReturnDto>()
             .ForMember(dest => dest.AuthorName, opt => opt.MapFrom(src => src.Author.UserName))
             .ForMember(dest => dest.CategoryName, opt => opt.MapFrom(src => src.Category.Name))
             .ForMember(dest => dest.TagsName, opt => opt.MapFrom(src => src.Tags.Select(t => t.Name)));

            CreateMap<Comment, CommentToReturnDto>()
           .ForMember(dest => dest.AuthorName, opt => opt.MapFrom(src => src.Author.UserName));

        }

    }
}
